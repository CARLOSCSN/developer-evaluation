# Módulo de Sales (Vendas) - Documentação Completa

## 📋 Visão Geral

O módulo de Sales foi implementado seguindo os padrões DDD (Domain-Driven Design) e CQRS do projeto Ambev.DeveloperEvaluation. Este módulo gerencia o ciclo completo de vendas, desde a criação até o cancelamento, com cálculo automático de descontos baseado em regras de negócio.

## 🏗️ Arquitetura

### Camadas Implementadas

```
├── Domain (Ambev.DeveloperEvaluation.Domain)
│   ├── Entities/
│   │   ├── Sale.cs
│   │   └── SaleItem.cs
│   ├── Repositories/
│   │   └── ISaleRepository.cs
│   └── Events/
│       ├── SaleCreatedEvent.cs
│       ├── SaleModifiedEvent.cs
│       ├── SaleCancelledEvent.cs
│       └── ItemCancelledEvent.cs
│
├── Application (Ambev.DeveloperEvaluation.Application)
│   └── Sales/
│       ├── CreateSale/
│       ├── GetSale/
│       ├── GetSales/
│       ├── UpdateSale/
│       ├── DeleteSale/
│       └── CancelItem/
│
├── ORM (Ambev.DeveloperEvaluation.ORM)
│   ├── Mapping/
│   │   ├── SaleConfiguration.cs
│   │   └── SaleItemConfiguration.cs
│   └── Repositories/
│       └── SaleRepository.cs
│
└── WebApi (Ambev.DeveloperEvaluation.WebApi)
    └── Features/Sales/
        ├── SalesController.cs
        └── [CreateSale, GetSale, GetSales, UpdateSale, DeleteSale, CancelItem]/
```

## 📊 Modelo de Dados

### Entidade Sale
```csharp
- Id: Guid (PK)
- SaleNumber: string (único, gerado automaticamente)
- Date: DateTime
- CustomerId: int (External Identity)
- CustomerName: string (desnormalizado)
- Branch: string
- TotalAmount: decimal
- Cancelled: bool
- CreatedAt: DateTime
- UpdatedAt: DateTime?
- Items: List<SaleItem>
```

### Entidade SaleItem
```csharp
- Id: Guid (PK)
- SaleId: Guid (FK)
- ProductId: int (External Identity)
- ProductName: string (desnormalizado)
- Quantity: int
- UnitPrice: decimal
- Discount: decimal (0.0 a 1.0)
- TotalItemAmount: decimal
- Cancelled: bool
```

## 💼 Regras de Negócio

### Cálculo de Descontos Automático

| Quantidade | Desconto | Observação |
|------------|----------|------------|
| < 4 | 0% | Sem desconto |
| 4 - 9 | 10% | Desconto aplicado ao valor total do item |
| 10 - 20 | 20% | Desconto aplicado ao valor total do item |
| > 20 | ❌ ERRO | "Não é permitido vender mais de 20 itens do mesmo produto" |

### Validações

- ✅ Todos os itens devem ter quantidade entre 1 e 20
- ✅ Preços unitários devem ser > 0
- ✅ Cliente e Branch são obrigatórios
- ✅ Vendas canceladas não podem ser alteradas
- ✅ Descontos são recalculados automaticamente em atualizações

## 🌐 Endpoints da API

### 1. **POST** `/api/sales` - Criar Venda

**Request Body:**
```json
{
  "customerId": 1,
  "customerName": "João Silva",
  "branch": "Filial São Paulo",
  "date": "2024-01-15T10:30:00Z",
  "items": [
    {
      "productId": 101,
      "productName": "Produto A",
      "quantity": 10,
      "unitPrice": 50.00
    }
  ]
}
```

**Response (201 Created):**
```json
{
  "success": true,
  "message": "Sale created successfully",
  "data": {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "saleNumber": "A1B2C3D4E5F6G7H8",
    "date": "2024-01-15T10:30:00Z",
    "customerId": 1,
    "customerName": "João Silva",
    "branch": "Filial São Paulo",
    "totalAmount": 400.00,
    "items": [
      {
        "id": "7c9e6679-7425-40de-944b-e07fc1f90ae7",
        "productId": 101,
        "productName": "Produto A",
        "quantity": 10,
        "unitPrice": 50.00,
        "discount": 0.20,
        "totalItemAmount": 400.00
      }
    ]
  }
}
```

### 2. **GET** `/api/sales` - Listar Vendas (com paginação e filtros)

**Query Parameters:**
- `page` (int, default: 1)
- `size` (int, default: 10)
- `branch` (string, optional)
- `customerId` (int, optional)
- `minDate` (DateTime, optional)
- `maxDate` (DateTime, optional)
- `cancelled` (bool, optional)

**Exemplo:**
```
GET /api/sales?page=1&size=10&branch=Filial%20SP&cancelled=false
```

**Response (200 OK):**
```json
{
  "success": true,
  "message": "Sales retrieved successfully",
  "data": {
    "sales": [
      {
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "saleNumber": "A1B2C3D4E5F6G7H8",
        "date": "2024-01-15T10:30:00Z",
        "customerId": 1,
        "customerName": "João Silva",
        "branch": "Filial São Paulo",
        "totalAmount": 400.00,
        "cancelled": false,
        "itemCount": 1
      }
    ],
    "totalCount": 1,
    "currentPage": 1,
    "totalPages": 1
  }
}
```

### 3. **GET** `/api/sales/{id}` - Obter Venda Específica

**Response (200 OK):**
```json
{
  "success": true,
  "message": "Sale retrieved successfully",
  "data": {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "saleNumber": "A1B2C3D4E5F6G7H8",
    "date": "2024-01-15T10:30:00Z",
    "customerId": 1,
    "customerName": "João Silva",
    "branch": "Filial São Paulo",
    "totalAmount": 400.00,
    "cancelled": false,
    "items": [...]
  }
}
```

### 4. **PUT** `/api/sales/{id}` - Atualizar Venda

**Request Body:** (mesmo formato do POST)

**Response (200 OK):** Retorna a venda atualizada com descontos recalculados

### 5. **DELETE** `/api/sales/{id}` - Cancelar Venda

**Response (200 OK):**
```json
{
  "success": true,
  "message": "Sale cancelled successfully"
}
```

### 6. **PATCH** `/api/sales/{id}/items/{itemId}` - Cancelar Item Específico (DIFERENCIAL)

**Response (200 OK):**
```json
{
  "success": true,
  "message": "Item cancelled successfully",
  "data": {
    "success": true,
    "message": "Item cancelled successfully",
    "updatedTotalAmount": 350.00
  }
}
```

## 🎯 Eventos de Domínio (DIFERENCIAL)

Todos os eventos são logados via `ILogger` com informações detalhadas:

### 1. SaleCreatedEvent
```
Log: "Sale created: ID={SaleId}, SaleNumber={SaleNumber}, Customer={CustomerName}, Branch={Branch}, TotalAmount={TotalAmount}, ItemCount={ItemCount}"
```

### 2. SaleModifiedEvent
```
Log: "Sale modified: ID={SaleId}, SaleNumber={SaleNumber}, Customer={CustomerName}, Branch={Branch}, TotalAmount={TotalAmount}, ItemCount={ItemCount}"
```

### 3. SaleCancelledEvent
```
Log: "Sale cancelled: ID={SaleId}, SaleNumber={SaleNumber}, Customer={CustomerName}, Branch={Branch}, TotalAmount={TotalAmount}"
```

### 4. ItemCancelledEvent
```
Log: "Item cancelled: ItemID={ItemId}, SaleID={SaleId}, Product={ProductName}, Quantity={Quantity}"
```

## 🔧 Configuração e Execução

### 1. Pré-requisitos
```bash
- .NET 8 SDK
- Docker & Docker Compose
- PostgreSQL (via Docker)
```

### 2. Executar via Docker Compose
```bash
cd template/backend
docker-compose up -d
```

### 3. Executar Migrations
```bash
cd template/backend/src/Ambev.DeveloperEvaluation.WebApi
dotnet ef migrations add AddSalesModule --project ../Ambev.DeveloperEvaluation.ORM
dotnet ef database update --project ../Ambev.DeveloperEvaluation.ORM
```

### 4. Executar a API
```bash
cd template/backend/src/Ambev.DeveloperEvaluation.WebApi
dotnet run
```

A API estará disponível em: `https://localhost:5119` ou `http://localhost:7181`

Swagger UI: `https://localhost:5119/swagger`

### 5. Executar Testes
```bash
cd template/backend
dotnet test
```

## ✅ Testes Implementados

### Testes de Domínio (`SaleItemTests`)
- ✅ Quantidade < 4: sem desconto
- ✅ Quantidade 4-9: desconto de 10%
- ✅ Quantidade 10-20: desconto de 20%
- ✅ Quantidade > 20: lança exceção
- ✅ Cálculo correto do total com desconto

### Testes de Application (`CreateSaleHandlerTests`)
- ✅ Criação válida com descontos aplicados
- ✅ Validação falha com quantidade > 20
- ✅ Repositório é chamado corretamente
- ✅ Eventos são logados

## 📝 Padrões Seguidos

### ✅ Arquitetura
- [x] DDD (Domain-Driven Design)
- [x] CQRS (Command Query Responsibility Segregation)
- [x] Mediator Pattern (MediatR)
- [x] Repository Pattern
- [x] External Identity Pattern

### ✅ Código
- [x] Clean Code
- [x] SOLID Principles
- [x] Dependency Injection
- [x] AutoMapper para mapeamentos
- [x] FluentValidation para validações
- [x] Tratamento de exceções centralizado

### ✅ Testes
- [x] Unit Tests com xUnit
- [x] Mocking com NSubstitute
- [x] Assertions com FluentAssertions
- [x] Padrão AAA (Arrange, Act, Assert)

## 📚 Recursos Adicionais

- **Swagger**: Acesse `/swagger` para documentação interativa da API
- **Logs**: Verifique os logs da aplicação para rastreamento de eventos
- **Banco de Dados**: PostgreSQL rodando na porta 5432 (via Docker)

## 🎉 Conclusão

O módulo de Sales foi implementado completamente seguindo todos os padrões do projeto. Todas as features solicitadas foram entregues, incluindo os diferenciais de eventos de domínio e cancelamento de itens individuais.

# Developer Evaluation Project

`READ CAREFULLY`

## Instructions
**The test below will have up to 7 calendar days to be delivered from the date of receipt of this manual.**

- The code must be versioned in a public Github repository and a link must be sent for evaluation once completed
- Upload this template to your repository and start working from it
- Read the instructions carefully and make sure all requirements are being addressed
- The repository must provide instructions on how to configure, execute and test the project
- Documentation and overall organization will also be taken into consideration

## Use Case
**You are a developer on the DeveloperStore team. Now we need to implement the API prototypes.**

As we work with `DDD`, to reference entities from other domains, we use the `External Identities` pattern with denormalization of entity descriptions.

Therefore, you will write an API (complete CRUD) that handles sales records. The API needs to be able to inform:

* Sale number
* Date when the sale was made
* Customer
* Total sale amount
* Branch where the sale was made
* Products
* Quantities
* Unit prices
* Discounts
* Total amount for each item
* Cancelled/Not Cancelled

It's not mandatory, but it would be a differential to build code for publishing events of:
* SaleCreated
* SaleModified
* SaleCancelled
* ItemCancelled

If you write the code, **it's not required** to actually publish to any Message Broker. You can log a message in the application log or however you find most convenient.

### Business Rules

* Purchases above 4 identical items have a 10% discount
* Purchases between 10 and 20 identical items have a 20% discount
* It's not possible to sell above 20 identical items
* Purchases below 4 items cannot have a discount

These business rules define quantity-based discounting tiers and limitations:

1. Discount Tiers:
   - 4+ items: 10% discount
   - 10-20 items: 20% discount

2. Restrictions:
   - Maximum limit: 20 items per product
   - No discounts allowed for quantities below 4 items

---

## 🚀 Como Executar o Projeto

### Pré-requisitos
- .NET 8 SDK
- Docker & Docker Compose

### 1. Subir infraestrutura (PostgreSQL, MongoDB, Redis)
```bash
docker-compose -f template/backend/docker-compose.yml up -d ambev.developerevaluation.database ambev.developerevaluation.nosql ambev.developerevaluation.cache
```

### 2. Executar migrations
```bash
cd template/backend/src/Ambev.DeveloperEvaluation.WebApi
dotnet ef database update --project ../Ambev.DeveloperEvaluation.ORM
```

### 3. Rodar a API
```bash
cd template/backend/src/Ambev.DeveloperEvaluation.WebApi
dotnet run
```
API disponível em: `https://localhost:7181` | Swagger: `https://localhost:7181/swagger`

### 4. Executar testes
```bash
cd template/backend
dotnet test
```

---

## 📚 Documentação

- **[Documentação completa do módulo Sales](SALES_MODULE_DOCUMENTATION.md)** — endpoints, regras de negócio, eventos e exemplos
- [Visão geral do projeto](/.doc/overview.md)
- [Tech Stack](/.doc/tech-stack.md)
- [Frameworks](/.doc/frameworks.md)
- [Estrutura do projeto](/.doc/project-structure.md)

---

## ✅ Funcionalidades Implementadas

### Módulo Sales (Vendas)
| Funcionalidade | Status |
|---------------|--------|
| CRUD completo de vendas | ✅ |
| Cálculo automático de descontos (0%, 10%, 20%) | ✅ |
| Validação de máximo 20 itens por produto | ✅ |
| Paginação, ordenação e filtros | ✅ |
| Eventos de domínio (SaleCreated, SaleModified, SaleCancelled, ItemCancelled) | ✅ |
| Cancelamento de venda | ✅ |
| Cancelamento de item individual (com recálculo de total) | ✅ |
| Testes unitários | ✅ |

---

## Overview
This section provides a high-level overview of the project and the various skills and competencies it aims to assess for developer candidates. 

See [Overview](/.doc/overview.md)

## Tech Stack
This section lists the key technologies used in the project, including the backend, testing, frontend, and database components. 

See [Tech Stack](/.doc/tech-stack.md)

## Frameworks
This section outlines the frameworks and libraries that are leveraged in the project to enhance development productivity and maintainability. 

See [Frameworks](/.doc/frameworks.md)

## Project Structure
This section describes the overall structure and organization of the project files and directories. 

See [Project Structure](/.doc/project-structure.md)
using Ambev.DeveloperEvaluation.Application.Users.DeleteUser;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.DeleteUser;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Mappings;

public class DeleteUserRequestProfile : Profile
{
    public DeleteUserRequestProfile()
    {
        CreateMap<DeleteUserRequest, DeleteUserCommand>();
    }
}
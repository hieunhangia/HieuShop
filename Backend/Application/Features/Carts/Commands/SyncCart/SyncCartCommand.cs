using Application.Features.Carts.DTOs;
using MediatR;

namespace Application.Features.Carts.Commands.SyncCart;

public class SyncCartCommand : IRequest<CartDto>
{
    public Guid UserId { get; set; }
}
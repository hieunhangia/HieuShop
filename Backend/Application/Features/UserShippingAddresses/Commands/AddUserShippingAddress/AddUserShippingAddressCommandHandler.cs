using Application.Common.Exceptions;
using Domain.Entities.Users;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.UserShippingAddresses.Commands.AddUserShippingAddress;

public class AddUserShippingAddressCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<AddUserShippingAddressCommand>
{
    public async Task Handle(AddUserShippingAddressCommand request, CancellationToken cancellationToken)
    {
        var ward = await unitOfWork.Wards.GetByIdAsync(request.WardId);

        if (ward == null)
        {
            throw new NotFoundException($"Không tìm thấy phường/xã với Id: {request.WardId}");
        }

        unitOfWork.UserShippingAddresses.Add(new UserShippingAddress
        {
            RecipientName = request.RecipientName,
            RecipientPhone = request.RecipientPhone,
            DetailAddress = request.DetailAddress,
            WardId = request.WardId,
            UserId = request.UserId
        });
        await unitOfWork.CompleteAsync();
    }
}
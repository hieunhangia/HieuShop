using Application.Common.Exceptions;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.UserShippingAddresses.Commands.RemoveUserShippingAddress;

public class RemoveUserShippingAddressCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<RemoveUserShippingAddressCommand>
{
    public async Task Handle(RemoveUserShippingAddressCommand request, CancellationToken cancellationToken)
    {
        var shippingAddress = await unitOfWork.UserShippingAddresses.GetByIdAsync(request.ShippingAddressId);
        if (shippingAddress != null)
        {
            if (shippingAddress.UserId != request.UserId)
            {
                throw new ForbiddenAccessException("Bạn không có quyền xóa địa chỉ này.");
            }

            unitOfWork.UserShippingAddresses.Remove(shippingAddress);
            await unitOfWork.CompleteAsync();
        }
    }
}
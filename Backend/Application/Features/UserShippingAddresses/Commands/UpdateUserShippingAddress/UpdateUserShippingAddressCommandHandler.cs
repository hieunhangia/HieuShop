using Application.Common.Exceptions;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.UserShippingAddresses.Commands.UpdateUserShippingAddress;

public class UpdateUserShippingAddressCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateUserShippingAddressCommand>
{
    public async Task Handle(UpdateUserShippingAddressCommand request, CancellationToken cancellationToken)
    {
        var shippingAddress = await unitOfWork.UserShippingAddresses.GetByIdAsync(request.ShippingAddressId);
        if (shippingAddress == null)
        {
            throw new NotFoundException($"Không tìm thấy địa chỉ giao hàng với ID: {request.ShippingAddressId}");
        }

        if (shippingAddress.UserId != request.UserId)
        {
            throw new ForbiddenAccessException("Bạn không có quyền cập nhật địa chỉ này.");
        }

        shippingAddress.RecipientName = request.RecipientName;
        shippingAddress.RecipientPhone = request.RecipientPhone;
        shippingAddress.DetailAddress = request.DetailAddress;
        shippingAddress.WardId = request.WardId;

        unitOfWork.UserShippingAddresses.Update(shippingAddress);
        await unitOfWork.CompleteAsync();
    }
}
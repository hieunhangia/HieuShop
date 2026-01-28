using Application.Common.Exceptions;
using Application.Features.UserShippingAddresses.DTOs;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.UserShippingAddresses.Queries.GetUserShippingAddress;

public class GetUserShippingAddressQueryHandler(IUnitOfWork unitOfWork, UserShippingAddressMapper mapper)
    : IRequestHandler<GetUserShippingAddressQuery, UserShippingAddressDetailDto>
{
    public async Task<UserShippingAddressDetailDto> Handle(GetUserShippingAddressQuery request,
        CancellationToken cancellationToken)
    {
        var shippingAddress = await unitOfWork.UserShippingAddresses
            .GetByIdWithDetailReadOnlyAsync(request.ShippingAddressId);
        if (shippingAddress == null)
        {
            throw new NotFoundException($"Không tìm thấy địa chỉ giao hàng với Id: {request.ShippingAddressId}");
        }

        return shippingAddress.UserId != request.UserId
            ? throw new ForbiddenAccessException("Bạn không có quyền truy cập địa chỉ này.")
            : mapper.MapToDetailDto(shippingAddress);
    }
}
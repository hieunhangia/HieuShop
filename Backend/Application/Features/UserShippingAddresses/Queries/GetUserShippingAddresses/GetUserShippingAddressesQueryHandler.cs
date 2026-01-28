using Application.Features.UserShippingAddresses.DTOs;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.UserShippingAddresses.Queries.GetUserShippingAddresses;

public class GetUserShippingAddressesQueryHandler(IUnitOfWork unitOfWork, UserShippingAddressMapper mapper)
    : IRequestHandler<GetUserShippingAddressesQuery, IReadOnlyList<UserShippingAddressSummaryDto>>
{
    public async Task<IReadOnlyList<UserShippingAddressSummaryDto>> Handle(GetUserShippingAddressesQuery request,
        CancellationToken cancellationToken) =>
        mapper.MapToSummaryDtoList(await unitOfWork.UserShippingAddresses
            .GetListByUserIdWithDetailReadOnlyAsync(request.UserId));
}
using Application.Common.Exceptions;
using Domain.Entities.Users;
using Domain.Interfaces;
using MediatR;
using UnauthorizedAccessException = Application.Common.Exceptions.UnauthorizedAccessException;

namespace Application.Features.Coupons.Commands.PurchaseCoupon;

public class PurchaseCouponCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<PurchaseCouponCommand>
{
    public async Task Handle(PurchaseCouponCommand request, CancellationToken cancellationToken)
    {
        var user = await unitOfWork.Users.GetByIdAsync(request.UserId);
        if (user == null)
        {
            throw new UnauthorizedAccessException("Bạn phải đăng nhập để thực hiện hành động này.");
        }

        var coupon = await unitOfWork.Coupons.GetByIdAsync(request.CouponId);

        if (coupon is not { IsActive: true })
        {
            throw new NotFoundException("Mã giảm giá không tồn tại hoặc không còn hiệu lực.");
        }

        if (user.LoyaltyPoints < coupon.LoyaltyPointsCost)
        {
            throw new BadRequestException("Bạn không đủ điểm để mua mã giảm giá này.");
        }

        user.LoyaltyPoints -= coupon.LoyaltyPointsCost;
        unitOfWork.Users.Update(user);
        unitOfWork.UserCoupons.Add(new UserCoupon
        {
            UserId = user.Id,
            CouponId = coupon.Id,
        });

        await unitOfWork.CompleteAsync();
    }
}
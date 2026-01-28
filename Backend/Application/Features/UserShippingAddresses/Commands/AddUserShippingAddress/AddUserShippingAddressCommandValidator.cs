using Domain.Constants;
using FluentValidation;

namespace Application.Features.UserShippingAddresses.Commands.AddUserShippingAddress;

public class AddUserShippingAddressCommandValidator : AbstractValidator<AddUserShippingAddressCommand>
{
    public AddUserShippingAddressCommandValidator()
    {
        const int recipientNameMaxLength = DataSchema.UserShippingAddress.RecipientNameMaxLength;
        const int recipientPhoneLength = DataSchema.UserShippingAddress.RecipientPhoneLength;
        const int detailAddressMaxLength = DataSchema.UserShippingAddress.DetailAddressMaxLength;
        RuleFor(x => x.RecipientName)
            .NotEmpty().WithMessage("Tên người nhận là bắt buộc.")
            .MaximumLength(recipientNameMaxLength)
            .WithMessage($"Tên người nhận không được vượt quá {recipientNameMaxLength} ký tự.");

        RuleFor(x => x.RecipientPhone)
            .NotEmpty().WithMessage("Số điện thoại người nhận là bắt buộc.")
            .Length(recipientPhoneLength, recipientPhoneLength)
            .WithMessage($"Số điện thoại người nhận phải có đúng {recipientPhoneLength} ký tự.")
            .Matches(@"^0\d+$").WithMessage("Số điện thoại người nhận không hợp lệ.");

        RuleFor(x => x.DetailAddress)
            .NotEmpty().WithMessage("Địa chỉ chi tiết là bắt buộc.")
            .MaximumLength(detailAddressMaxLength)
            .WithMessage($"Địa chỉ chi tiết không được vượt quá {detailAddressMaxLength} ký tự.");
    }
}
using FluentValidation;
namespace xPlugUniAdmissionManager.Assets.AppKits.RegWizValidation;

public class ContactVal : AbstractValidator<ContactVM>

{
    public ContactVal()
    {
        RuleFor(e => e.Email).NotEmpty().WithMessage(UIValidationMsg.EMAIL_REQUIRED).EmailAddress().WithMessage("A valid email is required");
        RuleFor(e => e.MobileNo).NotEmpty().Length(11, 11).WithMessage(UIValidationMsg.MOBILE_NUMBER_REQUIRED)
            .Must((biodata, mobileNo) => biodata.MobileNo.IsMobileNumberValid())
            .WithMessage(UIValidationMsg.INVALID_MOBILE_NUMBER);
    }
}

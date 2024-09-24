using FluentValidation;

namespace xPlugUniAdmissionManager.Assets.AppKits.RegWizValidation;

public class BioDataVal : AbstractValidator<BioDataVM>
{
    public BioDataVal()
    {
        RuleFor(e => e.Surname).NotEmpty().WithMessage(UIValidationMsg.SURNAME_REQUIRED).Length(3, 50);
        RuleFor(e => e.FirstName).NotEmpty().WithMessage(UIValidationMsg.FIRST_NAME_REQUIRED).Length(3, 50);
        
        RuleFor(e => e.DateOfBirth).NotEmpty().WithMessage(UIValidationMsg.DATE_OF_BIRTH_REQUIRED);
        RuleFor(e => e.Gender).NotEmpty().WithMessage(UIValidationMsg.GENDER_REQUIRED);
        RuleFor(e => e.MaritalStatus).NotEmpty().WithMessage(UIValidationMsg.MARITAL_STATUS_REQUIRED);
    }
}

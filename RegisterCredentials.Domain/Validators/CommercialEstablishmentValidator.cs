using FluentValidation;
using RegisterCredentials.Domain.Entities;

namespace RegisterCredentials.Domain.Validators
{
    public class CommercialEstablishmentValidator : AbstractValidator<CommercialEstablishment>
    {
        public CommercialEstablishmentValidator() {
            RuleFor(x => x)
                .NotEmpty()
                .NotNull();

            RuleFor(x => x.CompanyName)
                .NotEmpty()
                .NotNull()
                .MinimumLength(3)
                .MaximumLength(60);

            RuleFor(x => x.DocumentNumber)
                .NotEmpty()
                .NotNull()
                .Must(e => e.Length is 11 or 14)
                .WithMessage("character count must be between 11 and 14");

            RuleFor(x => x.DocumentType)
                .NotEmpty()
                .NotNull()
                .IsInEnum();

            RuleFor(x => x.Enabled)
                .NotEmpty()
                .NotNull()
                .Must(e => e == false || e == true);

            RuleFor(x => x.PaymentSchemes)
                .NotEmpty()
                .NotNull()
                .ForEach(x => x.IsInEnum());

            RuleFor(x => x.BankAccount)
                .NotEmpty()
                .NotNull()
                .ChildRules(x =>
                {
                    x.RuleFor(x => x.Ispb)
                        .NotEmpty()
                        .NotNull();

                    x.RuleFor(x => x.Branch)
                        .NotEmpty()
                        .NotNull();

                    x.RuleFor(e => e.Account)
                        .NotNull()
                        .NotEmpty();

                    x.RuleFor(e => e.AccountType)
                        .NotEmpty()
                        .NotNull()
                        .IsInEnum();

                    x.RuleFor(e => e.DocumentNumber)
                        .NotNull()
                        .NotEmpty()
                        .Must(e => e.Length is 11 or 14)
                        .WithMessage("character count must be between 11 and 14");

                    x.RuleFor(e => e.DocumentType)
                        .NotEmpty()
                        .NotNull()
                        .IsInEnum();
                });

        }
    }
}

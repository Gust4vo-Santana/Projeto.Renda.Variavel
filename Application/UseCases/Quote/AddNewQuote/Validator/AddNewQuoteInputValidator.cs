using FluentValidation;

namespace Application.UseCases.Quote.AddNewQuote.Validator
{
    public class AddNewQuoteInputValidator : AbstractValidator<AddNewQuoteInput>
    {
        public AddNewQuoteInputValidator()
        {
            ValidateId();
            ValidateAssetId();
            ValidatePrice();
            ValidateDate();
        }

        private void ValidateId()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id must not be empty.");
        }

        private void ValidateAssetId()
        {
            RuleFor(x => x.AssetId)
                .NotEmpty().WithMessage("AssetId must not be empty.")
                .GreaterThan(0).WithMessage("AssetId must be greater than 0.");
        }

        private void ValidatePrice()
        {
            RuleFor(x => x.Price)
                .NotEmpty().WithMessage("Price must not be empty.")
                .GreaterThan(0).WithMessage("Price must be greater than 0.");
        }

        private void ValidateDate()
        {
            RuleFor(x => x.Date)
                .NotEmpty().WithMessage("Date must not be empty.")
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Date must not be in the future.");
        }
    }
}

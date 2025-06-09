using FluentValidation;

namespace Application.UseCases.Quote.Validator
{
    public class GetLatestQuoteInputValidator : AbstractValidator<GetLatestQuoteInput>
    {
        public GetLatestQuoteInputValidator()
        {
            ValidateAssetId();
        }

        private void ValidateAssetId()
        {
            RuleFor(x => x.AssetId)
                .NotEmpty().WithMessage("AssetId is required.")
                .GreaterThan(0).WithMessage("AssetId must be greater than 0.");
        }
    }
}

using FluentValidation;

namespace Application.UseCases.Operation.GetGlobalAveragePriceByAsset.Validator
{
    public class GetGlobalAveragePriceInputValidator : AbstractValidator<GetGlobalAveragePriceByAssetInput>
    {
        public GetGlobalAveragePriceInputValidator()
        {
            ValidateAssetId();
        }

        private void ValidateAssetId()
        {
            RuleFor(x => x.AssetId)
                .NotEmpty().WithMessage("AssetId must not be empty.")
                .GreaterThan(0).WithMessage("AssetId must be greater than 0.");
        }
    }
}

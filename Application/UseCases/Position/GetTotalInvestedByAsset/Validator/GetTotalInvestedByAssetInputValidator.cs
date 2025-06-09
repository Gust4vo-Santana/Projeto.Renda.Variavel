using FluentValidation;

namespace Application.UseCases.Position.GetTotalInvestedByAsset.Validator
{
    public class GetTotalInvestedByAssetInputValidator : AbstractValidator<GetTotalInvestedByAssetInput>
    {
        public GetTotalInvestedByAssetInputValidator()
        {
            ValidateUserId();
            ValidateAssetId();
        }

        private void ValidateUserId()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required.")
                .GreaterThan(0).WithMessage("UserId must be greater than 0.");
        }

        private void ValidateAssetId()
        {
            RuleFor(x => x.AssetId)
                .NotEmpty().WithMessage("AssetId is required.")
                .GreaterThan(0).WithMessage("AssetId must be greater than 0.");
        }
    }
}

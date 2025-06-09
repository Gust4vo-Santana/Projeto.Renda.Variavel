using FluentValidation;

namespace Application.UseCases.Position.GetPositionByAsset.Validator
{
    public class GetPositionByAssetInputValidator : AbstractValidator<GetPositionByAssetInput>
    {
        public GetPositionByAssetInputValidator()
        {
            ValidateAssetId();
            ValidateUserId();
        }

        private void ValidateAssetId()
        {
            RuleFor(x => x.AssetId)
                .NotEmpty().WithMessage("AssetId must not be empty.")
                .GreaterThan(0).WithMessage("AssetId must be greater than 0.");
        }

        private void ValidateUserId()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("AssetId must not be empty.")
                .GreaterThan(0).WithMessage("AssetId must be greater than 0.");
        }
    }
}

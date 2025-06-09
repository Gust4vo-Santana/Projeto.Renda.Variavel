using FluentValidation;

namespace Application.UseCases.Position.GetAveragePriceByUser.Validator
{
    public class GetAveragePriceByUserInputValidator : AbstractValidator<GetAveragePriceByUserInput>
    {
        public GetAveragePriceByUserInputValidator()
        {
            ValidateUserId();
            ValidateAssetId();
        }

        private void ValidateUserId()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("UserId must be greater than 0.");
        }

        private void ValidateAssetId()
        {
            RuleFor(x => x.AssetId)
                .GreaterThan(0).WithMessage("AssetId must be greater than 0.");
        }
    }
}

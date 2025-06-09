using FluentValidation;

namespace Application.UseCases.Position.GetGlobalPosition.Validator
{
    public class GetGlobalPositionInputValidator : AbstractValidator<GetGlobalPositionInput>
    {
        public GetGlobalPositionInputValidator()
        {
            ValidateUserId();
        }

        private void ValidateUserId()
        {
            RuleFor(x => x.UserId)
                .NotEmpty()
                .GreaterThan(0).WithMessage("UserId must be greater than 0.");
        }
    }
}

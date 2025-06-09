using FluentValidation;

namespace Application.UseCases.Operation.GetBrokerageFeeByUser.Validator
{
    public class GetBrokerageFeeByUserInputValidator : AbstractValidator<GetBrokerageFeeByUserInput>
    {
        public GetBrokerageFeeByUserInputValidator()
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

namespace Application.UseCases.Operation.GetBrokerageFeeByUser
{
    public record GetBrokerageFeeByUserInput
    {
        public long UserId { get; init; }
    }
}

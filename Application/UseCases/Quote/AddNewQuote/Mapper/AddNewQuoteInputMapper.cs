using QuoteEntity = Domain.Entities.Quote;

namespace Application.UseCases.Quote.AddNewQuote.Mapper
{
    public static class AddNewQuoteInputMapper
    {
        public static QuoteEntity MapToDomainEntity(this AddNewQuoteInput input)
        {
            return new QuoteEntity
            {
                Id = input.Id,
                AssetId = input.AssetId,
                Price = input.Price,
                Date = input.Date
            };
        }
    }
}

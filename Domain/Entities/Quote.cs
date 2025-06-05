namespace Domain.Entities
{
    public class Quote
    {
        public long Id { get; private set; }
        public long AssetId { get; private set; }
        public decimal Price { get; private set; }
        public DateTime Date { get; private set; }

        public Quote()
        {
        }

        public Quote(long id, long assetId, decimal price, DateTime date)
        {
            Id = id;
            AssetId = assetId;
            Price = price;
            Date = date;
        }
    }
}

namespace Domain.Entities
{
    public class Quote
    {
        public long Id { get; set; }
        public long AssetId { get; set; }
        public decimal Price { get; set; }
        public DateTime Date { get; set; }
        public Asset Asset { get; set; }

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

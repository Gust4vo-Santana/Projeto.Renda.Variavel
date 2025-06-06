namespace Domain.Entities
{
    public class Position
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long AssetId { get; set; }
        public decimal Quantity { get; set; }
        public decimal AveragePrice { get; set; }
        public decimal PAndL { get; set; }
        public User User { get; set; }
        public Asset Asset { get; set; }

        public Position()
        {
        }

        public Position(long id, long userId, long assetId, decimal quantity, decimal averagePrice, decimal pAndL)
        {
            Id = id;
            UserId = userId;
            AssetId = assetId;
            Quantity = quantity;
            AveragePrice = averagePrice;
            PAndL = pAndL;
        }
    }
}

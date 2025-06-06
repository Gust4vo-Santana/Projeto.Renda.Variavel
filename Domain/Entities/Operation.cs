using Domain.ValueObjects;

namespace Domain.Entities
{
    public class Operation
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long AssetId { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public OperationType OperationType { get; set; }
        public decimal BrokerageFee { get; set; }
        public DateTime Date { get; set; }
        public User User { get; set; }
        public Asset Asset { get; set; }

        public Operation()
        {
        }

        public Operation(long id, 
                         long userId, 
                         long assetId, 
                         decimal quantity, 
                         decimal price, 
                         OperationType operationType, 
                         decimal brokerageFee, 
                         DateTime date)
        {
            Id = id;
            UserId = userId;
            AssetId = assetId;
            Quantity = quantity;
            Price = price;
            OperationType = operationType;
            BrokerageFee = brokerageFee;
            Date = date;
        }
    }
}

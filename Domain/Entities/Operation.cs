using Domain.ValueObjects;

namespace Domain.Entities
{
    public class Operation
    {
        public long Id { get; private set; }
        public long UserId { get; private set; }
        public long AssetId { get; private set; }
        public decimal Quantity { get; private set; }
        public decimal Price { get; private set; }
        public OperationType OperationType { get; private set; }
        public decimal BrokerageFee { get; private set; }
        public DateTime Date { get; private set; }

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

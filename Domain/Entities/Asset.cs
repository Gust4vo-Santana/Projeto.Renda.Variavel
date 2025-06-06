namespace Domain.Entities
{
    public class Asset
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public IEnumerable<Position> Positions { get; set; }
        public IEnumerable<Operation> Operations { get; set; }
        public IEnumerable<Quote> Quotes { get; set; }

        public Asset()
        {
        }

        public Asset(long id, string code, string name)
        {
            Id = id;
            Code = code;
            Name = name;
        }
    }
}

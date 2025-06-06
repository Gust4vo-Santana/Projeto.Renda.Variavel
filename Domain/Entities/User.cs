namespace Domain.Entities
{
    public class User
    {
        public long Id { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public decimal BrokerageFee { get; private set; }
        public IEnumerable<Position> Positions { get; set; }
        public IEnumerable<Operation> Operations { get; set; }

        public User()
        {
        }

        public User(long id, string name, string email, decimal brokerageFee)
        {
            Id = id;
            Name = name;
            Email = email;
            BrokerageFee = brokerageFee;
        }
    }
}

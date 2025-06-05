namespace Domain.Entities
{
    public class Asset
    {
        public long Id { get; private set; }
        public string Code { get; private set; }
        public string Name { get; private set; }

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

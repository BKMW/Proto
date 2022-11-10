namespace API6.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public Address Address { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is User user &&
                   Name == user.Name &&
                   Email == user.Email &&
                   EqualityComparer<Address>.Default.Equals(Address, user.Address);
        }

        public override string? ToString()
        {
            return base.ToString();
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Email, Address);
        }
    }
}

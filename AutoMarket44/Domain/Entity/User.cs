using AutoMarket44.Domain.Enum;
using System.Data;

namespace AutoMarket44.Domain.Entity
{
    public class User
    {
        public long Id { get; set; }

        public string Password { get; set; }

        public string Name { get; set; }

        public Role Role { get; set; }

        public Profile Profile { get; set; }

        public Basket Basket { get; set; }
    }
}

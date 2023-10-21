using AutoMarket44.Domain.Enum;

namespace AutoMarket44.Domain.Entity
{
    public class Car
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Model { get; set; }

        public double Speed { get; set; }

        public decimal Price { get; set; }

        public DateTime DateCreate { get; set; }

        public TypeCar TypeCar { get; set; }

        //public byte[]? Avatar { get; set; }
    }
}

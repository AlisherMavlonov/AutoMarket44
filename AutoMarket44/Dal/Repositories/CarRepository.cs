using AutoMarket44.Dal.Interfaces;
using AutoMarket44.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace AutoMarket44.Dal.Repositories
{
    public class CarRepository : IBaseRepository<Car>
    {
        private readonly ApplicationDbContext db;

        public CarRepository(ApplicationDbContext context)
        {
            db = context;
        }

        public async Task Create(Car entity)
        {
            await db.Cars.AddAsync(entity);
            await db.SaveChangesAsync();
        }

        public async Task Delete(long id)
        {
            var entity = await db.Cars.FirstOrDefaultAsync(x => x.Id == id);
            db.Cars.Remove(entity);
            await db.SaveChangesAsync();
        }

        public IQueryable<Car> GetAll()
        {
            return db.Cars;
        }

        public async Task<Car> Update(Car entity)
        {
            var car = await db.Cars.FirstOrDefaultAsync(x=>x.Id == entity.Id);
            car.Description = entity.Description;
            car.Price = entity.Price;
            car.TypeCar = entity.TypeCar;
            car.Model = entity.Model;
            car.Name = entity.Name;
            car.Speed = entity.Speed;
            await db.SaveChangesAsync();

            return entity;
        }
    }
}

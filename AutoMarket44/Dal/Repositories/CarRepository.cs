using AutoMarket44.Dal.Interfaces;
using AutoMarket44.Domain.Entity;

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

        public async Task Delete(Car entity)
        {
            db.Cars.Remove(entity);
            await db.SaveChangesAsync();
        }

        public IQueryable<Car> GetAll()
        {
            return db.Cars;
        }

        public async Task<Car> Update(Car entity)
        {
            db.Cars.Update(entity);
            await db.SaveChangesAsync();

            return entity;
        }
    }
}

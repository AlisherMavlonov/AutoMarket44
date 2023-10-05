using AutoMarket44.Dal.Interfaces;
using AutoMarket44.Domain.Entity;

namespace AutoMarket44.Dal.Repositories
{
    public class BasketRepository : IBaseRepository<Basket>
    {
        private readonly ApplicationDbContext db;

        public BasketRepository(ApplicationDbContext context)
        {
            db = context;
        }

        public async Task Create(Basket entity)
        {
            await db.Baskets.AddAsync(entity);
            await db.SaveChangesAsync();
        }

        public async Task Delete(Basket entity)
        {
            db.Baskets.Remove(entity);
            await db.SaveChangesAsync();
        }

        public IQueryable<Basket> GetAll()
        {
            return db.Baskets;
        }

        public async Task<Basket> Update(Basket entity)
        {
            db.Baskets.Update(entity);
            await db.SaveChangesAsync();

            return entity;
        }
    }
}

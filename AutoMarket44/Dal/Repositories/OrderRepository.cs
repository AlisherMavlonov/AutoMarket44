using AutoMarket44.Dal.Interfaces;
using AutoMarket44.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace AutoMarket44.Dal.Repositories
{
    public class OrderRepository : IBaseRepository<Order>
    {
        private readonly ApplicationDbContext db;

        public OrderRepository(ApplicationDbContext context)
        {
            db = context;
        }

        public async Task Create(Order entity)
        {
            await db.Orders.AddAsync(entity);
            await db.SaveChangesAsync();
        }

        public async Task Delete(long id)
        {
            var order = await db.Orders.FirstOrDefaultAsync(x=>x.Id == id);
            db.Orders.Remove(order);
            await db.SaveChangesAsync();
        }

        public IQueryable<Order> GetAll()
        {
            return db.Orders;
        }

        public async Task<Order> Update(Order entity)
        {
            db.Orders.Update(entity);
            await db.SaveChangesAsync();

            return entity;
        }
    }
}

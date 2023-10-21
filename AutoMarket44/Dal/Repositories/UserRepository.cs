using AutoMarket44.Dal.Interfaces;
using AutoMarket44.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace AutoMarket44.Dal.Repositories
{
    public class UserRepository : IBaseRepository<User>
    {
        private readonly ApplicationDbContext db;

        public UserRepository(ApplicationDbContext context)
        {
            db = context;
        }

        public async Task Create(User entity)
        {
            await db.Users.AddAsync(entity);
            await db.SaveChangesAsync();
        }

        public async Task Delete(long id)
        {
            var entity = await db.Users.FirstOrDefaultAsync(x=>x.Id == id);
            db.Users.Remove(entity);
            await db.SaveChangesAsync();
        }

        public IQueryable<User> GetAll()
        {
            return db.Users;
        }

        public async Task<User> Update(User entity)
        {
            db.Users.Update(entity);
            await db.SaveChangesAsync();

            return entity;
        }
    }
}

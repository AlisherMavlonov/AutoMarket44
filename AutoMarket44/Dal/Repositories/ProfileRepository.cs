﻿using AutoMarket44.Dal.Interfaces;
using AutoMarket44.Domain.Entity;

namespace AutoMarket44.Dal.Repositories
{
    public class ProfileRepository : IBaseRepository<Profile>
    {
        private readonly ApplicationDbContext db;
        public ProfileRepository(ApplicationDbContext context)
        {
            db = context;
        }

        public async Task Create(Profile entity)
        {
            await db.Profiles.AddAsync(entity);
            await db.SaveChangesAsync();
        }

        public async Task Delete(Profile entity)
        {
            db.Profiles.Remove(entity);
            await db.SaveChangesAsync();
        }

        public IQueryable<Profile> GetAll()
        {
            return db.Profiles;
        }

        public async Task<Profile> Update(Profile entity)
        {
            db.Profiles.Update(entity);
            await db.SaveChangesAsync();

            return entity;
        }
    }
}

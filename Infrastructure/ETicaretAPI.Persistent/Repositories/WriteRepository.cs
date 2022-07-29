using ETicaretAPI.Application.Repositories;
using ETicaretAPI.Domain.Entities.Common;
using ETicaretAPI.Persistent.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Persistent.Repositories
{
    public class WriteRepository<T> : IWriteRepository<T> where T : BaseEntity
    {
  
        private readonly ETicaretAPIDbContext _context;

        public WriteRepository(ETicaretAPIDbContext context)
        {
            _context = context;
        }

        public DbSet<T> Table => _context.Set<T>();

        public async Task<bool> AddAsync(T model)
        {
            EntityEntry<T> entityEntry = await Table.AddAsync(model);
           await SaveAsync();
            return entityEntry.State == EntityState.Added;
        }

        public async Task<bool> AddRangeAsync(List<T> models)
        {
            
             await Table.AddRangeAsync(models); //void
          await  SaveAsync();
            return true;
        }

        public bool Remove(T model)
        {
            EntityEntry<T> entityEntry =Table.Remove(model);
            SaveAsync();
            return entityEntry.State == EntityState.Deleted;
        }

        public async Task<bool> RemoveAsync(string id) //asenkron çünkü
        {
            var query = Table.AsQueryable();
            T model = await query.FirstOrDefaultAsync(p => p.Id == Guid.Parse(id));
            if (model != null)
                Remove(model); //üstteki remove fonksiyonumuz
            await SaveAsync();
            if (model == null)
                return false;

            else
                return true;
        }

        public bool RemoveRange(List<T> models)
        {
            Table.RemoveRange(models);
            SaveAsync();
            return true;
        }

        public bool Update(T model)
        {
           EntityEntry entityentry= Table.Update(model);
            SaveAsync();
            return entityentry.State == EntityState.Modified;
        }
        public Task<int> SaveAsync()
       => _context.SaveChangesAsync();

    }
}

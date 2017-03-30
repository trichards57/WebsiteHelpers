using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebsiteHelpers.Exceptions;
using WebsiteHelpers.Interfaces;

namespace WebsiteHelpers
{
    public abstract class ItemService<TDbContext, TModel> : IItemService<TModel>
        where TModel : class, IIdentifiable
        where TDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ItemService{T}"/> class.
        /// </summary>
        /// <param name="database">The database.</param>
        public ItemService(TDbContext database)
        {
            Database = database;
        }

        protected TDbContext Database { get; }

        public virtual async Task<int> AddAsync(TModel item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            Database.Add(item);

            await Database.SaveChangesAsync();

            return item.Id;
        }

        public async Task DeleteAsync(int id)
        {
            var item = await Database.FindAsync<TModel>(id);
            if (item != null)
            {
                Database.Remove(item);
                await Database.SaveChangesAsync();
            }
        }

        public abstract Task<IEnumerable<TModel>> GetAllAsync();

        public virtual async Task<TModel> GetAsync(int id)
        {
            return await Database.FindAsync<TModel>(id);
        }

        public async Task UpdateAsync(TModel item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            try
            {
                Database.Update(item);
                await Database.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new IdNotFoundException("No entity found to update.", ex);
            }
        }
    }
}

﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebsiteHelpers.Interfaces
{
    public interface IItemService<TModel>
    {
        Task<int> AddAsync(TModel item);

        Task DeleteAsync(int id);

        Task<IEnumerable<TModel>> GetAllAsync();

        Task<TModel> GetAsync(int id);

        Task UpdateAsync(TModel item);
    }
}

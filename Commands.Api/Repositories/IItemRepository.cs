using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Commands.Api.Entities;

namespace Commands.Api.Repositories
{
     public interface IItemRepository
    {
        Task<Item> GetItemAsync(Guid id);
        Task<Item> GetItemByNameAsync(string name);
        Task<IEnumerable<Item>> GetItemsAsync();
        Task CreateItemAsync(Item item);
        Task UpdateItemAsync(Item item);
        Task DeleteItemAsync(Guid id);
    }

}
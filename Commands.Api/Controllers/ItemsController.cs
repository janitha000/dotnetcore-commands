using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Commands.Api.Dtos;
using Commands.Api.Entities;
using Commands.Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Commands.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ItemsController : ControllerBase
    {
        private readonly IItemRepository repository;
        private readonly ILogger<ItemsController> logger;
        public ItemsController(IItemRepository repository, ILogger<ItemsController> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<ItemDto>> GetItemsAsync() 
        {
            var items = await repository.GetItemsAsync();
            var itemDtos = items.Select( item => item.AsDto());
            logger.LogInformation($"{DateTime.UtcNow.ToString()} : Items retrieved");
            return itemDtos;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDto>> GetItemAsync(Guid id)
        {
            var item = await repository.GetItemAsync(id);
            
            if(item is null)
            {
                return NotFound();
            }

            return item.AsDto();
        }

        [HttpPost]
        public async  Task<ActionResult<ItemDto>> CreateItemAsync(CreateItemDto itemDto)
        {
            Item item = new() {
                Id = Guid.NewGuid(),
                Name = itemDto.Name,
                Price = itemDto.Price,
                createdDate = DateTimeOffset.UtcNow
            };
            await repository.CreateItemAsync(item);

            return CreatedAtAction(nameof(GetItemAsync), new {id = item.Id} , item.AsDto());
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateItemAsync(Guid id, UpdateDto itemDto)
        {
            var item = await repository.GetItemAsync(id);
            if(item is null)
            {
                logger.LogError($"{DateTime.UtcNow.ToString()}: Item retreival failed for GUID {id} ");
                return NotFound();
            }

            Item updatedItem = item with 
            {
                Name = itemDto.Name,
                Price = itemDto.Price
            };

            await repository.UpdateItemAsync(updatedItem);
            return NoContent();

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteItemAsync(Guid id)
        {
            var item = await repository.GetItemAsync(id);
            if(item is null)
            {
                return NotFound();
            }

            await repository.DeleteItemAsync(id);
            return NoContent();
        }

    }
}
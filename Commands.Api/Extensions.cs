using Commands.Api.Dtos;
using Commands.Api.Entities;

namespace Commands.Api
{
    public static class Extensions 
    {
        public static ItemDto AsDto(this Item item)
        {
            return new ItemDto{
                Id = item.Id,
                Name = item.Name,
                Price = item.Price,
                CreatedDate = item.createdDate
            };
        }
    }
}
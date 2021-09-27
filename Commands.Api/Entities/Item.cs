using System;

namespace Commands.Api.Entities
{
    public record Item 
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public decimal Price { get; init; }
        public DateTimeOffset createdDate { get; init; }
    }
}


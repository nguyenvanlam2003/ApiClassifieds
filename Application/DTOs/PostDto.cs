using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class PostDto
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public int? CategoryId { get; set; }
        public string Title { get; set; }
        public string? Address { get; set; }
        public decimal? Price { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public string? Status { get; set; }
        public int ViewCount { get; set; } 
         public List<AttributeDto>? Attributes { get; set; }
    }
    public class AttributeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public int PostId { get; set; }
    }
}

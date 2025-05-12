using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Post : BaseEntity
    {
        public int? UserId { get; set; }
        public int? CategoryId { get; set; }
        public string Title { get; set; }
        public string? Address { get; set; }
        public decimal? Price { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public string? Status { get; set; }
        public int ViewCount { get; set; } 
        public Category Category{ get; set; }
        public User User { get; set; }
    }
}

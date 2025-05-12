using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class PostReview : BaseEntity
    {
        public int PostId { get; set; }
        public int? UserId { get; set; } // mã user là admin
        public string Status { get; set; }
        public string? Comment { get; set; } 
        public Post Post { get; set; }
        public User User { get; set; }
    }
}

using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Attribute: BaseEntity
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public int PostId { get; set; }
        public Post Post { get; set; }
    }
}

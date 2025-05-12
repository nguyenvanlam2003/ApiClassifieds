using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int? ParentId { get; set; }

        // Nếu đây là category cha, thì phải có ít nhất 1 category con
        public List<CategoryChildDto>? Children { get; set; }
    }
    public class CategoryChildDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}

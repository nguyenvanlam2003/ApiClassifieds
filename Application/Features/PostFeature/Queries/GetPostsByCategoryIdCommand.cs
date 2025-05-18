using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.PostFeature.Queries
{
    class GetPostsByCategoryIdCommand :  IRequest<string>
    {
        public int CategoryId { get; set; }
        public int Page { get; set; } 
        public int PageSize { get; set; } 
        public string? SearchTerm { get; set; }
        public string? SortBy { get; set; }
        public string? SortDirection { get; set; } = "asc";
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookBazaar.Application.DTOs
{
    public class BookQueryDto
    {
        public bool? IsSold { get; set; }
        public string? Title { get; set; }
        public string? Author { get; set; }
        public string? Condition { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
    }
}

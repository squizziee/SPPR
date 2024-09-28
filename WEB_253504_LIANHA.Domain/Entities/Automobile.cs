using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEB_253504_LIANHA.Domain.Entities
{
    public class Automobile
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? CategoryId { get; set; }
        public double PriceInUsd { get; set; }
        public string? ImageUrl { get; set; }
    }
}

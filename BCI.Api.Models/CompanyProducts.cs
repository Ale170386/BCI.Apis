using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCI.Api.Models
{
    public class CompanyProducts
    {
        public Guid CompanyId { get; set; }
        public Company Company { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        [StringLength(1000)]
        public string Description { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCI.Api.Models
{
    [Table("Product")]
    public class Product
    {
        public int Id { get; set; }
        [StringLength(1000)]
        public string Name { get; set; } = string.Empty;
    }
}

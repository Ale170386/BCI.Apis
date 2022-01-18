using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCI.Api.Models
{
    public class SalesAmount
    {
        public int Id { get; set; }
        [StringLength(1000)]
        public string Name { get; set; }
    }
}

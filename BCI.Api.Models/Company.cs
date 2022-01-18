using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCI.Api.Models
{
    [Table("Company")]
    public class Company
    {
        public Guid Id { get; set; }
        [StringLength(100)]
        public string RegionId { get; set; } = string.Empty;
        [StringLength(100)]
        public string ComunaId { get; set; } = string.Empty;
        [StringLength(1000)]
        public string Address { get; set; } = string.Empty;
        public int SalesAmountId { get; set; }
        public SalesAmount  SalesAmount { get; set; } = new SalesAmount();
    }
}

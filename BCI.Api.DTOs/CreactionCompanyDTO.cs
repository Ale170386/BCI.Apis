using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCI.Api.DTOs
{
    public class CreactionCompanyDTO
    {
        public string RegionId { get; set; } = string.Empty;
        public string ComunaId { get; set; } = String.Empty;
        public string Address { get; set; } = String.Empty ;
        public int SalesAmountId { get; set; }
        public List<CreationProductDTO> Products { get; set; } = new List<CreationProductDTO>();
    }
}

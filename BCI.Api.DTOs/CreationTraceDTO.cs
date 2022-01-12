using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCI.Api.DTOs
{
    public class CreationTraceDTO
    {
        public Guid RequestId { get; set; }
        public int PageId { get; set; }
    }
}

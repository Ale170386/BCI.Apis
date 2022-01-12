using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCI.Api.DTOs
{
    public class CreactionPollDTO
    {
        public int Stars { get; set; }
        public string Opinion { get; set; } = string.Empty;
        public Guid RequestId { get; set; }
    }
}

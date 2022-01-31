using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCI.Api.Models
{
    public class ProcessLog
    {
        public Guid Id { get; set; }
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        public DateTime ProcessDate { get; set; }
        public bool Error { get; set; } = false;
        public string Message { get; set; } = string.Empty;
    }
}

using BCI.Api.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCI.Api.Models
{
    [Table("Trace")]
    public class Trace : IAuditable
    {
        public int Id { get; set; }
        public Guid RequestId { get; set; }
        public int PageId { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }
}

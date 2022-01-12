using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCI.Api.Models
{
    [Table("Poll")]
    public class Poll
    {
        public int Id { get; set; }
        public int Stars { get; set; }
        public string Opinion { get; set; } = string.Empty;
        public Guid RequestId { get; set; }
    }
}

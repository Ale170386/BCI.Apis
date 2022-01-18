using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCI.Api.Models
{
    [Table("Client")]
    public class Client
    {
        public Guid Id { get; set; }
        [StringLength(50)]
        public string RutClient { get; set; } = string.Empty;
        [StringLength(500)]
        public string Name { get; set; } = string.Empty;
        [StringLength(500)]
        public string LastName { get; set; } = string.Empty;
        [StringLength(20)]
        public string Phone { get; set; } = string.Empty;
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;
        [StringLength(500)]
        public string CompanyName { get; set; } = string.Empty;
        [StringLength(50)]
        public string CompanyRut { get; set; } = string.Empty;
        [StringLength(1000)]
        public string CompanyCategory { get; set; } = string.Empty;
        public int YearsOld { get; set; }
    }
}

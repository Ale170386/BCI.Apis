using BCI.Api.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCI.Api.Models
{
    [Table("ActivationRequest")]
    public class ActivationRequest : IAuditable
    {
        public Guid Id { get; set; }
        public Client Client { get; set; } = new Client();
        public Company Company { get; set; } = new Company();
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public bool Sent { get; set; }
    }
}

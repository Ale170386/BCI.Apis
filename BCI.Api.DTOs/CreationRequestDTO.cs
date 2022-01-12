using System.ComponentModel.DataAnnotations;

namespace BCI.Api.DTOs
{
    public class CreationRequestDTO
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public CreationClientDTO Client { get; set; } = new CreationClientDTO();
        [Required]
        public CreactionCompanyDTO Company { get; set; } = new CreactionCompanyDTO();
    }
}
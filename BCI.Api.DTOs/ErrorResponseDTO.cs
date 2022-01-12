using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCI.Api.DTOs
{
    public class ErrorResponseDTO
    {
        public IEnumerable<string> ErrorMessages { get; set; }
        public ErrorResponseDTO() : this(new List<string>()) { }
        public ErrorResponseDTO(string errorMessage) : this(new List<string>() { errorMessage }) { }

        public ErrorResponseDTO(IEnumerable<string> errorMessages)
        {
            ErrorMessages = errorMessages;
        }
    }
}

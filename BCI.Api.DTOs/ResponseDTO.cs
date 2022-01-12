using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCI.Api.DTOs
{
    public class ResponseDTO
    {
        public bool Succeeded { get; set; } = true;
        public ErrorResponseDTO ErrorResponse { get; set; } = new ErrorResponseDTO();
    }
}

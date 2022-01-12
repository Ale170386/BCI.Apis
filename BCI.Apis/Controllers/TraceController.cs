using BCI.Api.Business.BusinessTrace;
using BCI.Api.DTOs;
using BCI.Apis.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace BCI.Apis.Controllers
{
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiKey]
    public class TraceController : Controller
    {
        private readonly ITraceBL traceBL;

        public TraceController(ITraceBL traceBL)
        {
            this.traceBL = traceBL;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreationTraceDTO creationTraceDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestModelState();
            }

            var responseDTO = await traceBL.CreateTrace(creationTraceDTO);

            if (!responseDTO.Succeeded)
                return Conflict(responseDTO.ErrorResponse);

            return Ok();
        }

        private IActionResult BadRequestModelState()
        {
            IEnumerable<string> errorMessages = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));

            return BadRequest(new ErrorResponseDTO(errorMessages));
        }
    }
}

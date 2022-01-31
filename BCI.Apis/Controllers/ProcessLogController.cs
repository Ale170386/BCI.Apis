using BCI.Api.Business.BusinessProcessLog;
using BCI.Api.DTOs;
using BCI.Apis.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace BCI.Apis.Controllers
{
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiKey]
    public class ProcessLogController : Controller
    {
        private readonly IProcessLogBL processLogBL;

        public ProcessLogController(IProcessLogBL processLogBL)
        {
            this.processLogBL = processLogBL;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ProcessLogDTO processLogDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestModelState();
            }

            await processLogBL.RegisterLog(processLogDTO);

            return Ok();
        }

        private IActionResult BadRequestModelState()
        {
            IEnumerable<string> errorMessages = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));

            return BadRequest(new ErrorResponseDTO(errorMessages));
        }
    }
}

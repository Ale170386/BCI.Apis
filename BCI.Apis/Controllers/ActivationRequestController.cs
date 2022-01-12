using BCI.Api.Business.BusinessRequest;
using BCI.Api.DTOs;
using BCI.Apis.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace BCI.Apis.Controllers
{
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiKey]
    public class ActivationRequestController : Controller
    {
        private readonly IActivationRequestBL activationRequestBL;

        public ActivationRequestController(IActivationRequestBL activationRequestBL)
        {
            this.activationRequestBL = activationRequestBL;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreationRequestDTO creationRequestDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestModelState();
            }            

            var responseDTO = await activationRequestBL.CreateActivationRequest(creationRequestDTO);

            if (!responseDTO.Succeeded)
                return Conflict(responseDTO.ErrorResponse);

            return Ok();
        }

        [HttpPost("Poll")]
        public async Task<IActionResult> Post([FromBody] CreactionPollDTO creactionPollDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestModelState();
            }

            var responseDTO = await activationRequestBL.CreatePoll(creactionPollDTO);

            if (!responseDTO.Succeeded)
                return Conflict(responseDTO.ErrorResponse);

            return Ok();
        }

        [HttpGet("Products")]
        public async Task<List<ProductDTO>> GetProducts()
        {
            return await activationRequestBL.GetAllProducts();
        }

        [HttpGet("SalesAmount")]
        public async Task<List<SalesAmountDTO>> GetSalesAmount()
        {
            return await activationRequestBL.GetAllSalesAmount();
        }

        private IActionResult BadRequestModelState()
        {
            IEnumerable<string> errorMessages = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));

            return BadRequest(new ErrorResponseDTO(errorMessages));
        }
    }
}

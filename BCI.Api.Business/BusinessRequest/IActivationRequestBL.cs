using BCI.Api.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCI.Api.Business.BusinessRequest
{
    public interface IActivationRequestBL
    {
        public Task<ResponseDTO> CreateActivationRequest(CreationRequestDTO creationRequestDTO); 
        public Task<List<ProductDTO>> GetAllProducts();
        public Task<List<SalesAmountDTO>> GetAllSalesAmount();
        public Task<ResponseDTO> CreatePoll(CreactionPollDTO creactionPollDTO);
        public Task<ResponseDTO> CreateRequestCSV();
    }
}

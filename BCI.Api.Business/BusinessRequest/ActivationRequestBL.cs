using AutoMapper;
using BCI.Api.Data.DataRequest;
using BCI.Api.DTOs;
using BCI.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCI.Api.Business.BusinessRequest
{
    public class ActivationRequestBL : IActivationRequestBL
    {
        private readonly IActivationRequestDAL activationRequestDAL;
        private readonly IMapper mapper;

        public ActivationRequestBL(IActivationRequestDAL activationRequestDAL, IMapper mapper)
        {
            this.activationRequestDAL = activationRequestDAL;
            this.mapper = mapper;
        }

        public async Task<ResponseDTO> CreateActivationRequest(CreationRequestDTO creationRequestDTO)
        {
            ResponseDTO responseDTO = new ();
            try
            {
                var activationRequest = mapper.Map<ActivationRequest>(creationRequestDTO);
                activationRequest = await activationRequestDAL.CreateActivationRequest(activationRequest);

                List<CompanyProducts> companyProducts = (from product in creationRequestDTO.Company.Products
                                                        select new CompanyProducts
                                                        {
                                                            CompanyId = activationRequest.Company.Id,
                                                            ProductId = product.ProductId,
                                                            Description = product.Description
                                                        }).ToList();

                await activationRequestDAL.CreateCompanyProducts(companyProducts);
            }
            catch (Exception ex)
            {
                responseDTO.Succeeded = false;
                responseDTO.ErrorResponse = new ErrorResponseDTO($"Ha ocurrido un error al guardar la solicitud: {ex.Message}");
            }

            return responseDTO;
        }

        public async Task<ResponseDTO> CreatePoll(CreactionPollDTO creactionPollDTO)
        {
            ResponseDTO responseDTO = new();
            try
            {
                var poll = mapper.Map<Poll>(creactionPollDTO);
                await activationRequestDAL.CreatePoll(poll);

            }
            catch (Exception ex)
            {
                responseDTO.Succeeded = false;
                responseDTO.ErrorResponse = new ErrorResponseDTO($"Ha ocurrido un error al guardar la encuesta: {ex.Message}");
            }

            return responseDTO;
        }
        public async Task<List<ProductDTO>> GetAllProducts()
        {
            List<Product> products = await activationRequestDAL.GetAllProducts();
            return mapper.Map<List<ProductDTO>>(products);
        }

        public async Task<List<SalesAmountDTO>> GetAllSalesAmount()
        {
            List<SalesAmount> salesAmounts = await activationRequestDAL.GetAllSalesAmount();
            return mapper.Map<List<SalesAmountDTO>>(salesAmounts);
        }
    }
}

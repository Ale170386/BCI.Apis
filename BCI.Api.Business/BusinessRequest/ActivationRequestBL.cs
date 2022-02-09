using AutoMapper;
using BCI.Api.Business.BusinessProcessLog;
using BCI.Api.Business.Utilities;
using BCI.Api.Data.DataProcessLog;
using BCI.Api.Data.DataRequest;
using BCI.Api.DTOs;
using BCI.Api.Models;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace BCI.Api.Business.BusinessRequest
{
    public class ActivationRequestBL : IActivationRequestBL
    {
        private readonly IActivationRequestDAL activationRequestDAL;
        private readonly IMapper mapper;
        private readonly IFTP fTP;

        public ActivationRequestBL(IActivationRequestDAL activationRequestDAL, IMapper mapper, IFTP fTP)
        {
            this.activationRequestDAL = activationRequestDAL;
            this.mapper = mapper;
            this.fTP = fTP;
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

        /// <summary>
        /// Creates a comma delimeted string of all the objects property values names.
        /// </summary>
        /// <param name="obj">object.</param>
        /// <returns>string.</returns>
        private async Task<string> RequestToCsvData(List<ActivationRequest> requestList)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(String.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18}{19}",
                        "fecha_Carga",
                        "RutEmpresa",
                        "DV",
                        "NombreEmpresa",
                        "RutApoderado",
                        "DV_Apod",
                        "NombreApoderado",
                        "ApellidoApod",                        
                        "TelefonoApoderado",
                        "EmailApoderado",
                        "Rubro",
                        "Antiguedad",
                        "CodigoVentasAnuales",
                        "Region",
                        "Comuna",
                        "Direccion",
                        "CodigoInsignt",
                        "Otro",
                        "fechaEnvio",      
                        Environment.NewLine
                        ));


            foreach (ActivationRequest request in requestList)
            {
                List<CompanyProducts> products = await this.activationRequestDAL.GetProductsByCompanyId(request.Company.Id);

                sb.Append(String.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18}{19}",
                        request.Created.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                        request.Client.CompanyRut.Remove(request.Client.CompanyRut.Length - 1),
                        request.Client.CompanyRut[^1],
                        request.Client.CompanyName,
                        request.Client.RutClient.Remove(request.Client.RutClient.Length - 1),
                        request.Client.RutClient[^1],
                        request.Client.Name,
                        request.Client.LastName,
                        request.Client.Phone,
                        request.Client.Email,
                        request.Client.CompanyCategory,
                        request.Client.YearsOld,
                        request.Company.SalesAmountId,
                        request.Company.RegionId,
                        request.Company.ComunaId,
                        request.Company.Address,
                        String.Join(";", products.Select(s => s.ProductId).ToArray()),
                        products.Exists(e => e.Description != "") ? products.Where(w => w.Description != "").First().Description : "",
                        DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                        Environment.NewLine
                        ));
            }

            return sb.ToString();
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
        public async Task<ResponseDTO> CreateRequestCSV()
        {
            ResponseDTO responseDTO = new ResponseDTO();
            try
            {
                List<ActivationRequest> requestList = await this.activationRequestDAL.GetAllActivationRequests();

                if (requestList.Count > 0)
                {

                    string result = await this.RequestToCsvData(requestList);
                    string tempPath = $"{AppContext.BaseDirectory}temp\\Leads_Pyme_{DateTime.Now.ToString("yyyyMMdd")}.csv";

                    File.WriteAllText(tempPath, result.ToString());
                    fTP.UploadFtpSingleFile(tempPath);

                    //Actualizar lista obtenida marcando el campo Sent como true
                    await this.activationRequestDAL.UpdateSentRequests(requestList);
                }
                
                responseDTO.Succeeded = true;
                responseDTO.Message = $"Se generaron {requestList.Count} líneas en archivo csv";
                
            }
            catch (Exception er)
            {
                responseDTO.Succeeded = true;
                responseDTO.Message = $"Ocurrió un error en proceso de exportación de datos a CSV: { er.Message }";
            }

            return responseDTO;
        }
    }
}

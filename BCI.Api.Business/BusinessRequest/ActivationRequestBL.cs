using AutoMapper;
using BCI.Api.Business.Utilities;
using BCI.Api.Data.DataRequest;
using BCI.Api.DTOs;
using BCI.Api.Models;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
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

                string result = this.RequestToCsvData(activationRequest, companyProducts);
                string tempPath = $"{AppContext.BaseDirectory}temp\\{activationRequest.Id}.csv";

                File.WriteAllText(tempPath, result.ToString());
                fTP.UploadFtpSingleFile(tempPath);
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
        private string RequestToCsvData(ActivationRequest request, List<CompanyProducts> companyProducts)
        {
            if (request == null)
            {
                throw new ArgumentNullException("obj", "Value can not be null or Nothing!");
            }

            StringBuilder sb = new StringBuilder();
            sb.Append(String.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18}{19}",
                        "fecha_Carga",
                        "RutEmpresa",
                        "DV",
                        "NombreEmpresa",
                        "RutApoderado",
                        "DV_Apod",
                        "NombreApoderado",
                        "ApePatApod",
                        "ApeMatApod",
                        "TelefonoApoderado",
                        "EmailApoderado",
                        "Rubro",
                        "Antiguedad",
                        "CodigoVentasAnuales",
                        "Region",
                        "Comuna",
                        "Direccion",
                        "CodigoInsignt",
                        "fechaEnvio",
                        Environment.NewLine
                        ));


            sb.Append(String.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18}{19}",
                        request.Created,
                        request.Client.CompanyRut.Remove(request.Client.CompanyRut.Length - 1),
                        request.Client.CompanyRut[request.Client.CompanyRut.Length - 1],
                        request.Client.CompanyName,
                        "",//Rut Apoderado
                        "",//DV Rut Apoderado
                        request.Client.Name,
                        request.Client.LastName,
                        "",//Apellido Materno
                        request.Client.Phone,
                        request.Client.Email,
                        request.Client.CompanyCategory,
                        request.Client.YearsOld,
                        request.Company.SalesAmountId,
                        request.Company.RegionId,
                        request.Company.ComunaId,
                        request.Company.Address,
                        String.Join("\"", companyProducts.Select(s => $"{s.ProductId}:{s.Description}").ToArray()),
                        DateTime.Now,
                        Environment.NewLine
                        ));

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
    }
}

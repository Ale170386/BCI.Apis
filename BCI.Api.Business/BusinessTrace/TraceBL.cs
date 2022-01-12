using AutoMapper;
using BCI.Api.Data.DataTrace;
using BCI.Api.DTOs;
using BCI.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCI.Api.Business.BusinessTrace
{
    public class TraceBL : ITraceBL
    {
        private readonly ITraceDAL traceDAL;
        private readonly IMapper mapper;

        public TraceBL(ITraceDAL traceDAL, IMapper mapper)
        {
            this.traceDAL = traceDAL;
            this.mapper = mapper;
        }

        public async Task<ResponseDTO> CreateTrace(CreationTraceDTO creationTraceDTO)
        {
            ResponseDTO responseDTO = new();
            try
            {
                var trace = mapper.Map<Trace>(creationTraceDTO);
                await traceDAL.RegisterTrace(trace);
            }
            catch (Exception ex)
            {
                responseDTO.Succeeded = false;
                responseDTO.ErrorResponse = new ErrorResponseDTO($"Ha ocurrido un error al guardar la solicitud: {ex.Message}");
            }
            

            return responseDTO;
        }
    }
}

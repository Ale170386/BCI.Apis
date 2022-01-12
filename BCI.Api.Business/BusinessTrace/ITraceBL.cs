using BCI.Api.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCI.Api.Business.BusinessTrace
{
    public interface ITraceBL
    {
        public Task<ResponseDTO> CreateTrace(CreationTraceDTO creationTraceDTO);
    }
}

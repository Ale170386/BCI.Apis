using AutoMapper;
using BCI.Api.Data.DataProcessLog;
using BCI.Api.DTOs;
using BCI.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCI.Api.Business.BusinessProcessLog
{
    public class ProcessLogBL : IProcessLogBL
    {
        private readonly IProcessLogDAL processLogDAL;
        private readonly IMapper mapper;

        public ProcessLogBL(IProcessLogDAL processLogDAL, IMapper mapper)
        {
            this.processLogDAL = processLogDAL;
            this.mapper = mapper;
        }

        public Task RegisterLog(ProcessLogDTO processLogDTO)
        {
            ProcessLog processLog = this.mapper.Map<ProcessLog>(processLogDTO);
            this.processLogDAL.RegisterLog(processLog);
            return Task.CompletedTask;
        }
    }
}

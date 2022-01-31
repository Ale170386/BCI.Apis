using BCI.Api.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCI.Api.Business.BusinessProcessLog
{
    public interface IProcessLogBL
    {
        public Task RegisterLog(ProcessLogDTO processLogDTO);
    }
}

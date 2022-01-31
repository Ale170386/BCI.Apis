using BCI.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCI.Api.Data.DataProcessLog
{
    public interface IProcessLogDAL
    {
        public Task RegisterLog(ProcessLog processLog);
    }
}

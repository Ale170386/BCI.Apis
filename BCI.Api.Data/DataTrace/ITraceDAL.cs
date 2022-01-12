using BCI.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCI.Api.Data.DataTrace
{
    public interface ITraceDAL
    {
        public Task RegisterTrace(Models.Trace trace);
    }
}

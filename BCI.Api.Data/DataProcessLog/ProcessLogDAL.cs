using BCI.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCI.Api.Data.DataProcessLog
{
    public class ProcessLogDAL : IProcessLogDAL
    {
        private readonly ApplicationDbContext applicationDbContext;

        public ProcessLogDAL(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        public async Task RegisterLog(ProcessLog processLog)
        {
            this.applicationDbContext.ProcessLog.Add(processLog);
            await this.applicationDbContext.SaveChangesAsync();
        }
    }
}

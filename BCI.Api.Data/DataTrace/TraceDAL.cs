using BCI.Api.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCI.Api.Data.DataTrace
{
    public class TraceDAL : ITraceDAL
    {
        private readonly ApplicationDbContext applicationDbContext;

        public TraceDAL(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        public async Task RegisterTrace(Trace trace)
        {
            IEnumerable<Trace> traces = await this.applicationDbContext.Trace
                                                .Where(w => w.RequestId == trace.RequestId
                                                            && w.PageId == trace.PageId)
                                                .ToListAsync();
            if (!traces.Any())
            {
                applicationDbContext.Trace.Add(trace);
                await applicationDbContext.SaveChangesAsync();
            }
        }
    }
}

using BCI.Api.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCI.Api.Data.DataRequest
{
    public class ActivationRequestDAL : IActivationRequestDAL
    {
        private readonly ApplicationDbContext applicationDbContext;
        private readonly string processName = "CSV Process";

        public ActivationRequestDAL(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        public async Task<ActivationRequest> CreateActivationRequest(ActivationRequest activationRequestDTO)
        {
            activationRequestDTO.Company.SalesAmount = applicationDbContext.SalesAmount
                                                            .Where(s => s.Id == activationRequestDTO.Company.SalesAmountId)
                                                            .First();

            applicationDbContext.ActivationRequest.Add(activationRequestDTO);
            await applicationDbContext.SaveChangesAsync();
            return activationRequestDTO;
        }

        public async Task CreateCompanyProducts(List<CompanyProducts> companyProducts)
        {
            applicationDbContext.CompanyProducts.AddRange(companyProducts);
            await applicationDbContext.SaveChangesAsync();
        }

        public async Task CreatePoll(Poll poll)
        {
            applicationDbContext.Poll.Add(poll);
            await applicationDbContext.SaveChangesAsync();
        }

        public async Task<List<ActivationRequest>> GetAllActivationRequests()
        {
            try
            {
                return await applicationDbContext.ActivationRequest.Include(c => c.Client).Include(c => c.Company).Where(w => !w.Sent).ToListAsync();
            }
            catch (Exception e)
            {
                throw;
            }            
        }

        public async Task<List<Product>> GetAllProducts()
        {
            return await applicationDbContext.Products.ToListAsync();
        }

        public async Task<List<SalesAmount>> GetAllSalesAmount()
        {
            return await applicationDbContext.SalesAmount.ToListAsync();
        }

        public async Task<List<CompanyProducts>> GetProductsByCompanyId(Guid companyId)
        {
            return await applicationDbContext.CompanyProducts.Where(w => w.CompanyId == companyId).ToListAsync();
        }

        public Task UpdateSentRequests(List<ActivationRequest> activationRequests)
        {
            activationRequests.Select(c => { c.Sent = true; return c; }).ToList();
            
            applicationDbContext.SaveChanges();

            return Task.CompletedTask;
        }

        
    }
}

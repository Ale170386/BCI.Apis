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

        public async Task<List<Product>> GetAllProducts()
        {
            return await applicationDbContext.Products.ToListAsync();
        }

        public async Task<List<SalesAmount>> GetAllSalesAmount()
        {
            return await applicationDbContext.SalesAmount.ToListAsync();
        }
    }
}

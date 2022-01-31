using BCI.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCI.Api.Data.DataRequest
{
    public interface IActivationRequestDAL
    {
        public Task<ActivationRequest> CreateActivationRequest(ActivationRequest activationRequestDTO);
        public Task CreateCompanyProducts(List<CompanyProducts> companyProducts);
        public Task CreatePoll(Poll poll);
        public Task<List<Product>> GetAllProducts();
        public Task<List<SalesAmount>> GetAllSalesAmount();
        public Task<List<ActivationRequest>> GetAllActivationRequests();
        public Task<List<CompanyProducts>> GetProductsByCompanyId(Guid companyId);
        public Task UpdateSentRequests(List<ActivationRequest> activationRequests);
    }
}

using BCI.Api.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCI.Api.Business.BusinessRequest
{
    public interface ICredentials
    {
        public FtpCredentialDto GetFtpCredencialsSection(string section);
    }
}

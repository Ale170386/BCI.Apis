using BCI.Api.DTOs;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCI.Api.Business.BusinessRequest
{
    public class Credentials : ICredentials
    {
        private readonly IConfiguration configuration;

        public Credentials(IConfiguration configuration)
        {
            this.configuration = configuration;
        }        

        public FtpCredentialDto GetFtpCredencialsSection(string section)
        {
            IConfigurationSection configurationSection = configuration.GetSection(section);
            return new FtpCredentialDto
            {
                DownloadDirectory = configurationSection.GetSection("downloadDirectory").Value,
                UploadDirectory = configurationSection.GetSection("uploadDirectory").Value,
                User = configurationSection.GetSection("user").Value,
                Password = configurationSection.GetSection("password").Value,
                Server = configurationSection.GetSection("server").Value,
                Port = configurationSection.GetSection("port").Value != "" ? Convert.ToInt32(configurationSection.GetSection("port").Value) : 0
            };
        }
    }
}

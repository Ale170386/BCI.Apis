using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCI.Api.Business.BusinessRequest
{
    public interface IFTP
    {
        public string UploadSFtpSingleFile(string filePath);
        public string UploadFtpSingleFile(string filePath);
    }
}

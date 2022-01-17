using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCI.Api.DTOs
{
    /// <summary>
    /// Dto credenciales Ftp
    /// </summary>
    public class FtpCredentialDto
    {
        /// <summary>
        /// 
        /// </summary>
        public int Port { get; set; }
        /// <summary>
        /// Dominio
        /// </summary>
        public string Server { get; set; }
        /// <summary>
        /// Usuario
        /// </summary>
        public string User { get; set; }
        /// <summary>
        /// Contraseña
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// Ruta de bajada
        /// </summary>
        public string DownloadDirectory { get; set; }
        /// <summary>
        /// Ruta de subida
        /// </summary>
        public string UploadDirectory { get; set; }

    }
}

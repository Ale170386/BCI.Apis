using BCI.Api.Business.Utilities;
using BCI.Apis.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BCI.Apis.Controllers
{
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiKey]
    public class EncryptionController : ControllerBase
    {
        /// <summary>
        /// Encriptar 
        /// </summary>
        /// <param name="text">Texto a encriptar</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("Encript"), MapToApiVersion("1.0")]
        public string Encript([Required] string text)
        {
            return EncryptionHelper.Encrypt(text);
        }
        /// <summary>
        /// Desencriptar 
        /// </summary>
        /// <param name="text">Texto a desencriptar</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("Decrypt"), MapToApiVersion("1.0")]
        public string Decrypt([Required] string text)
        {
            return EncryptionHelper.Decrypt(text);
        }
    }
}

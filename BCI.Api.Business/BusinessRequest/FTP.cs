using BCI.Api.Business.Utilities;
using BCI.Api.DTOs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BCI.Api.Business.BusinessRequest
{
    public class FTP : IFTP
    {        
        private readonly ICredentials credentials;        

        public FTP(ICredentials credentials)
        {
            this.credentials = credentials;            
        }

        public string UploadSFtpSingleFile(string filePath)
        {
            FtpCredentialDto credential = credentials.GetFtpCredencialsSection("FTP");
            List<string> filesToDelete = new List<string>();
            try
            {

                if (credential.Port == 0)
                {
                    using (SftpClient sftp = new SftpClient(credential.Server, credential.User, EncryptionHelper.Decrypt(credential.Password)))
                    {
                        sftp.Connect();
                        //sftp.CreateDirectory(credential.UploadDirectory.ToString(credential.UploadDirectory));
                        sftp.ChangeDirectory(credential.UploadDirectory);
                        using (var fileStream = new FileStream(filePath, FileMode.Open))
                        {
                            sftp.BufferSize = 4 * 1024;
                            sftp.UploadFile(fileStream, Path.GetFileName(filePath));
                        }

                        string tempDirectory = Path.Combine(Path.GetDirectoryName(filePath), "temp");
                        if (Directory.Exists(tempDirectory) == false)
                            Directory.CreateDirectory(tempDirectory);

                        File.Copy(filePath, Path.Combine(Path.GetDirectoryName(filePath), "temp", Path.GetFileName(filePath)));
                        File.Delete(filePath);
                    }
                }
                else
                {
                    using (SftpClient sftp = new SftpClient(credential.Server, credential.Port, credential.User, EncryptionHelper.Decrypt(credential.Password)))
                    {
                        sftp.Connect();
                        sftp.ChangeDirectory(credential.UploadDirectory);
                        using (var fileStream = new FileStream(filePath, FileMode.Open))
                        {
                            sftp.BufferSize = 4 * 1024;
                            sftp.UploadFile(fileStream, Path.GetFileName(filePath));
                        }

                        string tempDirectory = Path.Combine(Path.GetDirectoryName(filePath), "temp");
                        if (Directory.Exists(tempDirectory) == false)
                            Directory.CreateDirectory(tempDirectory);

                        File.Copy(filePath, Path.Combine(Path.GetDirectoryName(filePath), "temp", Path.GetFileName(filePath)));
                        File.Delete(filePath);
                    }
                }
                return "Ok";
            }
            catch (Exception ex)
            {
                return $"Error al subir archivos al Sftp: {ex.Message}";
            }
        }        

        public string UploadFtpSingleFile(string filePath)
        {
            String result = "OK";
            try
            {                
                FtpCredentialDto credential = credentials.GetFtpCredencialsSection("FTP");

                //Crea directorio según año/mes
                string ftpPath = this.CreatePathDirectory(credential);

                /* Create an FTP Request */
                FtpWebRequest ftpRequest = (FtpWebRequest)FtpWebRequest.Create($"{ftpPath}{Path.GetFileName(filePath)}");
                /* Log in to the FTP Server with the User Name and Password Provided */
                ftpRequest.Credentials = new NetworkCredential(credential.User, EncryptionHelper.Decrypt(credential.Password));
                /* When in doubt, use these options */
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = true;
                ftpRequest.KeepAlive = true;
                /* Specify the Type of FTP Request */
                ftpRequest.Method = WebRequestMethods.Ftp.UploadFile;
                /* Establish Return Communication with the FTP Server */
                Stream ftpStream = ftpRequest.GetRequestStream();
                /* Open a File Stream to Read the File for Upload */
                FileStream localFileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Read);// 這邊要加 ：  FileAccess.Read
                /* Buffer for the Downloaded Data */
                const int bufferSize = 2048;
                byte[] byteBuffer = new byte[bufferSize];
                int bytesSent = localFileStream.Read(byteBuffer, 0, bufferSize);
                
                /* Upload the File by Sending the Buffered Data Until the Transfer is Complete */
                try
                {
                    while (bytesSent != 0)
                    {
                        ftpStream.Write(byteBuffer, 0, bytesSent);
                        bytesSent = localFileStream.Read(byteBuffer, 0, bufferSize);
                    }
                }
                catch (Exception ex)
                {
                    result = "Error al subir archivo: " + ex.Message;
                    Console.WriteLine(ex.ToString());
                }
                /* Resource Cleanup */
                localFileStream.Close();
                ftpStream.Close();
                ftpRequest = null;
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); result = "Error al subir archivo: " + ex.Message; }
            return result;
        }

        private string CreatePathDirectory(FtpCredentialDto credential)
        {
            String ftpPath = String.Empty;
            try
            {
                //Crea directorio de Año en FTP
                ftpPath = $"FTP://{credential.Server}{credential.UploadDirectory}{DateTime.Now.ToString("yyyy")}/";
                WebRequest request = WebRequest.Create(ftpPath);
                request.Method = WebRequestMethods.Ftp.MakeDirectory;
                request.Credentials = new NetworkCredential(credential.User, EncryptionHelper.Decrypt(credential.Password));
                using (var resp = (FtpWebResponse)request.GetResponse())
                {
                    //Crea directorio de mes en FTP
                    ftpPath += $"{DateTime.Now.ToString("MMMM")}/";
                    request = WebRequest.Create(ftpPath);
                    request.Method = WebRequestMethods.Ftp.MakeDirectory;
                    request.Credentials = new NetworkCredential(credential.User, EncryptionHelper.Decrypt(credential.Password));
                    using (var resp2 = (FtpWebResponse)request.GetResponse())
                    {
                        if (!(resp2.StatusCode == FtpStatusCode.PathnameCreated))
                            return "ERROR";
                    }
                }
                request = null;
                return ftpPath;
            }
            catch (WebException ex)
            {
                FtpWebResponse response = (FtpWebResponse)ex.Response;
                if (response.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
                {
                    response.Close();

                    try
                    {
                        //Crea directorio de mes en FTP
                        ftpPath += $"{DateTime.Now.ToString("MMMM")}/";
                        WebRequest request = WebRequest.Create(ftpPath);
                        request.Method = WebRequestMethods.Ftp.MakeDirectory;
                        request.Credentials = new NetworkCredential(credential.User, EncryptionHelper.Decrypt(credential.Password));
                        using (var resp2 = (FtpWebResponse)request.GetResponse())
                        {
                            if (!(resp2.StatusCode == FtpStatusCode.PathnameCreated))
                                return "Error";
                        }

                        return ftpPath;
                    }
                    catch (WebException er)
                    {
                        FtpWebResponse errorResponse = (FtpWebResponse)er.Response;
                        if (errorResponse.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
                        {
                            errorResponse.Close();
                            return ftpPath;
                        }
                        else
                        {
                            errorResponse.Close();
                            return "ERROR";
                        }
                    }

                    
                }
                else
                {
                    response.Close();
                    return "ERROR";
                }
            }                        
        }
    }
}

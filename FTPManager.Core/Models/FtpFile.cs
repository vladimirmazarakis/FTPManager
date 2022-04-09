using FTPManager.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Text;
using System.Threading.Tasks;

namespace FTPManager.Core.Models
{
    public class FtpFile : IFtpSavable
    {
        public string FullName { get; set; }
        public string Name { get; set; }
        public string Extension { get; set; }
        public string ServerPath { get; set; }

        public FtpFile(string fullName, string name, string extension, string serverPath)
        {
            FullName = fullName;
            Name = name;
            Extension = extension;
            ServerPath = serverPath;
        }

        public async Task Save(FtpClientBase ftpClient, string path) 
        {
            string fullPathToGetFile = ftpClient.BaseAddress + ServerPath;
            Console.WriteLine(fullPathToGetFile);
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(fullPathToGetFile);
                request.CachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.CacheIfAvailable);
                request.Method = WebRequestMethods.Ftp.DownloadFile;
                request.Credentials = new NetworkCredential(ftpClient.Credentials.Username, ftpClient.Credentials.Password);
                request.KeepAlive = false;
                request.UseBinary = true;
                request.UsePassive = true;
                FtpWebResponse response = (FtpWebResponse)await request.GetResponseAsync();
                using (var responseStream = response.GetResponseStream()) 
                {
                    using (var fileStream = File.Create(path)) 
                    {
                        await responseStream.CopyToAsync(fileStream);
                        Console.WriteLine("Download Complete", response.StatusDescription);
                    }
                }
            }
            catch (WebException e)
            {
                Console.WriteLine(e.Message.ToString());
                String status = ((FtpWebResponse)e.Response).StatusDescription;
                Console.WriteLine(status);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }
        }
    }
}

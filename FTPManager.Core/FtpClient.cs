using FTPManager.Core.Abstractions;
using FTPManager.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FTPManager.Core
{
    public class FtpClient : FtpClientBase
    {
        private string _baseAddress = "";
        private FtpCredentials _credentials;

        public FtpClient(string baseAddress, FtpCredentials ftpCredentials) : base(baseAddress, ftpCredentials)
        {
        }

        public override string BaseAddress { get { return _baseAddress; } set { _baseAddress = value; } }

        public override FtpCredentials Credentials { get { return _credentials; } set { _credentials = value; } }

        public override FtpDirectory GetDirectory(string serverPath)
        {
            FtpDirectory directory = null;
            string newServerPath;
            if (string.IsNullOrEmpty(serverPath)) 
            {
                newServerPath = "";
                directory = new FtpDirectory("host", newServerPath);
            }
            else
            {
                try
                {
                    newServerPath = serverPath.Replace(_baseAddress, "");
                    string directoryName = newServerPath.Substring(newServerPath.LastIndexOf('/'));
                    directory = new FtpDirectory(directoryName, newServerPath);
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            List<FtpFile> filesFound = new List<FtpFile>();
            List<FtpDirectory> directoriesFound = new List<FtpDirectory>();
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(_baseAddress + serverPath);
                request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                request.Credentials = new NetworkCredential(_credentials.Username, _credentials.Password);
                request.KeepAlive = false;
                request.UseBinary = true;
                request.UsePassive = true;
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
               
                using (var stream = response.GetResponseStream())
                {
                    using (var reader = new StreamReader(stream))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            string[] newLine = line.Split(' ');
                            if (newLine.Any(str => str.Contains("<DIR>")))
                            {
                                string directoryOnServerPath = serverPath + "/" + newLine.LastOrDefault();
                                var tempDirectory = new FtpDirectory(newLine.LastOrDefault(), directoryOnServerPath);
                                directoriesFound.Add(tempDirectory);
                            }
                            else
                            {
                                int indexOfLastDot = newLine.LastOrDefault().LastIndexOf('.');
                                string fileName = newLine.LastOrDefault().Substring(0, indexOfLastDot);
                                string fileExtension = newLine.LastOrDefault().Substring(indexOfLastDot);
                                string fileOnServerPath = serverPath + "/" + newLine.LastOrDefault();
                                var tempFile = new FtpFile(newLine.LastOrDefault(), fileName, fileExtension, fileOnServerPath);
                                filesFound.Add(tempFile);
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            directory.Directories = directoriesFound;
            directory.Files = filesFound;
            return directory;
        }
        public override FtpFile GetFile(string serverPath)
        {
            if (string.IsNullOrEmpty(serverPath)) 
            {
                return null;
            }
            FtpFile file = null;
            try
            {
                string fileFullName = serverPath.Substring(serverPath.LastIndexOf('/'));
                string fileName = fileFullName.Substring(0, fileFullName.LastIndexOf('.'));
                string fileExtension = fileFullName.Substring(fileFullName.LastIndexOf('.'));
                file = new FtpFile(fileFullName, fileName, fileExtension, serverPath);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(_baseAddress + serverPath);
            request.Method = WebRequestMethods.Ftp.GetFileSize;
            request.Credentials = new NetworkCredential(_credentials.Username, _credentials.Password);
            request.KeepAlive = false;
            request.UseBinary = true;
            request.UsePassive = true;
            try
            {
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.Message);
            }
            return file;
        }
    }
}

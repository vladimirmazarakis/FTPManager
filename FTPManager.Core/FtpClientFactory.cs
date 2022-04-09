using FTPManager.Core.Abstractions;
using FTPManager.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPManager.Core
{
    public class FtpClientFactory : FtpClientFactoryBase
    {
        public override FtpClientBase CreateClient(string host, string username)
        {
            if (string.IsNullOrEmpty(host) || string.IsNullOrEmpty(username)) 
            {
                return null;
            }
            string baseAddress = $"ftp://{host}";
            FtpCredentials credentials = new FtpCredentials(username);
            return new FtpClient(baseAddress, credentials);
        }

        public override FtpClientBase CreateClient(string host, int? port, string username)
        {
            if (string.IsNullOrEmpty(host) || string.IsNullOrEmpty(username) || port == null)
            {
                return null;
            }
            string baseAddress = $"ftp://{host}:{port}";
            FtpCredentials credentials = new FtpCredentials(username);
            return new FtpClient(baseAddress, credentials);
        }

        public override FtpClientBase CreateClient(string host, int? port, string username, string password)
        {
            if (string.IsNullOrEmpty(host) || string.IsNullOrEmpty(username) || port == null || string.IsNullOrEmpty(password))
            {
                return null;
            }
            string baseAddress = $"ftp://{host}:{port}";
            FtpCredentials credentials = new FtpCredentials(username,password);
            return new FtpClient(baseAddress, credentials);
        }

        public override FtpClientBase CreateClient(string host, string username, string password)
        {
            if (string.IsNullOrEmpty(host) || string.IsNullOrEmpty(username))
            {
                return null;
            }
            string baseAddress = $"ftp://{host}";
            FtpCredentials credentials = new FtpCredentials(username, password);
            return new FtpClient(baseAddress, credentials);
        }
    }
}

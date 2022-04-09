using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPManager.Core.Models
{
    /// <summary>
    /// Contains credentials for FTP server.
    /// </summary>
    public class FtpCredentials
    {
        public string Username { get; set; }
        public string? Password { get; set; }
        public FtpCredentials(string username)
        {
            Username = username;
        }
        public FtpCredentials(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}

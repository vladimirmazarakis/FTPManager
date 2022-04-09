using FTPManager.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPManager.Core.Abstractions
{
    /// <summary>
    /// Interface for the FTPClient class
    /// </summary>
    public abstract class FtpClientBase
    {
        /// <summary>
        /// BaseAddress for Ftp requests.
        /// </summary>
        public abstract string BaseAddress { get; set; }
        /// <summary>
        /// Credentials for Ftp requests.
        /// </summary>
        public abstract FtpCredentials Credentials { get; set; }
        public FtpClientBase(string baseAddress, FtpCredentials ftpCredentials)
        {
            BaseAddress = baseAddress;
            Credentials = ftpCredentials;
        }
        /// <summary>
        /// Gets file info from the server.
        /// </summary>
        /// <param name="serverPath">The path to the file on the server.</param>
        /// <returns>FtpFile.</returns>
        public abstract FtpFile GetFile(string serverPath);
        /// <summary>
        /// Gets directory info from the server.
        /// </summary>
        /// <param name="serverPath">The path to the directory on the server.</param>
        /// <returns></returns>
        public abstract FtpDirectory GetDirectory(string serverPath);
    }
}

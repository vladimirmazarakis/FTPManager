using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPManager.Core.Abstractions
{
    /// <summary>
    /// Generates Ftp Clients.
    /// </summary>
    public abstract class FtpClientFactoryBase
    {
        public abstract FtpClientBase CreateClient(string host, string username);
        public abstract FtpClientBase CreateClient(string host, int? port, string username);
        public abstract FtpClientBase CreateClient(string host, int? port, string username, string password);
        public abstract FtpClientBase CreateClient(string host, string username, string password);

    }
}

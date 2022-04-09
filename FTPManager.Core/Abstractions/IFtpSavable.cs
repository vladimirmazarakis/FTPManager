using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPManager.Core.Abstractions
{
    public interface IFtpSavable
    {
        Task Save(FtpClientBase ftpClient, string path);
    }
}

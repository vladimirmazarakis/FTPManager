using FTPManager.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPManager.Core.Models
{
    public class FtpDirectory : IFtpSavable
    {
        public string Name { get; set; }
        public string ServerPath { get; set; }
        public IEnumerable<FtpFile>? Files { get; set; }
        public IEnumerable<FtpDirectory>? Directories { get; set; }

        public FtpDirectory(string name, string serverPath, IEnumerable<FtpFile> files, IEnumerable<FtpDirectory> directories)
        {
            Name = name;
            ServerPath = serverPath;
            Files = files;
            Directories = directories;
        }

        public FtpDirectory(string name, string serverPath)
        {
            Name = name;
            ServerPath = serverPath;
        }

        public Task Save(FtpClientBase ftpClient, string path)
        {
            throw new NotImplementedException();
        }
    }
}

using FTPManager.Core.Abstractions;
using FTPManager.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FTPManager.WPF.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private FtpClientBase _ftpClient;
        private string _currentPath = "";
        private string _logsText = "";

        public FtpClientBase FtpClient 
        {
            get 
            {
                return _ftpClient;
            }
            set 
            {
                _ftpClient = value;
                ForceUpdate(nameof(FtpClient)); 
            } 
        }

        public string LogsText 
        {   get 
            {
                return _logsText;
            }
            set 
            {
                _logsText = value;
                ForceUpdate(nameof(LogsText));
            }
        }

        public string CurrentPath 
        {
            get
            {
                return _currentPath;   
            }
            set
            {
                _currentPath = value;
                ForceUpdate(nameof(CurrentPath));
            }
        }

        public async Task<FtpDirectory> Establish()
        {
            _currentPath = "";
            LogLine("Trying to establish connection...");
            var ftpDirectory = await Task.Run(() => _ftpClient.GetDirectory(_currentPath));
            if(ftpDirectory is null)
            {
                LogLine("Connection failed.");
            }
            else
            {
                LogLine("Connection established.");
                LogLine("Loading directory...");
            }
            return ftpDirectory;
        }

        public async Task<FtpDirectory> GoTo(string path)
        {
            if(_ftpClient is null)
            {
                return null;
            }
            _currentPath = path;
            LogLine("Loading directory...");
            var ftpDirectory = await Task.Run(() => _ftpClient.GetDirectory(_currentPath));
            if (ftpDirectory is null)
            {
                LogLine("Failed.");
            }
            else
            {
                LogLine("Success.");
            }
            return ftpDirectory;
        }

        public async Task<FtpDirectory> GoBack() 
        {
            if(FtpClient is null)
            {
                return null;
            }
            int indexOfLastSlash = _currentPath.LastIndexOf('/');
            if (indexOfLastSlash == -1)
            {
                return null;
            }
            string newPath = _currentPath.Substring(0,indexOfLastSlash);
            LogLine("Loading directory...");
            var ftpDirectory = await Task.Run(() => _ftpClient.GetDirectory(newPath));
            if (ftpDirectory is null) 
            {
                LogLine("Failed.");
                return null;
            }
            LogLine("Success.");
            _currentPath = newPath;
            return ftpDirectory;
        }

        public async Task<FtpDirectory> Refresh() 
        {
            if(FtpClient is null)
            {
                return null;
            }
            var ftpDirectory = await Task.Run(() => _ftpClient.GetDirectory(_currentPath));
            return ftpDirectory;
        }

        public void LogLine(string message) 
        {
            _logsText += $"[{DateTime.UtcNow}] {message}\n";
        }
    }
}

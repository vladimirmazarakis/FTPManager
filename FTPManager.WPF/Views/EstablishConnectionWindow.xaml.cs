using FTPManager.Core.Abstractions;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FTPManager.WPF.Views
{
    /// <summary>
    /// Interaction logic for EstablishConnectionWindow.xaml
    /// </summary>
    public partial class EstablishConnectionWindow
    {
        private readonly MainWindow _mainWindow;
        private readonly FtpClientFactoryBase _ftpClientFactory;
        public EstablishConnectionWindow(MainWindow mainWindow, FtpClientFactoryBase ftpClientFactory)
        {
            _mainWindow = mainWindow;
            _ftpClientFactory = ftpClientFactory;
            InitializeComponent();
        }

        private void EstablishButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(HostTextBox.Text) || string.IsNullOrEmpty(UsernameTextBox.Text))
            {
                this.ShowMessageAsync("Error.", "Host and username are required.");
                return;
            }
            if (string.IsNullOrEmpty(PortTextBox.Text) && string.IsNullOrEmpty(PasswordTextBox.Text))
            {
                _mainWindow.FtpClient = _ftpClientFactory.CreateClient(HostTextBox.Text, UsernameTextBox.Text);
            }
            else if (string.IsNullOrEmpty(PortTextBox.Text) && !string.IsNullOrEmpty(PasswordTextBox.Text)) 
            {
                _mainWindow.FtpClient = _ftpClientFactory.CreateClient(HostTextBox.Text, UsernameTextBox.Text, PasswordTextBox.Text);
            }
            else if (!string.IsNullOrEmpty(PortTextBox.Text) && string.IsNullOrEmpty(PasswordTextBox.Text))
            {
                _mainWindow.FtpClient = _ftpClientFactory.CreateClient(HostTextBox.Text, int.Parse(PortTextBox.Text), UsernameTextBox.Text);
            }
            else
            {
                _mainWindow.FtpClient = _ftpClientFactory.CreateClient(HostTextBox.Text, int.Parse(PortTextBox.Text), UsernameTextBox.Text, PasswordTextBox.Text);
            }
            _mainWindow.Establish();
            this.Close();
        }
    }
}

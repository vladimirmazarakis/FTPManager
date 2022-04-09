using FTPManager.Core.Abstractions;
using FTPManager.Core.Models;
using FTPManager.WPF.ViewModels;
using MahApps.Metro.Controls;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FTPManager.WPF.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly FtpClientFactoryBase _ftpClientFactory;
        private readonly MainWindowViewModel _viewModel;
        public MainWindow(FtpClientFactoryBase ftpClientFactory)
        {
            _ftpClientFactory = ftpClientFactory;
            _viewModel = new MainWindowViewModel();
            this.DataContext = _viewModel;
            CompositionTarget.Rendering += CompositionTarget_Rendering;
            InitializeComponent();
        }

        private void CompositionTarget_Rendering(object? sender, EventArgs e)
        {
            LogsTextBox.Text = _viewModel.LogsText;
        }

        private FtpDirectory _currentDirectory;

        private void EstablishMenuItem_Click(object sender, RoutedEventArgs e)
        {
            EstablishConnectionWindow establishConnectionWindow = new EstablishConnectionWindow(this, _ftpClientFactory);
            establishConnectionWindow.Show();
        }

        public async void Establish()
        {
            _currentDirectory = await _viewModel.Establish();
            if(_currentDirectory is null)
            {
                await this.ShowMessageAsync("Error.", "Could not establish connection.");
                _viewModel.LogLine("Failed.");
            }
            else
            {
                _viewModel.LogLine("Success.");
            }
            ForceUpdate();
        }

        public FtpClientBase FtpClient 
        { 
            get 
            { 
                return _viewModel.FtpClient; 
            } 
            set 
            { 
                _viewModel.FtpClient = value; 
            } 
        }

        private async void ForceUpdate() 
        {
            _currentDirectory = await _viewModel.Refresh();
            if (_currentDirectory == null) 
            {
                return;
            }
            FilesListView.Items.Clear();
            foreach (var directory in _currentDirectory.Directories)
            {
                FilesListView.Items.Add(directory);
            }
            foreach (var file in _currentDirectory.Files)
            {
                FilesListView.Items.Add(file);
            }
            CurrentPathTextBox.Text = _viewModel.CurrentPath;
        }

        private async void RefreshMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ForceUpdate();
        }

        private async void FilesListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (FilesListView.SelectedIndex == -1)
            {
                return;
            }

            if (FilesListView.SelectedItem is FtpDirectory)
            {
                var tempDirectory = (FtpDirectory)FilesListView.SelectedItem;
                _currentDirectory = await _viewModel.GoTo(tempDirectory.ServerPath);
                if (_currentDirectory == null)
                {
                    return;
                }
                FilesListView.Items.Clear();
                foreach (var directory in _currentDirectory.Directories)
                {
                    FilesListView.Items.Add(directory);
                }
                foreach (var file in _currentDirectory.Files)
                {
                    FilesListView.Items.Add(file);
                }
                CurrentPathTextBox.Text = _viewModel.CurrentPath;
            }
        }

        private async void GoBackButton_Click(object sender, RoutedEventArgs e)
        {
            var newDirectory = await _viewModel.GoBack();
            if (newDirectory is null) 
            {
                return;
            }
            _currentDirectory = newDirectory;
            ForceUpdate();
        }
    }
}

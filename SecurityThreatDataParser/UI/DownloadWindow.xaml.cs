using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Windows;

namespace SecurityThreatDataParser.UI
{
    public partial class DownloadWindow : Window
    {
        public DownloadWindow()
        {
            InitializeComponent();
        }

        private void ButtonConfirm_Click(Object sender, RoutedEventArgs e)
        {
            try
            {
                if (File.Exists(MainWindow.DataBasePath) && new FileInfo(MainWindow.DataBasePath).Length == 0)
                {
                    File.Delete(MainWindow.DataBasePath);
                }

                ButtonConfirm.IsEnabled = false;
                ButtonRefuse.IsEnabled = false;
                using (var webClient = new WebClient())
                {
                    ProgressBar.Visibility = Visibility.Visible;
                    webClient.DownloadFileCompleted += WebClient_DownloadFileCompleted;
                    webClient.DownloadProgressChanged += WebClient_DownloadProgressChanged;
                    webClient.DownloadFileAsync(new Uri("https://bdu.fstec.ru/files/documents/thrlist.xlsx"),
                        MainWindow.DataBasePath);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show($"Error: {exception.Message}");
                Close();
            }
        }

        private void ButtonRefuse_Click(Object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void WebClient_DownloadFileCompleted(Object sender, AsyncCompletedEventArgs e)
        {
            if (ProgressBar.Value != 100)
            {
                if (File.Exists(MainWindow.DataBasePath) && new FileInfo(MainWindow.DataBasePath).Length == 0)
                {
                    File.Delete(MainWindow.DataBasePath);
                }

                MessageBox.Show(
                    "Unable to download the file. Please try again");
            }

            Close();
        }

        private void WebClient_DownloadProgressChanged(Object sender, DownloadProgressChangedEventArgs e)
        {
            ProgressBar.Value = e.ProgressPercentage;
        }
    }
}
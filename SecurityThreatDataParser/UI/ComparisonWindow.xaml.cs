using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Windows;
using SecurityThreatDataParser.Core.Data;
using static System.Net.WebRequest;

namespace SecurityThreatDataParser.UI
{
    public partial class ComparisonWindow
    {
        private readonly String _url = "https://bdu.fstec.ru/files/documents/thrlist.xlsx";
        private readonly List<ThreatInfo> _difNewList = new List<ThreatInfo>();
        private readonly List<ThreatInfo> _difPrevList = new List<ThreatInfo>();
        private readonly List<ICloneable> _prevList = new List<ICloneable>();
        public Int32 CountOfUpdated = -1;

        public ComparisonWindow()
        {
            InitializeComponent();
            try
            {
                if (File.Exists(MainWindow.DataBasePath) && new FileInfo(MainWindow.DataBasePath).Length == 0)
                {
                    File.Delete(MainWindow.DataBasePath);
                }

                MainWindow.FullListOfRisks = MainWindow.EnumerateRisks();
                MainWindow.FullListOfRisks.ForEach((item) => { _prevList.Add((ICloneable) item.Clone()); });
                var httpWebRequest = (HttpWebRequest) Create(_url);

                using ((HttpWebResponse) httpWebRequest.GetResponse())
                {
                }

                using (var webClient = new WebClient())
                {
                    webClient.DownloadFile(new Uri(_url),
                        MainWindow.DataBasePath);
                }

                var metrics = MainWindow.EnumerateRisks();
                MainWindow.FullListOfRisks = metrics;
                var count = MainWindow.FullListOfRisks.Count > _prevList.Count
                    ? _prevList.Count
                    : MainWindow.FullListOfRisks.Count;
                for (var i = 0; i < count; i++)
                {
                    if (!MainWindow.FullListOfRisks[i].Equals(_prevList[i]))
                    {
                        _difNewList.Add(MainWindow.FullListOfRisks[i]);
                        _difPrevList.Add((ThreatInfo) _prevList[i]);
                    }
                }

                for (var i = count; i < MainWindow.FullListOfRisks.Count; i++)
                {
                    _difNewList.Add(MainWindow.FullListOfRisks[i]);
                }

                for (var i = count; i < _prevList.Count; i++)
                {
                    _difPrevList.Add((ThreatInfo) _prevList[i]);
                }

                CountOfUpdated = _difPrevList.Count > _difNewList.Count ? _difPrevList.Count : _difNewList.Count;
                if (CountOfUpdated > 0)
                {
                    PrevData.ItemsSource = _difPrevList;
                    NewData.ItemsSource = _difNewList;
                }
            }
            catch (WebException)
            {
                MessageBox.Show(
                    "Unable to download the file. Please try again");
                Close();
            }
            catch (Exception exception)
            {
                MessageBox.Show($"Error: {exception.Message}");
                Close();
            }
        }

        private void Button_Click(Object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
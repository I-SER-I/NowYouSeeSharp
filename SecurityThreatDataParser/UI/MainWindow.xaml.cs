using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ClosedXML.Excel;
using Microsoft.Win32;
using PagedList;
using SecurityThreatDataParser.Core.Data;
using SecurityThreatDataParser.Core.Enums;

namespace SecurityThreatDataParser.UI
{
    public partial class MainWindow : Window
    {
        public static String DataBasePath =
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\thrlist.xlsx";

        public static List<ThreatInfo> FullListOfRisks = new List<ThreatInfo>();
        public static List<ShortThreatInfo> ShortListOfRisks = new List<ShortThreatInfo>();
        private static Int32 _pageNumber = 1;
        private static Int32 _countOfPages;
        private const Int32 PageSize = 25;
        private static IPagedList<ShortThreatInfo> _pagedShortList;
        private static IPagedList<ThreatInfo> _pagedList;
        private TypeOfPagedList _pagedListType;

        private TypeOfPagedList PagedListType
        {
            get => _pagedListType;
            set
            {
                _pagedListType = value;
                ChangeVisibilityDataGrid();
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            if (!File.Exists(DataBasePath) || File.Exists(DataBasePath) && new FileInfo(DataBasePath).Length == 0)
            {
                var downloadWindow = new DownloadWindow();
                downloadWindow.ShowDialog();
            }

            if (File.Exists(DataBasePath) && new FileInfo(DataBasePath).Length != 0)
            {
                LoadLocalDataBaseToDataGrid();
            }
        }

        private void ChangeVisibilityDataGrid()
        {
            if (_pagedListType == TypeOfPagedList.Short)
            {
                for (var i = 2; i < DataGrid.Columns.Count; i++)
                {
                    DataGrid.Columns[i].Visibility = Visibility.Hidden;
                }
            }
            else
            {
                for (var i = 2; i < DataGrid.Columns.Count; i++)
                {
                    DataGrid.Columns[i].Visibility = Visibility.Visible;
                }
            }
        }

        private void ButtonUpdate_Click(Object sender, RoutedEventArgs e)
        {
            if (File.Exists(DataBasePath) && new FileInfo(DataBasePath).Length == 0)
            {
                File.Delete(DataBasePath);
            }

            if (!File.Exists(DataBasePath) || File.Exists(DataBasePath) && new FileInfo(DataBasePath).Length == 0)
            {
                ButtonUpdate.IsEnabled = false;
                ButtonDownload.IsEnabled = true;
                ButtonSaveAs.IsEnabled = false;
                MessageBox.Show("Data Base not exists");
                return;
            }

            var change = new ComparisonWindow();
            if (change.CountOfUpdated == 0)
            {
                MessageBox.Show("No change");
            }

            if (change.CountOfUpdated > 0)
            {
                MessageBox.Show(
                    $"Data Base updated! Number of updated records: {change.CountOfUpdated}.");
            }

            if (change.CountOfUpdated != 0 && change.CountOfUpdated != -1)
            {
                change.ShowDialog();
            }

            if (change.CountOfUpdated == -1)
            {
                change.Close();
                ButtonUpdate.IsEnabled = false;
                ButtonDownload.IsEnabled = true;
                return;
            }

            change.Close();
            PagedListType = TypeOfPagedList.Short;
            _pageNumber = 1;
            ButtonPrev.IsEnabled = false;
            ButtonNext.IsEnabled = _countOfPages >= 2;
            _pagedShortList = ShortListOfRisks.ToPagedList(_pageNumber, PageSize);
            _pagedList = FullListOfRisks.ToPagedList(_pageNumber, PageSize);
            DataGrid.ItemsSource = _pagedShortList;
            ButtonChangeViewMode.Content = "Show All";
            _countOfPages = _pagedShortList.PageCount;
            LabelCountOfPages.Content = string.Format($"{_pageNumber}/{_countOfPages}");
        }

        public static List<ThreatInfo> EnumerateRisks()
        {
            var risks = new List<ThreatInfo>();
            using (var workbook = new XLWorkbook(DataBasePath))
            {
                ShortListOfRisks.Clear();
                var worksheet = workbook.Worksheets.Worksheet(1);
                for (var i = 3; i <= worksheet.RowsUsed().Count(); i++)
                {
                    var threat = new ThreatInfo()
                    {
                        Id = worksheet.Cell(i, 1).Value.ToString(),
                        Name = worksheet.Cell(i, 2).Value.ToString(),
                        Description = worksheet.Cell(i, 3).Value.ToString(),
                        SourceOfThreat = worksheet.Cell(i, 4).Value.ToString(),
                        InteractionObject = worksheet.Cell(i, 5).Value.ToString(),
                        ViolationOfConfidentiality = worksheet.Cell(i, 6).Value.ToString(),
                        IntegrityViolation = worksheet.Cell(i, 7).Value.ToString(),
                        ViolationOfAvailability = worksheet.Cell(i, 8).Value.ToString(),
                    };
                    var shortThreat = new ShortThreatInfo()
                    {
                        Id = worksheet.Cell(i, 1).Value.ToString(),
                        Name = worksheet.Cell(i, 2).Value.ToString()
                    };
                    risks.Add(threat);
                    ShortListOfRisks.Add(shortThreat);
                }

                _pagedShortList = ShortListOfRisks.ToPagedList(_pageNumber, PageSize);
                _pagedList = risks.ToPagedList(_pageNumber, PageSize);
                _countOfPages = _pagedShortList.PageCount;
            }

            return risks;
        }

        private void ButtonPrev_Click(Object sender, RoutedEventArgs e)
        {
            switch (PagedListType)
            {
                case TypeOfPagedList.Short:
                    if (!_pagedShortList.HasPreviousPage) return;
                    _pageNumber--;
                    _pagedShortList = ShortListOfRisks.ToPagedList(_pageNumber, PageSize);
                    DataGrid.ItemsSource = _pagedShortList;

                    ButtonPrev.IsEnabled = _pagedShortList.HasPreviousPage;
                    ButtonNext.IsEnabled = true;
                    LabelCountOfPages.Content = string.Format($"{_pageNumber}/{_countOfPages}");
                    break;
                case TypeOfPagedList.Full:
                    if (!_pagedList.HasPreviousPage) return;
                    _pageNumber--;
                    _pagedList = FullListOfRisks.ToPagedList(_pageNumber, PageSize);
                    DataGrid.ItemsSource = _pagedList;
                    ButtonPrev.IsEnabled = _pagedList.HasPreviousPage;
                    ButtonNext.IsEnabled = true;
                    LabelCountOfPages.Content = string.Format($"{_pageNumber}/{_countOfPages}");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void ButtonNext_Click(Object sender, RoutedEventArgs e)
        {
            switch (PagedListType)
            {
                case TypeOfPagedList.Short:
                    if (!_pagedShortList.HasNextPage) return;
                    _pageNumber++;
                    _pagedShortList = ShortListOfRisks.ToPagedList(_pageNumber, PageSize);
                    DataGrid.ItemsSource = _pagedShortList;
                    ButtonNext.IsEnabled = _pagedShortList.HasNextPage;

                    ButtonPrev.IsEnabled = true;
                    LabelCountOfPages.Content = string.Format($"{_pageNumber}/{_countOfPages}");
                    break;
                case TypeOfPagedList.Full:
                    if (!_pagedList.HasNextPage) return;
                    _pageNumber++;
                    _pagedList = FullListOfRisks.ToPagedList(_pageNumber, PageSize);
                    DataGrid.ItemsSource = _pagedList;
                    ButtonNext.IsEnabled = _pagedList.HasNextPage;

                    ButtonPrev.IsEnabled = true;
                    LabelCountOfPages.Content = string.Format($"{_pageNumber}/{_countOfPages}");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void ButtonDownload_Click(Object sender, RoutedEventArgs e)
        {
            if (!File.Exists(DataBasePath) || File.Exists(DataBasePath) && new FileInfo(DataBasePath).Length == 0)
            {
                var downloadWindow = new DownloadWindow();
                downloadWindow.ShowDialog();
            }

            if (File.Exists(DataBasePath) && new FileInfo(DataBasePath).Length != 0)
            {
                LoadLocalDataBaseToDataGrid();
            }
        }

        private void LoadLocalDataBaseToDataGrid()
        {
            try
            {
                FullListOfRisks = EnumerateRisks();
                ButtonPrev.IsEnabled = false;
                ButtonNext.IsEnabled = _countOfPages >= 2;
                _pageNumber = 1;
                _pagedShortList = ShortListOfRisks.ToPagedList(_pageNumber, PageSize);
                DataGrid.ItemsSource = _pagedShortList;
                ButtonChangeViewMode.Content = "Show all";
                LabelCountOfPages.Content = string.Format($"{_pageNumber}/{_countOfPages}");
                PagedListType = TypeOfPagedList.Short;
                if (!File.Exists(DataBasePath))
                {
                    ButtonUpdate.IsEnabled = false;
                    ButtonPrev.IsEnabled = false;
                    ButtonNext.IsEnabled = false;
                    ButtonChangeViewMode.IsEnabled = false;
                    ButtonDownload.IsEnabled = true;
                    ButtonSaveAs.IsEnabled = false;
                }
                else
                {
                    ButtonUpdate.IsEnabled = true;
                    ButtonNext.IsEnabled = _countOfPages >= 2;
                    ButtonChangeViewMode.IsEnabled = true;
                    ButtonDownload.IsEnabled = false;
                    ButtonSaveAs.IsEnabled = true;
                }
            }
            catch (FileFormatException exception)
            {
                File.Delete(DataBasePath);
                ButtonUpdate.IsEnabled = false;
                ButtonDownload.IsEnabled = true;
                ButtonSaveAs.IsEnabled = false;
                MessageBox.Show($"Error: {exception.Message}");
            }
            catch (Exception exception)
            {
                ButtonUpdate.IsEnabled = false;
                ButtonDownload.IsEnabled = true;
                ButtonSaveAs.IsEnabled = false;
                MessageBox.Show($"Error: {exception.Message}");
            }
        }

        private void ButtonChangeViewMode_Click(Object sender, RoutedEventArgs e)
        {
            switch (PagedListType)
            {
                case TypeOfPagedList.Short:
                    _pagedList = FullListOfRisks.ToPagedList(_pageNumber, PageSize);
                    DataGrid.ItemsSource = _pagedList;
                    PagedListType = TypeOfPagedList.Full;
                    ButtonChangeViewMode.Content = "Show Preview";
                    break;
                case TypeOfPagedList.Full:
                    _pagedShortList = ShortListOfRisks.ToPagedList(_pageNumber, PageSize);
                    DataGrid.ItemsSource = _pagedShortList;
                    PagedListType = TypeOfPagedList.Short;
                    ButtonChangeViewMode.Content = "Show All";
                    break;
            }
        }

        private void ButtonSaveAs_Click(Object sender, RoutedEventArgs e)
        {
            var tempShortList = new List<ICloneable>();
            var tempList = new List<ICloneable>();
            FullListOfRisks.ForEach(item => { tempList.Add((ICloneable) item.Clone()); });
            ShortListOfRisks.ForEach((item) => { tempShortList.Add((ICloneable) item.Clone()); });
            try
            {
                var threat = EnumerateRisks();
                var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add("Sheet");
                worksheet.Cell(1, "A").Value = "Идентификатор угрозы";
                worksheet.Cell(1, "B").Value = "Наименование угрозы";
                worksheet.Cell(1, "C").Value = "Описание угрозы";
                worksheet.Cell(1, "D").Value = "Источник угрозы";
                worksheet.Cell(1, "E").Value = "Объект воздействия угрозы";
                worksheet.Cell(1, "F").Value = "Нарушение конфиденциальности";
                worksheet.Cell(1, "G").Value = "Нарушение целостности";
                worksheet.Cell(1, "H").Value = "Нарушение доступности";
                for (var i = 0; i < threat.Count; i++)
                {
                    worksheet.Cell(i + 2, "A").Value = threat[i].Id;
                    worksheet.Cell(i + 2, "B").Value = threat[i].Name;
                    worksheet.Cell(i + 2, "C").Value = threat[i].Description;
                    worksheet.Cell(i + 2, "D").Value = threat[i].SourceOfThreat;
                    worksheet.Cell(i + 2, "E").Value = threat[i].InteractionObject;
                    worksheet.Cell(i + 2, "F").Value = threat[i].ViolationOfConfidentiality;
                    worksheet.Cell(i + 2, "G").Value = threat[i].IntegrityViolation;
                    worksheet.Cell(i + 2, "H").Value = threat[i].ViolationOfAvailability;
                }

                var rngTable = worksheet.Range("A1:H" + (threat.Count + 1));
                rngTable.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                rngTable.Style.Border.BottomBorder = XLBorderStyleValues.Thin;

                worksheet.Columns().AdjustToContents();
                var dlg = new SaveFileDialog()
                {
                    Filter = "Книга Excel (*.xlsx)|*.xlsx",
                    InitialDirectory = @"c:\"
                };
                if (dlg.ShowDialog() == true)
                {
                    workbook.SaveAs(dlg.FileName);
                }
            }
            catch (FileFormatException exception)
            {
                File.Delete(DataBasePath);
                ButtonUpdate.IsEnabled = false;
                ButtonDownload.IsEnabled = true;
                ButtonSaveAs.IsEnabled = false;
                FullListOfRisks.Clear();
                ShortListOfRisks.Clear();
                tempList.ForEach((item) => { FullListOfRisks.Add((ThreatInfo) item.Clone()); });
                tempShortList.ForEach((item) => { ShortListOfRisks.Add((ShortThreatInfo) item.Clone()); });
                _pagedShortList = ShortListOfRisks.ToPagedList(_pageNumber, PageSize);
                _pagedList = FullListOfRisks.ToPagedList(_pageNumber, PageSize);
                _countOfPages = _pagedShortList.PageCount;
                MessageBox.Show($"Error: {exception.Message}");
            }
            catch (Exception exception)
            {
                ButtonUpdate.IsEnabled = false;
                ButtonDownload.IsEnabled = true;
                ButtonSaveAs.IsEnabled = false;
                FullListOfRisks.Clear();
                ShortListOfRisks.Clear();
                tempList.ForEach((item) => { FullListOfRisks.Add((ThreatInfo) item.Clone()); });
                tempShortList.ForEach(item => { ShortListOfRisks.Add((ShortThreatInfo) item.Clone()); });
                _pagedShortList = ShortListOfRisks.ToPagedList(_pageNumber, PageSize);
                _pagedList = FullListOfRisks.ToPagedList(_pageNumber, PageSize);
                _countOfPages = _pagedShortList.PageCount;
                MessageBox.Show($"Error: {exception.Message}");
            }
        }
    }
}
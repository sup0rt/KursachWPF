using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
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
using System.IO;
using System.Windows.Forms.DataVisualization.Charting;
using Word = Microsoft.Office.Interop.Word;
using Excel = Microsoft.Office.Interop.Excel;

namespace WpfApp1.pages
{
    /// <summary>
    /// Логика взаимодействия для Report2.xaml
    /// </summary>
    public partial class Report2 : Page
    {
        public string GetNewFileName(string filePath, string fileName, string ext)
        {
            string fullPath = System.IO.Path.Combine(filePath, $"{fileName}№{1}{ext}");
            int counter = 1;

            while (File.Exists(fullPath))
            {
                fullPath = System.IO.Path.Combine(filePath, $"{fileName}№{counter}{ext}");
                counter++;
            }

            return fullPath;
        }
        public Report2()
        {
            InitializeComponent();
            PartsQuantity.ChartAreas.Add(new ChartArea("Main"));
            var currentSeries = new Series("Самые востребованные запчасти за период")
            {
                IsValueShownAsLabel = true,
            };
            PartsQuantity.Series.Add(currentSeries);

            diagramCmb.ItemsSource = Enum.GetValues(typeof(SeriesChartType));
        }
        private dynamic GetTopDemandedParts()
        {
            var startDate = start.SelectedDate ?? DateTime.Today.AddMonths(-1);
            var endDate = end.SelectedDate ?? DateTime.Today.AddMonths(1);
            int topCount = int.TryParse(topPartsTB.Text, out int count) ? count : 5;

            var orders = Entities.GetContext().Order.ToList();
            var ordersDetail = Entities.GetContext().OrderDetail.ToList();
            var parts = Entities.GetContext().Part.ToList();

            return orders.Where(o => o.OrderDateTime >= startDate && o.OrderDateTime <= endDate)
                .Join(ordersDetail,
                    o => o.OrderID,
                    od => od.OrderID,
                    (o, od) => od)
                .GroupBy(od=>od.PartID)
                .Where(g=>g.Key.HasValue)
                .Select(g => new
                {
                    PartID = g.Key.Value,
                    TotalQuantity = g.Sum(od => od.Quantity),
                    PartName = parts.FirstOrDefault(p => p.PartID == g.Key).PartName
                })
                .OrderByDescending(x => x.TotalQuantity)
                .Take(topCount)
                .ToList();
        }
        private void goBackbtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void wordBtn_Click(object sender, RoutedEventArgs e)
        {
            var data = GetTopDemandedParts();
            var startDate = start.SelectedDate ?? DateTime.Today.AddMonths(-1);
            var endDate = end.SelectedDate ?? DateTime.Today.AddMonths(1);

            var application = new Word.Application();
            Word.Document document = application.Documents.Add();

            Word.Paragraph titleParagraph = document.Paragraphs.Add();
            Word.Range titleRange = titleParagraph.Range;
            titleRange.Text = $"Отчет по самым востребованным запчастям в период с {DateTime.Now.ToString("dd-MM-yyyy")} по {DateTime.Now.ToString("dd-MM-yyyy")}";
            titleRange.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            titleRange.Font.Name = "Times New Roman";
            titleRange.Font.Size = 16;
            titleRange.Font.Bold = 1;
            titleRange.InsertParagraphAfter();

            Word.Paragraph tableParagraph = document.Paragraphs.Add();
            Word.Range tableRange = tableParagraph.Range;
            Word.Table partsTable = document.Tables.Add(tableRange, data.Count + 1, 2);

            partsTable.Borders.InsideLineStyle = partsTable.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
            partsTable.Range.Cells.VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;

            partsTable.Cell(1, 1).Range.Text = "Запчасть";
            partsTable.Cell(1, 2).Range.Text = "Продано";

            Word.Range headerRange = partsTable.Rows[1].Range;
            headerRange.Font.Bold = 1;
            headerRange.Font.Name = "Times New Roman";
            headerRange.Font.Size = 14;
            headerRange.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;

            for (int i = 0; i < data.Count; i++)
            {
                partsTable.Cell(i + 2, 1).Range.Text = data[i].PartName;
                partsTable.Cell(i + 2, 1).Range.Font.Name = "Times New Roman";
                partsTable.Cell(i + 2, 1).Range.Font.Size = 12;

                partsTable.Cell(i + 2, 2).Range.Text = data[i].TotalQuantity.ToString();
                partsTable.Cell(i + 2, 2).Range.Font.Name = "Times New Roman";
                partsTable.Cell(i + 2, 2).Range.Font.Size = 12;
            }
            string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string filePath = GetNewFileName(desktop, $"{topPartsTB.Text}MostDemandedPartsFrom-{startDate.ToString("dd-MM-yyyy")}-To-{endDate.ToString("dd-MM-yyyy")}", ".docx");
            try
            {
                document.SaveAs2(filePath);
                document.Close();
                application.Quit();

                MessageBox.Show($"Отчет сохранен в: {filePath}");
            }
            catch
            {
                MessageBox.Show(filePath);
            }
        }

        private void excelBtn_Click(object sender, RoutedEventArgs e)
        {
            var data = GetTopDemandedParts();
            var startDate = start.SelectedDate ?? DateTime.Today.AddMonths(-1);
            var endDate = end.SelectedDate ?? DateTime.Today.AddMonths(1);

            var application = new Excel.Application();
            Excel.Workbook workbook = application.Workbooks.Add();
            Excel.Worksheet worksheet = workbook.Worksheets[1];

            worksheet.Cells[1, 1] = $"{topPartsTB.Text} самых востребованных запчастей в период с {startDate.ToString("dd-MM-yyyy")} по {endDate.ToString("dd-MM-yyyy")}";
            Excel.Range headerRange = worksheet.Range["A1:I1"];
            headerRange.Merge();
            headerRange.Font.Bold = true;
            headerRange.Font.Size = 14;
            headerRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

            worksheet.Cells[4, 1] = "Запчасть";
            worksheet.Cells[4, 2] = "Продано";
            Excel.Range tableHeader = worksheet.Range["A4:B4"];
            tableHeader.Font.Bold = true;

            for (int i = 0; i < data.Count; i++)
            {
                worksheet.Cells[i + 5, 1] = data[i].PartName;
                worksheet.Cells[i + 5, 2] = data[i].TotalQuantity;

            }

            Excel.Range dataRange = worksheet.Range[$"A4:B{4 + data.Count}"];
            dataRange.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
            dataRange.Borders.Weight = Excel.XlBorderWeight.xlThin;
            dataRange.Columns.AutoFit();

            string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string filePath = GetNewFileName(desktop, $"{topPartsTB.Text}MostDemandedPartsFrom-{startDate.ToString("dd-MM-yyyy")}-To-{endDate.ToString("dd-MM-yyyy")}", ".xlsx");

            workbook.SaveAs(filePath);
            workbook.Close();
            application.Quit();

            MessageBox.Show($"Отчет сохранен в {filePath}");
        }

        private void diagramCmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var data = GetTopDemandedParts();

            if (diagramCmb.SelectedItem is SeriesChartType currentType)
            {
                Series currentSeries = PartsQuantity.Series.FirstOrDefault();
                currentSeries.ChartType = currentType;
                currentSeries.Points.Clear();

                foreach(var item in data)
                {
                    currentSeries.Points.AddXY(item.PartName, item.TotalQuantity);
                }
            }
        }
    }
}

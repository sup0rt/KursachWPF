using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.DataVisualization.Charting;
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
    /// Логика взаимодействия для Report3.xaml
    /// </summary>
    public partial class Report3 : Page
    {
        public Report3()
        {
            InitializeComponent();
            PartsQuantity.ChartAreas.Add(new ChartArea("Main"));
            var currentSeries = new Series("Количество заказов за период")
            {
                IsValueShownAsLabel = true,
            };
            PartsQuantity.Series.Add(currentSeries);

            diagramCmb.ItemsSource = Enum.GetValues(typeof(SeriesChartType));
        }
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

        private dynamic GetOrdersPerDate()
        {
            var startDate = start.SelectedDate ?? DateTime.Today.AddMonths(-1);
            var endDate = end.SelectedDate ?? DateTime.Today.AddMonths(1);
            var groupDate = startDate;

            var orders = Entities.GetContext().Order.ToList();
            
            

            return orders.Where(o => o.OrderDateTime >= startDate && o.OrderDateTime <= endDate)
                .AsEnumerable()
                .GroupBy(o => o.OrderDateTime.Date)
                .Select(g => new
                {
                    OrdersInDate = g.Count(),
                    CreationDate = g.Key.Date.ToString("dd-MM-yyyy"),
                })
                .OrderByDescending(x => x.CreationDate)
                .ToList();
        }

        private void goBackbtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void excelBtn_Click(object sender, RoutedEventArgs e)
        {
            var data = GetOrdersPerDate();
            var startDate = start.SelectedDate ?? DateTime.Today.AddMonths(-1);
            var endDate = end.SelectedDate ?? DateTime.Today.AddMonths(1);
            int total = Entities.GetContext().Order.Count(o => o.OrderDateTime >= startDate && o.OrderDateTime <= endDate);

            var application = new Excel.Application();
            Excel.Workbook workbook = application.Workbooks.Add();
            Excel.Worksheet worksheet = workbook.Worksheets[1];

            worksheet.Cells[1, 1] = $"Отчет о количестве заказов по датам в период с {startDate.ToString("dd-MM-yyyy")} по {endDate.ToString("dd-MM-yyyy")}";
            Excel.Range headerRange = worksheet.Range["A1:I1"];
            headerRange.Merge();
            headerRange.Font.Bold = true;
            headerRange.Font.Size = 14;
            headerRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

            
            worksheet.Cells[2, 1] = $"Всего заказов: {total}";
            worksheet.Range["A2"].Font.Bold = true;

            worksheet.Cells[4, 1] = "Дата";
            worksheet.Cells[4, 2] = "Заказов в дату";
            Excel.Range tableHeader = worksheet.Range["A4:B4"];
            tableHeader.Font.Bold = true;

            for (int i = 0; i < data.Count; i++)
            {
                worksheet.Cells[i + 5, 1] = data[i].CreationDate;
                worksheet.Cells[i + 5, 2] = data[i].OrdersInDate;

            }

            Excel.Range dataRange = worksheet.Range[$"A4:B{4 + data.Count}"];
            dataRange.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
            dataRange.Borders.Weight = Excel.XlBorderWeight.xlThin;
            dataRange.Columns.AutoFit();

            string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string filePath = GetNewFileName(desktop, $"OrdersFrom{startDate.ToString("dd-MM-yyyy")}To{endDate.ToString("dd-MM-yyyy")}", ".xlsx");

            workbook.SaveAs(filePath);
            workbook.Close();
            application.Quit();

            MessageBox.Show($"Отчет сохранен в {filePath}");
        }

        private void wordBtn_Click(object sender, RoutedEventArgs e)
        {
            var data = GetOrdersPerDate();
            var startDate = start.SelectedDate ?? DateTime.Today.AddMonths(-1);
            var endDate = end.SelectedDate ?? DateTime.Today.AddMonths(1);
            int total = Entities.GetContext().Order.Count(o => o.OrderDateTime >= startDate && o.OrderDateTime <= endDate);

            var application = new Word.Application();
            Word.Document document = application.Documents.Add();

            Word.Paragraph titleParagraph = document.Paragraphs.Add();
            Word.Range titleRange = titleParagraph.Range;
            titleRange.Text = $"Отчет о количестве заказов по датам в период с {startDate.ToString("dd-MM-yyyy")} по {endDate.ToString("dd-MM-yyyy")}";
            titleRange.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            titleRange.Font.Name = "Times New Roman";
            titleRange.Font.Size = 16;
            titleRange.Font.Bold = 1;
            titleRange.InsertParagraphAfter();

            Word.Paragraph totalParagraph = document.Paragraphs.Add();
            Word.Range totalRange = totalParagraph.Range;
            totalRange.Text = $"Всего заказов: {total.ToString()}";
            titleRange.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
            totalRange.Font.Name = "Times New Roman";
            totalRange.Font.Size = 14;
            totalRange.Font.Bold = 1;
            totalRange.InsertParagraphAfter();

            Word.Paragraph tableParagraph = document.Paragraphs.Add();
            Word.Range tableRange = tableParagraph.Range;
            Word.Table partsTable = document.Tables.Add(tableRange, data.Count + 1, 2);

            partsTable.Borders.InsideLineStyle = partsTable.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
            partsTable.Range.Cells.VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;

            partsTable.Cell(1, 1).Range.Text = "Дата";
            partsTable.Cell(1, 2).Range.Text = "Заказов в дату";

            Word.Range headerRange = partsTable.Rows[1].Range;
            headerRange.Font.Bold = 1;
            headerRange.Font.Name = "Times New Roman";
            headerRange.Font.Size = 14;
            headerRange.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;

            for (int i = 0; i < data.Count; i++)
            {
                partsTable.Cell(i + 2, 1).Range.Text = data[i].CreationDate;
                partsTable.Cell(i + 2, 1).Range.Font.Name = "Times New Roman";
                partsTable.Cell(i + 2, 1).Range.Font.Size = 12;

                partsTable.Cell(i + 2, 2).Range.Text = data[i].OrdersInDate.ToString();
                partsTable.Cell(i + 2, 2).Range.Font.Name = "Times New Roman";
                partsTable.Cell(i + 2, 2).Range.Font.Size = 12;
            }

            string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string filePath = GetNewFileName(desktop, $"OrdersFrom{startDate.ToString("dd-MM-yyyy")}To{endDate.ToString("dd-MM-yyyy")}", ".docx");
            document.SaveAs2(filePath);
            document.Close();
            application.Quit();

            MessageBox.Show($"Отчет сохранен в: {filePath}");
        }

        private void diagramCmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var data = GetOrdersPerDate();

            if (diagramCmb.SelectedItem is SeriesChartType currentType)
            {
                Series currentSeries = PartsQuantity.Series.FirstOrDefault();
                currentSeries.ChartType = currentType;
                currentSeries.Points.Clear();

                foreach (var item in data)
                {
                    currentSeries.Points.AddXY(item.CreationDate, item.OrdersInDate);
                }
            }
        }
    }
}

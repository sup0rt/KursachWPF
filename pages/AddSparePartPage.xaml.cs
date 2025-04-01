using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace WpfApp1.pages
{
    /// <summary>
    /// Логика взаимодействия для AddSparePartPage.xaml
    /// </summary>
    public partial class AddSparePartPage : Page
    {
        public AddSparePartPage(Part selectedPart)
        {
            InitializeComponent();
            categoryCmB.ItemsSource=Entities.GetContext().Category.ToList();
            manufacturerCmB.ItemsSource=Entities.GetContext().Manufacturer.ToList();
            supplierCmB.ItemsSource=Entities.GetContext().Supplier.ToList();
            _part = selectedPart ?? new Part();
            DataContext = _part;
        }
        private Part _part = new Part();
        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrEmpty(_part.PartName)) errors.AppendLine("Введите название запчасти");
            if (_part.Category == null) errors.AppendLine("Выберите категорию запчасти");
            if (_part.ManufacturerID == null) errors.AppendLine("Выберите производителя запчасти");
            if (_part.SupplierID == null) errors.AppendLine("Выберите поставщика номер запчасти");
            if (_part.Price <= 0) errors.AppendLine("Введите цену запчасти");


            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            try
            {
                var context = Entities.GetContext();
                if (_part.PartID == 0)
                {
                    context.Part.Add(_part);
                }
                else
                {
                    context.Entry(_part).State = System.Data.Entity.EntityState.Modified;
                }

                context.SaveChanges();
                MessageBox.Show("Запчасть добавлена", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                {
                    MessageBox.Show("Ошибка: {ex.ToString()}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}

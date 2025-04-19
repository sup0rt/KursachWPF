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

namespace WpfApp1.pages
{
    /// <summary>
    /// Логика взаимодействия для AdminManufacturerPage.xaml
    /// </summary>
    public partial class AdminManufacturerPage : Page
    {
        public AdminManufacturerPage()
        {
            InitializeComponent();
            dgSuppliers.ItemsSource = Entities.GetContext().Manufacturer.ToList();
        }

        private void addManufacturer_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddManufacturerPage(null));
        }

        private void editManufacturer_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddManufacturerPage((sender as Button).DataContext as Manufacturer));
        }
        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                Entities.GetContext().ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
                dgSuppliers.ItemsSource = Entities.GetContext().Manufacturer.ToList();
            }
        }

        private void deleteManufacturer_Click(object sender, RoutedEventArgs e)
        {
            var removing = dgSuppliers.SelectedItems.Cast<Manufacturer>().ToList();
            if (MessageBox.Show($"Вы уверенны, что хотите удалить выбранные записи?", "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    Entities.GetContext().Manufacturer.RemoveRange(removing);
                    Entities.GetContext().SaveChanges();
                    MessageBox.Show("Успешно удалено");

                    dgSuppliers.ItemsSource = Entities.GetContext().Manufacturer.ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void UpdateManufacturer()
        {
            var currentParts = Entities.GetContext().Manufacturer.ToList();
            currentParts = currentParts.Where(x => x.OrganizationName.ToLower().Contains(SelectedName.Text.ToLower())).ToList();
            dgSuppliers.ItemsSource = currentParts;
        }

        private void SelectedName_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateManufacturer();
        }

        private void btnClearFilter_Click(object sender, RoutedEventArgs e)
        {
            SelectedName.Clear();
            dgSuppliers.ItemsSource = Entities.GetContext().Manufacturer.ToList();
        }
    }
}

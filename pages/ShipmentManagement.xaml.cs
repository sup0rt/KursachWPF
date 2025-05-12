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
    /// Логика взаимодействия для ShipmentManagement.xaml
    /// </summary>
    public partial class ShipmentManagement : Page
    {
        public ShipmentManagement()
        {
            InitializeComponent();
            dgSuppliers.ItemsSource = Entities.GetContext().Shipment.ToList();
            SortPart.ItemsSource = Entities.GetContext().Part.ToList();
            SortSupplier.ItemsSource = Entities.GetContext().Supplier.ToList();
        }
        private void UpdateShipment()
        {
            var currentShipments = Entities.GetContext().Shipment.ToList();

            if (SortSupplier.SelectedItem != null)
            {
                currentShipments = currentShipments.Where(x => x.Supplier.OrganizationName.ToLower().Contains(SortPart.Text.ToLower())).ToList();
            }
            if (SortPart.SelectedItem != null)
            {
                currentShipments = currentShipments.Where(x => x.Part.PartName.ToLower().Contains(SortPart.Text.ToLower())).ToList();
            }

            dgSuppliers.ItemsSource = currentShipments;
        }

        private void btnClearFilter_Click(object sender, RoutedEventArgs e)
        {
            SortPart.SelectedItem = null;
            SortSupplier.SelectedItem = null;
            dgSuppliers.ItemsSource = Entities.GetContext().Shipment.ToList();
        }

        private void addEmployee_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ShipmentCreation(null));
        }

        private void editEmployee_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ShipmentCreation(dgSuppliers.SelectedItem as Shipment));
        }

        private void deleteEmployee_Click(object sender, RoutedEventArgs e)
        {
            var removing = dgSuppliers.SelectedItems.Cast<Shipment>().ToList();
            if (MessageBox.Show($"Вы уверенны, что хотите удалить выбранные записи?", "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    Entities.GetContext().Shipment.RemoveRange(removing);
                    Entities.GetContext().SaveChanges();
                    MessageBox.Show("Успешно удалено");

                    dgSuppliers.ItemsSource = Entities.GetContext().Shipment.ToList();
                    UpdateShipment();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void goBackbtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new EmployeePanel());
        }

        private void btnSearchFilter_Click(object sender, RoutedEventArgs e)
        {
            UpdateShipment();
        }
    }
}

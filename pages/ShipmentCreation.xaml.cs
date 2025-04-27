using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Security.Principal;
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
    /// Логика взаимодействия для ShipmentCreation.xaml
    /// </summary>
    public partial class ShipmentCreation : Page
    {
        public ShipmentCreation(Shipment selectedShipment)
        {
            InitializeComponent();
            partCmB.ItemsSource = Entities.GetContext().Part.ToList();
            supplierCmB.ItemsSource = Entities.GetContext().Supplier.ToList();

            _shipment = selectedShipment ?? new Shipment();
            DataContext = _shipment;
        }
        private Shipment _shipment = new Shipment();
        private void goBackbtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void addEmployee_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (_shipment.Supplier == null) errors.AppendLine("Выберите поставщика");
            if (_shipment.Part == null) errors.AppendLine("Выберите запчасть");
            if (_shipment.Quantity <= 0) errors.AppendLine("Введите количество запчасти");
            try
            {
                Convert.ToInt32(_shipment.Quantity);
            }
            catch
            {
                errors.AppendLine("Введите количество запчасти целым числом");
            }

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            try
            {
                var context = Entities.GetContext();

                _shipment.ShipmentDate = DateTime.Now;

                if (_shipment.ShipmentID == 0)
                {
                    context.Shipment.Add(_shipment);
                    context.SaveChanges();
                }
                else
                {
                    context.Entry(_shipment).State = System.Data.Entity.EntityState.Modified;
                }

                context.SaveChanges();
                MessageBox.Show("Поставка успешно добавлена", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                NavigationService.GoBack();
            }
            catch (DbEntityValidationException ex)
            {
                var errorMessages = ex.EntityValidationErrors
                .SelectMany(x => x.ValidationErrors)
                .Select(x => x.ErrorMessage);

                string fullErrorMessage = string.Join("\n", errorMessages);
                MessageBox.Show("Ошибки валидации:\n" + fullErrorMessage, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

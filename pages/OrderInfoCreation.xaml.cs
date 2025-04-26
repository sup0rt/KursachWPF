using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Data.SqlTypes;
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
    /// Логика взаимодействия для OrderInfoCreation.xaml
    /// </summary>
    public partial class OrderInfoCreation : Page
    {
        public OrderInfoCreation(Order thisOrder)
        {
            InitializeComponent();

            _order = thisOrder ?? new Order();
            DataContext = _order;

            partCmB.ItemsSource = Entities.GetContext().Part.ToList();
        }
        private Order _order = new Order();
        private OrderDetail _orderDetail = new OrderDetail();

        private void addPartsBtn_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (partCmB.SelectedItem == null) errors.AppendLine("Выберите запчасть");
            if (string.IsNullOrEmpty(quantity.Text)) errors.AppendLine("Выберите статус");
            if (!int.TryParse(quantity.Text, out int enteredQuantity) || enteredQuantity <= 0)errors.AppendLine("Введите корректное количество");


            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            try
            {
                _orderDetail.OrderID = _order.OrderID;
                _orderDetail.PartID = ((Part)partCmB.SelectedItem).PartID;
                _orderDetail.Quantity = Convert.ToInt32(quantity.Text);
                var context = Entities.GetContext();

                if (_orderDetail.OrderDetailID == 0)
                {
                    context.OrderDetail.Add(_orderDetail);
                }
                else
                {
                    context.Entry(_orderDetail).State = System.Data.Entity.EntityState.Modified;
                }

                context.SaveChanges();
                MessageBox.Show("Деталь добавлена в заказ", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                quantity.Clear();
                partCmB.SelectedItem = null;
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

        private void saveInfoBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new OrdersPage());
        }
    }
}

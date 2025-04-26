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
    /// Логика взаимодействия для OrdersPage.xaml
    /// </summary>
    public partial class OrdersPage : Page
    {
        public OrdersPage()
        {
            InitializeComponent();
            
            dgSuppliers.ItemsSource = Entities.GetContext().Order.ToList();
        }
        
        private void createOrderBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new OrderCreation(null));
        }

        private void editOrderBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new OrderCreation(dgSuppliers.SelectedItem as Order));
        }

        private void deleteOrderBtn_Click(object sender, RoutedEventArgs e)
        {
            var removing = dgSuppliers.SelectedItems.Cast<Order>().ToList();
            if (MessageBox.Show($"Вы уверенны, что хотите удалить выбранные записи?", "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    Entities.GetContext().Order.RemoveRange(removing);
                    Entities.GetContext().SaveChanges();
                    MessageBox.Show("Успешно удалено");

                    dgSuppliers.ItemsSource = Entities.GetContext().Order.ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void orderInfo_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new OrderInfo((sender as Button).DataContext as Order));
        }

        private void goBackbtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}

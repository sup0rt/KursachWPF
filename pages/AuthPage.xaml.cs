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
    /// Логика взаимодействия для AuthPage.xaml
    /// </summary>
    public partial class AuthPage : Page
    {
        public AuthPage()
        {
            InitializeComponent();
        }

        private void btnOrders_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Orders());
        }

        private void btnCatalog_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Catalog());
        }

        private void btnDealers_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Dealers());
        }

        private void btnSuppliers_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Manufacturers());
        }

        private void goBackbtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}

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
    /// Логика взаимодействия для AdminPanel.xaml
    /// </summary>
    public partial class AdminPanel : Page
    {
        public AdminPanel()
        {
            InitializeComponent();
        }

        private void employeeBTN_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AdminEmployeePage());
        }

        private void categoryBTN_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AdminCategoryPage());
        }

        private void manufacturerBTN_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AdminManufacturerPage());
        }

        private void supplierBTN_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AdminSupplierPage());
        }

        private void partBTN_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AdminPartPage());
        }

        private void goBackbtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}

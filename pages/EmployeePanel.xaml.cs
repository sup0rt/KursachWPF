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
    /// Логика взаимодействия для EmployeePanel.xaml
    /// </summary>
    public partial class EmployeePanel : Page
    {
        public EmployeePanel()
        {
            InitializeComponent();
        }

        private void ordersBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new OrdersPage());
        }

        private void statisticsBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new StatisticsPage());
        }

    }
}

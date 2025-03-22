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
    /// Логика взаимодействия для Dealers.xaml
    /// </summary>
    public partial class Dealers : Page
    {
        public Dealers()
        {
            InitializeComponent();
            dgSuppliers.ItemsSource= Entities.GetContext().Supplier.ToList();
        }
    }
}

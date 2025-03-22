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
    /// Логика взаимодействия для Catalog.xaml
    /// </summary>
    public partial class Catalog : Page
    {
        public Catalog()
        {
            InitializeComponent();
<<<<<<< HEAD
            dgParts.ItemsSource = Entities.GetContext().Part.ToList();
=======
            var currentPart = Entities.GetContext().SparePart.ToList();
            lvParts.ItemsSource = currentPart;
>>>>>>> e21f78e5adc016e9fe9bfa90735b8393c8197e1c
        }
    }
}

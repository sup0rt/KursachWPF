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
            var currentPart = Entities.GetContext().Part.ToList();
            lvParts.ItemsSource = currentPart;
        }

        private void UpdateParts()
        {
            var currentParts = Entities.GetContext().Part.ToList();
            currentParts = currentParts.Where(x => x.PartName.ToLower().Contains(SelectedName.Text.ToLower())).ToList();
            if (SortCategory.SelectedIndex == 0)
            {
                currentParts = currentParts.Where(x => x.Category.CategoryName.ToLower().Contains(SortCategory.Text.ToLower())).ToList();
            }
            if (SortManufacturer.SelectedIndex == 0)
            {
                currentParts = currentParts.Where(x => x.Manufacturer.OrganizationName.ToLower().Contains(SortManufacturer.Text.ToLower())).ToList();
            }
            lvParts.ItemsSource = currentParts;
        }

        private void SelectedName_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateParts();
        }

        private void SortCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateParts();
        }

        private void SortManufacturer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateParts();
        }


        private void btnClearFilter_Click(object sender, RoutedEventArgs e)
        {
            SelectedName.Clear();
            SortManufacturer.SelectedItem = null;
            SortCategory.SelectedItem = null;
            lvParts.ItemsSource = Entities.GetContext().Part.ToList();
        }

        private void goBackbtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void addToOrder_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

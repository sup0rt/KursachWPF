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
    /// Логика взаимодействия для AdminPartPage.xaml
    /// </summary>
    public partial class AdminPartPage : Page
    {
        public AdminPartPage()
        {
            InitializeComponent();
            var currentPart = Entities.GetContext().Part.ToList();
            lvParts.ItemsSource = currentPart;
        }

        private void addSparePart_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddSparePartPage(null));
        }

        private void editSparePart_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddSparePartPage((sender as Button).DataContext as Part));
        }
        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                Entities.GetContext().ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
                lvParts.ItemsSource = Entities.GetContext().Part.ToList();
            }
        }

        private void deleteSparePart_Click(object sender, RoutedEventArgs e)
        {
            var removing = lvParts.SelectedItems.Cast<Part>().ToList();
            if(MessageBox.Show($"Вы уверенны, что хотите удалить выбранные записи?", "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    Entities.GetContext().Part.RemoveRange(removing);
                    Entities.GetContext().SaveChanges();
                    MessageBox.Show("Успешно удалено");

                    lvParts.ItemsSource = Entities.GetContext().Part.ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}

﻿using System;
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
    /// Логика взаимодействия для AdminSupplierPage.xaml
    /// </summary>
    public partial class AdminSupplierPage : Page
    {
        public AdminSupplierPage()
        {
            InitializeComponent();
            dgSuppliers.ItemsSource=Entities.GetContext().Supplier.ToList();
        }

        private void addDealer_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddDealerPage(null));
        }

        private void editDealer_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddDealerPage((sender as Button).DataContext as Supplier));
        }
        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                Entities.GetContext().ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
                dgSuppliers.ItemsSource = Entities.GetContext().Supplier.ToList();
            }
        }

        private void deleteDealer_Click(object sender, RoutedEventArgs e)
        {
            var removing = dgSuppliers.SelectedItems.Cast<Supplier>().ToList();
            if (MessageBox.Show($"Вы уверенны, что хотите удалить выбранные записи?", "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    Entities.GetContext().Supplier.RemoveRange(removing);
                    Entities.GetContext().SaveChanges();
                    MessageBox.Show("Успешно удалено");

                    dgSuppliers.ItemsSource = Entities.GetContext().Supplier.ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Security.Principal;
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
    /// Логика взаимодействия для OrderCreation.xaml
    /// </summary>
    public partial class OrderCreation : Page
    {
        public OrderCreation(Order selectedOrder)
        {
            InitializeComponent();

            dgSuppliers.ItemsSource = Entities.GetContext().Customer.ToList();
            statusCmb.ItemsSource = Entities.GetContext().OrderStatus.ToList();

            _order = selectedOrder ?? new Order();
            DataContext = _order;
        }
        private Order _order = new Order();
        private void goBackbtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
        
        private void addOrder_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (dgSuppliers.SelectedItem == null) errors.AppendLine("Выберите клиента");
            if (dgSuppliers.SelectedItems.Count > 1) errors.AppendLine("Выберите только одного клиента");
            if (_order.OrderStatus == null) errors.AppendLine("Выберите статус");

           

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            try
            {
                _order.OrderDateTime = DateTime.Now;
                _order.CustomerID = ((Customer)dgSuppliers.SelectedItem).CustomerID;
                var context = Entities.GetContext();

                if (_order.OrderID == 0)
                {
                    context.Order.Add(_order);
                    context.SaveChanges();
                }
                else
                {
                    context.Entry(_order).State = System.Data.Entity.EntityState.Modified;
                }

                context.SaveChanges();
                MessageBox.Show("Заказ успешно создан", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                NavigationService.Navigate(new OrderInfoCreation(_order));
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
    }
}

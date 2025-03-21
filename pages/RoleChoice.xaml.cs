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
    /// Логика взаимодействия для RoleChoice.xaml
    /// </summary>
    public partial class RoleChoice : Page
    {
        public RoleChoice()
        {
            InitializeComponent();
        }



        private void employee_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AuthPageEmployee());
        }

        private void customer_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Registration());
        }

        private void admin_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AdminPanel());
        }
    }
}

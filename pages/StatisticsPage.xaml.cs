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
    /// Логика взаимодействия для StatisticsPage.xaml
    /// </summary>
    public partial class StatisticsPage : Page
    {
        public StatisticsPage()
        {
            InitializeComponent();
        }

        private void report1Btn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Report1());
        }

        private void report2Btn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Report2());
        }

        private void report3Btn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Report3());
        }

        private void report4Btn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Report4());
        }

        private void report5Btn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Report5());
        }

        private void goBackbtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new EmployeePanel());
        }
    }
}

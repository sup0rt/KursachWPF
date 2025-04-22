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
    /// Логика взаимодействия для AdminEmployeePage.xaml
    /// </summary>
    public partial class AdminEmployeePage : Page
    {
        public AdminEmployeePage()
        {
            InitializeComponent();
            dgSuppliers.ItemsSource = Entities.GetContext().Employee.ToList();
            SortDepartment.ItemsSource = Entities.GetContext().Department.ToList();
            SortPosition.ItemsSource = Entities.GetContext().Position.ToList();
        }

        private void addEmployee_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddEmployee(null));
        }

        private void editEmployee_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddEmployee((sender as Button).DataContext as Employee));
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if(Visibility == Visibility.Visible)
            {
                Entities.GetContext().ChangeTracker.Entries().ToList().ForEach(x=>x.Reload());
                dgSuppliers.ItemsSource= Entities.GetContext().Employee.ToList();
            }
        }

        private void deleteEmployee_Click(object sender, RoutedEventArgs e)
        {
            var removing = dgSuppliers.SelectedItems.Cast<Employee>().ToList();
            if (MessageBox.Show($"Вы уверенны, что хотите удалить выбранные записи?", "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    Entities.GetContext().Employee.RemoveRange(removing);
                    Entities.GetContext().SaveChanges();
                    MessageBox.Show("Успешно удалено");

                    dgSuppliers.ItemsSource = Entities.GetContext().Employee.ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void UpdateEmployee()
        {
            var currentParts = Entities.GetContext().Employee.ToList();
            currentParts = currentParts.Where(

            x=>(x.FirstName + " " + x.MiddleName + " " + x.LastName).ToLower().Contains(SelectedName.Text.ToLower()) ||
            (x.LastName + " " + x.FirstName + " " + x.MiddleName).ToLower().Contains(SelectedName.Text.ToLower()) || 
            x.LastName.ToLower().Contains(SelectedName.Text.ToLower()) ||
            x.MiddleName.ToLower().Contains(SelectedName.Text.ToLower()) || 
            x.FirstName.ToLower().Contains(SelectedName.Text.ToLower())


            ).ToList();
            if(SortDepartment.SelectedIndex == 0)
            {
                currentParts = currentParts.Where(x => x.Department.DepartmentName.ToLower().Contains(SortDepartment.Text.ToLower())).ToList();
            }
            if (SortPosition.SelectedIndex == 0)
            {
                currentParts = currentParts.Where(x => x.Position.PositionName.ToLower().Contains(SortPosition.Text.ToLower())).ToList();
            }

            dgSuppliers.ItemsSource = currentParts;
        }

        private void SelectedName_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateEmployee();
        }

        private void SortPosition_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateEmployee();
        }

        private void SortDepartment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateEmployee();
        }

        private void btnClearFilter_Click(object sender, RoutedEventArgs e)
        {
            SelectedName.Clear();
            SortPosition.SelectedItem = null;
            SortDepartment.SelectedItem = null;
            dgSuppliers.ItemsSource = Entities.GetContext().Employee.ToList();
        }

        private void goBackbtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}

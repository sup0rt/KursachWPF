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
    /// Логика взаимодействия для AuthPageEmployee.xaml
    /// </summary>
    public partial class AuthPageEmployee : Page
    {
        public AuthPageEmployee()
        {
            InitializeComponent();
        }

        private void signInButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(loginInTB.Text) | string.IsNullOrEmpty(passwordInTB.Password))
            {
                MessageBox.Show("Введите логин и пароль");
                return;
            }
            var employeeAccount = Entities.GetContext().EmployeeAccount.AsNoTracking().FirstOrDefault(ea => ea.Username == loginInTB.Text);
            bool isValid = PasswordHasher.VerifyPassword(passwordInTB.Password, employeeAccount.Password, employeeAccount.Salt);
            if (!isValid)
            {
                MessageBox.Show("Неверный логин или пароль");
                return;
            }
            else
            {
                loginInTB.Clear();
                passwordInTB.Clear();
                NavigationService.Navigate(new WarehouseDetails());
            }
        }

        private void goBackbtn_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show($"Вы уверенны, что хотите выйти из программы?", "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }
    }
}

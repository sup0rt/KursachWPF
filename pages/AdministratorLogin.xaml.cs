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
    /// Логика взаимодействия для AdministratorLogin.xaml
    /// </summary>
    public partial class AdministratorLogin : Page
    {
        public AdministratorLogin()
        {
            InitializeComponent();
        }

        private void goBackbtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void signInButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(loginTB.Text) | string.IsNullOrEmpty(passwordTB.Password))
            {
                MessageBox.Show("Введите логин и пароль");
                return;
            }
            var adminAccount = Entities.GetContext().AdminAccount.AsNoTracking().FirstOrDefault(aa => aa.Username == loginTB.Text);
            bool isValid = PasswordHasher.VerifyPassword(passwordTB.Password, adminAccount.Password, adminAccount.Salt);
            if (!isValid)
            {
                MessageBox.Show("Неверный логин или пароль");
                return;
            }
            else
            {
                loginTB.Clear();
                passwordTB.Clear();
                codeTB.Clear();
                NavigationService.Navigate(new AdminPanel());
            }
        }
    }
}

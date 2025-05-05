using System;
using System.Collections.Generic;
using System.Data.Entity;
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
        private int counter = 0;
        private void goBackbtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
        private async Task<bool> ValidateCode(string code)
        {
            try
            {
                var currentTime = DateTime.Now;

                return await Entities.GetContext().AccessCode.AnyAsync(ac => ac.AccessCode1 == code && currentTime <= ac.ExpirationDate);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }
        private async void signInButton_Click(object sender, RoutedEventArgs e)
        {
            if(counter == 3)
            {
                MessageBox.Show("Превышено количество попыток ввода кода. Приложение закрывается");
                Application.Current.Shutdown();
            }
            if (string.IsNullOrWhiteSpace(loginTB.Text) | string.IsNullOrEmpty(passwordTB.Password))
            {
                MessageBox.Show("Введите логин и пароль");
                return;
            }
            var adminAccount = Entities.GetContext().AdminAccount.AsNoTracking().FirstOrDefault(aa => aa.Username == loginTB.Text);
            if (adminAccount == null)
            {
                MessageBox.Show("Пользователь с такими данными не найден");
                return;
            }
            bool isValid = PasswordHasher.VerifyPassword(passwordTB.Password, adminAccount.Password, adminAccount.Salt);
            if (!isValid)
            {
                MessageBox.Show("Неверный логин или пароль");
                return;
            }
            if (string.IsNullOrEmpty(codeTB.Text))
            {
                MessageBox.Show("Введите код");
                return;
            }
            isValid = await ValidateCode(codeTB.Text);
            if (!isValid)
            {
                MessageBox.Show("Неверный код");
                counter++;
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

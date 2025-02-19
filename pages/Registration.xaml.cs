using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для Registration.xaml
    /// </summary>
    public partial class Registration : Page
    {
        public Registration()
        {
            InitializeComponent();
        }

        public static bool isValidMail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return false;
            }

            try
            {
                MailAddress address = new MailAddress(email);
                return address.Address == email;
            }
            catch (FormatException)
            {
                return false;
            }
        }
        public static string GetHash(string password)
        {
            using (var hash = SHA1.Create())
            {
                return string.Concat(hash.ComputeHash(Encoding.UTF8.GetBytes(password)).Select(x => x.ToString("X2")));
            }
        }

        private void signUpButton_Click(object sender, RoutedEventArgs e)
        {
            if (loginTB.Text.Length > 0)
            {
                using (var db = new Entities())
                {
                    var customer = db.Customers.AsNoTracking().FirstOrDefault(c => c.login == loginTB.Text);
                    if (customer != null) { MessageBox.Show("Пользователь с такими данными уже существует"); return; }
                }
                bool en = true;
                bool number = false;
                for (int i = 0; i < passwordTB.Text.Length; i++)
                {
                    if (passwordTB.Text[i] >= 'А' && passwordTB.Text[i] <= 'Я') en = false;
                    if (passwordTB.Text[i] >= '0' && passwordTB.Text[i] <= '9') number = true;
                }
                var regex = new Regex(@"^((\+7))\d{10}$");

                StringBuilder errors = new StringBuilder();

                if (passwordTB.Text.Length < 6) errors.AppendLine("Пароль дольжен быть больше 6 символов");
                if (!regex.IsMatch(phoneTB.Text)) errors.AppendLine("Укажите номер телефона в формате +7хххххххххх");
                if (!en) errors.AppendLine("Пароль должен быть на английском языке");
                if (!number) errors.AppendLine("В пароле должна быть минимум 1 цифра");
                if (!isValidMail(emailTB.Text)) errors.AppendLine("Введите корректный email");

                if (errors.Length > 0)
                {
                    MessageBox.Show(errors.ToString());
                    return;
                }
                else
                {
                    try
                    {
                        Entities db = new Entities();
                        Customers customer = new Customers
                        {
                            FirstName = nameTB.Text,
                            MiddleName = middlenameTB.Text,
                            LastName = surnameTB.Text,
                            login = loginTB.Text,
                            password = GetHash(passwordTB.Text),
                            Email = emailTB.Text,
                            Phone = phoneTB.Text,
                            Address = adressTB.Text,
                        };
                        db.Customers.Add(customer);
                        db.SaveChanges();
                        nameTB.Clear();
                        middlenameTB.Clear();
                        surnameTB.Clear();
                        loginTB.Clear();
                        passwordTB.Clear();
                        emailTB.Clear();
                        phoneTB.Clear();
                        adressTB.Clear();
                        MessageBox.Show("Вы зарегистрировались", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                        NavigationService.Navigate(new AuthPage());
                    }
                    catch (DbEntityValidationException ex)
                    {
                        StringBuilder validationErrors = new StringBuilder();
                        foreach (var validationResult in ex.EntityValidationErrors)
                        {
                            validationErrors.AppendLine($"Entity: {validationResult.Entry.Entity.GetType().Name}");
                            foreach (var error in validationResult.ValidationErrors)
                            {
                                validationErrors.AppendLine($"  Property: {error.PropertyName}, Error: {error.ErrorMessage}");
                            }
                        }
                        MessageBox.Show($"Ошибка валидации:\n{validationErrors}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        MessageBox.Show("хэш: ", GetHash(passwordTB.Text));
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при регистрации: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Введите логин");
            }
        }

        private void signInButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(loginInTB.Text) | string.IsNullOrEmpty(passwordInTB.Password))
            {
                MessageBox.Show("Введите логин и пароль");
                return;
            }

            string _password = GetHash(passwordInTB.Password);
            using (var db = new Entities())
            {
                var customer = db.Customers.AsNoTracking().FirstOrDefault(c => c.login == loginInTB.Text && c.password == passwordInTB.Password);
                if (customer != null)
                {
                    MessageBox.Show("Клиент с такими данными не найден");
                    return;
                }
                loginInTB.Clear();
                passwordInTB.Clear();
                MessageBox.Show("Типо успешный вход");
                NavigationService.Navigate(new AuthPage());
            }
        }
    }
}

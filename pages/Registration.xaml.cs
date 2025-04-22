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
        

        private void signUpButton_Click(object sender, RoutedEventArgs e)
        {
            if (loginTB.Text.Length > 0)
            {
                using (var db = new Entities())
                {
                    var customer = db.CustomerAccount.AsNoTracking().FirstOrDefault(c => c.Username == loginTB.Text);
                    if (customer != null) { MessageBox.Show("Пользователь с такими данными уже существует"); return; }
                }
                bool en = true;
                bool number = false;
                for (int i = 0; i < passwordTB.Password.Length; i++)
                {
                    if (passwordTB.Password[i] >= 'А' && passwordTB.Password[i] <= 'Я') en = false;
                    if (passwordTB.Password[i] >= '0' && passwordTB.Password[i] <= '9') number = true;
                }
                var regex = new Regex(@"^((\+7))\d{10}$");

                StringBuilder errors = new StringBuilder();

                if (passwordTB.Password.Length < 6) errors.AppendLine("Пароль дольжен быть больше 6 символов");
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
                        Customer customer = new Customer
                        {
                            LastName = lastNameTB.Text,
                            FirstName = nameTB.Text,
                            MiddleName = middlenameTB.Text,
                            Email= emailTB.Text,  
                            PhoneNumber = phoneTB.Text,   
                            DeliveryAddress = adressTB.Text,
                        };
                        CustomerAccount customerAccount = new CustomerAccount
                        {
                           
                            Username = loginTB.Text,
                            Password = PasswordHasher.CreateHash(passwordTB.Password, out string salt),
                            Salt = salt,
                        };
                        db.Customer.Add(customer);
                        db.CustomerAccount.Add(customerAccount);
                        db.SaveChanges();
                        nameTB.Clear();
                        middlenameTB.Clear();
                        lastNameTB.Clear();
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
            var customerAccount = Entities.GetContext().CustomerAccount.AsNoTracking().FirstOrDefault(ca => ca.Username == loginInTB.Text);
            bool isValid = PasswordHasher.VerifyPassword(passwordInTB.Password, customerAccount.Password, customerAccount.Salt);
            if (!isValid)
            {
                MessageBox.Show("Неверный логин или пароль");
                return;
            }
            else
            {
                loginInTB.Clear();
                passwordInTB.Clear();
                NavigationService.Navigate(new AuthPage());
            }  
        }

        private void goBackbtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}

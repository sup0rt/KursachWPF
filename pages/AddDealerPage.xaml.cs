using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
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
    /// Логика взаимодействия для AddDealerPage.xaml
    /// </summary>
    public partial class AddDealerPage : Page
    {
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

        public AddDealerPage(Supplier selectedSupplier)
        {
            InitializeComponent();
            _supplier = selectedSupplier ?? new Supplier();
            DataContext = _supplier;
        }
        private Supplier _supplier = new Supplier();

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            var regex = new Regex(@"^((\+7))\d{10}$");
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrEmpty(_supplier.OrganizationName)) errors.AppendLine("Введите название организации");
            if (string.IsNullOrEmpty(_supplier.PhysicalAddress)) errors.AppendLine("Введите адрес организаци");
            if (string.IsNullOrEmpty(_supplier.Email)) errors.AppendLine("Введите email организации");
            if (string.IsNullOrEmpty(_supplier.PhoneNumber)) errors.AppendLine("Введите контактный номер организации");
            if (!regex.IsMatch(_supplier.PhoneNumber)) errors.AppendLine("Укажите номер телефона в формате +7хххххххххх");
            if (!isValidMail(_supplier.Email)) errors.AppendLine("Введите корректный email");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            try
            {
                var context = Entities.GetContext();
                if (_supplier.SupplierID == 0)
                {
                    context.Supplier.Add(_supplier);
                }
                else
                {
                    context.Entry(_supplier).State = System.Data.Entity.EntityState.Modified;
                }

                context.SaveChanges();
                MessageBox.Show("Организация добавлена", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                {
                    MessageBox.Show("Ошибка: " + ex.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void goBackbtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}

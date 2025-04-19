using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Security.Principal;
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
    /// Логика взаимодействия для AddEmployee.xaml
    /// </summary>
    public partial class AddEmployee : Page
    {
        public AddEmployee(Employee selectedEmployee)
        {
            InitializeComponent();
            departmentsCmB.ItemsSource = Entities.GetContext().Department.ToList();
            positionCmB.ItemsSource = Entities.GetContext().Position.ToList();
            
            _employee = selectedEmployee ?? new Employee();
            DataContext = _employee;
        }
        private Employee _employee = new Employee();
        private EmployeeAccount _account = new EmployeeAccount();
        private void addEmployee_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrEmpty(_employee.FirstName)) errors.AppendLine("Введите имя сотрудника");
            if (string.IsNullOrEmpty(_employee.MiddleName)) errors.AppendLine("Введите отчество сотрудника");
            if (string.IsNullOrEmpty(_employee.LastName)) errors.AppendLine("Введите фамилию сотрудника");
            if (string.IsNullOrEmpty(loginTB.Text)) errors.AppendLine("Введите логин сотрудника");
            if (string.IsNullOrEmpty(passwordTB.Password)) errors.AppendLine("Введите пароль сотрудника");
            if (_employee.Department == null) errors.AppendLine("Выберите отдел сотрудника");
            if (_employee.Position == null) errors.AppendLine("Выберите должность сотрудника");
            if (_employee.Salary <= 0) errors.AppendLine("Введите зарплату");

            if (passwordTB.Password.Length > 0)
            {
                bool en = true;
                bool number = false;
                for (int i = 0; i < passwordTB.Password.Length; i++)
                {
                    if (passwordTB.Password[i] >= 'А' && passwordTB.Password[i] <= 'Я') en = false;
                    if (passwordTB.Password[i] >= '0' && passwordTB.Password[i] <= '9') number = true;
                }

                if (passwordTB.Password.Length < 6) errors.AppendLine("Пароль должен быть больше 6 символов");
                if (!en) errors.AppendLine("Пароль должен быть на английском языке");
                if (!number) errors.AppendLine("В пароле должна быть минимум 1 цифра");
            }

            if (loginTB.Text.Length > 0)
            {
                using (var db = new Entities())
                {
                    var employee = db.EmployeeAccount.AsNoTracking().FirstOrDefault(em => em.Username == loginTB.Text);
                    if (employee != null) errors.AppendLine("Пользователь с таким логином уже существует");
                }
            }

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            try
            {
                var context = Entities.GetContext();

                _account.Username = loginTB.Text;
                _account.Password = PasswordHasher.CreateHash(passwordTB.Password, out string salt);
                _account.Salt = salt;

                if (_employee.EmployeeID == 0)
                {
                    context.Employee.Add(_employee);
                    context.SaveChanges(); 

                    _account.EmployeeID = _employee.EmployeeID;
                    context.EmployeeAccount.Add(_account);
                }
                else
                {
                    _account.EmployeeID = _employee.EmployeeID;
                    context.Entry(_employee).State = System.Data.Entity.EntityState.Modified;
                    context.Entry(_account).State = System.Data.Entity.EntityState.Modified;
                }

                context.SaveChanges();
                MessageBox.Show("Сотрудник успешно добавлен", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                NavigationService.GoBack();
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

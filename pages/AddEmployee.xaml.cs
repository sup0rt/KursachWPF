using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
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
        public AddEmployee()
        {
            InitializeComponent();
            departmentsCmB.ItemsSource = Entities.GetContext().Department.ToList();
            positionCmB.ItemsSource = Entities.GetContext().Position.ToList();
        }

        private void addEmployee_Click(object sender, RoutedEventArgs e)
        {
            if (loginTB.Text.Length > 0)
            {
                using (var db = new Entities())
                {
                    var customer = db.CustomerAccount.AsNoTracking().FirstOrDefault(c => c.login == loginTB.Text);
                    if (customer != null) { MessageBox.Show("Пользователь с такими данными уже существует"); return; }
                }
                bool en = true;
                bool number = false;
                for (int i = 0; i < passwordTB.Password.Length; i++)
                {
                    if (passwordTB.Password[i] >= 'А' && passwordTB.Password[i] <= 'Я') en = false;
                    if (passwordTB.Password[i] >= '0' && passwordTB.Password[i] <= '9') number = true;
                }

                StringBuilder errors = new StringBuilder();

                if (passwordTB.Password.Length < 6) errors.AppendLine("Пароль дольжен быть больше 6 символов");
                if (!en) errors.AppendLine("Пароль должен быть на английском языке");
                if (!number) errors.AppendLine("В пароле должна быть минимум 1 цифра");

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
                        
                        var position = db.Position.AsNoTracking().FirstOrDefault(p => p.name == positionCmB.SelectedValue.ToString());
                        var department = db.Department.AsNoTracking().FirstOrDefault(d => d.name == departmentsCmB.SelectedValue.ToString());
                        Employee employee = new Employee
                        {
                            lastName = lastNameTB.Text,
                            firstName = nameTB.Text,
                            middleName = middlenameTB.Text,
                            positionID = position.IDposition,
                            departmentID = department.IDdepartment,
                            salary = Convert.ToDecimal(salaryTB.Text),
                        };
                        EmployeeAccount employeeAccount = new EmployeeAccount
                        {

                            login = loginTB.Text,
                            passwordHash = PasswordHasher.CreateHash(passwordTB.Password, out string salt),
                            salt = salt,
                        };
                        db.Employee.Add(employee);
                        db.EmployeeAccount.Add(employeeAccount);
                        db.SaveChanges();
                        MessageBox.Show("Сотрудник добавлен", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                        NavigationService.GoBack();
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
    }
}

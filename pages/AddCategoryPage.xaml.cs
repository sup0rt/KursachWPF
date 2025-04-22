using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
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
    /// Логика взаимодействия для AddCategoryPage.xaml
    /// </summary>
    public partial class AddCategoryPage : Page
    {
        public AddCategoryPage(Category selectdCategory = null)
        {
            InitializeComponent();
            _category = selectdCategory ?? new Category();
            DataContext = _category;

        }
        private Category _category = new Category();
        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrEmpty(_category.CategoryName)) errors.AppendLine("Введите название категории");
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            try
            {
                var context = Entities.GetContext();
                if (_category.CategoryID == 0)
                {
                    context.Category.Add(_category);
                }
                else
                {
                    context.Entry(_category).State = System.Data.Entity.EntityState.Modified;
                }

                context.SaveChanges();
                MessageBox.Show("Категория добавлена", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
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

using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using ООО__Валимо_.Classes;
using ООО__Валимо_;

namespace ООО__Валимо_
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool isShowPass = false;
        string answerCapcha = "";
        bool isIncorrect = false, freeze = false;

        public MainWindow()
        {
            InitializeComponent();
            try
            {
                Helper.DB = new Entity.DBValimo();
            }
            catch
            {
                MessageBox.Show("БД отстутствует.");
                return;
            }
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Переход в каталог после авторизации
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ButtonAutorization_Click(object sender, RoutedEventArgs e)
        {
            string login = tbLogin.Text,
                password = bPassword.Password;
            StringBuilder sb = new StringBuilder();
            if (String.IsNullOrEmpty(login))
            {
                sb.AppendLine("Вы забыли ввести логин!");
            }
            if (String.IsNullOrEmpty(password))
            {
                sb.AppendLine("Вы забыли ввести пароль!");
            }
            if (sb.Length > 0)
            {
                MessageBox.Show(sb.ToString());
                return;
            }

            List<Entity.User> users = Helper.DB.User.ToList();

            bool flag = false;
            foreach (Entity.User user in users)
            {
                if (user.login == login && user.password == password)
                {
                    Helper.User = user;
                    flag = true;
                    break;
                }
            }
            if (!flag)
            {
                MessageBox.Show("Пользователь не найден!");
                return;
            }
            openCatalog();
        }

        /// <summary>
        /// Переход в каталог для Гостя
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonGuest_Click(object sender, RoutedEventArgs e)
        {
            openCatalog();
        }

        void openCatalog()
        {
            Catalog catalog = new Catalog();
            Hide();
            catalog.ShowDialog();
            Show();
        }

        private void showPass()
        {
            txtPassword.Text = bPassword.Password;
            bPassword.Visibility = Visibility.Hidden;
            txtPassword.Visibility = Visibility.Visible;
            Debug.WriteLine("1234");

        }
        private void hidePass()
        {
            txtPassword.Visibility = Visibility.Hidden;
            bPassword.Visibility = Visibility.Visible;
        }
        private void btnShowPass_Click(object sender, RoutedEventArgs e)
        {
            if (isShowPass)
            {
                isShowPass = false;
                hidePass();
            }
            else
            {
                isShowPass = true;
                showPass();
            }
        }
    }
}

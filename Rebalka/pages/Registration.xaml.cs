using System;
using System.Collections.Generic;
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
using System.Xaml;

namespace Rebalka
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

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            FrameClass.frame.Navigate(new Login());
        }

        private void BtnRegistration_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(TBSurname.Text.Replace(" ", "").Length == 0)
                {
                    MessageBox.Show("Фамилия не указана");
                    return;
                }
                if (TBName.Text.Replace(" ", "").Length == 0)
                {
                    MessageBox.Show("Имя не указано");
                    return;
                }
                if (TBPatronomic.Text.Replace(" ", "").Length == 0)
                {
                    MessageBox.Show("Отчество не указано");
                    return;
                }
                //Regex regex = new Regex("((?= ^[8])(.{ 11}$))| (?= ^\\+? (7))(.{ 12})$"); // Регулярное выражение для телефона
                //DateTime dateTime = new DateTime();
                //Console.WriteLine(dateTime); // 01.01.0001 0:00:00
                //Console.WriteLine(DateTime.Now); // Текущая дата и время
                //Console.WriteLine(DateTime.Today);
                //DateTime date1 = new DateTime(2015, 7, 20, 18, 30, 25);  // 20.07.2015 18:30:25
                //Console.WriteLine(date1.AddHours(-3)); // 20.07.2015 15:30:25
                //public TimeSpan (int hours, int minutes, int seconds);
                //TimeSpan.FromDays(10) - TimeSpan.FromSeconds(1); // 9.23:59:59
                Regex regex = new Regex("(?=.*[0-9].*[0-9])(?=.*[a-zA-Z])(?=.*[!@#$&*])^(.{4,6})$"); // Регулярное выражение для проверки пароля (минимум 2 цифры, 1 латинская буква и один спец символ (символов от 4 до 6))
                if (regex.IsMatch(TBPassword.Text) == false)
                {
                    MessageBox.Show("Пароль не соответствует требованиям");
                    return;
                }
                if (Base.date.User.Where(x => x.UserLogin == TBLogin.Text).ToList().Count > 0)
                {
                    MessageBox.Show("Пользователь с таким логиным уже есть");
                    return;
                }
                User user = new User();
                user.UserSurname = TBSurname.Text;
                user.UserName = TBName.Text;
                user.UserPatronymic = TBPatronomic.Text;
                user.UserLogin = TBLogin.Text;
                user.UserPassword = TBPassword.Text;
                user.UserRole = 3;
                Base.date.User.Add(user);
                Base.date.SaveChanges();
                MessageBox.Show("Пользователь зарегистрирован");
                FrameClass.frame.Navigate(new Login());
            }
            catch
            {
                MessageBox.Show("При регистрации пользоватедля возникла ошибка!");
            }
        }
    }
}

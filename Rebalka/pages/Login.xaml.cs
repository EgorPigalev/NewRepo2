using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
using System.Windows.Threading;

namespace Rebalka
{
    /// <summary>
    /// Логика взаимодействия для Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        public static User user;
        public static bool correctValue;
        int kolError;
        int countTime;
        DispatcherTimer disTimer = new DispatcherTimer();

        public Login()
        {
            InitializeComponent();
            user = null;
            kolError = 0; // кол-во неудачных входов
            correctValue = false; // корректность ввода капчи
            disTimer.Interval = new TimeSpan(0, 0, 1); // интервал времени для таймера
            disTimer.Tick += new EventHandler(DisTimer_Tick);
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            User user1 = Base.date.User.FirstOrDefault(x => x.UserLogin == TBLogin.Text && x.UserPassword == TBPassword.Text);
            if (user1 != null)
            {
                if (kolError == 0)
                {
                    FrameClass.frame.Navigate(new ListProducts());
                }
                else
                {
                    correctValue = false;
                    CAPCHA captcha = new CAPCHA();
                    captcha.ShowDialog();
                    if (correctValue) // Если капча пройдена
                    {
                        user = user1;
                        FrameClass.frame.Navigate(new ListProducts());
                    }
                }
            }
            else
            {
                MessageBox.Show("Пользователь с таким логиным и паролем не найден!");
                correctValue = false;
                CAPCHA captcha = new CAPCHA();
                captcha.ShowDialog();
                kolError++;
                if (!correctValue) // Если капча не пройдена
                {
                    BtnLogin.IsEnabled = false;
                    countTime = 10;
                    tbNewCode.Text = "Повторно авторизоваться можно через " + countTime + " секунд";
                    tbNewCode.Visibility = Visibility.Visible;
                    disTimer.Start();
                }
            }
        }

        private void BtnGuest_Click(object sender, RoutedEventArgs e)
        {
            FrameClass.frame.Navigate(new ListProducts());
        }

        private void BtnRegistration_Click(object sender, RoutedEventArgs e)
        {
            FrameClass.frame.Navigate(new Registration());
        }

        private void DisTimer_Tick(object sender, EventArgs e)
        {
            if (countTime == 0) // Если 10 секунд закончились
            {
                BtnLogin.IsEnabled = true;
                disTimer.Stop();
                tbNewCode.Visibility = Visibility.Collapsed;
            }
            else
            {
                tbNewCode.Text = "Повторно авторизоваться можно через " + countTime + " секунд";
            }
            countTime--;
        }
    }
}

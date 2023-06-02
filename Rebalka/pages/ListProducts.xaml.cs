using System;
using System.Collections.Generic;
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

namespace Rebalka
{
    /// <summary>
    /// Логика взаимодействия для ListProducts.xaml
    /// </summary>
    public partial class ListProducts : Page
    {
        public ListProducts()
        {
            InitializeComponent();
            BtnAdd.Visibility = Visibility.Collapsed;
            if (Login.user != null)
            {
                TBUser.Text = Login.user.UserSurname + " " + Login.user.UserName + " " + Login.user.UserPatronymic;
                if (Login.user.Role.RoleName == "Администратор")
                {
                    BtnAdd.Visibility = Visibility.Visible;
                }
            }
            LVProducts.ItemsSource = Base.date.Product.ToList();
            TBCountField.Text = Base.date.Product.ToList().Count + " из " + Base.date.Product.ToList().Count;
            CBSort.SelectedIndex = 0;
            List<Manufacturers> manufacturers = Base.date.Manufacturers.ToList();
            CBFilt.Items.Add("Все производители");
            foreach(Manufacturers m in manufacturers)
            {
                CBFilt.Items.Add(m.ProductManufacturer);
            }
            CBFilt.SelectedIndex = 0;
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            FrameClass.frame.Navigate(new Login());
        }

        private void TBSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            Search();
        }

        private void CBSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Search();
        }

        private void Search()
        {
            List<Product> products = Base.date.Product.ToList();
            if(TBSearch.Text.Length > 0)
            {
                products = products.Where(x => x.ProductName.ToLower().Contains(TBSearch.Text.ToLower()) || x.ProductDescription.ToLower().Contains(TBSearch.Text.ToLower()) || x.Manufacturers.ProductManufacturer.ToLower().Contains(TBSearch.Text.ToLower())).ToList();
            }
            switch(CBSort.SelectedIndex)
            {
                case 1:
                    products = products.OrderBy(x => x.ProductCost).ToList();
                    break;
                case 2:
                    products = products.OrderByDescending(x => x.ProductCost).ToList();
                    break;
            }
            if(CBFilt.SelectedIndex > 0)
            {
                products = products.Where(x => x.Manufacturers.ProductManufacturer == CBFilt.SelectedValue).ToList();
            }
            LVProducts.ItemsSource = products;
            TBCountField.Text = products.Count + " из " + Base.date.Product.ToList().Count;
            if(products.Count == 0)
            {
                MessageBox.Show("Данные не найдены");
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            int index = Convert.ToInt32(button.Uid);
            if(Base.date.OrderProduct.Where(x => x.ProductID == index).ToList().Count == 0)
            {
                if(MessageBox.Show("Вы точно хотите удалить товар?", "Системное сообщение", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    try
                    {
                        Product product = Base.date.Product.FirstOrDefault(x => x.ProductID == index);
                        Base.date.Product.Remove(product);
                        Base.date.SaveChanges();
                        LVProducts.ItemsSource = Base.date.Product.ToList();
                        TBCountField.Text = Base.date.Product.ToList().Count + " из " + Base.date.Product.ToList().Count;
                        MessageBox.Show("Товар успешно удален");
                    }
                    catch
                    {
                        MessageBox.Show("При удаление товара возникла ошибка!");
                    }
                }
            }
            else
            {
                MessageBox.Show("Товар нельзя удалить так как с ним есть заказ");
            }
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            int index = Convert.ToInt32(button.Uid);
            Product product = Base.date.Product.FirstOrDefault( x => x.ProductID == index);
            FrameClass.frame.Navigate(new AddAndUpdate(product));
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            FrameClass.frame.Navigate(new AddAndUpdate());
        }

        private void BtnDelete_Loaded(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            button.Visibility = Visibility.Collapsed;
            if(Login.user != null)
            {
                if(Login.user.Role.RoleName == "Администратор")
                {
                    button.Visibility = Visibility.Visible;
                }
            }
        }

        private void BtnUpdate_Loaded(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            button.Visibility = Visibility.Collapsed;
            if (Login.user != null)
            {
                if (Login.user.Role.RoleName == "Администратор")
                {
                    button.Visibility = Visibility.Visible;
                }
            }
        }
    }
}

using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using static System.Net.Mime.MediaTypeNames;

namespace Rebalka
{
    /// <summary>
    /// Логика взаимодействия для AddAndUpdate.xaml
    /// </summary>
    public partial class AddAndUpdate : Page
    {
        Product product;
        bool newProduct;
        string image;
        public AddAndUpdate()
        {
            InitializeComponent();
            newProduct = true;
            BtnAddAndUpdate.Content = "Добавть";
            TBHeader.Text = "Добавление продукта";
            image = "\\resources\\picture.png";
            IMPhoto.Source = new BitmapImage(new Uri(image, UriKind.Relative));
            BtnUpdPhoto.Content = "Добавить";
            CreateField();
        }
        public AddAndUpdate(Product product)
        {
            InitializeComponent();
            newProduct = false;
            this.product = product;
            BtnAddAndUpdate.Content = "Изменить";
            TBHeader.Text = "Изменение продукта";
            SPID.Visibility = Visibility.Visible;
            TBID.Text = product.ProductID.ToString();
            CreateField();
            if(product.ProductPhoto != null)
            {
                image = product.ProductPhoto;
                IMPhoto.Source = new BitmapImage(new Uri(Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + image));
                BtnUpdPhoto.Content = "Изменить";
            }
            else
            {
                image = "\\resources\\picture.png";
                IMPhoto.Source = new BitmapImage(new Uri(image, UriKind.Relative));
                BtnUpdPhoto.Content = "Добавить";
            }
            TBName.Text = product.ProductName;
            TBDescription.Text = product.ProductDescription;
            CBCategoria.SelectedIndex = product.Categorys.ProductCategoryID - 1;
            CBUnit.SelectedIndex = product.Units.ProductUnitID - 1;
            CBSuplier.SelectedIndex = product.Supliers.ProductSuplierID - 1;
            TBCountInSclad.Text = product.ProductQuantityInStock.ToString();
            TBCost.Text = product.ProductCost.ToString();
        }

        private void CreateField()
        {
            CBCategoria.ItemsSource = Base.date.Categorys.ToList();
            CBCategoria.SelectedValuePath = "ProductCategoryID";
            CBCategoria.DisplayMemberPath = "ProductCategory";
            CBUnit.ItemsSource = Base.date.Units.ToList();
            CBUnit.SelectedValuePath = "ProductUnitID";
            CBUnit.DisplayMemberPath = "ProductUnit";
            CBSuplier.ItemsSource = Base.date.Supliers.ToList();
            CBSuplier.SelectedValuePath = "ProductSuplierID";
            CBSuplier.DisplayMemberPath = "ProductSuplier";
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            FrameClass.frame.Navigate(new ListProducts());
        }

        private void BtnAddAndUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (newProduct)
                {
                    product = new Product();
                    product.ProductID = Base.date.Product.Max(x => x.ProductID) + 1;
                }
                product.ProductName = TBName.Text;
                product.ProductCategoryID = CBCategoria.SelectedIndex + 1;
                product.ProductUnitID = CBUnit.SelectedIndex + 1;
                product.ProductSuplierID = CBSuplier.SelectedIndex + 1;
                product.ProductQuantityInStock = Convert.ToInt32(TBCountInSclad.Text);
                product.ProductCost = Convert.ToDouble(TBCost.Text);
                product.ProductDescription = TBDescription.Text;
                if (image == "\\resources\\picture.png")
                {
                    product.ProductPhoto = null;
                }
                else
                {
                    product.ProductPhoto = image;
                }
                if(newProduct)
                {
                    Base.date.Product.Add(product);
                }
                Base.date.SaveChanges();
                if (newProduct)
                {
                    MessageBox.Show("Товар успешно добавлен");
                }
                else
                {
                    MessageBox.Show("Товар успешно изменен");
                }
                FrameClass.frame.Navigate(new ListProducts());
            }
            catch
            {
                if(newProduct)
                {
                    MessageBox.Show("При добавление товара возникла ошибка");
                }
                else
                {
                    MessageBox.Show("При изменение товара возникла ошибка");
                }
            }
        }

        private void TBCountInSclad_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if(!(Char.IsDigit(e.Text, 0)))
            {
                e.Handled = true;
            }
        }

        private void TBCost_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!(Char.IsDigit(e.Text, 0)) && (e.Text != ","))
            {
                e.Handled = true;
            }
        }

        private void BtnDeletePhoto_Click(object sender, RoutedEventArgs e)
        {
            image = "\\resources\\picture.png";
            IMPhoto.Source = new BitmapImage(new Uri(image, UriKind.Relative));
            BtnUpdPhoto.Content = "Добавить";
        }

        private void BtnUpdPhoto_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string path;
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.ShowDialog();
                path = openFileDialog.FileName;
                if(path != null)
                {
                    string newFilePath = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "\\image\\" + System.IO.Path.GetFileName(path); // Путь куда копировать файл
                    if (!File.Exists(newFilePath)) // Проверка наличия картинки в папке
                    {
                        File.Copy(path, newFilePath);
                    }
                    image = "\\image\\" + System.IO.Path.GetFileName(path);
                    IMPhoto.Source = new BitmapImage(new Uri(Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + image));
                    BtnUpdPhoto.Content = "Изменить";
                }
            }
            catch
            {
                MessageBox.Show("При добавлении нового фото возникла ошибка!");
            }
        }
    }
}
using ООО__Валимо_.Entity;
using ООО__Валимо_.Classes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ООО__Валимо_
{
    /// <summary>
    /// Логика взаимодействия для Catalog.xaml
    /// </summary>
    public partial class Catalog : Window
    {

        int cntProducts, cntAllProducts;
        List<Product> products;
        List<string> genriesStr;
        List<Genre> genries;
        public Catalog()
        {
            InitializeComponent();
            updateListBox();
            genries = new List<Genre>();
            genriesStr = new List<string>();
            genriesStr.Add("По умолчанию");
            foreach (Genre genre in Helper.DB.Genre)
            {
                genriesStr.Add(genre.name);
                genries.Add(genre);
            }
            comboBoxCategory.ItemsSource = null;
            comboBoxCategory.ItemsSource = genriesStr;
            comboBoxCategory.SelectedIndex = 0;
            cntAllProducts = Helper.DB.Product.Count();
            tbResult.Text = $"Найдено {cntAllProducts} из {cntAllProducts}";
        }

        public void updateListBox()
        {
            products = new List<Product>();
            foreach (Product product in Helper.DB.Product)
            {
                if (product.image == null)
                    product.image = "../../Images/picture.png";
                else
                    product.image = "../../Images/" + product.image;
                
                product.costWithDis = product.cost * Convert.ToDecimal(product.discount);


                products.Add(product);
            }
            listBoxProducts.ItemsSource = products;
        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            Close(); 
        }

        private void applySort()
        {
            if (comboBoxCategory == null) return;
            sort();
            for (int i = 0; i < products.Count; i++)
            {
                switch (comboBoxCategory.SelectedIndex)
                {
                    case 1:
                        if (products[i].genre != genries[0].id)
                        {
                            products.RemoveAt(i); i--; continue;
                        }
                        break;
                    case 2:
                        if (products[i].genre != genries[1].id)
                        {
                            products.RemoveAt(i); i--; continue;
                        }
                        break;
                    case 3:
                        if (products[i].genre != genries[2].id)
                        {
                            products.RemoveAt(i); i--; continue;
                        }
                        break;
                }
                switch (comboBoxPrices.SelectedIndex)
                {
                    case 1:
                        if (products[i].discount >= 10)
                        {
                            products.RemoveAt(i); i--; continue;
                        }
                        break;
                    case 2:
                        if (products[i].discount < 10 || products[i].discount >= 15)
                        {
                            products.RemoveAt(i); i--; continue;
                        }
                        break;
                    case 3:
                        if (products[i].discount < 15)
                        {
                            products.RemoveAt(i); i--; continue;
                        }
                        break;
                }
            }
            string text = tbSearch.Text;
            if (text != "")
            {
                for (int i = 0; i < products.Count; i++)
                {
                    if (!products[i].name.Contains(text))
                    {
                        products.Remove(products[i]);
                        i--;
                    } 
                }
            }
            listBoxProducts.ItemsSource = products;
            cntProducts = products.Count;
            tbResult.Text = $"Найдено {cntProducts} из {cntAllProducts}";
        }

        public void sort()
        {
            updateListBox();
            switch (comboBoxSort.SelectedIndex)
            {
                case 1:
                    products.Sort(delegate(Product a, Product b) { return a.cost.CompareTo(b.cost); });
                    break;
                case 2:
                    products.Sort(delegate (Product a, Product b) { return a.cost.CompareTo(b.cost); });
                    products.Reverse();
                    break;
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            int index = listBoxProducts.SelectedIndex;
            if (index == -1) return;
            string command = (sender as MenuItem).Header as string;
            MessageBox.Show($"Выбран элемент с индексом: {index}, команда: {command}");
        }

        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            applySort();
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            applySort();
        }
    }
}

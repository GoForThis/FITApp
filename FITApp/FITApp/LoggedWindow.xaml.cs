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
using System.Windows.Shapes;

namespace FITApp
{
    /// <summary>
    /// Interaction logic for LoggedWindow.xaml
    /// </summary>
    public partial class LoggedWindow : Window
    {
        public LoggedWindow(string User)
        {
            InitializeComponent();
            UserDisplay.Content = User;
            DBEntities db = new DBEntities();
            var products = from d in db.Products
                        where d.User == User
                           select d;
            this.Grid_of_products.ItemsSource = products.ToList();
        }

        private void Click_View(object sender, RoutedEventArgs e)
        {
            DateTime dateTime = (DateTime)DateToView.SelectedDate;

            string User = (string)UserDisplay.Content;

            DBEntities db = new DBEntities();
            var products = from d in db.Products
                           where d.User == User && d.Date == dateTime
                           select d;
            this.Grid_of_products.ItemsSource = products.ToList();
        }

        private void Click_Add(object sender, RoutedEventArgs e)
        {
            DBEntities db = new DBEntities();
            Product productObject = new Product()
            {
                Name = Name_Add.Text,
                Calories = Calories_Add.Text,
                Comment = Comment_Add.Text,
                User = (string)UserDisplay.Content,
                Date = (DateTime)Date_Add.SelectedDate
            };
            db.Products.Add(productObject);
            db.SaveChanges();

            Name_Add.Text = "";
            Calories_Add.Text = "";
            Comment_Add.Text = "";
            
            
        }

        private int updatingProductID = 0;
        private void Grid_of_products_changed(object sender, SelectionChangedEventArgs e)
        {
            if (this.Grid_of_products.SelectedIndex >= 0)
            {
                if (this.Grid_of_products.SelectedItems.Count > 0)
                {
                    if (this.Grid_of_products.SelectedItems[0].GetType() == typeof(Product))
                    {
                        Product product = (Product)this.Grid_of_products.SelectedItems[0];
                        Name_Display.Content = product.Name;
                        Calories_Display.Content = product.Calories;
                        Comment_Display.Content = product.Comment;

                        updatingProductID = product.Id;
                    }
                }
            }
        }

        private void Click_Delete(object sender, RoutedEventArgs e)
        {
            DBEntities db = new DBEntities();

            var r = from d in db.Products
                    where d.Id == updatingProductID
                    select d;
            Product obj = r.SingleOrDefault();

            if (obj != null)
            {
                db.Products.Remove(obj);
                db.SaveChanges();
            }
            Grid_of_products.ItemsSource = db.Products.ToList();
        }
    }
}

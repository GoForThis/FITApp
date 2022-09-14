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
using System.Text.RegularExpressions;
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

            var goal = from d in db.Goals
                           where d.User == User 
                           select d;
            if (goal.Count() > 0)
            {
                Goal_Display.Content = goal;
            }
            else
            {
                Goal_Display.Content = "";
                Calories_NULL.Content = "";
            }
            ATE.Content = "";
            Refresh_Status();

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
            Refresh_Status();
        }

        private void Click_Add(object sender, RoutedEventArgs e)
        {
            DBEntities db = new DBEntities();
            int Calories = Int16.Parse(Calories_Add.Text);

            Product productObject = new Product()
            {
                Name = Name_Add.Text,
                Calories = Calories,
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

        private void Click_Change(object sender, RoutedEventArgs e)
        {
            int PPM = 0;
            int Weight = int.Parse(Regex.Replace(CBox_Weight.Items[CBox_Weight.SelectedIndex].ToString(), "[^0-9]", ""));
            int Height = int.Parse(Regex.Replace(CBox_Weight.Items[CBox_Height.SelectedIndex].ToString(), "[^0-9]", ""));
            int Age = int.Parse(Regex.Replace(CBox_Weight.Items[CBox_Age.SelectedIndex].ToString(), "[^0-9]", ""));
            if (RB_Male.IsChecked == true)
            {
                //string numberonly = Regex.Replace(CBox_Weight.Items[CBox_Weight.SelectedIndex].ToString(), "[^0-9]", "");
                //MessageBox.Show(numberonly);

                PPM = (int)(66.47 + (13.75 * Weight) + (5 * Height) - (6.75 * Age));
            }
            else if(RB_Female.IsChecked == true)
            {

                PPM = (int)(665.09 + (9.56 * Weight) + (1.85 * Height) - (4.67 * Age));
            }
            DBEntities db = new DBEntities();
            Goal goal = new Goal()
            {
                Calories = PPM,
                User = (string)UserDisplay.Content
            };
            db.Goals.Add(goal);
            db.SaveChanges();

            Goal_Display.Content = PPM;
            Calories_NULL.Content = "Calories";

        }
        public void Refresh_Status()
        {

            if(DateToView.SelectedDate.ToString() != "")
            {
                DBEntities db = new DBEntities();
                int sum = 0;
                DateTime dateTime = (DateTime)DateToView.SelectedDate;
                string User = (string)UserDisplay.Content;

                var r = from d in db.Products
                      where d.User == User && d.Date == dateTime
                      select d.Calories;

                foreach (var item in r)
                {
                    sum = (int)(sum + item);
                }
                ATE.Content = "You ate: " + sum + " Calories";
                
                if(Goal_Display.Content != "")
                {
                    int goal = Int16.Parse((string)Goal_Display.Content);
                    if (goal == sum)
                    {
                        INFO.Content = "GOOD JOB!";
                    }
                    else if(goal > sum)
                    {
                        INFO.Content = "IT'S GOOD, YOU CAN EAT MORE";
                    }
                    else
                    {
                        INFO.Content = "YOU ATE TO MUCH!";
                    }
                }
            }
        }
    }
}

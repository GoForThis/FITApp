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
    }
}

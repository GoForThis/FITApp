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

namespace FITApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SignIn_Button(object sender, RoutedEventArgs e)
        {
            string UserName = Name_Login.Text;
            string Password = Password_Login.Password;
            if(Login(UserName, Password))
            {
                MessageBox.Show("Logged in");
            }
            else
            {
                MessageBox.Show("UserName or Password is incorrect");
            }
            Name_Login.Text = "";
            Password_Login.Password = "";
        }
        private void SignUp_Button(object sender, RoutedEventArgs e)
        {
            string UserName = Name_Register.Text;
            string Password = Password_Register.Password;
            if(Register(UserName, Password))
            {
                MessageBox.Show("Successfuly registered! Your Username: " + UserName);
            }
            else
            {
                MessageBox.Show("User with given Username already exists");
            }

            Name_Register.Text = "";
            Password_Register.Password = "";
        }

        public bool Register(string UserName, string Password)
        {
            DBEntities db = new DBEntities();
            var users = from d in db.Users
                        select d.UserName;

            foreach (var item in users)
            {
                if (item == UserName)
                    return false;
            }
            User user = new User()
            {
                UserName = UserName,
                Password = Password
            };
            db.Users.Add(user);
            db.SaveChanges();

            return true;
        }
        public bool Login(string UserName, string Password)
        {
            DBEntities db = new DBEntities();
            var users = from d in db.Users
                        where d.UserName == UserName
                        select d.Password;
            foreach (var item in users)
            {
                if (item == Password)
                    return true;
                else
                    return false;
            }

            return false;
        }

       
    }
        
}

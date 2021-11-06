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
using System.Data.SqlClient;
using System.Data;

namespace AlfaBank
{
    /// <summary>
    /// Interaction logic for CreateAccount.xaml
    /// </summary>
    public partial class CreateAccount : Window
    {
        public CreateAccount()
        {
            InitializeComponent();
        }

        private void Create_Button_Click(object sender, RoutedEventArgs e)
        {
            string UID = Database_Control.GenerateID();
            string First_N = FirstName_Text.Text;
            string Last_N = Lastname_Text.Text;
            string Email = Email_Text.Text;
            string Password = Pass_Text.Password.ToString();

            if (FirstName_Text.Text == "" || Lastname_Text.Text == "" || Email_Text.Text == "")
            {
                MessageBox.Show("Please fill it correctly!", "Warning");
            }
            else if (Pass_Text.Password.ToString() != RePass_Text.Password.ToString())
            {
                MessageBox.Show("Your Passwords doesn't match!", "Warning");
            }
            else
            {
                Database_Control.DB_CreateUser(UID,First_N, Last_N, Email,Password);
                this.Close();
                MainWindow a = new MainWindow();
                a.Show();
            }
        }

        private void GoBack_Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow a = new MainWindow();
            this.Close();
            a.Show();
        }

        private void FirstnameChange(object sender, RoutedEventArgs e)
        {
            if (FirstName_Text.Text == "First name")
            {
                FirstName_Text.Text = "";
            }
        }

        private void LastnameChange(object sender, RoutedEventArgs e)
        {
            if (Lastname_Text.Text == "Last name")
            {
                Lastname_Text.Text = "";
            }
        }

        private void CityChange(object sender, RoutedEventArgs e)
        {
            if (Email_Text.Text == "E-mail")
            {
                Email_Text.Text = "";
            }
        }

    }
}

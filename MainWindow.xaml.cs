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
using System.Data.SqlClient;
using System.Data;

namespace AlfaBank
{
    public partial class MainWindow : Window
    {
        CustomerSession cs = new CustomerSession();
        public MainWindow()
        {
            Database_Control.DB_Open();
            InitializeComponent();
        }
        private void Login_Button_Click(object sender, RoutedEventArgs e)
        {
            string sql_command = "SELECT ID,PASSWORD FROM [dbo].[Users]";
            cs.CustomerID = Username_Text.Text;
            cs.CustomerPass = Password_PassBox.Password.ToString();

            foreach (DataRow row in Database_Control.DB_GetTable(sql_command).Rows)
            {
                string ID_Check = row["ID"].ToString();
                string Pass_Check = row["Password"].ToString();
                if ( cs.CustomerID == ID_Check && cs.CustomerPass == Pass_Check )
                {
                    cs.Authenticated = true;
                    break;
                }
            }

            if (cs.Authenticated == true)
            {
                this.Hide();
                Window1 a = new Window1(cs);
                a.Show();
            }
            else
            {
                MessageBox.Show("Your Username or Password is wrong!", "Warning");
            }
        }
        private void Exit_Button_Click(object sender, RoutedEventArgs e)
        {
            Database_Control.DB_Close();
            cs.Authenticated = false;
            cs.CustomerID = null;
            cs.CustomerPass = null;
            System.Windows.Application.Current.Shutdown();
        }

        private void CreateUser_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            CreateAccount a = new CreateAccount();
            a.Show();
        }

        private void UsernameChange(object sender, RoutedEventArgs e)
        {
            if (Username_Text.Text == "Username")
            {
             Username_Text.Text = "";
            }
        }
    }
}

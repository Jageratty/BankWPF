using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Configuration;
using System.Windows.Documents;
using System.Data.SqlClient;
using System.Data;

namespace AlfaBank
{
    public partial class Window1 : Window
    {
        CustomerSession session;
        private MainWindow ref_MainWindow;
        public Window1(CustomerSession cs) //we have passed the mainwindows session to here
        {
            session = cs;
            InitializeComponent();
            this.ref_MainWindow = new MainWindow();
        }
        private void Transfer_Complete_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (session.Authenticated == true)
                {
                    string curreny = "";
                    if (Radio_HUF.IsChecked == true)
                    {
                        curreny = "HUF";
                        Database_Control.DB_Transfer(session.CustomerID, curreny, float.Parse(Transfer_Ammount_Text.Text), Transfer_Target_Text.Text);
                    }
                    else if (Radio_Euro.IsChecked == true)
                    {
                        curreny = "EURO";
                        Database_Control.DB_Transfer(session.CustomerID, curreny, float.Parse(Transfer_Ammount_Text.Text), Transfer_Target_Text.Text);
                    }
                    else if (Radio_USD.IsChecked == true)
                    {
                        curreny = "USD";
                        Database_Control.DB_Transfer(session.CustomerID, curreny, float.Parse(Transfer_Ammount_Text.Text), Transfer_Target_Text.Text);
                    }
                    else
                    {
                        MessageBox.Show("Please select currency!", "Warning");
                    }

                }
            } catch (System.FormatException ex)
            {
                MessageBox.Show("Please enter numbers in to the ammount!","Warning!");
            }
        }
        private void Show_History(object sender, RoutedEventArgs e) //GotFocus
        {
            string xxx = session.CustomerID;
            HistoryOutput_Datagrid.ItemsSource = Database_Control.DB_Show_History(xxx).DefaultView;
        }
        private void Show_Balance(object sender, RoutedEventArgs e) //GotFocus
        {
            string xxx = session.CustomerID;
            Balance_DataGrid.ItemsSource = Database_Control.DB_Balance(xxx).DefaultView;
        }

        private void Safe_Exit_Button_Click(object sender, RoutedEventArgs e)
        {
            session.Authenticated = false;
            Database_Control.DB_Close();
            this.Close();
            this.ref_MainWindow.Visibility = System.Windows.Visibility.Visible;
        }

        private void toCSV_Button_Click(object sender, RoutedEventArgs e)
        {
            Database_Control.DB_History_CSV(session.CustomerID);
        }
    }
}

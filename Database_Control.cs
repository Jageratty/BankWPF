using System;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace AlfaBank
{
    public static class Database_Control
    {
        // When we are going to update the table variables with is going to effected has to be like "+xxxx+",but read only variables has to be like '"+xxx+"' 
        public static void DB_Transfer(string UID,string currency,float ammount,string TargetID) 
        {
            {
            try 
            {
                string command_sql_text = "UPDATE [dbo].[Table] " +
                "SET "+currency+"-="+ammount+" " +
                "   WHERE ID = '"+ UID +"' ";
                SqlCommand command_send = new SqlCommand(command_sql_text,DB_Open());
                command_send.CommandType = CommandType.Text;
                command_send.ExecuteNonQuery();
                DB_History_Sender(UID, TargetID, ammount, currency);
                // Adding to the target ID
                command_sql_text = "UPDATE [dbo].[Table] " +
                "SET "+currency+"+="+ammount+" " +
                "WHERE ID = '"+TargetID+"' ";
                SqlCommand command_receive = new SqlCommand(command_sql_text, DB_Open());
                command_receive.CommandType = CommandType.Text;
                command_receive.ExecuteNonQuery();
                MessageBox.Show("Transaction completed!");
                DB_History_Receiver(UID, TargetID, ammount, currency);
            }
            catch (SqlException ex)
            {
                MessageBox.Show("An error occurred\nPlease check the target accounts ID", "Warning");
            }
            catch (Exception other_ex)
            {
                MessageBox.Show("An error occurred\nUnfortunatelly we couldn't complete the task for you\nPlease try again later", "Warning");
            }
            }
        }

        // Access to the path is denied if we try to download in to spesific path.
        // It can be solved in the security settings but computer will be vulnerable thats why the csv file is downloding to the /bin/debug.
        public static void DB_History_CSV(string UID)
        {
            DataTable dataTable = DB_Show_History(UID);
            string filePath = "History_CSV.csv"; 
                StringBuilder fileContent = new StringBuilder();

                foreach (var col in dataTable.Columns)
                {
                    fileContent.Append(col.ToString() + ",");
                }

                fileContent.Replace(",", System.Environment.NewLine, fileContent.Length - 1, 1);

                foreach (DataRow dr in dataTable.Rows)
                {
                    foreach (var column in dr.ItemArray)
                    {
                        fileContent.Append("\"" + column.ToString() + "\",");
                    }

                    fileContent.Replace(",", System.Environment.NewLine, fileContent.Length - 1, 1);
                }
                System.IO.File.WriteAllText(filePath, fileContent.ToString());
            MessageBox.Show("Download has been completed\n AlfaBank/bin/Debug/net5.0-windows/History_CSV");
        }
        public static void DB_History_Sender(string UID,string other_UID,float money_ammount,string money_currency) // ID is not primary key beacuse we need duplicate ID to hold the track
        {
            string date_print = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.K");
            string commnd_string = "INSERT INTO[dbo].[History](ID,Date,ToWho,Transfer,Currency,Destination) VALUES(@ID,@Date, @ToWho, @Transfer, @Currency,@Output)";
            SqlCommand obj_command = new SqlCommand(commnd_string, DB_Open());
            obj_command.CommandType = CommandType.Text;
            obj_command.Parameters.AddWithValue("@ID", UID);
            obj_command.Parameters.AddWithValue("@Date", date_print);
            obj_command.Parameters.AddWithValue("@ToWho", other_UID);
            obj_command.Parameters.AddWithValue("@Transfer", money_ammount);
            obj_command.Parameters.AddWithValue("@Currency", money_currency);
            obj_command.Parameters.AddWithValue("@Output", "Sender");
            obj_command.ExecuteNonQuery();
        }
        public static void DB_History_Receiver(string UID, string other_UID, float money_ammount, string money_currency) // ID is not primary key beacuse we need duplicate ID to hold the track
        {
            string date_print = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.K");
            string commnd_string = "INSERT INTO[dbo].[History](ID,Date,ToWho,Transfer,Currency,Destination) VALUES(@ID,@Date, @ToWho, @Transfer, @Currency,@Output)";
            SqlCommand obj_command = new SqlCommand(commnd_string, DB_Open());
            obj_command.CommandType = CommandType.Text;
            obj_command.Parameters.AddWithValue("@ID", other_UID);
            obj_command.Parameters.AddWithValue("@Date", date_print);
            obj_command.Parameters.AddWithValue("@ToWho", UID);
            obj_command.Parameters.AddWithValue("@Transfer", money_ammount);
            obj_command.Parameters.AddWithValue("@Currency", money_currency);
            obj_command.Parameters.AddWithValue("@Output", "Receiver");
            obj_command.ExecuteNonQuery();
        }
        public static DataTable DB_Show_History(string UID)
        {
            SqlConnection connection = DB_Open();
            DataTable table = new DataTable();
            SqlDataAdapter adap = new SqlDataAdapter("SELECT * FROM [dbo].[History] WHERE ID='" + UID + "' ", connection);
            adap.Fill(table);
            return table;
        }
        public static DataTable DB_Balance(string UID)
        {
            SqlConnection connection = DB_Open();
            DataTable table = new DataTable();
            SqlDataAdapter adap = new SqlDataAdapter("SELECT * FROM [dbo].[Table] WHERE ID='" + UID + "' ", connection);
            adap.Fill(table);
            return table;
        }
        public static DataTable DB_GetTable(string SQL_Text)
        {
            SqlConnection cn_connection = DB_Open();
            DataTable Data_Table = new DataTable();
            SqlDataAdapter SQL_Adapter = new SqlDataAdapter(SQL_Text, cn_connection);
            SQL_Adapter.Fill(Data_Table);
            return Data_Table;
        }
        public static void DB_CreateUser(string a, string b, string c, string d,string e)
        {
            try
            {
                string commnd_string = "INSERT INTO[dbo].[Users](ID,FirstName, LastName, Email, Password) VALUES(@ID,@FirstName, @LastName, @Email, @Password)";
                SqlCommand obj_command = new SqlCommand(commnd_string, DB_Open());
                obj_command.CommandType = CommandType.Text;
                obj_command.Parameters.AddWithValue("@ID", a);
                obj_command.Parameters.AddWithValue("@FirstName", b);
                obj_command.Parameters.AddWithValue("@LastName", c);
                obj_command.Parameters.AddWithValue("@Email", d);
                obj_command.Parameters.AddWithValue("@Password", e);
                obj_command.ExecuteNonQuery();
                MessageBox.Show("You've successfuly created a user!");
                MessageBox.Show("Your ID is : " + a + "\nPlease take note your ID!", "Warning");
            }
            catch (SqlException ex)
            {
                MessageBox.Show("An error occurred\nUnfortunatelly we couldn't create a user for you\nPlease try again later", "Warning");
            }
            catch (Exception other_ex)
            {
                MessageBox.Show("An error occurred\nUnfortunatelly we couldn't create a user for you\nPlease try again later", "Warning");
            }
        }
        public static string GenerateID()
        {
            string get_id_text = "SELECT ID FROM [dbo].[Users]";
            Random rnd = new Random();
            int number = rnd.Next(1,1000);
            string ID = "ALF" + number;

            foreach (DataRow row in DB_GetTable(get_id_text).Rows)
            {
                string ID_Check = row["ID"].ToString();
                if (ID_Check != ID)
                {
                    continue;
                }
                else
                {
                    number = rnd.Next(1,1000);
                    ID = "ALF" + number;
                }
            }
            return ID;
        }
        public static SqlConnection DB_Open() //It has a return type beacuse we have used it for exp. line : 26
        {
            SqlConnection DB_bank_connection = new SqlConnection(@"Data Source = (localdb)\MSSQLLocalDB; Initial Catalog = AlfaBank; Integrated Security = True; Connect Timeout = 30; Encrypt = False; TrustServerCertificate = False; ApplicationIntent = ReadWrite; MultiSubnetFailover = False");
            if (DB_bank_connection.State != ConnectionState.Open)
            { 
                DB_bank_connection.Open(); 
            }
            return DB_bank_connection;
        }
        public static SqlConnection DB_Close() //It has a return type beacuse we have used it for exp. line : 26
        {
            SqlConnection DB_bank_connection = new SqlConnection(@"Data Source = (localdb)\MSSQLLocalDB; Initial Catalog = AlfaBank; Integrated Security = True; Connect Timeout = 30; Encrypt = False; TrustServerCertificate = False; ApplicationIntent = ReadWrite; MultiSubnetFailover = False");
            if (DB_bank_connection.State == ConnectionState.Open)
            {
                DB_bank_connection.Close();
            }
            return DB_bank_connection;
        }
    }
}
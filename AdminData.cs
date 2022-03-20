using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
namespace DALSprint1
{
    public class AdminData
    {
        static string _connectionString = "Data Source=BTECH1830156;Initial Catalog=Sprint1Database;Integrated Security=true";

        // private static string _connectionString = ConfigurationManager.ConnectionStrings["Sprint1connection"].ConnectionString;
        private static SqlConnection connection = new SqlConnection(_connectionString);
        public static List<string> SendAdminDetails()
        {         
                List<string> sendAdmin = new List<string>();


                string query = "Select * from AdminInfo";
                SqlDataAdapter sdr = new SqlDataAdapter(query, connection);
                DataTable dt = new DataTable();
                sdr.Fill(dt);
                DataRow row = dt.Rows[0];                     
                sendAdmin.Add(row[0].ToString());
                sendAdmin.Add(row[1].ToString());
            return sendAdmin;
        }
        public static bool CheckAuthentication(string username,string password)
        {
            bool isValidUser;
            string query = "Select * from AdminInfo";
            SqlDataAdapter sdr = new SqlDataAdapter(query, connection);
            DataTable dt = new DataTable();
            sdr.Fill(dt);
            DataRow row = dt.Rows[0];
            if(row[2].ToString().Equals(username) && row[3].ToString().Equals(password))
            {
                isValidUser= true;
            }
            else
            {
                isValidUser= false;
            }
            return isValidUser;
        }

    }
}

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Data;
//using System.Data.SqlClient;
//using System.Configuration;
//namespace DALSprint1
//{
//    public class AddressData
//    {
//        static string _connectionString = "Data Source=BTECH1830156;Initial Catalog=Sprint1Database;Integrated Security=true";

//        // private static readonly string _connectionString = ConfigurationManager.ConnectionStrings["Sprint1connection"].ConnectionString;
//        private static SqlConnection connection = new SqlConnection(_connectionString);

//        public static string AddAddress(List<string> newAddress)
//        {
//            string message;
//            try
//            {
//                List<string> checkIfExist = SendAddressByAddressCode(newAddress[0]);
//                if (checkIfExist.Count == 0)
//                {
//                    message = "Successfully Added";

//                    string query = "Insert into Address values(@AddCode,@AddLine1,@AddLine2,@LandMark,@City,@State,@PinCode,@Country)";
//                    SqlCommand cmd = new SqlCommand(query, connection);
//                    cmd.Parameters.AddWithValue("@AddCode", newAddress[0]);
//                    cmd.Parameters.AddWithValue("@AddLine1", newAddress[1]);
//                    cmd.Parameters.AddWithValue("@AddLine2", newAddress[2]);
//                    cmd.Parameters.AddWithValue("@LandMark", newAddress[3]);
//                    cmd.Parameters.AddWithValue("@City", newAddress[4]);
//                    cmd.Parameters.AddWithValue("@State", newAddress[5]);
//                    cmd.Parameters.AddWithValue("@PinCode", Convert.ToInt64(newAddress[6]));
//                    cmd.Parameters.AddWithValue("@Country", newAddress[7]);
//                    connection.Open();
//                    cmd.ExecuteNonQuery();
//                }
//                else
//                {
//                    throw new InvalidObjectException("AddressCode does not exist: " + newAddress[0]);
//                }
//            }
//            catch (Exception ex)
//            {
//                throw new InvalidObjectException(ex.Message);
//            }
//            finally
//            {
//                connection.Close();
//            }
//            return message;
//        }

//        public static List<string> SendAddressByAddressCode(string addressCode)
//        {
//            //string status = "Fetched"; // find out what to do you with this data;
//            List<string> sendAddress = new List<string>();
//            string query = "Select * from Address where AddressCode=@code";
//            SqlDataAdapter sdr = new SqlDataAdapter(query, connection);
//            sdr.SelectCommand.Parameters.AddWithValue("@code", addressCode);
//            DataTable dt = new DataTable();
//            sdr.Fill(dt);
//            if (dt.Rows.Count > 0)
//            {
//                foreach (DataRow row in dt.Rows)
//                {
//                    if (row[0].ToString() == addressCode)
//                    {
//                        sendAddress.Add(row[0].ToString());
//                        sendAddress.Add(row[1].ToString());
//                        sendAddress.Add(row[2].ToString());
//                        sendAddress.Add(row[3].ToString());
//                        sendAddress.Add(row[4].ToString());
//                        sendAddress.Add(row[5].ToString());
//                        sendAddress.Add(row[6].ToString());
//                        sendAddress.Add(row[7].ToString());
//                        break;
//                    }
//                }
//            }
//            else
//            {
//                throw new InvalidObjectException("No Address Found");
//            }
//            return sendAddress;
//        }
//    }
//}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjectLayer;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
namespace DALSprint1
{
    public class SendData
    {
        static string _connectionString = "Data Source=BTECH1830156;Initial Catalog=Sprint1Database;Integrated Security=true";

         //private static string _connectionString = ConfigurationManager.ConnectionStrings["Sprint1connection"].ConnectionString;
        private static SqlConnection connection = new SqlConnection(_connectionString);




        public static string SaveDrugDosage(DrugDosage objDrugDosage)
        {

                string message = "Successfully Added";
                try
                {
                    string query = "sp_SaveDrugDosage";//"Insert into DrugDosage values(@DrugDosageId,@DosageDuration)";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@DrugDosageId", objDrugDosage.DrugDosageId);
                    cmd.Parameters.AddWithValue("@DosageDuration", objDrugDosage.DosageDuration);

                    connection.Open();
                    int c = (int)cmd.ExecuteNonQuery();
                    if (c <= 0)
                    {
                        throw new InvalidObjectException("DrugDosage Insertion Failed");
                    }

                }
                catch (Exception ex)
                {
                    throw new InvalidObjectException("DrugDosage Section: "+ex.Message);
                }
                finally
                {
                    connection.Close();
                }
                return message;
            
        }
        public static DrugDosage SendDosageById(string dosageId)

        {
            DrugDosage sendDosage = new DrugDosage();
            try
            {

                string query = "sp_FetchDrugDosageById";//"Select * from DrugDosage where DrugDosageId=@id";
                SqlDataAdapter sdr = new SqlDataAdapter(query, connection);
                sdr.SelectCommand.CommandType = CommandType.StoredProcedure;
                sdr.SelectCommand.Parameters.AddWithValue("@id", dosageId);
                DataTable dt = new DataTable();
                sdr.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    sendDosage.DrugDosageId = row[0].ToString();
                    sendDosage.DosageDuration = row[1].ToString();                 
                }
                else
                {
                    throw new InvalidObjectException("Sorry! DosageId does not exist");
                }
            }
            catch (Exception ex)
            {
                throw new InvalidObjectException(ex.Message);
            }
            return sendDosage;

        }
        public static List<Drug> SendAllDrugAndDoseByDrugDosageId(string drugDosageId)
        {
            List<Drug> sendDrugs = new List<Drug>();
            try
            {

                string query = "sp_FetchAllDrugAndDoseByDosageId";//"Select * from Drug where DrugDosageInvoledId=@id";
                SqlDataAdapter sdr = new SqlDataAdapter(query, connection);
                sdr.SelectCommand.CommandType = CommandType.StoredProcedure;
                sdr.SelectCommand.Parameters.AddWithValue("@id", drugDosageId);
                DataTable dt = new DataTable();
                sdr.Fill(dt);
                if (dt.Rows.Count == 0)
                {
                    throw new InvalidObjectException("No drug found");
                }
                else
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        sendDrugs.Add(
                            new Drug()
                            {
                                DrugId = row[0].ToString(),
                                DrugName = row[1].ToString(),
                                BrandName = row[2].ToString(),
                                MRP = (float)Convert.ToDouble(row[3].ToString()),
                                ManufacturingDate = Convert.ToDateTime(row[4].ToString()).Date,
                                ExpiryDate = Convert.ToDateTime(row[5].ToString()).Date,
                                Indications = row[6].ToString(),
                                Composition = row[7].ToString(),
                                BatchNo = Convert.ToInt64(row[8].ToString()),
                                DrugDosageInvolvedId = row[9].ToString(),
                                DrugDose = row[10].ToString(),
                            }
                            );
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidObjectException(ex.Message);
            }
            return sendDrugs;
        }



        public static string AddAddress(Address newAddress)
        {
            string message="Address Successfully Saved";
            try
            {

                string query = "sp_saveAddress";//"Insert into Address values(@AddCode,@AddLine1,@AddLine2,@LandMark,@City,@State,@PinCode,@Country)";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@AddCode", newAddress.AddressCode);
                    cmd.Parameters.AddWithValue("@AddLine1", newAddress.AddressLine1);
                    cmd.Parameters.AddWithValue("@AddLine2", newAddress.AddressLine2);
                    cmd.Parameters.AddWithValue("@LandMark", newAddress.LandMark);
                    cmd.Parameters.AddWithValue("@City", newAddress.City);
                    cmd.Parameters.AddWithValue("@State", newAddress.State);
                    cmd.Parameters.AddWithValue("@PinCode", newAddress.PinCode);
                    cmd.Parameters.AddWithValue("@Country", newAddress.Country);
                    connection.Open();
                    int c= cmd.ExecuteNonQuery();
                if(c<=0)
                {
                    throw new InvalidObjectException("Address Could not be Saved");
                }

            }
            catch (Exception ex)
            {
                throw new InvalidObjectException("Address Section: "+ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return message;
        }
        public static Address SendAddressByAddressCode(string addressCode)
        {
            Address sendAddress = new Address();

            string query = "sp_fetchAddressByAddressCode";//"Select * from Address where AddressCode=@code";
            SqlDataAdapter sdr = new SqlDataAdapter(query, connection);
            sdr.SelectCommand.CommandType = CommandType.StoredProcedure;
            sdr.SelectCommand.Parameters.AddWithValue("@code", addressCode);
            DataTable dt = new DataTable();
            sdr.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    if (row[0].ToString() == addressCode)
                    {
                        sendAddress.AddressCode = row[0].ToString();
                        sendAddress.AddressLine1 = row[1].ToString();
                        sendAddress.AddressLine2 = row[2].ToString();
                        sendAddress.LandMark = row[3].ToString();
                        sendAddress.City = row[4].ToString();
                        sendAddress.State = row[5].ToString();
                        sendAddress.PinCode = Convert.ToInt64(row[6].ToString());
                        sendAddress.Country = row[7].ToString();                 
                    }
                }
            }
            else
            {
                throw new InvalidObjectException("No Address Found");
            }
            return sendAddress;
        }



        public static string SaveConsultation(Consultation consultation)
        {
            string message = "Successfully Added";
            try
            {
                string query = "sp_saveConsultation";// "Insert into Consultation values(@Id,@Date,@PhysicianId,@AppBookingId,@Diagnosis,@DosageId)";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", consultation.PrescriptionId);
                cmd.Parameters.AddWithValue("@Date", consultation.PrescriptionDate.Date);
                cmd.Parameters.AddWithValue("@PhysicianId", consultation.CPhysician);
                cmd.Parameters.AddWithValue("@AppBookingId", consultation.AppointmentDetails);
                cmd.Parameters.AddWithValue("@Diagnosis", consultation.Diagnosis);
                cmd.Parameters.AddWithValue("@DosageId", consultation.Dosage);

                connection.Open();
                int c = cmd.ExecuteNonQuery();
                if (c <= 0)
                {
                    throw new InvalidObjectException("Consultation failed");
                }
            }
            catch (Exception ex)
            {
                throw new InvalidObjectException("Consultation Layer: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return message;
        }
        public static Consultation SendConsultationDetails(string prescriptionId)
        {
            Consultation sendConsultation = new Consultation();
            try
            {

                string query = "sp_fetchConsultationById";//"Select * from Consultation where PrescriptionId=@id";
                SqlDataAdapter sdr = new SqlDataAdapter(query, connection);
                sdr.SelectCommand.CommandType = CommandType.StoredProcedure;
                sdr.SelectCommand.Parameters.AddWithValue("@id", prescriptionId);
                DataTable dt = new DataTable();
                sdr.Fill(dt);
                if (dt.Rows.Count == 0)
                {
                    throw new InvalidObjectException("No Consultation Record Available with PrescriptionId : " + prescriptionId);
                }
                else
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        if (row[0].ToString() == prescriptionId)
                        {
                            sendConsultation.PrescriptionId = row[0].ToString();
                            sendConsultation.PrescriptionDate =Convert.ToDateTime( row[1].ToString());
                            sendConsultation.CPhysician = row[2].ToString();
                            sendConsultation.AppointmentDetails = row[3].ToString();
                            sendConsultation.Diagnosis = row[4].ToString();
                            sendConsultation.Dosage = row[5].ToString();

                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidObjectException("Consultation Layer: " + ex.Message);
            }
            return sendConsultation;
        }
    


        public static string AddNewDrug(Drug newDrug)
        {
            string message = "Successfully Added";
            try
            {
                string query = "sp_AddDrug";//"Insert into Drug values(@Id,@Name,@BrandName,@MRP,@MFGDate,@EXPDate,@Indications,@Composition,@BatchNo,@DrugDosageInvoledId,@DrugDose)";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", newDrug.DrugId);
                cmd.Parameters.AddWithValue("@Name", newDrug.DrugName);
                cmd.Parameters.AddWithValue("@BrandName", newDrug.BrandName);
                cmd.Parameters.AddWithValue("@MRP", newDrug.MRP);
                cmd.Parameters.AddWithValue("@MFGDate", newDrug.ManufacturingDate.Date);
                cmd.Parameters.AddWithValue("@EXPDate", newDrug.ExpiryDate.Date);
                cmd.Parameters.AddWithValue("@Indications", newDrug.Indications);
                cmd.Parameters.AddWithValue("@Composition", newDrug.Composition);
                cmd.Parameters.AddWithValue("@BatchNo", newDrug.BatchNo);
                cmd.Parameters.AddWithValue("@DrugDosageInvoledId", newDrug.DrugDosageInvolvedId);
                cmd.Parameters.AddWithValue("@DrugDose", newDrug.DrugDose);

                connection.Open();
                int c = cmd.ExecuteNonQuery();
                if (c <= 0)
                {
                    throw new InvalidObjectException("Insertion Failed");
                }
            }
            catch (Exception ex)
            {
                throw new InvalidObjectException(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return message;
        }
        public static string UpdateDrugPrice(string drugId, float newPrice)
        {

            string query = "sp_UpdateDrugMRP";//"update Drug set MRP=@newMRP where Id=@id";
            string status = "Updated";
            Drug checkIfExist = SendDrugById(drugId);
            try
            {
                if (checkIfExist.DrugId!=drugId)
                {
                    throw new InvalidObjectException("Drug Id could not be found");
                }
                else
                {
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@newMRP", newPrice);
                    cmd.Parameters.AddWithValue("@id", drugId);
                    connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            catch (Exception ex)
            {
                throw new InvalidObjectException(ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return status;
        }
        public static Drug SendDrugById(string drugId)
        {
            Drug sendDrug = new Drug();
            try
            {

                string query = "sp_FetchDrugById";// "Select * from Drug where Id=@id";
                SqlDataAdapter sdr = new SqlDataAdapter(query, connection);
                sdr.SelectCommand.CommandType = CommandType.StoredProcedure;
                sdr.SelectCommand.Parameters.AddWithValue("@id", drugId);
                DataTable dt = new DataTable();
                sdr.Fill(dt);
                if (dt.Rows.Count == 0)
                {
                    throw new InvalidObjectException("Drug Id could not be found");
                }
                else
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        if (row[0].ToString() == drugId)
                        {
                            sendDrug.DrugId = row[0].ToString();
                            sendDrug.DrugName = row[1].ToString();
                            sendDrug.BrandName = row[2].ToString();
                            sendDrug.MRP = (float)Convert.ToDouble(row[3].ToString());
                            sendDrug.ManufacturingDate = Convert.ToDateTime(row[4].ToString()).Date;
                            sendDrug.ExpiryDate = Convert.ToDateTime(row[5].ToString()).Date;
                            sendDrug.Indications = row[6].ToString();
                            sendDrug.Composition = row[7].ToString();
                            sendDrug.BatchNo = Convert.ToInt64(row[8].ToString());
                            sendDrug.DrugDosageInvolvedId = row[9].ToString();
                            sendDrug.DrugDose = row[10].ToString();

                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidObjectException(ex.Message);
            }
            return sendDrug;

        }
        public static List<Drug> SendAllDrugs()
        {

            List<Drug> drugs = new List<Drug>();
            try
            {
                string query = "sp_fetchAllDrug";
                SqlDataAdapter sdr = new SqlDataAdapter(query, connection);
                sdr.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                sdr.Fill(dt);
                if (dt.Rows.Count == 0)
                {
                    throw new InvalidObjectException("No drug found");
                }
                else
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        drugs.Add(
                            new Drug()
                            {
                                DrugId = row[0].ToString(),
                                DrugName = row[1].ToString(),
                                BrandName = row[2].ToString(),
                                MRP = (float)Convert.ToDouble(row[3].ToString()),
                                ManufacturingDate = Convert.ToDateTime(row[4].ToString()).Date,
                                ExpiryDate = Convert.ToDateTime(row[5].ToString()).Date,
                                Indications = row[6].ToString(),
                                Composition = row[7].ToString(),
                                BatchNo = Convert.ToInt64(row[8].ToString()),
                                DrugDosageInvolvedId = row[9].ToString(),
                                DrugDose = row[10].ToString(),

                            }
                            );
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidObjectException(ex.Message);
            }
            return drugs;
        }
        public static string DeleteExpiredDrugs()
        {
            string status = "Removed";
            try
            {  
                string query = "sp_DeleteDrugById";//"Delete from Drug where Id=@id";
                string id;
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.CommandType = CommandType.StoredProcedure;
                List<Drug> drugsList = SendAllDrugs();
                connection.Open();
                foreach (Drug drug in drugsList)
                {
                    if ((drug.ExpiryDate.Date - DateTime.Now.Date).Days <= 0)
                    {
                        id = drug.DrugId;
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidObjectException(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return status;
        }
        public static string UpdateDosageDetailsInDrug(string drugId, string newDosage)
        {
            string query = "sp_UpdateDrugDosageDetails";//"update Drug set DrugDosageInvoledId=@newupdate where Id=@id";
            string status = "Updated";
            Drug checkIfExist = SendDrugById(drugId);
            try
            {
                if (checkIfExist.DrugId!=drugId)
                {
                    throw new InvalidObjectException("Drug Id could not be found");
                }
                else
                {
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@newupdate", newDosage);
                    cmd.Parameters.AddWithValue("@id", drugId);
                    connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            catch (Exception ex)
            {
                throw new InvalidObjectException(ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return status;
        }
        public static string UpdateDrugDoseDetails(string drugId,string newDrugDose)
        {

            string query = "sp_UpdateDrugDoseDetails";//"update Drug set DrugDose=@newupdate where Id=@id";
                string status = "Updated";
                Drug checkIfExist = SendDrugById(drugId);
                try
                {
                    if (checkIfExist.DrugId != drugId)
                    {
                        throw new InvalidObjectException("Drug Id could not be found");
                    }
                    else
                    {
                        SqlCommand cmd = new SqlCommand(query, connection);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@newupdate", newDrugDose);
                        cmd.Parameters.AddWithValue("@id", drugId);
                        connection.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

                catch (Exception ex)
                {
                    throw new InvalidObjectException(ex.Message);
                }
                finally
                {
                    connection.Close();
                }

                return status;

        }



        public static string AddNewPhysician(Physician newPhysician)
        {
            string message = "Successfully Added";
            try
            {   
                string query = "sp_AddPhysician";// "Insert into Physician values(@Id,@FirstName,@MiddleName,@LastName,@Speciality,@Qualification,@PNum,@Email)";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", newPhysician.PhysicianId);
                cmd.Parameters.AddWithValue("@FirstName", newPhysician.PhysicianFirstName);
                cmd.Parameters.AddWithValue("@MiddleName", newPhysician.PhysicianMiddleName);
                cmd.Parameters.AddWithValue("@LastName", newPhysician.PhysicianLastName);
                cmd.Parameters.AddWithValue("@Speciality", newPhysician.PhysicianSpeciality);
                cmd.Parameters.AddWithValue("@Qualification", newPhysician.PhysicianQualification);
                cmd.Parameters.AddWithValue("@PNum", newPhysician.PhysicianPhoneNumber);
                cmd.Parameters.AddWithValue("@Email", newPhysician.PhysicianEmail);
                connection.Open();
                int c = (int)cmd.ExecuteNonQuery();
                if (c <= 0)
                {
                    throw new InvalidObjectException("InsertionFailed");
                }
            }
            catch (Exception ex)
            {
                throw new InvalidObjectException(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return message;
        }
        public static string DeletePhysician(string physicianId)
        {
            string status = "Physician Deleted";
            try
            {
                Physician checkIfExist = SendPhysicianById(physicianId);
                if (checkIfExist.PhysicianId != physicianId)
                {
                    throw new InvalidObjectException("Sorry! PhysicianId does not exist");
                }
                else
                {  
                    string query = "sp_DeletePhysician";//"Delete from Physician where Id=@id";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", physicianId);
                    connection.Open();
                    cmd.ExecuteNonQuery();
                }

            }
            catch (Exception ex)
            {
                throw new InvalidObjectException(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return status;
        }
        public static List<Physician> SendAllPhysicians()
        {

            List<Physician> physicians = new List<Physician>();
            try
            {
                string query = "sp_fetchAllPhysician";
                SqlDataAdapter sdr = new SqlDataAdapter(query, connection);
                sdr.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                sdr.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        physicians.Add(new Physician()
                        {
                            PhysicianId = row[0].ToString(),
                            PhysicianFirstName = row[1].ToString(),
                            PhysicianMiddleName = row[2].ToString(),
                            PhysicianLastName = row[3].ToString(),
                            PhysicianSpeciality = row[4].ToString(),
                            PhysicianQualification = row[5].ToString(),
                            PhysicianPhoneNumber = Convert.ToInt64(row[6].ToString()),
                            PhysicianEmail = row[7].ToString(),
                        });
                    }
                }
                else
                {
                    throw new InvalidObjectException("Sorry! No Physician Found");
                }
            }
            catch (Exception ex)
            {
                throw new InvalidObjectException(ex.Message);
            }
            return physicians;

        }
        public static Physician SendPhysicianById(string physicianId)
        {
            Physician sendPhysician = new Physician();
            try
            {

                string query = "sp_FetchPhysicianById";//"select * from Physician where Id=@id";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", physicianId);
                connection.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        sendPhysician.PhysicianId = sdr["Id"].ToString();
                        sendPhysician.PhysicianFirstName = sdr["FirstName"].ToString();
                        sendPhysician.PhysicianMiddleName = sdr["MiddleName"].ToString();
                        sendPhysician.PhysicianLastName = sdr["MiddleName"].ToString();
                        sendPhysician.PhysicianSpeciality = sdr["Speciality"].ToString();
                        sendPhysician.PhysicianQualification = sdr["Qualification"].ToString();
                        sendPhysician.PhysicianPhoneNumber = Convert.ToInt64(sdr["PhoneNumber"].ToString());
                        sendPhysician.PhysicianEmail = sdr["Email"].ToString();
                    }
                    sdr.Close();
                }
                else
                {
                    throw new InvalidObjectException("Sorry! PhysicianId does not exist");
                }
            }
            catch (Exception ex)
            {
                throw new InvalidObjectException(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return sendPhysician;

        }
        public static string UpdatePhysicianDetails(string physicianId, string parameterName, string newUpdate)
        {

            string query;
            object tempNewUpdate = newUpdate;

            if (parameterName.Equals("FirstName"))
            {
                query = "update Physician set FirstName=@newUpdate where Id=@id";
            }

            else if (parameterName.Equals("MiddleName"))
            {
                query = "update Physician set MiddleName=@newUpdate where Id=@id";
            }

            else if (parameterName.Equals("LastName"))
            {
                query = "update Physician set LastName=@newUpdate where Id=@id";
            }
            else if (parameterName.Equals("Speciality"))
            {
                query = "update Physician set Speciality=@newUpdate where Id=@id";
            }
            else if (parameterName.Equals("Qualification"))
            {
                query = "update Physician set Qualification=@newUpdate where Id=@id";
            }
            else if (parameterName.Equals("Email"))
            {
                query = "update Physician set Email=@newUpdate where Id=@id";
            }
            else
            {
                tempNewUpdate = Convert.ToInt64(newUpdate);
                query = "update Physician set PhoneNumber=@newUpdate where Id=@id";
            }

            string status = "Updated";
            try
            {
                Physician checkIfExist = SendPhysicianById(physicianId);
                if (checkIfExist.PhysicianId!=physicianId)
                {
                    throw new InvalidObjectException("Sorry! PhysicianId does not exist");
                }
                else
                {
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@newUpdate", tempNewUpdate);
                    cmd.Parameters.AddWithValue("@id", physicianId);
                    connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new InvalidObjectException(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return status;
        }



        public static string NewAppointment(Appointment newAppointment)
        {
            string status = "Appointment Fixed";

            try
            {
                string query = "sp_saveAppointment"; //"Insert into Appointment values(@BId,@BDate,@BTime,@ADate,@ATime,@APatient,@PDoctor,@Fee,@FeeStatus,@AStatus)";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BId", newAppointment.BookingId);
                cmd.Parameters.AddWithValue("@BDate", newAppointment.BookingDateAndTime.Date);
                cmd.Parameters.AddWithValue("@BTime", newAppointment.BookingDateAndTime.TimeOfDay);
                cmd.Parameters.AddWithValue("@ADate", newAppointment.AppointmentDateAndTime.Date);
                cmd.Parameters.AddWithValue("@ATime", newAppointment.AppointmentDateAndTime.TimeOfDay);
                cmd.Parameters.AddWithValue("@APatient", newAppointment.AppointedPatient);
                cmd.Parameters.AddWithValue("@PDoctor", newAppointment.PreferredDoctor);
                cmd.Parameters.AddWithValue("@Fee", newAppointment.ConsultationFee);
                cmd.Parameters.AddWithValue("@FeeStatus", newAppointment.FeeStatus);
                cmd.Parameters.AddWithValue("@AStatus", newAppointment.AppointmentStatus);
                connection.Open();
                int c = cmd.ExecuteNonQuery();
                if (c <= 0)
                {
                    throw new InvalidObjectException("Appointment cannot be fixed");
                }
            }
            catch (Exception ex)
            {
                throw new InvalidObjectException("Appointment Section: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }


            return status;
        }
        public static Appointment SendAppointmentDetails(string bookingId)
        {
            Appointment sendAppointment= new Appointment();
            try
            {

                string query = "sp_fetchAppointmentByBookingId";//"Select * from Appointment where BookingId=@bookingid";
                SqlDataAdapter sdr = new SqlDataAdapter(query, connection);
                sdr.SelectCommand.CommandType = CommandType.StoredProcedure;
                sdr.SelectCommand.Parameters.AddWithValue("@bookingId", bookingId);
                DataTable dt = new DataTable();
                sdr.Fill(dt);
                if (dt.Rows.Count == 0)
                {
                    throw new InvalidObjectException("No Appointment Available with BookingId : " + bookingId);
                }
                else
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        if (row[0].ToString() == bookingId)
                        {
                            sendAppointment.BookingId = row[0].ToString();
                            sendAppointment.BookingDateAndTime = Convert.ToDateTime(row[1].ToString()).Date + Convert.ToDateTime(row[2].ToString()).TimeOfDay;
                            sendAppointment.AppointmentDateAndTime = Convert.ToDateTime(row[3].ToString()).Date + Convert.ToDateTime(row[4].ToString()).TimeOfDay;
                            sendAppointment.AppointedPatient = row[5].ToString();
                            sendAppointment.PreferredDoctor = row[6].ToString();
                            sendAppointment.ConsultationFee = (float)Convert.ToDouble(row[7].ToString());
                            sendAppointment.FeeStatus = row[8].ToString();
                            sendAppointment.AppointmentStatus = row[9].ToString();
                        
                           break;                      
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidObjectException("Appointment Section: " + ex.Message);
            }
            return sendAppointment;
        }
        public static List<Appointment> SendAllAppointmentDetails()
        {
            List<Appointment> appointments = new List<Appointment>();
            try
            {
                string query = "sp_fetchAppointmentByBookingId"; //"sp_fetchAllAppointment";
                SqlDataAdapter sdr = new SqlDataAdapter(query, connection);
                sdr.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                sdr.Fill(dt);
                if (dt.Rows.Count == 0)
                {
                    throw new InvalidObjectException("No Appointment Details Available");
                }
                else
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        appointments.Add(
                            new Appointment()
                            {
                                BookingId = row[0].ToString(),
                                BookingDateAndTime = Convert.ToDateTime(row[1].ToString()).Date + Convert.ToDateTime(row[2].ToString()).TimeOfDay,
                                AppointmentDateAndTime = Convert.ToDateTime(row[3].ToString()).Date + Convert.ToDateTime(row[4].ToString()).TimeOfDay,
                                AppointedPatient = row[5].ToString(),
                                PreferredDoctor = row[6].ToString(),
                                ConsultationFee = (float)Convert.ToDouble(row[7].ToString()),
                                FeeStatus = row[8].ToString(),
                                AppointmentStatus = row[9].ToString()
                            }                          
                            );
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidObjectException("Appointment Section: " + ex.Message);
            }
            return appointments;
        }
        public static string CancelAppointment(string bookingId)
        {
            string status = "Cancelled";
            try
            {
                Appointment checkIfExist = SendAppointmentDetails(bookingId);
                if (checkIfExist.BookingId!= bookingId)
                {
                    throw new InvalidObjectException("Appointment Record could not be found " + bookingId);
                }
                else
                {
                    string query = "sp_updateAppointmentStatusToCancel";//"update Appointment set AppointmentStatus=@cancelled where BookingId=@id";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@cancelled", "Cancelled");
                    cmd.Parameters.AddWithValue("@id", bookingId);
                    connection.Open();
                    int a = cmd.ExecuteNonQuery();
                    if (a <= 0)
                    {
                        throw new InvalidObjectException("Appointment could not be cancelled " + bookingId);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidObjectException("Appointment Section: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return status;
        }
        public static string UpdateAppointmentDetails()
        {
            string status ="Nothing To Update";
            try
            {
                string query = "sp_updateAppointmentStatus";//"update Appointment set AppointmentStatus=@completed where BookingId=@id";
                string id;
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.CommandType = CommandType.StoredProcedure;
                List<Appointment> appointmentsList = SendAllAppointmentDetails();
                connection.Open();
                foreach (Appointment appointment in appointmentsList)
                {
                    int count = (appointment.AppointmentDateAndTime.Date - DateTime.Now.Date).Days;
                    int countHours = (appointment.AppointmentDateAndTime.TimeOfDay - DateTime.Now.TimeOfDay).Hours;
                    if (count < 0)
                    {

                        id = appointment.BookingId;
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.Parameters.AddWithValue("@completed", "Completed");
                        int c = cmd.ExecuteNonQuery();
                        if (c <= 0)
                        {
                            throw new InvalidObjectException("Appointment Status could not be updated");
                        }
                        status = "AppointmentStatus Updated";
                    }
                    else if (count == 0)
                    {
                        if (countHours <= 0)
                        {
                            id = appointment.BookingId;
                            cmd.Parameters.AddWithValue("@id", id);
                            cmd.Parameters.AddWithValue("@completed", "Completed");
                            int c = cmd.ExecuteNonQuery();
                            if (c <= 0)
                            {
                                throw new InvalidObjectException("Appointment Status could not be updated");
                            }
                            status = "AppointmentStatus Updated";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidObjectException("Appointment Section: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return status;
        }



        public static string AddNewPatient(Patient newPatient)
        {
            string message = "Successfully Added";
            try
            {
                string query = "sp_AddPatient";//"Insert into Patient values(@Id,@FirstName,@MiddleName,@LastName,@Address,@DOB,@BMI,@IsDiabetic,@MedicalHistory,@PNum)";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", newPatient.PatientId);
                cmd.Parameters.AddWithValue("@FirstName", newPatient.PatientFirstName);
                cmd.Parameters.AddWithValue("@MiddleName", newPatient.PatientMiddleName);
                cmd.Parameters.AddWithValue("@LastName", newPatient.PatientLastName);
                cmd.Parameters.AddWithValue("@Address", newPatient.PatientAddressCode);
                cmd.Parameters.AddWithValue("@DOB", newPatient.PatientDOB.Date);
                cmd.Parameters.AddWithValue("@BMI", newPatient.PatientBMI);
                cmd.Parameters.AddWithValue("@IsDiabetic", newPatient.IsDiabeticPatient.ToString());
                cmd.Parameters.AddWithValue("@MedicalHistory", newPatient.PatientMedicalHistory);
                cmd.Parameters.AddWithValue("@PNum", newPatient.PatientPhoneNumber);
                connection.Open();
                int c = (int)cmd.ExecuteNonQuery();
                if (c <= 0)
                {
                    throw new InvalidObjectException("Insertion Failed");
                }

            }
            catch (Exception ex)
            {
                throw new InvalidObjectException(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return message;
        }
        public static string DeletePatient(string patientId)
        {
            string status = "Patient Deleted";
            try
            {
                Patient checkIfExist = SendPatientById(patientId);
                if (checkIfExist.PatientId != patientId)
                {
                    throw new InvalidObjectException("PatientId could not be found");
                }
                else
                {

                    string query = "sp_DeletePatient";// "Delete from Patient where Id=@id";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", patientId);
                    connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            catch (Exception ex)
            {
                throw new InvalidObjectException(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return status;
        }
        public static List<Patient> SendAllPatients()
        {

            List<Patient> patients = new List<Patient>();
            try
            {
                string query = "sp_fetchAllPatient";
                SqlDataAdapter sdr = new SqlDataAdapter(query, connection);
                sdr.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                sdr.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        patients.Add(
                            new Patient()
                            {
                                PatientId = row[0].ToString(),
                                PatientFirstName = row[1].ToString(),
                                PatientMiddleName = row[2].ToString(),
                                PatientLastName = row[3].ToString(),
                                PatientAddressCode = row[4].ToString(),
                                PatientDOB = Convert.ToDateTime(row[5].ToString()).Date,
                                PatientBMI = (float)Convert.ToDouble(row[6].ToString()),
                                IsDiabeticPatient = Convert.ToBoolean(row[7].ToString()),
                                PatientMedicalHistory = row[8].ToString(),
                                PatientPhoneNumber = Convert.ToInt64(row[9].ToString())
                            }
                            );
                    }
                }
                else
                {
                    throw new InvalidObjectException("Sorry! No Patient's Record is found");
                }
            }
            catch (Exception ex)
            {
                throw new InvalidObjectException(ex.Message);
            }
            return patients;
        }
        public static Patient SendPatientById(string patientId)
        {
            Patient sendPatient = new Patient();
            try
            {

                string query = "sp_FetchPatientById";//"Select * from Patient where Id=@id";
                SqlDataAdapter sdr = new SqlDataAdapter(query, connection);
                sdr.SelectCommand.CommandType = CommandType.StoredProcedure;
                sdr.SelectCommand.Parameters.AddWithValue("@id", patientId);
                DataTable dt = new DataTable();
                sdr.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        if (row[0].ToString() == patientId)
                        {
                            sendPatient.PatientId = row[0].ToString();
                            sendPatient.PatientFirstName = row[1].ToString();
                            sendPatient.PatientMiddleName = row[2].ToString();
                            sendPatient.PatientLastName = row[3].ToString();
                            sendPatient.PatientAddressCode = row[4].ToString();
                            sendPatient.PatientDOB = Convert.ToDateTime(row[5].ToString()).Date;
                            sendPatient.PatientBMI = (float)Convert.ToDouble(row[6].ToString());
                            sendPatient.IsDiabeticPatient = Convert.ToBoolean(row[7].ToString());
                            sendPatient.PatientMedicalHistory = row[8].ToString();
                            sendPatient.PatientPhoneNumber = Convert.ToInt64(row[9].ToString());
                            break;
                        }
                    }
                }
                else
                {
                    throw new InvalidObjectException("Sorry! PatientId does not exist");
                }
            }
            catch (Exception ex)
            {
                throw new InvalidObjectException(ex.Message);
            }
            return sendPatient;

        }
        public static List<Patient> SendPatientsByPhoneNo(long phoneNumber)
        {

            List<Patient> patients = new List<Patient>();
            try
            {   
                string query ="sp_FetchPatientByPhoneNumber";// "Select * from Patient where PNum=@pNo";
                SqlDataAdapter sdr = new SqlDataAdapter(query, connection);
                sdr.SelectCommand.CommandType = CommandType.StoredProcedure;
                sdr.SelectCommand.Parameters.AddWithValue("@pNo", phoneNumber);
                DataTable dt = new DataTable();
                sdr.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        patients.Add(
                            new Patient()
                            {
                                PatientId = row[0].ToString(),
                                PatientFirstName = row[1].ToString(),
                                PatientMiddleName = row[2].ToString(),
                                PatientLastName = row[3].ToString(),
                                PatientAddressCode = row[4].ToString(),
                                PatientDOB = Convert.ToDateTime(row[5].ToString()).Date,
                                PatientBMI = (float)Convert.ToDouble(row[6].ToString()),
                                IsDiabeticPatient = Convert.ToBoolean(row[7].ToString()),
                                PatientMedicalHistory = row[8].ToString(),
                                PatientPhoneNumber = Convert.ToInt64(row[9].ToString())
                            }
                            );
                    }
                }
                else
                {
                    throw new InvalidObjectException("Sorry! No Patient found with the PhoneNumber"+phoneNumber);
                }
            }
            catch(Exception ex)
            {
                throw new InvalidObjectException(ex.Message);
            }
            return patients;
        }
        public static string UpdatePatientDetails(string patientId,string parameterName,string newUpdate)
        {

            
                string query;
                object tempNewUpdate = newUpdate;

                if (parameterName.Equals("FirstName"))
                {
                    query = "update Patient set FirstName=@newUpdate where Id=@id";
                }

                else if (parameterName.Equals("MiddleName"))
                {
                    query = "update Patient set MiddleName=@newUpdate where Id=@id";
                }

                else if (parameterName.Equals("LastName"))
                {
                    query = "update Patient set LastName=@newUpdate where Id=@id";
                }
                else if (parameterName.Equals("Address"))
                {
                    query = "update Patient set Address=@newUpdate where Id=@id";
                }
                else if (parameterName.Equals("DOB"))
                {
                tempNewUpdate = Convert.ToDateTime(newUpdate).Date;
                    query = "update Patient set DOB=@newUpdate where Id=@id";
                }
                else if (parameterName.Equals("BMI"))
                {
                tempNewUpdate = (float)Convert.ToDouble(newUpdate);
                    query = "update Patient set BMI=@newUpdate where Id=@id";
                }
                else if (parameterName.Equals("IsDiabetic"))
                {
               
                    query = "update Patient set IsDiabetic=@newUpdate where Id=@id";
                }
                else if (parameterName.Equals("MedicalHistory"))
                {
                    query = "update Patient set MedicalHistory=@newUpdate where Id=@id";
                }
                else
                {
                    tempNewUpdate = Convert.ToInt64(newUpdate);
                    query = "update Patient set PNum=@newUpdate where Id=@id";
                }

                string status = "Updated";
                try
                {
                    Patient checkIfExist = SendPatientById(patientId);
                    if (checkIfExist.PatientId != patientId)
                    {
                        throw new InvalidObjectException("Sorry! PatientId does not exist");
                    }
                    else
                    {
                        SqlCommand cmd = new SqlCommand(query, connection);
                        cmd.Parameters.AddWithValue("@newUpdate", tempNewUpdate);
                        cmd.Parameters.AddWithValue("@id", patientId);
                        connection.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                throw new InvalidObjectException(ex.Message);
                }
                finally
                {
                    connection.Close();
                }
                return status;
            
        }
    }
}

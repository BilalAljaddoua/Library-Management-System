using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using GeneratSettings;
//Because the code is automatically generated by my own Code Generator, press (ctrl + K + D) to organize the code .    
//For More Detailes Visit My Account On GitHub : https://github.com/BilalAljaddoua . (-; 
/// <summary>
/// Provides data access methods for managing Book in the database.
/// </summary>
namespace DataAccessLayer
{
    /// <summary>
    /// Represents a Boo data transfer object (DTO) for transferring Boo information.
    /// </summary>
    public class clsBookDTO
    {
        public enum enMode { AddNew = 0, Update = 1 };
        private enMode Mode { set; get; }

        public int? BookID { set; get; }
        public string Title { set; get; }
        public string author { set; get; }
        public DateTime? PublicationYear { set; get; }
        public string ISBN { set; get; }


        /// <summary>
        /// Initializes a new instance of the <see cref="clsBookDTO"/> class with specified properties.
        /// </summary>
        public clsBookDTO(int? BookID, string Title, string author, DateTime? publicationyear,string ISBN)
        {
            this.BookID = BookID;
            this.Title = Title;
            this.author = author;
            this.PublicationYear = publicationyear;
            this.ISBN = ISBN;
        }
    }
    public class clsBookData
    {
        /// <summary>
        /// Logs an error to the Windows Event Log with the specified method name and exception details.
        /// </summary>
        /// <param name="methodName">The name of the method where the error occurred.</param>
        /// <param name="ex">The exception that was thrown.</param>
        static void LogErrors(string methodName, Exception ex)
        {
            string source = "Library ";
            string logName = "Application";

            if (!EventLog.SourceExists(source))
            {
                EventLog.CreateEventSource(source, logName);
            }

            EventLog.WriteEntry(source, $"Error in " + methodName + ":" + ex.Message, EventLogEntryType.Error);
        }
        /// <summary>
        /// Retrieves all Book from the database.
        /// </summary>
        /// <returns>A list of <see cref="clsBookDTO"/> objects representing all Book.</returns>
        public static List<clsBookDTO> GetAllBook()
        {
            var BookList = new List<clsBookDTO>();
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_SelectFormBookTable", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                BookList.Add(new clsBookDTO(
                                      Convert.ToInt32(reader["BookID"]),
                                      Convert.ToString(reader["Title"]),
                                      Convert.ToString(reader["author"]),
                                      Convert.ToDateTime(reader["publicationyear"]),
                                      Convert.ToString(reader["ISBN"])));
                            }
                        }
                    }
                    catch (Exception ex) { LogErrors("GetAll Method", ex); }

                    return BookList;
                }
            }
        }/// <summary>
         /// Finds a Book by their unique BookID.
         /// </summary>
         /// <param name="BookID">The unique identifier for the Boo.</param>
         /// <returns>A <see cref="clsBookDTO"/> object if the Boo is found, otherwise null.</returns>
        static public clsBookDTO FindByBookID(int BookID)
        {
            clsBookDTO BookDTO = null;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_FindFormBookTable", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@BookID", BookID);
                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                BookDTO = new clsBookDTO(
                                      Convert.ToInt32(reader["BookID"]),
                                      Convert.ToString(reader["Title"]),
                                      Convert.ToString(reader["author"]),
                                      Convert.ToDateTime(reader["publicationyear"]),
                                      Convert.ToString(reader["ISBN"]));
                            }
                        }
                    }
                    catch (Exception ex) { LogErrors("Find Method", ex); }

                    return BookDTO;
                }
            }
        }
        /// <summary>
        /// Adds a new Boo to the database.
        /// </summary>
        /// <param name="BookDTO">The <see cref="clsBookDTO"/> object containing Book information.</param>
        /// <returns>The BookID of the newly added Boo, or null if the operation fails.</returns>
        static public int? AddToBookTable(clsBookDTO BookDTO)
        {
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_InsertIntoBookTable", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Title", BookDTO.Title);
                    command.Parameters.AddWithValue("@author", BookDTO.author);
                    command.Parameters.AddWithValue("@publicationyear", BookDTO.PublicationYear);
                    command.Parameters.AddWithValue("@ISBN", BookDTO.ISBN);

                    SqlParameter parameter = new SqlParameter("@BookID", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(parameter);
                    int? BookID = null;
                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        BookID = (int)command.Parameters["@BookID"].Value;
                    }
                    catch (Exception ex) { LogErrors("AddNew Method", ex); }

                    return BookID;
                }
            }
        }

        /// <summary>
        /// Updates an existing Boo's information in the database.
        /// </summary>
        /// <param name="BookDTO">The <see cref="clsBookDTO"/> object containing updated Boo information.</param>
        /// <returns>True if the update is successful, otherwise false.</returns>
        static public bool UpdateBookTable(clsBookDTO BookDTO)
        {
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_UpdateBookTable", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@BookID", BookDTO.BookID);
                    command.Parameters.AddWithValue("@Title", BookDTO.Title);
                    command.Parameters.AddWithValue("@author", BookDTO.author);
                    command.Parameters.AddWithValue("@publicationyear", BookDTO.PublicationYear);
                    command.Parameters.AddWithValue("@ISBN", BookDTO.ISBN);

                    SqlParameter parameter = new SqlParameter("@IsSuccess", SqlDbType.Bit)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(parameter);
                    bool IsSuccess = false;
                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        IsSuccess = (bool)command.Parameters["@IsSuccess"].Value;
                    }
                    catch (Exception ex) { LogErrors("Update Method", ex); }


                    return IsSuccess;
                }
            }
        }

        /// <summary>
        /// Deletes a Boo from the database based on their BookID.
        /// </summary>
        /// <param name="BookID">The unique identifier for the Boo to delete.</param>
        /// <returns>True if the deletion is successful, otherwise false.</returns>
        static public bool DeleteBook(int BookID)
        {
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_DeleteFormBookTable", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@BookID", BookID);
                    SqlParameter parameter = new SqlParameter("@IsSuccess", SqlDbType.Bit)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(parameter);
                    bool IsSuccess = false;
                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        IsSuccess = (bool)command.Parameters["@IsSuccess"].Value;
                    }
                    catch (Exception ex) { LogErrors("Delete Method", ex); }

                    return IsSuccess;
                }
            }
        }

    }
}


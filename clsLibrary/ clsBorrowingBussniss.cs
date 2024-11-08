using System;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using DataAccessLayer;
//Because the code is automatically generated by my own Code Generator, press (ctrl + K + D) to organize the code .    
//For More Detailes Visit My Account On GitHub : https://github.com/BilalAljaddoua . (-; 
namespace Bussiness_Layer
{
    public class clsBorrowing
    {

        /// <summary>
        /// Enumeration representing the mode of the operation (Add or Update).
        /// </summary>
        public enum enMode { AddNew = 0, Update = 1 };

        /// <summary>
        /// Gets or sets the mode of the operation (AddNew or Update).
        /// </summary>
        public enMode Mode { set; get; }
        public int? RecordID { set; get; }
        public int? PatronID { set; get; }
        public int? BookID { set; get; }
        public DateTime? borrowingDate { set; get; }
        public DateTime? ReturnDate { set; get; }

        /// <summary>
        /// Gets the Data Transfer Object (DTO) representing the current Borrowin.
        /// </summary>
        public clsBorrowingDTO BDTO
        {
            get
            {
                return new clsBorrowingDTO(
                   this.RecordID,
                   this.PatronID,
                   this.BookID,
                   this.borrowingDate,
                   this.ReturnDate);
            }
        }
        /// <summary>
        /// Represents a business layer class for managing Borrowin operations and interactions.
        /// </summary>
        public clsBorrowing(clsBorrowingDTO BorrowingDTO, enMode nMode = enMode.AddNew)
        {
            this.RecordID = BorrowingDTO.RecordID;
            this.PatronID = BorrowingDTO.PatronID;
            this.BookID = BorrowingDTO.BookID;
            this.borrowingDate = BorrowingDTO.borrowingDate;
            this.ReturnDate = BorrowingDTO.ReturnDate;
            this.Mode = nMode;
        }
        /// <summary>
        /// Adds a new Borrowin to the database.
        /// </summary>
        /// <returns>True if the Borrowin was added successfully; otherwise, false.</returns>
        private bool _AddBorrowing()
        {
            this.RecordID = clsBorrowingData.AddToBorrowingTable(BDTO);
            return (this.RecordID != null);
        }
        /// <summary>
        /// Retrieves all Borrowing from the database.
        /// </summary>
        /// <returns>A list of <see cref="clsBorrowingDTO"/> objects representing all Borrowing.</returns>
        static public List<clsBorrowingDTO> GetAllBorrowing()
        {
            return clsBorrowingData.GetAllBorrowing();
        }
        /// <summary>
        /// Updates an existing Borrowin's information in the database.
        /// </summary>
        /// <returns>True if the update was successful; otherwise, false.</returns>
        private bool _UpdateBorrowing()
        {
            return clsBorrowingData.UpdateBorrowingTable(BDTO);
        }
        /// <summary>
        /// Finds a Borrowing by their RecordID.
        /// </summary>
        /// <param name="RecordID">The unique identifier for the Borrowin.</param>
        /// <returns>A <see cref="clsBorrowing"/> object if the Borrowin is found; otherwise, null.</returns>
        static public clsBorrowing FindByRecordID(int RecordID)
        {
            var Borrowin = clsBorrowingData.FindByRecordID(RecordID);
            if (Borrowin != null)
            {
                return new clsBorrowing(Borrowin, enMode.Update);
            }
            else
                return null;
        }

        /// <summary>
        /// Deletes a Borrowin from the database based on their RecordID
        /// </summary>
        /// <param name="RecordID">The unique identifier for the Borrowin to delete.</param>
        /// <returns>True if the deletion was successful; otherwise, false.</returns>
        public static bool DeleteBorrowing(int RecordID)
        {
            return clsBorrowingData.DeleteBorrowing(RecordID);
        }
        /// <summary>
        /// Saves the current User record in the database based on the specified mode.
        /// </summary>
        /// <returns>True if the operation is successful; otherwise, false.</returns>
        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddBorrowing())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:                    return _UpdateBorrowing();

            }

            return false;
        }
    }
}


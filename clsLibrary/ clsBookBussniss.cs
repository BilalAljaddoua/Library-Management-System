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
    public class clsBook
    {

        /// <summary>
        /// Enumeration representing the mode of the operation (Add or Update).
        /// </summary>
        public enum enMode { AddNew = 0, Update = 1 };

        /// <summary>
        /// Gets or sets the mode of the operation (AddNew or Update).
        /// </summary>
        public enMode Mode { set; get; }
        public int? BookID { set; get; }
        public string Title { set; get; }
        public string author { set; get; }
        public DateTime? PublicationYear { set; get; }
        public string ISBN { set; get; }

        /// <summary>
        /// Gets the Data Transfer Object (DTO) representing the current Boo.
        /// </summary>
        public clsBookDTO BDTO
        {
            get
            {
                return new clsBookDTO(
                   this.BookID,
                   this.Title,
                   this.author,
                   this.PublicationYear,
                   this.ISBN);
            }
        }
        /// <summary>
        /// Represents a business layer class for managing Boo operations and interactions.
        /// </summary>
        public clsBook(clsBookDTO BookDTO, enMode nMode = enMode.AddNew)
        {
            this.BookID = BookDTO.BookID;
            this.Title = BookDTO.Title;
            this.author = BookDTO.author;
            this.PublicationYear = BookDTO.PublicationYear;
            this.ISBN = BookDTO.ISBN;
            this.Mode = nMode;
        }
        /// <summary>
        /// Adds a new Boo to the database.
        /// </summary>
        /// <returns>True if the Boo was added successfully; otherwise, false.</returns>
        private bool _AddBook()
        {
            this.BookID = clsBookData.AddToBookTable(BDTO);
            return (this.BookID != null);
        }
        /// <summary>
        /// Retrieves all Book from the database.
        /// </summary>
        /// <returns>A list of <see cref="clsBookDTO"/> objects representing all Book.</returns>
        static public List<clsBookDTO> GetAllBook()
        {
            return clsBookData.GetAllBook();
        }
        /// <summary>
        /// Updates an existing Boo's information in the database.
        /// </summary>
        /// <returns>True if the update was successful; otherwise, false.</returns>
        private bool _UpdateBook()
        {
            return clsBookData.UpdateBookTable(BDTO);
        }
        /// <summary>
        /// Finds a Book by their BookID.
        /// </summary>
        /// <param name="BookID">The unique identifier for the Boo.</param>
        /// <returns>A <see cref="clsBook"/> object if the Boo is found; otherwise, null.</returns>
        static public clsBook FindByBookID(int BookID)
        {
            var Book = clsBookData.FindByBookID(BookID);
            if (Book != null)
            {
                return new clsBook(Book, enMode.Update);
            }
            else
                return null;
        }

        /// <summary>
        /// Deletes a Boo from the database based on their BookID
        /// </summary>
        /// <param name="BookID">The unique identifier for the Boo to delete.</param>
        /// <returns>True if the deletion was successful; otherwise, false.</returns>
        public static bool DeleteBook(int BookID)
        {
            return clsBookData.DeleteBook(BookID);
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
                    if (_AddBook())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:                    return _UpdateBook();

            }

            return false;
        }
    }
}


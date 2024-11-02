using Bussiness_Layer;
using DataAccessLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers.Book
{
    [Route("api/books/")] // Defines the base route for book-related API endpoints
    [ApiController] // Indicates that this class is an API controller
    public class BooksController : ControllerBase
    {
        /// <summary>
        /// Gets all books from the library.
        /// </summary>
        /// <returns>A list of books.</returns>
        [HttpGet(Name = "GetBookByID")]
        [ProducesResponseType(StatusCodes.Status200OK)] // Successful response
        [ProducesResponseType(StatusCodes.Status404NotFound)] // Not found response
        public ActionResult<IEnumerable<clsBookDTO>> GetAllBooks()
        {
            List<clsBookDTO> booksList = clsBook.GetAllBook(); // Fetch all books
            if (booksList.Count != 0)
            {
                return Ok(booksList); // Return the list of books if found
            }
            else
            {
                return NotFound("There are not any book in the library"); // Return 404 if no books are found
            }
        }

        /// <summary>
        /// Gets a book by its ID.
        /// </summary>
        /// <param name="id">The ID of the book.</param>
        /// <returns>The book information.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)] // Successful response
        [ProducesResponseType(StatusCodes.Status404NotFound)] // Not found response
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // Bad request response
        public ActionResult<IEnumerable<clsBookDTO>> GetBookByID(int id)
        {
            if (id < 0)
            {
                return BadRequest("Invalied Number :" + id); // Return 400 if the ID is invalid
            }
            var book = clsBook.FindByBookID(id); // Find the book by ID
            if (book == null)
            {
                return NotFound("There are not found a book with id:" + id); // Return 404 if not found
            }
            return Ok(book.BDTO); // Return book data
        }

        /// <summary>
        /// Adds a new book record.
        /// </summary>
        /// <param name="NewBook">The new book data.</param>
        /// <returns>The added book information.</returns>
        [HttpPost(Name = "AddNewBook")]
        [ProducesResponseType(StatusCodes.Status200OK)] // Successful response
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // Bad request response
        [ProducesResponseType(StatusCodes.Status500InternalServerError)] // Server error response
        public ActionResult<clsBookDTO> AddNewBook(clsBookDTO NewBook)
        {
            // Validate the ISBN and other parameters
            if (!clsUtils.IsValidISBN(NewBook.ISBN.ToString()))
            {
                return BadRequest("The ISBN that you provided is wrong, try another one like :1-56619-909-3");
            }
            if (!clsUtils.IsName(NewBook.author) || !clsUtils.IsName(NewBook.Title) ||
                !clsUtils.IsDate(NewBook.PublicationYear.Value))
            {
                return BadRequest("There are some wrong parameters, Please Check..");
            }
            // Create a new book object and save it
            clsBook book1 = new clsBook(new clsBookDTO(null, NewBook.Title, NewBook.author, NewBook.PublicationYear, NewBook.ISBN));
            if (book1.Save())
            {
                NewBook.BookID = book1.BookID; // Set the BookID of the new book
                return CreatedAtRoute("GetBookByID", new { id = NewBook.BookID }, NewBook); // Return 201 Created response with location
            }
            else
                return StatusCode(500, "An internal server error occurred. Please try again later."); // Return 500 on failure
        }

        /// <summary>
        /// Updates an existing book record.
        /// </summary>
        /// <param name="id">The ID of the book to update.</param>
        /// <param name="UpdatedBook">The updated book data.</param>
        /// <returns>The updated book information.</returns>
        [HttpPut("{id}", Name = "UpdateBook")]
        [ProducesResponseType(StatusCodes.Status200OK)] // Successful response
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // Bad request response
        [ProducesResponseType(StatusCodes.Status404NotFound)] // Not found response
        [ProducesResponseType(StatusCodes.Status500InternalServerError)] // Server error response
        public ActionResult<clsBookDTO> UpdateBookInformation(int id, clsBookDTO UpdatedBook)
        {
            var book1 = clsBook.FindByBookID(id); // Find the book by ID
            if (book1 == null)
            {
                return NotFound($"The book with id :{id} is not found.."); // Return 404 if not found
            }
            // Validate the ISBN and other parameters
            if (!clsUtils.IsValidISBN(UpdatedBook.ISBN.ToString()))
            {
                return BadRequest("The ISBN that you provided is wrong, try another one like :1-56619-909-3");
            }
            if (!clsUtils.IsName(UpdatedBook.author) || !clsUtils.IsName(UpdatedBook.Title) ||
                !clsUtils.IsDate(UpdatedBook.PublicationYear.Value))
            {
                return BadRequest("There are some wrong parameters, Please Check..");
            }
            // Update book properties
            book1.author = UpdatedBook.author;
            book1.ISBN = UpdatedBook.ISBN;
            book1.PublicationYear = UpdatedBook.PublicationYear;
            book1.Title = UpdatedBook.Title;

            if (book1.Save())
                return Ok(book1.BDTO); // Return updated book data
            else
                return StatusCode(500, "An internal server error occurred. Please try again later."); // Return 500 on failure
        }

        /// <summary>
        /// Deletes a book record.
        /// </summary>
        /// <param name="id">The ID of the book to delete.</param>
        /// <returns>A confirmation message.</returns>
        [HttpDelete("{id}", Name = "DeleteBook")]
        [ProducesResponseType(StatusCodes.Status200OK)] // Successful response
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // Bad request response
        [ProducesResponseType(StatusCodes.Status404NotFound)] // Not found response
        [ProducesResponseType(StatusCodes.Status500InternalServerError)] // Server error response
        public ActionResult DeleteBook(int id)
        {
            if (clsBook.FindByBookID(id) == null)
            {
                return NotFound("There is no book with id :" + id); // Return 404 if not found
            }
            if (!clsUtils.IsPositiveNumber(id.ToString()))
            {
                return BadRequest($"The id :{id} is invalid, Please Check.."); // Return 400 if ID is invalid
            }

            if (clsBook.DeleteBook(id))
            {
                return Ok($"The book with id : {id} deleted successfully"); // Return success message
            }
            else
                return StatusCode(500, "An internal server error occurred. Please try again later."); // Return 500 on failure
        }
    }
}

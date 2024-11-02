using Bussiness_Layer;
using DataAccessLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers.Book
{
    [Route("api/books/")]
    [ApiController]
    public class BookmanagementController : ControllerBase
    {
        [HttpGet(Name = "GetBookByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<clsBookDTO>> GetAllBooks()
        {
            List<clsBookDTO> booksList = clsBook.GetAllBook();
            if (booksList.Count != 0)
            {
                return Ok(booksList);
            }
            else
            { return NotFound("There are not any book in the library"); }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<IEnumerable<clsBookDTO>> GetBookByID(int id)
        {
            if (id < 0)
            {
                return BadRequest("Invalied Number :" + id);
            }
            var book = clsBook.FindByBookID(id);
            if (book == null)
            {
                return NotFound("There are not found a book with id:" + id);
            }
            return Ok(book.BDTO);
        }

        [HttpPost(Name ="AddNewBook")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<clsBookDTO> AddNewBook(clsBookDTO NewBook)
        {
            if (!clsUtils.IsValidISBN(NewBook.ISBN.ToString()))
            {
                return BadRequest("The ISBN that you provied is wrong ,try anothr one like :1-56619-909-3");
            }
            if (!clsUtils.IsName(NewBook.author) || !clsUtils.IsName(NewBook.Title) || !clsUtils.IsName(NewBook.author) ||
                 !clsUtils.IsDate(NewBook.PublicationYear.Value))
            {
                return BadRequest("There are some wrong parameters, Please Check..");
            }
            clsBook book1 = new clsBook(new clsBookDTO(null, NewBook.Title, NewBook.author, NewBook.PublicationYear, NewBook.ISBN));
            if (book1.Save())
            {
                NewBook.BookID = book1.BookID;
                return CreatedAtRoute("GetBookByID", new { id = NewBook.BookID },NewBook);
            }
            else
                return StatusCode(500, "An internal server error occurred. Please try again later.");

        }

        [HttpPut("{id}",Name ="UpdateBook")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<clsBookDTO> UpdateBookInformation(int id, clsBookDTO UpdatedBook)
        {
            clsBook book1 = clsBook.FindByBookID(id);
            if (book1 == null)
            {
                return NotFound($"The book with id :{id} is not found..");
            }
            if (!clsUtils.IsValidISBN(UpdatedBook.ISBN.ToString()))
            {
                return BadRequest("The ISBN that you provied is wrong ,try anothr one like :1-56619-909-3");
            }
            if (!clsUtils.IsName(UpdatedBook.author) || !clsUtils.IsName(UpdatedBook.Title) || !clsUtils.IsName(UpdatedBook.author) ||
                 !clsUtils.IsDate(UpdatedBook.PublicationYear.Value))
            {
                return BadRequest("There are some wrong parameters, Please Check..");
            }
            book1.author = UpdatedBook.author;
            book1.ISBN = UpdatedBook.ISBN;
            book1.PublicationYear = UpdatedBook.PublicationYear;
            book1.Title = UpdatedBook.Title;

            if (book1.Save())
                return Ok(book1.BDTO);
            else
                return StatusCode(500, "An internal server error occurred. Please try again later.");

        }

        [HttpDelete("{id}", Name = "DeleteBook")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult DeleteBook(int id)
        {
            if (clsBook.FindByBookID(id) == null)
            {
                return NotFound("There is no book with id :" + id);
            }
            if (!clsUtils.IsPositiveNumber(id.ToString()))
            {
                return BadRequest($"The id :{id} is invalied , Please Check..");
            }

            if (clsBook.DeleteBook(id))
            {
                return Ok($"The book with id : {id} deleted successfully");
            }
            else
                return StatusCode(500, "An internal server error occurred. Please try again later.");

        }


    }
}

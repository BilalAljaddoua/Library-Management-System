using Bussiness_Layer;
using DataAccessLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Library_Management_System.Controllers.Borrowing
{
    [Route("api/Borrowing/")] // Defines the base route for borrowing-related API endpoints
    [ApiController] // Indicates that this class is an API controller
    public class BorrowingController : ControllerBase
    {
        /// <summary>
        /// Gets all borrowings from the library.
        /// </summary>
        /// <returns>A list of borrowings.</returns>
        [HttpGet(Name = "GetBorrowingByID")]
        [ProducesResponseType(StatusCodes.Status200OK)] // Successful response
        [ProducesResponseType(StatusCodes.Status404NotFound)] // Not found response
        public ActionResult<IEnumerable<clsBorrowingDTO>> GetAllBorrowings()
        {
            List<clsBorrowingDTO> BorrowingsList = clsBorrowing.GetAllBorrowing(); // Fetch all borrowings
            if (BorrowingsList.Count != 0)
            {
                return Ok(BorrowingsList); // Return the list of borrowings if found
            }
            else
            {
                return NotFound("There are not any Borrowing in the library"); // Return 404 if no borrowings are found
            }
        }

        /// <summary>
        /// Gets a borrowing by its ID.
        /// </summary>
        /// <param name="id">The ID of the borrowing.</param>
        /// <returns>The borrowing information.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)] // Successful response
        [ProducesResponseType(StatusCodes.Status404NotFound)] // Not found response
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // Bad request response
        public ActionResult<IEnumerable<clsBorrowingDTO>> GetBorrowingByID(int id)
        {
            if (id < 0)
            {
                return BadRequest("Invalied Number :" + id); // Return 400 if the ID is invalid
            }
            var Borrowing = clsBorrowing.FindByRecordID(id); // Find the borrowing by ID
            if (Borrowing == null)
            {
                return NotFound("There are not found a Borrowing with id:" + id); // Return 404 if not found
            }
            return Ok(Borrowing.BDTO); // Return borrowing data
        }

        /// <summary>
        /// Adds a new borrowing record.
        /// </summary>
        /// <param name="NewBorrowing">The new borrowing data.</param>
        /// <returns>The added borrowing information.</returns>
        [HttpPost(Name = "AddNewBorrowing")]
        [ProducesResponseType(StatusCodes.Status200OK)] // Successful response
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // Bad request response
        [ProducesResponseType(StatusCodes.Status500InternalServerError)] // Server error response
        public ActionResult<clsBorrowingDTO> AddNewBorrowing(clsBorrowingDTO NewBorrowing)
        {
            // Validate the new borrowing data
            if (!clsUtils.IsPositiveNumber(NewBorrowing.PatronID.ToString()) ||
                !clsUtils.IsDate(NewBorrowing.ReturnDate.Value) ||
                !clsUtils.IsDate(NewBorrowing.borrowingDate.Value))
            {
                return BadRequest("There are some wrong parameters, Please Check..");
            }
            if (clsBook.FindByBookID(NewBorrowing.BookID.Value) == null)
            {
                return BadRequest($"There are no Book With ID :{NewBorrowing.BookID}, Please Check..");
            }
            if (clsPatron.FindByID(NewBorrowing.PatronID.Value) == null)
            {
                return BadRequest($"There are no Patron With ID :{NewBorrowing.PatronID}, Please Check..");
            }

            // Create a new borrowing object and save it
            clsBorrowing Borrowing1 = new clsBorrowing(new clsBorrowingDTO(NewBorrowing.RecordID, NewBorrowing.PatronID, NewBorrowing.BookID.Value, NewBorrowing.borrowingDate, NewBorrowing.ReturnDate));
            if (Borrowing1.Save())
            {
                NewBorrowing.RecordID = Borrowing1.RecordID; // Set the RecordID of the new borrowing
                return CreatedAtRoute("GetBorrowingByID", new { id = NewBorrowing.RecordID }, NewBorrowing); // Return 201 Created response with location
            }
            else
                return StatusCode(500, "An internal server error occurred. Please try again later."); // Return 500 on failure
        }

        /// <summary>
        /// Updates an existing borrowing record.
        /// </summary>
        /// <param name="id">The ID of the borrowing to update.</param>
        /// <param name="UpdatedBorrowing">The updated borrowing data.</param>
        /// <returns>The updated borrowing information.</returns>
        [HttpPut("{id}", Name = "UpdateBorrowing")]
        [ProducesResponseType(StatusCodes.Status200OK)] // Successful response
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // Bad request response
        [ProducesResponseType(StatusCodes.Status404NotFound)] // Not found response
        [ProducesResponseType(StatusCodes.Status500InternalServerError)] // Server error response
        public ActionResult<clsBorrowingDTO> UpdateBorrowingInformation(int id, clsBorrowingDTO UpdatedBorrowing)
        {
            var Borrowing1 = clsBorrowing.FindByRecordID(id); // Find the borrowing by ID
            if (Borrowing1 == null)
            {
                return BadRequest($"There are no Borrowing With ID :{UpdatedBorrowing.RecordID}, Please Check.."); // Return 400 if not found
            }

            // Validate updated data
            if (clsPatron.FindByID(UpdatedBorrowing.PatronID.Value) == null)
            {
                return BadRequest($"There are no Patron With ID :{UpdatedBorrowing.PatronID}, Please Check..");
            }
            if (!clsUtils.IsPositiveNumber(UpdatedBorrowing.PatronID.ToString()) ||
                !clsUtils.IsDate(UpdatedBorrowing.ReturnDate.Value) ||
                !clsUtils.IsDate(UpdatedBorrowing.borrowingDate.Value))
            {
                return BadRequest("There are some wrong parameters, Please Check..");
            }
            if (clsBook.FindByBookID(UpdatedBorrowing.BookID.Value) == null)
            {
                return BadRequest($"There are no Book With ID :{UpdatedBorrowing.BookID}, Please Check..");
            }

            // Update borrowing properties
            Borrowing1.ReturnDate = UpdatedBorrowing.ReturnDate;
            Borrowing1.borrowingDate = UpdatedBorrowing.borrowingDate;
            Borrowing1.PatronID = UpdatedBorrowing.PatronID;
            Borrowing1.BookID = UpdatedBorrowing.BookID;

            if (Borrowing1.Save())
                return Ok(Borrowing1.BDTO); // Return updated borrowing data
            else
                return StatusCode(500, "An internal server error occurred. Please try again later."); // Return 500 on failure
        }

        /// <summary>
        /// Deletes a borrowing record.
        /// </summary>
        /// <param name="id">The ID of the borrowing to delete.</param>
        /// <returns>A confirmation message.</returns>
        [HttpDelete("{id}", Name = "DeleteBorrowing")]
        [ProducesResponseType(StatusCodes.Status200OK)] // Successful response
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // Bad request response
        [ProducesResponseType(StatusCodes.Status404NotFound)] // Not found response
        [ProducesResponseType(StatusCodes.Status500InternalServerError)] // Server error response
        public ActionResult DeleteBorrowing(int id)
        {
            if (clsBorrowing.FindByRecordID(id) == null)
            {
                return NotFound("There is no Borrowing with id :" + id); // Return 404 if not found
            }
            if (!clsUtils.IsPositiveNumber(id.ToString()))
            {
                return BadRequest($"The id :{id} is invalid , Please Check.."); // Return 400 if ID is invalid
            }

            if (clsBorrowing.DeleteBorrowing(id))
            {
                return Ok($"The Borrowing with id : {id} deleted successfully"); // Return success message
            }
            else
                return StatusCode(500, "An internal server error occurred. Please try again later."); // Return 500 on failure
        }
    }
}

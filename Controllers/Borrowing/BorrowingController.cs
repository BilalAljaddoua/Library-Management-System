using Bussiness_Layer;
using DataAccessLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Library_Management_System.Controllers.Borrowing
{
    [Route("api/Borrowing/")]
    [ApiController]
    public class BorrowingController : ControllerBase
    {

        [HttpGet(Name = "GetBorrowingByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<clsBorrowingDTO>> GetAllBorrowings()
        {
            List<clsBorrowingDTO> BorrowingsList = clsBorrowing.GetAllBorrowing();
            if (BorrowingsList.Count != 0)
            {
                return Ok(BorrowingsList);
            }
            else
            { return NotFound("There are not any Borrowing in the library"); }
        }


        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<IEnumerable<clsBorrowingDTO>> GetBorrowingByID(int id)
        {
            if (id < 0)
            {
                return BadRequest("Invalied Number :" + id);
            }
            var Borrowing = clsBorrowing.FindByRecordID(id);
            if (Borrowing == null)
            {
                return NotFound("There are not found a Borrowing with id:" + id);
            }
            return Ok(Borrowing.BDTO);
        }

        [HttpPost(Name = "AddNewBorrowing")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<clsBorrowingDTO> AddNewBorrowing(clsBorrowingDTO NewBorrowing)
        {

            if (!clsUtils.IsPositiveNumber(NewBorrowing.PatronID.ToString()) || !clsUtils.IsDate(NewBorrowing.ReturnDate.Value) || !clsUtils.IsDate(NewBorrowing.borrowingDate.Value))
            {
                return BadRequest("There are some wrong parameters, Please Check..");
            }
            if (clsBook.FindByBookID(NewBorrowing.BookID.Value) == null)
            {
                return BadRequest($"There are no Boook With ID :{NewBorrowing.BookID}, Please Check..");
            }
            if (clsPatron.FindByID(NewBorrowing.PatronID.Value) == null)
            {
                return BadRequest($"There are no Patron With ID :{NewBorrowing.PatronID}, Please Check..");
            }

            clsBorrowing Borrowing1 = new clsBorrowing(new clsBorrowingDTO(NewBorrowing.RecordID, NewBorrowing.PatronID, NewBorrowing.BookID.Value, NewBorrowing.borrowingDate, NewBorrowing.ReturnDate));
            if (Borrowing1.Save())
            {
                NewBorrowing.RecordID = Borrowing1.RecordID;
                return CreatedAtRoute("GetBorrowingByID", new { id = NewBorrowing.RecordID }, NewBorrowing);
            }
            else
                return StatusCode(500, "An internal server error occurred. Please try again later.");

        }

        [HttpPut("{id}", Name = "UpdateBorrowing")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<clsBorrowingDTO> UpdateBorrowingInformation(int id, clsBorrowingDTO UpdatedBorrowing)
        {
            var Borrowing1 = clsBorrowing.FindByRecordID(id);
            if (Borrowing1 == null)
            {
                return BadRequest($"There are no Borrowing With ID :{UpdatedBorrowing.RecordID}, Please Check..");
            }

            if (clsPatron.FindByID(UpdatedBorrowing.PatronID.Value) == null)
            {
                return BadRequest($"There are no Patron With ID :{UpdatedBorrowing.PatronID}, Please Check..");
            }
            if (!clsUtils.IsPositiveNumber(UpdatedBorrowing.PatronID.ToString()) || !clsUtils.IsDate(UpdatedBorrowing.ReturnDate.Value) || !clsUtils.IsDate(UpdatedBorrowing.borrowingDate.Value))
            {
                return BadRequest("There are some wrong parameters, Please Check..");
            }
            if (clsBook.FindByBookID(UpdatedBorrowing.BookID.Value) == null)
            {
                return BadRequest($"There are no Boook With ID :{UpdatedBorrowing.BookID}, Please Check..");
            }


            if (!clsUtils.IsPositiveNumber(UpdatedBorrowing.PatronID.ToString()) || !clsUtils.IsDate(UpdatedBorrowing.ReturnDate.Value) || !clsUtils.IsDate(UpdatedBorrowing.borrowingDate.Value))
            {
                return BadRequest("There are some wrong parameters, Please Check..");
            }
            Borrowing1.ReturnDate = UpdatedBorrowing.ReturnDate;
            Borrowing1.borrowingDate = UpdatedBorrowing.borrowingDate;
            Borrowing1.PatronID = UpdatedBorrowing.PatronID;
            Borrowing1.BookID = UpdatedBorrowing.BookID;

            if (Borrowing1.Save())
                return Ok(Borrowing1.BDTO);
            else
                return StatusCode(500, "An internal server error occurred. Please try again later.");

        }

        [HttpDelete("{id}", Name = "DeleteBorrowing")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult DeleteBorrowing(int id)
        {
            if (clsBorrowing.FindByRecordID(id) == null)
            {
                return NotFound("There is no Borrowing with id :" + id);
            }
            if (!clsUtils.IsPositiveNumber(id.ToString()))
            {
                return BadRequest($"The id :{id} is invalied , Please Check..");
            }

            if (clsBorrowing.DeleteBorrowing(id))
            {
                return Ok($"The Borrowing with id : {id} deleted successfully");
            }
            else
                return StatusCode(500, "An internal server error occurred. Please try again later.");

        }



    }
}

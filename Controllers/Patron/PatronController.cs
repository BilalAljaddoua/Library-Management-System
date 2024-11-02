using Bussiness_Layer;
using DataAccessLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Library_Management_System.Controllers.Patron
{
    [Route("api/Patron/")]
    [ApiController]
    public class PatronController : ControllerBase
    {
        [HttpGet(Name = "GetPatronByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<clsPatronDTO>> GetAllPatrons()
        {
            try
            {
                List<clsPatronDTO> PatronsList = clsPatron.GetAllPatron();
                if (PatronsList.Count != 0)
                {
                    return Ok(PatronsList);
                }
                else
                { return NotFound("There are not any Patron in the library"); }
            }
            catch (Exception ex) { return StatusCode(500, "There are an error happend in the serverside , Error message :" + ex.Message); }
        }


        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<clsPatronDTO>> GetPatronByID(int id)
        {
            try
            {
            if (id < 0)
            {
                return BadRequest("Invalied Number :" + id);
            }
            var Patron = clsPatron.FindByID(id);
            if (Patron == null)
            {
                return NotFound("There are not found a Patron with id:" + id);
            }
                return Ok(Patron.PDTO);
            }
            catch (Exception ex) { return StatusCode(500, "There are an error happend in the serverside , Error message :" + ex.Message); }

        }

        [HttpPost(Name = "AddNewPatron")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<clsPatronDTO> AddNewPatron(clsPatronDTO NewPatron)
        {

            if (!clsUtils.IsName(NewPatron.Name))
            {
                return BadRequest("The Name is  wrong , Please Check..");
            }
 

            clsPatron Patron1 = new clsPatron(new clsPatronDTO(null, NewPatron.Name, NewPatron.Email, NewPatron.PhoneNumber));
            if (Patron1.Save())
            {
                NewPatron.ID = Patron1.ID;
                return CreatedAtRoute("GetPatronByID", new { id = NewPatron.ID }, NewPatron);
            }
            else
                return StatusCode(500, "An internal server error occurred. Please try again later.");

        }

        [HttpPut("{id}", Name = "UpdatePatron")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<clsPatronDTO> UpdatePatronInformation(int id, clsPatronDTO UpdatedPatron)
        { 
            if (!clsUtils.IsPositiveNumber(id.ToString()) )
            {
                return BadRequest($"The ID number : {id}  is wrong , Please Check..");
            }
            var Patron1 = clsPatron.FindByID(id);
            if (Patron1 == null)
            {
                return BadRequest($"There are no Patron With ID :{UpdatedPatron.ID}, Please Check..");
            }
         //Should write a cod to check the email

            if ( !clsUtils.IsName(UpdatedPatron.Name))
            {
                return BadRequest($"The is invalid :{UpdatedPatron.Name}, Please Check..");
            }
            Patron1.Email = UpdatedPatron.Email;
            Patron1.PhoneNumber = UpdatedPatron.PhoneNumber;
            Patron1.Name = UpdatedPatron.Name;
 
            if (Patron1.Save())
                return Ok(Patron1.PDTO);
            else
                return StatusCode(500, "An internal server error occurred. Please try again later.");

        }

        [HttpDelete("{id}", Name = "DeletePatron")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult DeletePatron(int id)
        { 
            if (!clsUtils.IsPositiveNumber(id.ToString()))
            {
                return BadRequest($"The id :{id} is invalied , Please Check..");
            }

            if (clsPatron.FindByID(id) == null)
            {
                return NotFound("There is no Patron with id :" + id);
            }

            if (clsPatron.DeletePatron(id))
            {
                return Ok($"The Patron with id : {id} deleted successfully");
            }
            else
                return StatusCode(500, "An internal server error occurred. Please try again later.");

        }


    }
}

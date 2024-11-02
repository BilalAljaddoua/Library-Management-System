using Bussiness_Layer;
using DataAccessLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Library_Management_System.Controllers.Patron
{
    [Route("api/Patron/")] // Defines the base route for the API endpoints related to patrons
    [ApiController] // Indicates that this class is an API controller
    public class PatronController : ControllerBase
    {
        /// <summary>
        /// Gets all patrons from the library.
        /// </summary>
        /// <returns>A list of patrons.</returns>
        [HttpGet(Name = "GetPatronByID")]
        [ProducesResponseType(StatusCodes.Status200OK)] // Successful response
        [ProducesResponseType(StatusCodes.Status404NotFound)] // Not found response
        [ProducesResponseType(StatusCodes.Status500InternalServerError)] // Server error response
        public ActionResult<IEnumerable<clsPatronDTO>> GetAllPatrons()
        {
            try
            {
                List<clsPatronDTO> PatronsList = clsPatron.GetAllPatron(); // Fetch all patrons
                if (PatronsList.Count != 0)
                {
                    return Ok(PatronsList); // Return the list of patrons if found
                }
                else
                {
                    return NotFound("There are not any Patron in the library"); // Return 404 if no patrons are found
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "There are an error happend in the serverside , Error message :" + ex.Message); // Return 500 on exception
            }
        }

        /// <summary>
        /// Gets a patron by their ID.
        /// </summary>
        /// <param name="id">The ID of the patron.</param>
        /// <returns>The patron information.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)] // Successful response
        [ProducesResponseType(StatusCodes.Status404NotFound)] // Not found response
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // Bad request response
        [ProducesResponseType(StatusCodes.Status500InternalServerError)] // Server error response
        public ActionResult<IEnumerable<clsPatronDTO>> GetPatronByID(int id)
        {
            try
            {
                if (id < 0)
                {
                    return BadRequest("Invalied Number :" + id); // Return 400 if the ID is invalid
                }
                var Patron = clsPatron.FindByID(id); // Find the patron by ID
                if (Patron == null)
                {
                    return NotFound("There are not found a Patron with id:" + id); // Return 404 if patron not found
                }
                return Ok(Patron.PDTO); // Return patron data
            }
            catch (Exception ex)
            {
                return StatusCode(500, "There are an error happend in the serverside , Error message :" + ex.Message); // Return 500 on exception
            }
        }

        /// <summary>
        /// Adds a new patron to the library.
        /// </summary>
        /// <param name="NewPatron">The new patron data.</param>
        /// <returns>The added patron information.</returns>
        [HttpPost(Name = "AddNewPatron")]
        [ProducesResponseType(StatusCodes.Status200OK)] // Successful response
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // Bad request response
        [ProducesResponseType(StatusCodes.Status500InternalServerError)] // Server error response
        public ActionResult<clsPatronDTO> AddNewPatron(clsPatronDTO NewPatron)
        {
            // Validate the new patron data
            if (!clsUtils.IsName(NewPatron.Name))
            {
                return BadRequest("The Name is  wrong , Please Check..");
            }
            if (!clsUtils.IsValidEmail(NewPatron.Email))
            {
                return BadRequest($"The Email that you provied is wrong :{NewPatron.Email}, Please Check..");
            }
            if (!clsUtils.IsPositiveNumber(NewPatron.PhoneNumber))
            {
                return BadRequest($"The PhoneNumber that you provied is wrong :{NewPatron.PhoneNumber}, Please Check..");
            }

            // Create a new patron object and save it
            clsPatron Patron1 = new clsPatron(new clsPatronDTO(null, NewPatron.Name, NewPatron.Email, NewPatron.PhoneNumber));
            if (Patron1.Save())
            {
                NewPatron.ID = Patron1.ID; // Set the ID of the new patron
                return CreatedAtRoute("GetPatronByID", new { id = NewPatron.ID }, NewPatron); // Return 201 Created response with location
            }
            else
                return StatusCode(500, "An internal server error occurred. Please try again later."); // Return 500 on failure
        }

        /// <summary>
        /// Updates an existing patron's information.
        /// </summary>
        /// <param name="id">The ID of the patron to update.</param>
        /// <param name="UpdatedPatron">The updated patron data.</param>
        /// <returns>The updated patron information.</returns>
        [HttpPut("{id}", Name = "UpdatePatron")]
        [ProducesResponseType(StatusCodes.Status200OK)] // Successful response
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // Bad request response
        [ProducesResponseType(StatusCodes.Status404NotFound)] // Not found response
        [ProducesResponseType(StatusCodes.Status500InternalServerError)] // Server error response
        public ActionResult<clsPatronDTO> UpdatePatronInformation(int id, clsPatronDTO UpdatedPatron)
        {
            if (!clsUtils.IsPositiveNumber(id.ToString()))
            {
                return BadRequest($"The ID number : {id}  is wrong , Please Check..");
            }
            var Patron1 = clsPatron.FindByID(id); // Find the patron by ID
            if (Patron1 == null)
            {
                return BadRequest($"There are no Patron With ID :{UpdatedPatron.ID}, Please Check.."); // Return 400 if not found
            }
            // Validate updated data
            if (!clsUtils.IsValidEmail(Patron1.Email))
            {
                return BadRequest($"The Email that you provied is wrong :{UpdatedPatron.Email}, Please Check..");
            }
            if (!clsUtils.IsPositiveNumber(UpdatedPatron.PhoneNumber))
            {
                return BadRequest($"The PhoneNumber that you provied is wrong :{UpdatedPatron.PhoneNumber}, Please Check..");
            }
            if (!clsUtils.IsName(UpdatedPatron.Name))
            {
                return BadRequest($"The is invalid :{UpdatedPatron.Name}, Please Check..");
            }
            // Update patron's properties
            Patron1.Email = UpdatedPatron.Email;
            Patron1.PhoneNumber = UpdatedPatron.PhoneNumber;
            Patron1.Name = UpdatedPatron.Name;

            if (Patron1.Save())
                return Ok(Patron1.PDTO); // Return updated patron data
            else
                return StatusCode(500, "An internal server error occurred. Please try again later."); // Return 500 on failure
        }

        /// <summary>
        /// Deletes a patron from the library.
        /// </summary>
        /// <param name="id">The ID of the patron to delete.</param>
        /// <returns>A confirmation message.</returns>
        [HttpDelete("{id}", Name = "DeletePatron")]
        [ProducesResponseType(StatusCodes.Status200OK)] // Successful response
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // Bad request response
        [ProducesResponseType(StatusCodes.Status404NotFound)] // Not found response
        [ProducesResponseType(StatusCodes.Status500InternalServerError)] // Server error response
        public ActionResult DeletePatron(int id)
        {
            if (!clsUtils.IsPositiveNumber(id.ToString()))
            {
                return BadRequest($"The id :{id} is invalied , Please Check.."); // Return 400 if ID is invalid
            }

            if (clsPatron.FindByID(id) == null)
            {
                return NotFound("There is no Patron with id :" + id); // Return 404 if not found
            }

            if (clsPatron.DeletePatron(id))
            {
                return Ok($"The Patron with id : {id} deleted successfully"); // Return success message
            }
            else
                return StatusCode(500, "An internal server error occurred. Please try again later."); // Return 500 on failure
        }
    }
}

using LibraryManagement.Data;
using LibraryManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibraryCardController : ControllerBase
    {
        private readonly ApplicationDbContext db; // Assuming LibraryContext is your DbContext

        public LibraryCardController(ApplicationDbContext db)
        {
            this.db = db;
        }

        // Get all library cards
        [Route("GetAllLibraryCards")]
        [HttpGet]
        public IActionResult GetAllLibraryCards()
        {
            var libraryCards = db.libraryCards.ToList();
            return Ok(libraryCards);
        }

        // Get library card by User ID
        [Route("GetLibraryCardByUserId/{userId}")]
        [HttpGet]
        public IActionResult GetLibraryCardByUserId(int userId)
        {
            var libraryCard = db.libraryCards.FirstOrDefault(lc => lc.UserId == userId);

            if (libraryCard == null)
            {
                return NotFound("Library card not found.");
            }

            return Ok(libraryCard);
        }

        // Issue new library card
        [Route("IssueLibraryCard")]
        [HttpPost]
        public IActionResult IssueLibraryCard([FromBody] LibraryCard libraryCard)
        {
            if (libraryCard == null)
            {
                return BadRequest("Invalid library card data.");
            }

            // Check if the user already has a card
            var existingCard = db.libraryCards.FirstOrDefault(lc => lc.UserId == libraryCard.UserId);
            if (existingCard != null)
            {
                return Conflict("User already has a library card.");
            }

            // Issue new library card
            libraryCard.IssuedDate = DateTime.Now;
            db.libraryCards.Add(libraryCard);
            db.SaveChanges();

            return Ok("Library card issued successfully.");
        }

        // Delete a library card (optional, in case a card needs to be revoked)
        [Route("DeleteLibraryCard/{id}")]
        [HttpDelete]
        public IActionResult DeleteLibraryCard(int id)
        {
            var libraryCard = db.libraryCards.FirstOrDefault(lc => lc.LibraryCardId == id);
            if (libraryCard == null)
            {
                return NotFound("Library card not found.");
            }

            db.libraryCards.Remove(libraryCard);
            db.SaveChanges();

            return Ok("Library card deleted successfully.");
        }
    }
}

using LibraryManagement.Data;
using LibraryManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly ApplicationDbContext db;

        public BooksController(ApplicationDbContext db)
        {
            this.db = db;
        }

        [Route("GetAllBooks")]
        [HttpGet]
        public IActionResult GetAllBooks()
        {
            var books = db.books.ToList();
            return Ok(books);
        }

        [Route("AddBook")]
        [HttpPost]
        public IActionResult AddBook(Book book)
        {
            db.books.Add(book);
            db.SaveChanges();
            return CreatedAtAction(nameof(GetAllBooks), new { id = book.BookId }, book);
        }

        [Route("UpdateBook/{id}")]
        [HttpPut]
        public IActionResult UpdateBook(int id,Book book)
        {
            if (id != book.BookId)
                return BadRequest();
            db.Entry(book).State = EntityState.Modified;
            db.SaveChanges();
            return NoContent();
        }

        [Route("DeleteBook/{id}")]
        [HttpDelete]
        public IActionResult DeleteBook(int id)
        {
            var book = db.books.Find(id);
            if (book == null)
                return NotFound();
            db.books.Remove(book);
            db.SaveChanges();
            return NoContent();
        }

        [Route("IssueBook/{bookId}/{userId}")]
        [HttpPost]
        public IActionResult IssueBook(int bookId, int userId)
        {
            // Retrieve the book by its ID
            var book = db.books.FirstOrDefault(b => b.BookId == bookId);
            if (book == null)
            {
                return NotFound("Book not found.");
            }

            // Check if the book is already issued
            if (book.IsIssued)
            {
                return Conflict("This book is already issued to another user.");
            }

            // Retrieve the user by their ID
            var user = db.users.FirstOrDefault(u => u.UserId == userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Issue the book to the user
            book.IssuedToUserId = userId;  // Link the book to the user
            book.IsIssued = true;          // Mark the book as issued

            // Save the changes to the database
            db.SaveChanges();

            return Ok("Book issued successfully.");
        }
    }
}

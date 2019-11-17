using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DAL;
using LibraryManagementSystem.ViewModels;
using AutoMapper;
using Microsoft.Extensions.Logging;
using DAL.Models;

namespace LibraryManagementSystem.Controllers
{
    [Route("api/[controller]")]
    public class BookController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;

        public BookController(IMapper mapper, IUnitOfWork unitOfWork, ILogger<RegisteredUserController> logger)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        // GET: api/book
        [HttpGet]
        public IActionResult Get()
        {
            var allBooks = _unitOfWork.Books.GetAllBooks();
            if (allBooks.Count() != 0)
            {
                return Ok(_mapper.Map<IEnumerable<BookViewModel>>(allBooks));
            }
            else
            {
                return Ok("Not Found");
            }
        }       

        // GET api/book/5
        [HttpGet("GetByID/{id}")]
        public IActionResult GetByID(int id)
        {
            var book = _unitOfWork.Books.GetBookByID(id);
            if (book != null)
            {
                return Ok(_mapper.Map<BookViewModel>(book));
            }
            else
            {
                return Ok("Not Found");
            }
        }

        // GET api/book/xyz
        [HttpGet("GetByName/{name}")]
        public IActionResult GetByName(string name)
        {
            var book = _unitOfWork.Books.GetBookByName(name);
            if (book != null)
            {
                return Ok(_mapper.Map<BookViewModel>(book));
            }
            else
            {
                return Ok("Not Found");
            }
        }

        // POST api/book
        [HttpPost]
        public IActionResult Post([FromBody]Book book)
        {
            var bookAdded = _unitOfWork.Books.AddBook(book);
            if (bookAdded != null)
            {
                _unitOfWork.SaveChanges();
                return Ok(_mapper.Map<BookViewModel>(bookAdded));
            }
            else
            {
                return Ok("Could not be added");
            }
        }

        // PUT api/book/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]Book book)
        {
            var bookUpdated = _unitOfWork.Books.UpdateBook(book);
            if (bookUpdated != null)
            {
                _unitOfWork.SaveChanges();
                return Ok(_mapper.Map<BookViewModel>(bookUpdated));
            }
            else
            {
                return Ok("Could not be updated");
            }
        }

        // DELETE api/book/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var bookToDelete = _unitOfWork.Books.DeleteBook(id);

            //delete reservations for this book before deleting book
            var reservationsAgainstBook = _unitOfWork.Reservations.GetReservationByBookId(id);
            if (reservationsAgainstBook.Count() > 0)
            {
                foreach (var r in reservationsAgainstBook)
                {
                    _unitOfWork.Reservations.DeleteReservation(r.Id);
                }
            }

            _unitOfWork.SaveChanges();

            if (bookToDelete != 0)
            {
                return Ok("Successfully deleted");
            }
            else
            {
                return Ok("Could not be deleted");
            }
        }
    }
}

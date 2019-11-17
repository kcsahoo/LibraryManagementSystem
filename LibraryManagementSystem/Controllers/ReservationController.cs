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
using DAL.Repositories.Interfaces;

namespace LibraryManagementSystem.Controllers
{
    [Route("api/[controller]")]
    public class ReservationController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        public ReservationController(IMapper mapper, IUnitOfWork unitOfWork, ILogger<RegisteredUserController> logger)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        // GET: api/reservation
        [HttpGet]
        public IActionResult Get()
        {
            var allReservations = _unitOfWork.Reservations.GetAllReservations();
            if (allReservations.Count() != 0)
            {
                return Ok(_mapper.Map<IEnumerable<ReservationViewModel>>(allReservations));
            }
            else
            {
                return Ok("Not Found");
            }
        }

        // GET: api/reservation
        [HttpGet("GetBooksCurrentlyOnLoan")]
        public IActionResult GetBooksCurrentlyOnLoan()
        {
            var allReservationsOutstanding = _unitOfWork.Reservations.GetAllReservations().Where(r=>r.ReturnedOn==null);
            if (allReservationsOutstanding.Count() != 0)
            {
                return Ok(_mapper.Map<IEnumerable<ReservationViewModel>>(allReservationsOutstanding));
            }
            else
            {
                return Ok("Not Found");
            }
        }

        // GET: api/reservation
        [HttpGet("GetBooksOutForLoanMoreThanOneWeek")]
        public IActionResult GetBooksOutForLoanMoreThanOneWeek()
        {
            var allReservationsOutstanding = _unitOfWork.Reservations.GetAllReservations().Where(r => r.ReturnedOn == null && r.LentOutOn.AddDays(7) < DateTime.Now);
            if (allReservationsOutstanding.Count() != 0)
            {
                return Ok(_mapper.Map<IEnumerable<ReservationViewModel>>(allReservationsOutstanding));
            }
            else
            {
                return Ok("Not Found");
            }
        }

        // GET api/reservation/5
        [HttpGet("GetByID/{id}")]
        public IActionResult GetByID(int id)
        {
            var reservation = _unitOfWork.Reservations.GetReservationByID(id);
            if (reservation != null)
            {
                return Ok(_mapper.Map<ReservationViewModel>(reservation));
            }
            else
            {
                return Ok("Not Found");
            }
        }

        // GET api/reservation/5
        [HttpGet("GetByUserID/{id}")]
        public IActionResult GetByUserID(int id)
        {
            var reservations = _unitOfWork.Reservations.GetReservationByUserId(id);
            if (reservations != null)
            {
                return Ok(_mapper.Map<IEnumerable<ReservationViewModel>>(reservations));
            }
            else
            {
                return Ok("Not Found");
            }
        }

        // GET api/reservation/5
        [HttpGet("GetByBookID/{id}")]
        public IActionResult GetByBookID(int id)
        {
            var reservations = _unitOfWork.Reservations.GetReservationByBookId(id);
            if (reservations != null)
            {
                return Ok(_mapper.Map<IEnumerable<ReservationViewModel>>(reservations));
            }
            else
            {
                return Ok("Not Found");
            }
        }

        // GET api/reservation/xyz
        [HttpGet("GetByComment/{comment}")]
        public IActionResult GetByComment(string comment)
        {
            var reservation = _unitOfWork.Reservations.GetReservationByComment(comment);
            if (reservation != null)
            {
                return Ok(_mapper.Map<ReservationViewModel>(comment));
            }
            else
            {
                return Ok("Not Found");
            }
        }

        // POST api/reservation
        [HttpPost]
        public IActionResult Post([FromBody]Reservation reservation)
        {            
            var user = _unitOfWork.RegisteredUsers.GetRegisteredUserByID(reservation.RegisteredUserId);
            var booksLoanedNotReturned = _unitOfWork.Reservations.GetReservationByBookId(reservation.BookId)
                .Where(r => r.ReturnedOn == null);

            var book = _unitOfWork.Books.GetBookByID(reservation.BookId);
            if (user.LendingLimit > booksLoanedNotReturned.Count() && book.UnitsInStock >0 && book.TotalUnits != 0)
            {
                reservation.ReturnedOn = null;
                var reservationAdded = _unitOfWork.Reservations.LoanBook(reservation);
                if (reservationAdded != null)
                {                    
                    book.UnitsInStock--;
                    _unitOfWork.Books.UpdateBook(book);
                    _unitOfWork.SaveChanges();
                    return Ok(_mapper.Map<ReservationViewModel>(reservationAdded));
                }
                else
                {
                    return Ok("Could not be added");
                }               
            }
            else
            {
                if (user.LendingLimit <= booksLoanedNotReturned.Count())
                {
                    return Ok("user has already exhausted his lending limit.");
                }
                if (book.TotalUnits == 0)
                {
                    return Ok("this book is not available for loan any more");
                }
                if (book.UnitsInStock <= 0 )
                {
                    var earliestReturnDate = booksLoanedNotReturned.OrderBy(r => r.ReturnBy).FirstOrDefault().ReturnBy;
                    return Ok("this book is not available for loan today. Will be availabe for loan from " + earliestReturnDate.ToShortDateString());
                }               
            }
            return Ok("this book is not available for loan.");
        }

        // PUT api/reservation/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]Reservation reservation)
        {
            var reservationAdded = _unitOfWork.Reservations.ReturnBook(reservation);
            if (reservationAdded != null)
            {
                var book = _unitOfWork.Books.GetBookByID(reservation.BookId);
                book.UnitsInStock++;
                _unitOfWork.Books.UpdateBook(book);
                _unitOfWork.SaveChanges();
                return Ok(_mapper.Map<ReservationViewModel>(reservationAdded));
            }
            else
            {
                return Ok("Could not be updated");
            }
        }

        // DELETE api/reservation/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var reservationToDelete = _unitOfWork.Reservations.DeleteReservation(id);
            _unitOfWork.SaveChanges();
            if (reservationToDelete != 0)
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

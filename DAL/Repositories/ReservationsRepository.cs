using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DAL.Models;
using DAL.Repositories.Interfaces;

namespace DAL.Repositories
{
    public class ReservationsRepository : Repository<Reservation>, IReservationsRepository
    {
        public ReservationsRepository(ApplicationDbContext context) : base(context)
        { }

        public IEnumerable<Reservation> GetAllReservations() {
            return _appContext.Reservations
                .Include(c => c.Book)
                .Include(c => c.RegisteredUser)
               .OrderByDescending(c => c.LentOutOn)
               .ToList();
        }
        public Reservation GetReservationByID(int ID) {
            return _appContext.Reservations
                .Include(c => c.Book)
                .Include(c => c.RegisteredUser)
                .Where(c=>c.Id==ID)
                .FirstOrDefault();
        }
        public Reservation GetReservationByComment(string Comment) {
            return _appContext.Reservations
               .Include(c => c.Book)
               .Include(c => c.RegisteredUser)
               .Where(c => c.Comments.ToLower().Contains(Comment.ToLower()))
               .FirstOrDefault();
        }
        public IEnumerable<Reservation> GetReservationByBookIdUserId(int bookId, int userId) {
            return _appContext.Reservations
                .Include(c => c.Book)
                .Include(c => c.RegisteredUser)
                .Where(c => c.BookId == bookId && c.RegisteredUserId == userId)
                .Select(r => r);
        }
        public IEnumerable<Reservation> GetReservationByBookId(int bookId)
        {
            return _appContext.Reservations
               .Include(c => c.Book)
               .Include(c => c.RegisteredUser)
               .Where(c => c.BookId == bookId)
               .Select(r => r);
        }

        public IEnumerable<Reservation> GetReservationByUserId(int userId)
        {
            return _appContext.Reservations
               .Include(c => c.Book)
               .Include(c => c.RegisteredUser)
               .Where(c => c.RegisteredUserId == userId)
               .Select(r => r);
        }
        public Reservation LoanBook(Reservation reservation) {
            Reservation existingReservation = GetReservationByBookIdUserId(reservation.BookId, reservation.RegisteredUserId)
                .Where(r => r.ReturnedOn == null).FirstOrDefault();
            if (existingReservation != null)
            {
                return null;
            }
            else
            {
                _appContext.Add<Reservation>(reservation);
                return reservation;
            }
        }
        public Reservation ReturnBook(Reservation reservation) {
            Reservation existingReservation = GetReservationByBookIdUserId(reservation.BookId, reservation.RegisteredUserId)
                .FirstOrDefault();
            if (existingReservation == null)
            {
                return null;
            }
            else
            {
                existingReservation.Comments = reservation.Comments;
                existingReservation.ReturnedOn = reservation.ReturnedOn;
                _appContext.Update<Reservation>(existingReservation);
                return reservation;
            }
        }
        public int DeleteReservation(int ID) {
            Reservation existingReservation = GetReservationByID(ID);

            if (existingReservation == null)
            {
                return 0;
            }
            else
            {
                _appContext.Remove<Reservation>(existingReservation);
                return 1;
            }
        }

        public int DeleteReservationForUser(int userId)
        {
            IEnumerable<Reservation> existingReservations = GetReservationByUserId(userId);
            int cnt = existingReservations.Count();
            if (cnt == 0)
            {
                return 0;
            }
            else
            {
                foreach (var r in existingReservations)
                {
                    _appContext.Remove<Reservation>(r);
                }
                return 1;
            }
        }

        public int DeleteReservationForBook(int bookId)
        {
            IEnumerable<Reservation> existingReservations = GetReservationByBookId(bookId);
            int cnt = existingReservations.Count();
            if (cnt == 0)
            {
                return 0;
            }
            else
            {
                foreach(var r in existingReservations)
                {
                    _appContext.Remove<Reservation>(r);
                }
                return 1;
            }
        }

        private ApplicationDbContext _appContext => (ApplicationDbContext)_context;
    }
}

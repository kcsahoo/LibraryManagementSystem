using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Repositories.Interfaces
{
    public interface IReservationsRepository : IRepository<Reservation>
    {
        IEnumerable<Reservation> GetAllReservations();
        Reservation GetReservationByID(int ID);
        Reservation GetReservationByComment(string Comment);
        IEnumerable<Reservation> GetReservationByBookIdUserId(int bookId, int userId);
        IEnumerable<Reservation> GetReservationByBookId(int bookId);
        IEnumerable<Reservation> GetReservationByUserId(int userId);
        Reservation LoanBook(Reservation reservation);
        Reservation ReturnBook(Reservation reservation);
        int DeleteReservation(int ID);
        int DeleteReservationForUser(int userId);
        int DeleteReservationForBook(int bookId);
    }
}

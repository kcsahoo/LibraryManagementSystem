using DAL.Models;
using System;
using System.Linq;


namespace LibraryManagementSystem.ViewModels
{
    public class ReservationViewModel
    {
        public int Id { get; set; }
        public string Comments { get; set; }
        public DateTime LentOutOn { get; set; }
        public DateTime ReturnBy { get; set; }
        public DateTime? ReturnedOn { get; set; }
        public int RegisteredUserId { get; set; }
        public RegisteredUser RegisteredUser { get; set; }

        public int BookId { get; set; }
        public Book Book { get; set; }
    }
}

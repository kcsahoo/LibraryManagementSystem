using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Reservation : AuditableEntity
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

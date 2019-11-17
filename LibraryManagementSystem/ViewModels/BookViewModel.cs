using System;
using System.Linq;


namespace LibraryManagementSystem.ViewModels
{
    public class BookViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }        
        public int UnitsInStock { get; set; }
        public int TotalUnits { get; set; }
    }
}

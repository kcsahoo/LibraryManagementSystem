using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LibraryManagementSystem.ViewModels
{
    public class RegisteredUserViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Gender { get; set; }

    }




    public class RegisteredUserViewModelValidator : AbstractValidator<RegisteredUserViewModel>
    {
        public RegisteredUserViewModelValidator()
        {
            RuleFor(register => register.Name).NotEmpty().WithMessage("Registered User name cannot be empty");
            RuleFor(register => register.Gender).NotEmpty().WithMessage("Gender cannot be empty");
        }
    }
}

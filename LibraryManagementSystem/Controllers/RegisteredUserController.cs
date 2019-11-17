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
    public class RegisteredUserController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;

        public RegisteredUserController(IMapper mapper, IUnitOfWork unitOfWork, ILogger<RegisteredUserController> logger)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }


        // GET: api/registereduser
        [HttpGet]
        public IActionResult Get()
        {
            var allRegisteredUsers = _unitOfWork.RegisteredUsers.GetAllRegisteredUsers();
            
            if (allRegisteredUsers.Count() != 0)
            {
                return Ok(_mapper.Map<IEnumerable<RegisteredUserViewModel>>(allRegisteredUsers));
            }
            else
            {
                return Ok("Not Found");
            }
        }    

        // GET api/registereduser/5
        [HttpGet("GetRegisteredUserById{id}")]
        public IActionResult Get(int id)
        {
            var registeredUser = _unitOfWork.RegisteredUsers.GetRegisteredUserByID(id);
            if (registeredUser != null)
            {
                return Ok(_mapper.Map<RegisteredUserViewModel>(registeredUser));
            }
            else
            {
                return Ok("Not Found");
            }
        }

        // GET api/registereduser/abc
        [HttpGet("GetRegisteredUserByName{name}")]
        public IActionResult Get(string name)
        {
            var registeredUser = _unitOfWork.RegisteredUsers.GetRegisteredUserByName(name);
            if (registeredUser != null)
            {
                return Ok(_mapper.Map<RegisteredUserViewModel>(registeredUser));
            }
            else
            {
                return Ok("Not Found");
            }
        }

        // POST api/registereduser
        [HttpPost]
        public IActionResult Post([FromBody]RegisteredUser registeredUser)
        {
            var userAdded = _unitOfWork.RegisteredUsers.AddRegisteredUser(registeredUser);
            if (userAdded != null)
            {
                _unitOfWork.SaveChanges();
                return Ok(_mapper.Map<RegisteredUserViewModel>(userAdded));
            }
            else
            {
                return Ok("Could not be added"); 
            }
        }



        // PUT api/registereduser/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]RegisteredUser registeredUser)
        {
            var userUpdated = _unitOfWork.RegisteredUsers.UpdateRegisteredUser(registeredUser);
            if (userUpdated != null)
            {
                _unitOfWork.SaveChanges();
                return Ok(_mapper.Map<RegisteredUserViewModel>(userUpdated));
            }
            else
            {
                return Ok("Could not be updated");
            }
        }

        // DELETE api/registereduser/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var userToDelete = _unitOfWork.RegisteredUsers.DeleteRegisteredUser(id);
            //delete reservations for this user before deleting user
            var reservationsAgainstUser = _unitOfWork.Reservations.GetReservationByUserId(id);
            if (reservationsAgainstUser.Count() > 0)
            {
                foreach(var r in reservationsAgainstUser)
                {
                    _unitOfWork.Reservations.DeleteReservation(r.Id);
                    
                    if (r.ReturnedOn == null)
                    {
                        var book = _unitOfWork.Books.GetBookByID(r.BookId);
                        book.UnitsInStock++;
                        _unitOfWork.Books.UpdateBook(book);
                    }
                }      
            }
            _unitOfWork.SaveChanges();
            if (userToDelete != 0)
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

using AutoMapper;
using DAL.Core;
using DAL.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.ViewModels
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<RegisteredUser, RegisteredUserViewModel>()
                .ReverseMap();

            CreateMap<Book, BookViewModel>()
                .ReverseMap();

            CreateMap<Reservation, ReservationViewModel>()
                .ReverseMap();
        }
    }
}

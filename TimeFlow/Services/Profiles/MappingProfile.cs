using AutoMapper;
using Domain.Entities;
using Services.Abstractions.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Services.Profiles
{
    public class MappingProfile : Profile
    {
            public MappingProfile()
            {
                CreateMap<Client, ClientDto>().ReverseMap();
                CreateMap<Client, ClientDtoForCreate>().ReverseMap();
                CreateMap<Client, ClientDtoForUpdate>().ReverseMap();

                CreateMap<Lesson, LessonDto>().ReverseMap();

                CreateMap<Lesson, LessonDtoForCreate>().ReverseMap();
                CreateMap<Lesson, LessonDtoForUpdate>().ReverseMap();
                CreateMap<Lesson, LessonDtoForAutoCreate>().ReverseMap();

                CreateMap<User, UserDto>().ReverseMap();
                CreateMap<User, UserDtoForCreate>().ReverseMap();
                CreateMap<User, UserDtoForUpdate>().ReverseMap();

                CreateMap<Subject, SubjectDto>().ReverseMap();
                CreateMap<Subject, SubjectDtoForCreate>().ReverseMap();
                CreateMap<Subject, SubjectDtoForUpdate>().ReverseMap();

                CreateMap<TimeSlot, TimeSlotDto>().ReverseMap();
                CreateMap<TimeSlot, TimeSlotDtoForCreate>().ReverseMap();
                CreateMap<TimeSlot, TimeSlotDtoForUpdate>().ReverseMap();
                CreateMap<TimeSlot, TimeSlotFilterDto>().ReverseMap();
            }
    }
}

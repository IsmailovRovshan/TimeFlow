using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstractions.DTO
{
    public record TeacherDto(Guid Id, string Login, string Password, string Email, string FullName);
    public record TeacherDtoForCreate(string Login, string Password, string Email, string FullName);
    public record TeacherDtoForUpdate(string Email);
}

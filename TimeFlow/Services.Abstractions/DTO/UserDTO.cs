
using Domain.Entities;
using System.Globalization;

namespace Services.Abstractions.DTO
{
    public record UserDto(
        Guid Id,
        string Login,
        string FullName,
        string Email,
        int? Age,
        int? Experiense,
        Role Role,
        List<TimeSlotDto> TimeSlots,
        List<LessonDto> Lessons,
        List<SubjectDto> Subjects
        );

    public record UserDtoForCreate(
        string Login,
        string Password, 
        string FullName,
        string Email,
        Role Role
        );

    public record UserDtoForUpdate(
        string Login,
        string Password,
        string FullName,
        string Email
        );


    public record RegisterDto(
        string Login,
        string Password,
        string FullName,
        string Email,
        Role Role
    );

    public record LoginDto(
        string Login,
        string Password
    );

    public record AuthResponse(
        string Token
    );
    

}

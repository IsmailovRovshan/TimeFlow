using Services.Abstractions.DTO;

public interface IAuthService
{
    Task<UserDto> RegisterAsync(RegisterDto dto);
    Task<AuthResponse> LoginAsync(LoginDto dto);
}


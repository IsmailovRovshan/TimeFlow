using AutoMapper;
using Domain.Entities;
using Domain.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Services;
using Services.Abstractions.DTO;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepo;
    private readonly IPasswordHasher<User> _hasher;
    private readonly JwtSettings _jwt;
    private readonly IMapper _mapper;

    public AuthService(
        IUserRepository userRepo,
        IPasswordHasher<User> hasher,
        IOptions<JwtSettings> jwtOptions,
        IMapper mapper)
    {
        _userRepo = userRepo;
        _hasher = hasher;
        _jwt = jwtOptions.Value;
        _mapper = mapper;
    }

    public async Task<UserDto> RegisterAsync(RegisterDto dto)
    {
        if (await _userRepo.GetByLoginAsync(dto.Login) != null)
            throw new InvalidOperationException("Пользователь с таким логином уже существует.");

        var user = new User
        {
            Login = dto.Login,
            FullName = dto.FullName,
            Email = dto.Email,
            Role = dto.Role
        };
        user.Password = _hasher.HashPassword(user, dto.Password);

        await _userRepo.AddAsync(user);
        return _mapper.Map<UserDto>(user);
    }

    public async Task<AuthResponse> LoginAsync(LoginDto dto)
    {
        var user = await _userRepo.GetByLoginAsync(dto.Login)
                   ?? throw new UnauthorizedAccessException("Неверный логин или пароль.");

        var result = _hasher.VerifyHashedPassword(user, user.Password, dto.Password);
        if (result == PasswordVerificationResult.Failed)
            throw new UnauthorizedAccessException("Неверный логин или пароль.");

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(ClaimTypes.Name,          user.Login),
            new Claim(ClaimTypes.Role,          user.Role.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwt.Issuer,
            audience: _jwt.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: creds
        );

        return new AuthResponse(new JwtSecurityTokenHandler().WriteToken(token));
    }
}

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using HAC_Pharma.Application.DTOs;
using HAC_Pharma.Domain.Entities;
using HAC_Pharma.Domain.Interfaces;

namespace HAC_Pharma.Application.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IConfiguration _configuration;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
    }

    public async Task<LoginResponseDTO?> LoginAsync(LoginRequestDTO request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null || !user.IsActive)
            return null;

        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: true);
        if (!result.Succeeded)
            return null;

        // Update last login
        user.LastLoginAt = DateTime.UtcNow;
        await _userManager.UpdateAsync(user);

        var roles = await _userManager.GetRolesAsync(user);
        var role = roles.FirstOrDefault() ?? "Viewer";

        var token = GenerateJwtToken(user, role);
        var refreshToken = GenerateRefreshToken();
        var expiryMinutes = int.Parse(_configuration["JwtSettings:DurationMinutes"] ?? "120");

        // Store refresh token
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(request.RememberMe ? 30 : 7);
        await _userManager.UpdateAsync(user);

        return new LoginResponseDTO
        {
            Token = token,
            RefreshToken = refreshToken,
            ExpiresIn = expiryMinutes * 60,
            User = new UserInfoDTO
            {
                Id = user.Id,
                Email = user.Email!,
                Name = user.FullName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = role,
                ProfileImage = user.ProfileImage,
                LastLoginAt = user.LastLoginAt
            }
        };
    }

    public async Task<LoginResponseDTO?> RefreshTokenAsync(string refreshToken)
    {
        var users = _userManager.Users.Where(u => u.RefreshToken == refreshToken && u.IsActive);
        var user = users.FirstOrDefault();
        
        if (user == null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            return null;

        var roles = await _userManager.GetRolesAsync(user);
        var role = roles.FirstOrDefault() ?? "Viewer";

        var newToken = GenerateJwtToken(user, role);
        var newRefreshToken = GenerateRefreshToken();
        var expiryMinutes = int.Parse(_configuration["JwtSettings:DurationMinutes"] ?? "120");

        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        await _userManager.UpdateAsync(user);

        return new LoginResponseDTO
        {
            Token = newToken,
            RefreshToken = newRefreshToken,
            ExpiresIn = expiryMinutes * 60,
            User = new UserInfoDTO
            {
                Id = user.Id,
                Email = user.Email!,
                Name = user.FullName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = role,
                ProfileImage = user.ProfileImage,
                LastLoginAt = user.LastLoginAt
            }
        };
    }

    public async Task<bool> LogoutAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return false;

        user.RefreshToken = null;
        user.RefreshTokenExpiryTime = null;
        await _userManager.UpdateAsync(user);
        return true;
    }

    public async Task<UserInfoDTO?> GetCurrentUserAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null || !user.IsActive)
            return null;

        var roles = await _userManager.GetRolesAsync(user);
        var role = roles.FirstOrDefault() ?? "Viewer";

        return new UserInfoDTO
        {
            Id = user.Id,
            Email = user.Email!,
            Name = user.FullName,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Role = role,
            ProfileImage = user.ProfileImage,
            LastLoginAt = user.LastLoginAt
        };
    }

    public Task<bool> ValidateTokenAsync(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]!);

        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = _configuration["JwtSettings:Issuer"],
                ValidateAudience = true,
                ValidAudience = _configuration["JwtSettings:Audience"],
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            }, out _);

            return Task.FromResult(true);
        }
        catch
        {
            return Task.FromResult(false);
        }
    }

    private string GenerateJwtToken(ApplicationUser user, string role)
    {
        var key = Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]!);
        var expiryMinutes = int.Parse(_configuration["JwtSettings:DurationMinutes"] ?? "120");

        var claims = new List<Claim>
        {
            new("id", user.Id),
            new("email", user.Email!),
            new("name", user.FullName),
            new("role", role),
            new("jti", Guid.NewGuid().ToString())
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(expiryMinutes),
            Issuer = _configuration["JwtSettings:Issuer"],
            Audience = _configuration["JwtSettings:Audience"],
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    private static string GenerateRefreshToken()
    {
        var randomBytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }
}

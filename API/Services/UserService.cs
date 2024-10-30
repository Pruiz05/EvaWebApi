using API.Context;
using API.Dtos;
using API.Models;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace API.Services
{
    public class UserService
    {

        private readonly AppDbContext _context;
        private readonly IValidator<RegisterUserDto> _validator;
        private readonly IConfiguration _configuration;


        public UserService(AppDbContext context, IValidator<RegisterUserDto> validator, IConfiguration configuration)
        {
            _context = context;
            _validator = validator;
            _configuration = configuration;
        }

        public async Task<ServiceResponse<object>> Register(RegisterUserDto userDto)
        {
            ValidationResult validationResult = _validator.Validate(userDto);
            if (!validationResult.IsValid)
            {
                var firstError = validationResult.Errors.First();
                return new ServiceResponse<object>
                {
                    Success = false,
                    Message = firstError.ErrorMessage,
                    Data = null
                };
            }

            var exist = await _context.Users.AnyAsync(x => x.Email == userDto.Email);
            if (exist)
            {
                return new ServiceResponse<object>
                {
                    Success = false,
                    Message = "This email is already been used",
                    Data = null
                };
            }

            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = userDto.Name,
                Email = userDto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(userDto.Password),
                IsActive = true,
                Created = DateTime.Now,
                LastLogin = DateTime.Now,
                Token = ""              
            };

            var token = GenerateJwtToken(user);
            user.Token = token;

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                if (userDto.Phones != null)
                {
                    foreach (var phoneDto in userDto.Phones)
                    {
                        var phone = new Phone
                        {
                            UserId = user.Id,
                            Number = phoneDto.Number,
                            CityCode = phoneDto.CityCode,
                            CountryCode = phoneDto.CountryCode
                        };
                        _context.Phones.Add(phone);
                    }
                    await _context.SaveChangesAsync();
                }

                await transaction.CommitAsync();

                return new ServiceResponse<object>
                {
                    Success = true,
                    Message = "User registered successfully.",
                    Data = new
                    {
                        id = user.Id,
                        created = user.Created,
                        modified = user.Modified,
                        last_login = user.LastLogin,
                        token = user.Token,
                        isActive = user.IsActive
                    }
                };
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw new Exception("Internal server error");
            }
        }

        public async Task<ServiceResponse<List<UserDto>>> GetAllUsers()
        {
            var users = await _context.Users.ToListAsync();

            // Mapeo de usuarios a un DTO (Data Transfer Object)
            var userDtos = users.Select(user => new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Created = user.Created,
                Modified = user.Modified,
                LastLogin = user.LastLogin,
                IsActive = user.IsActive
            }).ToList();

            return new ServiceResponse<List<UserDto>>
            {
                Success = true,
                Message = "Users retrieved successfully.",
                Data = userDtos
            };
        }



        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var bytekey = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                [
                    new(ClaimTypes.Email, user.Email),
                    new(ClaimTypes.Name, user.Name),
                    new("id", user.Id.ToString()),
                    new("name", user.Name)
                ]),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(bytekey), 
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);

        }
    }
}

using API.Context;
using API.Dtos;
using API.Models;
using API.Services;
using API.Validators;
using FluentValidation;
using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Moq;

namespace EvaUnitTest
{
    public class UserControllerTests
    {
        private readonly UserService _userService;
        private readonly Mock<IValidator<RegisterUserDto>> _validatorMock;
        private readonly RegisterUserValidator _validator;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly AppDbContext _context;

        public UserControllerTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                 .ConfigureWarnings(warnings => warnings.Ignore(InMemoryEventId.TransactionIgnoredWarning)) // Ignorar advertencia de transacciones
                .Options;
            _context = new AppDbContext(options);

            _validatorMock = new Mock<IValidator<RegisterUserDto>>();
            _configurationMock = new Mock<IConfiguration>();

            _configurationMock.Setup(c => c["Jwt:Key"]).Returns("VOmpu8s9Vel385iZw8O6GfbgvPaLPpnyerxPM1Akf3lrnvoz4nKdk25TerM");
            _configurationMock.Setup(c => c["PasswordPolicy:PasswordRegex"]).Returns("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{6,}$");

            _userService = new UserService(_context, _validatorMock.Object, _configurationMock.Object);

            _validator = new RegisterUserValidator("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{6,}$");
        }

        [Fact]
        public async Task RegisterUser_ShouldReturnSuccess_WhenUserIsValid()
        {
            // Arrange
            var userDto = new RegisterUserDto
            {
                Name = "TestUser",
                Email = "test@example.com",
                Password = "@123",
                Phones = [new RegisterPhonesDto { Number = "1234567890", CityCode = "123", CountryCode = "1" }]
            };

            _validatorMock.Setup(v => v.Validate(It.IsAny<RegisterUserDto>()))
                .Returns(new FluentValidation.Results.ValidationResult());

            // Act
            var response = await _userService.Register(userDto);

            // Assert
            Assert.True(response.Success);
            Assert.NotNull(response.Data);
        }

        [Fact]
        public async Task RegisterUser_ShouldReturnFailure_WhenEmailExists()
        {
            // Arrange
            var existingUser = new User { 
                Name = "Test name",
                Password ="12314",
                IsActive = true,
                Token = "qwer",
                Email = "duplicate@example.com" 
            
            };
            _context.Users.Add(existingUser);
            await _context.SaveChangesAsync();

            var userDto = new RegisterUserDto
            {
                Name = "TestUser",
                Email = "duplicate@example.com",
                Password = "Password@123",
                Phones = [new RegisterPhonesDto { Number = "1234567890", CityCode = "123", CountryCode = "1" }]
            };

            _validatorMock.Setup(v => v.Validate(It.IsAny<RegisterUserDto>()))
                .Returns(new FluentValidation.Results.ValidationResult());

            // Act
            var response = await _userService.Register(userDto);

            // Assert
            Assert.False(response.Success);
            Assert.Equal("This email is already been used", response.Message);
        }


        [Fact]
        public void Should_Have_Error_When_Name_Is_Empty()
        {
            // Arrange
            var model = new RegisterUserDto
            {
                Name = "",
                Email = "test@example.com",
                Password = "password123",
                Phones =
            [
                new RegisterPhonesDto()
                {
                    CityCode = "123",
                    CountryCode = "1",
                    Number = "1234567"
                }
            ]
            };

            // Act & Assert
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(user => user.Name)
                  .WithErrorMessage("Name is required.");
        }


        [Fact]
        public void Should_Have_Error_When_Email_Is_Empty_Or_Email_Invalid_Format()
        {
            // Arrange
            var model = new RegisterUserDto
            {
                Name = "john doe",
                Email = "",
                Password = "P@ssword123",
                Phones =
            [
                new RegisterPhonesDto()
                {
                    CityCode = "123",
                    CountryCode = "1",
                    Number = "1234567"
                }
            ]
            };

            // Act & Assert
            var result = _validator.TestValidate(model);
            result.ShouldHaveAnyValidationError();
        }


        [Theory]
        [InlineData("1231232")] // Solo números
        [InlineData("abcdefg")] // Solo letras minúsculas
        [InlineData("ABCDEFG")] // Solo letras mayúsculas
        [InlineData("Abc123")]  // Falta carácter especial
        //[InlineData("Abc123!")] // Contraseña válida --> debe fallar
        public void Should_Have_Error_When_Password_Has_Invalid_Format(string password)
        {
            // Arrange
            var model = new RegisterUserDto
            {
                Name = "john doe",
                Email = "jdoe@correo.com",
                Password = password,
                Phones =
                [
                    new RegisterPhonesDto()
                    {
                        CityCode = "123",
                        CountryCode = "1",
                        Number = "1234567"
                    }
                ]
            };

            // Act & Assert
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(user => user.Password)
                  .WithErrorMessage("Password must be at least 6 characters long, contain an uppercase letter, a lowercase letter, a number, and a special character.");
        }
    }
}
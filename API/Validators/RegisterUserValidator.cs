using API.Dtos;
using FluentValidation;

namespace API.Validators
{
    public class RegisterUserValidator : AbstractValidator<RegisterUserDto>
    {
        public RegisterUserValidator(string passwordPattern) 
        {
            RuleFor(u => u.Name)
              .NotEmpty().WithMessage("Name is required.")
              .Length(2, 50).WithMessage("Name must be between 2 and 50 characters.");


            RuleFor(u => u.Email)
              .NotEmpty().WithMessage("Email is required.")
              .Matches(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$")
              .WithMessage("Invalid email format.");

            RuleFor(u => u.Password)
              .NotEmpty().WithMessage("Password is required.")
              .Matches(passwordPattern)
              .WithMessage("Password must be at least 6 characters long, contain an uppercase letter, a lowercase letter, a number, and a special character.");
            ;

            RuleFor(u => u.Phones)
              .NotNull().WithMessage("Phones list is required.") 
              .NotEmpty().WithMessage("At least one phone is required."); 

            RuleForEach(u => u.Phones).ChildRules(phone =>
            {
                phone.RuleFor(p => p.Number)
                    .NotEmpty().WithMessage("Phone number is required.");

                phone.RuleFor(p => p.CityCode)
                    .NotEmpty().WithMessage("City code is required.");

                phone.RuleFor(p => p.CountryCode)
                    .NotEmpty().WithMessage("Country code is required.");
            });


        }
    }
}

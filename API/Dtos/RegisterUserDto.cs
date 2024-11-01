﻿namespace API.Dtos
{
    public class RegisterUserDto
    {
        public required string Name { get; set; }

        public required string Email { get; set; }

        public required string Password { get; set; }

        public required List<RegisterPhonesDto> Phones { get; set; }
    }
}

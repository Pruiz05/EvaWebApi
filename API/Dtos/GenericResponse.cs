using System.Net;

namespace API.Dtos
{
    public class ServiceResponse<T>
    {
        public bool Success { get; set; }
        public T? Data { get; set; }
        public required string Message { get; set; }
    }

    public class UserResponse
    {
        public Guid Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public DateTime LastLogin { get; set; }
        public required string Token { get; set; }
        public bool IsActive { get; set; }
    }


    public class UserDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Modified { get; set; }
        public DateTime LastLogin { get; set; }
        public bool IsActive { get; set; }
    }

    public class ErrorResponse
    {
        public required string Message { get; set; }
    }
}

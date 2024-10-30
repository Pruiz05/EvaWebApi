namespace API.Dtos
{
    public class RegisterResponseDto
    {
        public required string Id { get; set; }
        public required string Token { get; set; }

        public DateTime Created { get; set; }
        public DateTime? Modified { get; set; }
        public DateTime LastLogin { get; set; }
        public bool IsActive { get; set; }
    }
}


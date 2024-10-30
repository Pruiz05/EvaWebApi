using System.ComponentModel.DataAnnotations;

namespace API.Models
{

    /// <summary>
    /// User
    /// </summary>
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        [MaxLength(250)]
        public required string Name { get; set; }

        [MaxLength(150)]
        [EmailAddress]
        public required string Email { get; set; }
        
        public required string Password { get; set; }

        public DateTime Created { get; set; }
        public DateTime? Modified { get; set; }

        public DateTime LastLogin { get; set; }

        public required string Token { get; set; }

        public bool IsActive { get; set; } = true;

        public virtual ICollection<Phone> Phones { get; set; } = [];

    }
}

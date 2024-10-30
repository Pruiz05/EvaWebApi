using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    public class Phone
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public required Guid UserId { get; set; }

        public required string Number { get; set; }

        public required string CityCode { get; set; }

        public required string CountryCode { get; set; }

        public virtual User? User { get; set; }

    }
}

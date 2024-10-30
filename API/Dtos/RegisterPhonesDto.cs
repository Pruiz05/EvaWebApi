namespace API.Dtos
{
    public class RegisterPhonesDto
    {
        public required string Number { get; set; }
        public required string CityCode { get; set; }

        public required string CountryCode { get; set; }
    }
}

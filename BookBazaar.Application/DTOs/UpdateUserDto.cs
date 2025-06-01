namespace BookBazaar.Application.DTOs
{
    public class UpdateUserDto
    {
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? Password { get; set; }  // New password
    }

}

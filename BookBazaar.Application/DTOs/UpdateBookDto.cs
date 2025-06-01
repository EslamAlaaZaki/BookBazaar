namespace BookBazaar.Application.DTOs
{
    public class UpdateBookDto
    {
        public string? Title { get; set; }
        public string? Author { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public string? Condition { get; set; }
    }
}

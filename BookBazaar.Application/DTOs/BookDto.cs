namespace BookBazaar.Application.DTOs
{
    public class BookDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Condition { get; set; } = string.Empty;
        public bool IsSold { get; set; }

        public Guid SellerId { get; set; }
        public string SellerName { get; set; } = string.Empty;

        public Guid? BuyerId { get; set; }
        public string? BuyerName { get; set; }

        public DateTime CreatedDateTime { get; set; } = DateTime.UtcNow;
        public DateTime? SoldDateTime { get; set; }

        public DateTime? UpdatedDateTime { get; set; }
    }
}

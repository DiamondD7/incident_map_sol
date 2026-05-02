namespace IncidentMapAPI.Domain.Models.DTOs
{
    public class PromotionDTO
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string? ShopType { get; set; }
        public string? ShopName { get; set; }
        public string? Title { get; set; } //Main head line of the promotion/incident
        public string? Description { get; set; }
        public string? Link { get; set; } //link to website
        public string? Address { get; set; }
        public int DaysUntilExpiry { get; set; } = 0;
        public DateTime? Expiry { get; set; }
        public DateTime? StartedAt { get; set; }
        public bool? IsActive { get; set; } = true;
    }
}

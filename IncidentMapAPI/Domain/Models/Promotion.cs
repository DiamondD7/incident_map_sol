using System.ComponentModel.DataAnnotations;

namespace IncidentMapAPI.Domain.Models
{
    public class Promotion
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string? ShopType { get; set; }
        public string? ShopName { get; set; }
        public string? Title { get; set; } //Main head line of the promotion/incident
        public string? Description { get; set; }
        public string? Link { get; set; } //link to website
        public DateTime? Expiry { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

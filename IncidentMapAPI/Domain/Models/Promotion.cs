using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public string? Address { get; set; }
        public int? DiscountPercent { get; set; }
        public DateTime? StartedAt { get; set; } //date, the promotion starts
        public DateTime? Expiry { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool? IsActive { get; set; } = true; //manually set to false when promotion is no longer active.
        public bool? HasPromotion { get; set; } = true;
        public bool? IsAnAestheticShop { get; set; } = false;
        public ICollection<PromotionImages>? Images { get; set; } = new List<PromotionImages>();
        public ICollection<Deals>? Deals { get; set; } = new List<Deals>();

    }


    public class PromotionImages
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        [ForeignKey(nameof(Promotion))]
        public Guid PromotionId { get; set; }
        public string? ImageTitle { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime? CreatedAt { get; set; }
    }

    public class Deals
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [ForeignKey(nameof(Promotion))]
        public Guid PromotionId { get; set; }
        public string? DealTitle { get; set; }
        public string? DealDescription { get; set; }
        public int? DiscountPercent { get; set; }
        public DateTime? DealStart { get; set; }
        public DateTime? DealEnd { get; set; }
        public DateTime? CreatedAt { get; set; }
    }


}

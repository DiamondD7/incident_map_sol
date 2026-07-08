using IncidentMapAPI.Application.Interfaces.Repositories;
using IncidentMapAPI.Domain.Models;
using IncidentMapAPI.Domain.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace IncidentMapAPI.Infrastructure.Persistence.Repositories
{
    public class PromotionRepository : IPromotionRepository
    {
        private readonly AppDbContext _context;
        public PromotionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Promotion>> GetPromotionsAsync()
        {
            return await _context.PromotionTable.Include(p => p.Images).Include(c => c.Deals).Where(x => (x.Expiry > DateTime.Now || x.Expiry == null) && 
            x.IsActive == true && (x.StartedAt == null || x.StartedAt <= DateTime.Now)).ToListAsync();
        }

        public async Task<List<Promotion>> GetFilteredPromotions(PromotionDTO promotion)
        {
            var cafes = await _context.PromotionTable.Include(p => p.Images).Include(c => c.Deals).ToListAsync();

            if (!string.IsNullOrEmpty(promotion.ShopType))
            {
                cafes = cafes.Where(c => c.ShopType == promotion.ShopType && c.IsActive == true && (c.StartedAt == null || c.StartedAt <= DateTime.Now)).ToList();
            }

            if(promotion.DaysUntilExpiry != 0)
            {
                var expiryDate = DateTime.Now.AddDays(promotion.DaysUntilExpiry);
                cafes = cafes.Where(c => c.Expiry != null && c.Expiry <= expiryDate && c.IsActive == true).ToList();
            }

            if(promotion.IsAnAestheticShop != null)
            {
                cafes = cafes.Where(c => c.IsAnAestheticShop == promotion.IsAnAestheticShop && c.IsActive == true).ToList();
            }

            if(promotion.Latitude != 0 && promotion.Longitude != 0)
            {
                return cafes
                .Where(c => CalculateDistance(promotion.Latitude, promotion.Longitude, c.Latitude, c.Longitude) < 5 && c.IsActive == true && (c.StartedAt == null || c.StartedAt <= DateTime.Now))
                .ToList();
            }

            return cafes;
        }

        public async Task<bool> AddNewDeals(Deals deals)
        {
            if (deals == null)
            {
                return false;
            }


            var newDeals = new Deals
            {
                PromotionId = deals.PromotionId,
                DealTitle = deals.DealTitle,
                DealDescription = deals.DealDescription,
                DiscountPercent = deals.DiscountPercent,
                DealStart = deals.DealStart,
                DealEnd = deals.DealEnd,
                CreatedAt = deals.CreatedAt ?? DateTime.Now,
            };

            _context.DealsTable.Add(newDeals);
            _context.SaveChanges();

            return true;
        }


        public async Task<bool> AddIncidentAsync(Promotion promotion)
        {
            if (promotion == null)
            {
                return false;
            }

            var newPromotion = new Promotion
            {
                Latitude = promotion.Latitude,
                Longitude = promotion.Longitude,
                ShopType = promotion.ShopType,
                ShopName = promotion.ShopName,
                Title = promotion.Title,
                Description = promotion.Description,
                Link = promotion.Link,
                Address = promotion.Address,
                StartedAt = promotion.StartedAt,
                Expiry = promotion.Expiry,
                DiscountPercent = promotion.DiscountPercent,
                HasPromotion = promotion.HasPromotion,
                IsAnAestheticShop = promotion.IsAnAestheticShop,
                CreatedAt = DateTime.Now,
                Images = promotion.Images?.Select(img => new PromotionImages
                {
                    ImageTitle = img.ImageTitle,
                    ImageUrl = img.ImageUrl,
                    CreatedAt = img.CreatedAt ?? DateTime.Now
                }).ToList(),
                Deals = promotion.Deals?.Select(deal => new Deals
                {
                    DealTitle = deal.DealTitle,
                    DealDescription = deal.DealDescription,
                    DiscountPercent = deal.DiscountPercent,
                    DealStart = deal.DealStart,
                    DealEnd = deal.DealEnd,
                    CreatedAt = deal.CreatedAt ?? DateTime.Now
                }).ToList()
            };

            _context.PromotionTable.Add(newPromotion);
            _context.SaveChanges();

            return true;
        }

        public double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            double R = 6371; // km
            var dLat = Math.PI / 180 * (lat2 - lat1);
            var dLon = Math.PI / 180 * (lon2 - lon1);

            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(Math.PI / 180 * lat1) * Math.Cos(Math.PI / 180 * lat2) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c;
        }
    }
}

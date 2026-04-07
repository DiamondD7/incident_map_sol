using IncidentMapAPI.Application.Interfaces.Repositories;
using IncidentMapAPI.Domain.Models;
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
            return await _context.PromotionTable.ToListAsync();
        }

        public async Task<List<Promotion>> GetFilteredPromotions(Promotion promotion)
        {
            var cafes = await _context.PromotionTable.ToListAsync();

            if (!string.IsNullOrEmpty(promotion.ShopType))
            {
                cafes = cafes.Where(c => c.ShopType == promotion.ShopType).ToList();
            }

            if(promotion.Latitude != 0 && promotion.Longitude != 0)
            {
                return cafes
                .Where(c => CalculateDistance(promotion.Latitude, promotion.Longitude, c.Latitude, c.Longitude) < 5)
                .ToList();
            }

            return cafes;
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
                Expiry = promotion.Expiry,
                CreatedAt = DateTime.Now,
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

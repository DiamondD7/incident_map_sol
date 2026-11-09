using IncidentMapAPI.Application.Interfaces.Repositories;
using IncidentMapAPI.Domain.Models;
using IncidentMapAPI.Domain.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace IncidentMapAPI.Infrastructure.Persistence.Repositories
{
    public class PromotionRepository : IPromotionRepository
    {
        private const int DEAL_SCORE = 30;
        private const int AESTHETIC_SCORE = 20;
        private const int NEARBY_SCORE = 20;
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

            if (promotion.DaysUntilExpiry != 0)
            {
                var expiryDate = DateTime.Now.AddDays(promotion.DaysUntilExpiry);
                cafes = cafes.Where(c => c.Expiry != null && c.Expiry <= expiryDate && c.IsActive == true).ToList();
            }

            if (promotion.IsAnAestheticShop != null)
            {
                cafes = cafes.Where(c => c.IsAnAestheticShop == promotion.IsAnAestheticShop && c.IsActive == true).ToList();
            }

            if (promotion.Latitude != 0 && promotion.Longitude != 0)
            {
                return cafes
                .Where(c => CalculateDistance(promotion.Latitude, promotion.Longitude, c.Latitude, c.Longitude) < 5 && c.IsActive == true && (c.StartedAt == null || c.StartedAt <= DateTime.Now))
                .ToList();
            }

            return cafes;
        }


        public async Task<List<DecisionPlaceDTO>> GetDecisionPlace(DecisionRequestDTO request)
        {
            var promotions = await _context.PromotionTable.Include(x => x.Deals).Include(x => x.Images).ToListAsync();
            var placesFilteredByCategory = promotions.Where(x => x.ShopType == request.Category).ToList();
            var scorePlaces = placesFilteredByCategory
                .Select(place => new DecisionPlaceDTO
                {
                    Place = place,
                    Score = CalculateScore(place, request)
                })
                .Where(x=>x.Score > 0)
                .OrderByDescending(x => x.Score)
                .ToList();

            return scorePlaces;
            
        }



        public async Task<List<Promotion>> GetAvailablePromotions()
        {
            var nowUtc = DateTime.UtcNow;

            var nzTimeZone = TimeZoneInfo.FindSystemTimeZoneById("New Zealand Standard Time");
            var nowNz = TimeZoneInfo.ConvertTimeFromUtc(nowUtc, nzTimeZone);

            var promotions = await _context.PromotionTable.Include(o => o.Images).Include(o => o.Deals).ToListAsync();
            //var activeDeals = promotions.Where(x => x.Deals.Any(d => (d.ValidDays == null || d.ValidDays.Contains(nowNz.DayOfWeek)) && IsActiveTime(d, nowNz.TimeOfDay)))
            //    .ToList();

            var activeDeals = promotions
                .Select(p => new Promotion
                {
                    Latitude = p.Latitude,
                    Longitude = p.Longitude,
                    ShopName = p.ShopName,
                    Title = p.Title,
                    Description = p.Description,
                    ShopType = p.ShopType,
                    Link = p.Link,
                    Address = p.Address,
                    Deals = p.Deals
                            .Where(d => (d.ValidDays == null || d.ValidDays.Contains(nowNz.DayOfWeek)) && IsActiveTime(d, nowNz.TimeOfDay)).ToList(),
                    Images = p.Images
                })
                .Where(p => p.Deals.Any())
                 .OrderBy(p =>
                 {
                     var deal = p.Deals.First();

                     var endDateTime = nowNz.Date.Add(deal.EndTime ?? TimeSpan.Zero);

                     if (deal.EndTime < deal.StartTime &&
                         deal.EndTime <= nowNz.TimeOfDay) 
                     {
                         endDateTime = endDateTime.AddDays(1);
                     }

                     return endDateTime;
                 }).ToList();

            return activeDeals;
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
                StartTime = deals.StartTime,
                EndTime = deals.EndTime,
                ValidDays = deals.ValidDays,
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

        private int CalculateScore(Promotion place, DecisionRequestDTO request)
        {
            int score = 0;

            if (request.Preference == "Deals")
            {
                if (place.Deals != null && place.Deals.Any())
                {
                    score += DEAL_SCORE;

                    var distance = CalculateDistance(request.Latitude, request.Longitude, place.Latitude, place.Longitude);
                    if (distance <= 2)
                    {
                        score += NEARBY_SCORE; //NEARBY_SCORE is 20 atm
                    }
                    else if (distance <= 5)
                    {
                        score += NEARBY_SCORE - 5;
                    }
                    else if (distance <= 10)
                    {
                        score += NEARBY_SCORE - 10;
                    }
                    else if (distance <= 20)
                    {
                        score += NEARBY_SCORE - 15;
                    }else if(distance <= 30)
                    {
                        score += NEARBY_SCORE - 17;
                    }
                    else if (distance > 50)
                    {
                        score += NEARBY_SCORE - 19;
                    }
                }

                
            }
            
            if(request.Preference == "Aesthetic")
            {
                if(place.IsAnAestheticShop == true)
                {
                    score += AESTHETIC_SCORE;

                    var distance = CalculateDistance(request.Latitude, request.Longitude, place.Latitude, place.Longitude);
                    if (distance <= 2)
                    {
                        score += NEARBY_SCORE; //NEARBY_SCORE is 20 atm
                    }
                    else if (distance <= 5)
                    {
                        score += NEARBY_SCORE - 5;
                    }
                    else if (distance <= 10)
                    {
                        score += NEARBY_SCORE - 10;
                    }
                    else if (distance <= 20)
                    {
                        score += NEARBY_SCORE - 15;
                    }
                    else if (distance <= 30)
                    {
                        score += NEARBY_SCORE - 17;
                    }
                    else if (distance > 50)
                    {
                        score += NEARBY_SCORE - 19;
                    }
                }

                if (place.IsAnAestheticShop == true && place.Deals.Any())
                {
                    score += DEAL_SCORE;
                }
            }

            if(request.Preference == "Nearby")
            {
                var distance = CalculateDistance(request.Latitude, request.Longitude, place.Latitude, place.Longitude);

                if(distance <= 2)
                {
                    score += NEARBY_SCORE; //NEARBY_SCORE is 20 atm
                }
                else if(distance <= 5)
                {
                    score += NEARBY_SCORE - 5;
                }else if(distance <= 10)
                {
                    score += NEARBY_SCORE - 10;
                }

            }

            return score;
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

        public bool IsActiveTime(Deals d, TimeSpan currentTime)
        {
            // Normal case: 08:00 - 17:00
            if (d.StartTime <= d.EndTime)
            {
                return currentTime >= d.StartTime &&
                       currentTime <= d.EndTime;
            }

            // Overnight case: 17:00 - 02:00
            return currentTime >= d.StartTime ||
                   currentTime <= d.EndTime;
        }

    }
}

using IncidentMapAPI.Domain.Models;
using IncidentMapAPI.Domain.Models.DTOs;

namespace IncidentMapAPI.Application.Interfaces.Repositories
{
    public interface IPromotionRepository
    {
        Task<List<Promotion>> GetPromotionsAsync();
        Task<List<Promotion>> GetFilteredPromotions(PromotionDTO promotion);
        Task<List<Promotion>> GetAvailablePromotions();
        Task<bool> AddIncidentAsync(Promotion promotion);
        Task<bool> AddNewDeals(Deals deals);
        double CalculateDistance(double lat1, double lon1, double lat2, double lon2);
        bool IsActiveTime(Deals d, TimeSpan currentTime);
    }
}

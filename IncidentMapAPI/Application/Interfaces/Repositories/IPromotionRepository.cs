using IncidentMapAPI.Domain.Models;
using IncidentMapAPI.Domain.Models.DTOs;

namespace IncidentMapAPI.Application.Interfaces.Repositories
{
    public interface IPromotionRepository
    {
        Task<List<Promotion>> GetPromotionsAsync();
        Task<List<Promotion>> GetFilteredPromotions(PromotionDTO promotion);
        Task<bool> AddIncidentAsync(Promotion promotion);
        double CalculateDistance(double lat1, double lon1, double lat2, double lon2);
    }
}

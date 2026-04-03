using IncidentMapAPI.Domain.Models;

namespace IncidentMapAPI.Application.Interfaces.Repositories
{
    public interface IIncidentRepository
    {
        Task<List<Incident>> GetIncidentsAsync();
        Task<bool> AddIncidentAsync(Incident incident);
    }
}

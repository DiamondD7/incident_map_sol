using IncidentMapAPI.Application.Interfaces.Repositories;
using IncidentMapAPI.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace IncidentMapAPI.Infrastructure.Persistence.Repositories
{
    public class IncidentRepository : IIncidentRepository
    {
        private readonly AppDbContext _context;
        public IncidentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Incident>> GetIncidentsAsync()
        {
            return await _context.IncidentTable.ToListAsync();
        }

        public async Task<bool> AddIncidentAsync(Incident incident)
        {
            if(incident == null)
            {
                return false;
            }

            var newIncident = new Incident
            {
                Latitude = incident.Latitude,
                Longitude = incident.Longitude,
                IncidentType = incident.IncidentType,
                CreatedAt = DateTime.Now,
            };

            _context.IncidentTable.Add(newIncident);
            _context.SaveChanges();

            return true;
        }
    }
}

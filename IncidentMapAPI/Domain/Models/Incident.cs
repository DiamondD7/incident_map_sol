using System.ComponentModel.DataAnnotations;

namespace IncidentMapAPI.Domain.Models
{
    public class Incident
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string? IncidentType { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

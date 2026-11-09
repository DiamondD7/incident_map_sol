namespace IncidentMapAPI.Domain.Models.DTOs
{
    public class DecisionRequestDTO
    {
        public string Category { get; set; }
        public string Preference { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}

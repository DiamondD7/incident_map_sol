using IncidentMapAPI.Application.Interfaces.Repositories;
using IncidentMapAPI.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IncidentMapAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncidentController : ControllerBase
    {
        private readonly IIncidentRepository _incidentRepo;
        public IncidentController(IIncidentRepository incidentRepo)
        {
            _incidentRepo = incidentRepo;
        }

        [HttpGet("all-incidents")]
        public async Task<IActionResult> GetIncidents()
        {
            var loadIncidents = await _incidentRepo.GetIncidentsAsync();

            if(loadIncidents == null)
            {
                return NotFound(new {message="Not found", status = false, code=404});
            }

            return Ok(new { message = "Successful Request", status = true, code = 200, data = loadIncidents });
        }

        [HttpPost("new-incident")]
        public async Task<IActionResult> AddIncident([FromBody] Incident incident)
        {
            if(incident == null)
            {
                return BadRequest(new { status = false, message = "Bad request, incident may be null", code = 400 });
            }

            var newIncident = await _incidentRepo.AddIncidentAsync(incident);

            if(newIncident == false)
            {
                return BadRequest(new { status = false, message = "Bad request, something was wrong when adding an incident", code = 400 });
            }

            return Ok(new { status = true, message = "Successfully added new incident", code = 200 });
        }
    }
}

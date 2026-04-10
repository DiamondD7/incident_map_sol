using IncidentMapAPI.Application.Interfaces.Repositories;
using IncidentMapAPI.Domain.Models;
using IncidentMapAPI.Domain.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IncidentMapAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromotionController : ControllerBase
    {

        private readonly IPromotionRepository _promotion;
        public PromotionController(IPromotionRepository promotion)
        {
            _promotion = promotion;
        }

        [HttpGet("all-promotions")]
        public async Task<IActionResult> GetPromotions()
        {
            var loadPromotions = await _promotion.GetPromotionsAsync();

            if (loadPromotions == null)
            {
                return NotFound(new { message = "Not found", status = false, code = 404 });
            }

            return Ok(new { message = "Successful Request", status = true, code = 200, data = loadPromotions });
        }

        [HttpPost("promotion-by-location")]
        public async Task<IActionResult> GetPromotionsByLocation(PromotionDTO promotion)
        {
            var loadPromotions = await _promotion.GetFilteredPromotions(promotion);
            if (loadPromotions == null)
            {
                return NotFound(new { message = "Not found", status = false, code = 404 });
            }
            return Ok(new { message = "Successful Request", status = true, code = 200, data = loadPromotions });
        }

        [HttpPost("new-promotion")]
        public async Task<IActionResult> AddPromotion([FromBody] Promotion promotion)
        {
            if (promotion == null)
            {
                return BadRequest(new { status = false, message = "Bad request, incident may be null", code = 400 });
            }

            var newPromotion = await _promotion.AddIncidentAsync(promotion);

            if (newPromotion == false)
            {
                return BadRequest(new { status = false, message = "Bad request, something was wrong when adding an incident", code = 400 });
            }

            return Ok(new { status = true, message = "Successfully added new incident", code = 200 });
        }
    }
}

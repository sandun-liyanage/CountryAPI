using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Services;

namespace CountryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly ICountryService _service;

        public CountryController(ICountryService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get list of countries by ID
        /// </summary>
        /// <param name="ids">Country IDS (cca3 code) Ex: LKA, IND</param>
        /// <returns>
        /// Success: List of country data
        /// Error: 404 Error
        /// </returns>
        [HttpGet("GetByCodes")]
        public async Task<IActionResult> GetByCodes([FromQuery] List<string> ids)
        {
            var countries = await _service.GetCountriesByCodesAsync(ids);
            return countries.Any() ? Ok(countries) : NotFound("Error fetching data.");
        }

        /// <summary>
        /// Get country by ID
        /// </summary>
        /// <param name="id">ID (Country code - Ex: LKA)</param>
        /// <returns>
        /// Success: Returns country data for given country ID
        /// Error: 404 Error
        /// </returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByCode(string id)
        {
            var countries = await _service.GetCountriesByCodesAsync(new List<string> { id });
            return countries.Any() ? Ok(countries.FirstOrDefault()) : NotFound("Error fetching data.");
        }
    }
}

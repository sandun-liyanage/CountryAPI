using Core.DTO;

namespace ServiceLayer.Services
{
    public interface ICountryService
    {
        /// <summary>
        /// Get list of countries by ID
        /// </summary>
        /// <param name="ids">Country IDS (cca3 code) Ex: LKA, IND</param>
        /// <returns>
        /// Success: Country list
        /// Error: Empty list object
        /// </returns>
        Task<List<CountryDTO>> GetCountriesByCodesAsync(List<string> ids);
    }
}

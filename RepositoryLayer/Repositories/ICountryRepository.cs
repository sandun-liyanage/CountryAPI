using Core.DTO;

namespace RepositoryLayer.Repositories
{
    public interface ICountryRepository
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

        /// <summary>
        /// Saves the given country object in DB
        /// </summary>
        /// <param name="country">Country data</param>
        /// <returns>
        /// Success: true
        /// Error: false
        /// </returns>
        Task<bool> SaveAsync(CountryDTO country);
    }
}

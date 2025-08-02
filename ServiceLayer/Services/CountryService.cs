using Core.DTO;
using RepositoryLayer.Repositories;
using System.Text.Json;

namespace ServiceLayer.Services
{
    public class CountryService : ICountryService
    {
        private readonly HttpClient _httpClient;
        private readonly ICountryRepository _repo;

        public CountryService(IHttpClientFactory clientFactory, ICountryRepository repository)
        {
            _httpClient = clientFactory.CreateClient();
            _repo       = repository;
        }

        /// <summary>
        /// Get list of countries by ID
        /// </summary>
        /// <param name="ids">Country IDS (cca3 code) Ex: LKA, IND</param>
        /// <returns>
        /// Success: Country list
        /// Error: Empty list object
        /// </returns>
        public async Task<List<CountryDTO>> GetCountriesByCodesAsync(List<string> ids)
        {
            //Validate IDs
            if (ids == null || !ids.Any() || ids.Any(id => string.IsNullOrEmpty(id) || id.Length != 3 || !id.All(char.IsLetter)))
            {
                Console.WriteLine("Validation failed. Invalid country IDs.");
                return new List<CountryDTO>();
            }

            // Retrieve from DB if available.
            var countries = await _repo.GetCountriesByCodesAsync(ids);
            if (countries.Count == ids.Count)
                return countries;

            var foundIds = countries.Select(c => c.Id).ToHashSet();
            var pendingCountries = string.Join(",", ids.Where(id => !foundIds.Contains(id)));

            try
            {
                var stringIds = string.Join(",", ids);
                var response = await _httpClient.GetAsync($"https://restcountries.com/v3.1/alpha?codes={pendingCountries}&fields=cca3,name,region,population");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var external = JsonSerializer.Deserialize<List<CountryDTO>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                // Store retrieved data in DB
                if (external.Any())
                {
                    foreach (var c in external)
                        await _repo.SaveAsync(c);
                }

                countries.AddRange(external);
                return countries;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<CountryDTO>();
            }
        }
    }
}

using Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ServiceLayer.Services
{
    public class CountryService : ICountryService
    {
        private readonly HttpClient _httpClient;

        public CountryService(IHttpClientFactory clientFactory)
        {
            _httpClient = clientFactory.CreateClient();
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
            // Pending...


            try
            {
                var stringIds = string.Join(",", ids);
                var response = await _httpClient.GetAsync($"https://restcountries.com/v3.1/alpha?codes={stringIds}&fields=cca3,name,region,population");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var countries = JsonSerializer.Deserialize<List<CountryDTO>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                // Store retrieved data in DB
                // Pending...


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

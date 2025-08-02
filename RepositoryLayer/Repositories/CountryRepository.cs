using Core.AppSettings;
using Core.DTO;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;

namespace RepositoryLayer.Repositories
{
    public class CountryRepository : ICountryRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<CountryRepository> _logger;

        public CountryRepository(AppSettings settings, ILogger<CountryRepository> logger)
        {
            _connectionString = settings.ConnectionString;            _logger = logger;

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
            try
            {
                var paramNames = ids.Select((_, i) => $"@id{i}").ToList();
                string sql     = $"SELECT Id, Name, Region, Population FROM Countries WHERE Id IN ({string.Join(", ", paramNames)})";

                using var conn = new SqlConnection(_connectionString);
                using var cmd  = new SqlCommand(sql, conn);

                // Add each id as a separate parameter
                for (int i = 0; i < ids.Count; i++)
                    cmd.Parameters.AddWithValue($"@id{i}", ids[i]);

                await conn.OpenAsync();

                var result = new List<CountryDTO>();
                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    result.Add(new CountryDTO
                    {
                        Id         = reader["Id"].ToString(),
                        Name       = new CountryNameDTO
                        {
                            Common = reader["Name"].ToString()
                        },
                        Region     = reader["Region"].ToString(),
                        Population = Convert.ToInt64(reader["Population"])
                    });
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "Error Occurred");
                return new List<CountryDTO>();
            }
        }

        /// <summary>
        /// Saves the given country object in DB
        /// </summary>
        /// <param name="country">Country data</param>
        /// <returns>
        /// Success: true
        /// Error: false
        /// </returns>
        public async Task<bool> SaveAsync(CountryDTO country)
        {
            const string sql = @"INSERT INTO Countries (Id, Name, Region, Population)
                             VALUES (@Id, @Name, @Region, @Population)";

            try
            {
                using var conn = new SqlConnection(_connectionString);
                using var cmd  = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@Id", country.Id);
                cmd.Parameters.AddWithValue("@Name", country.Name.Common);
                cmd.Parameters.AddWithValue("@Region", country.Region);
                cmd.Parameters.AddWithValue("@Population", country.Population);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "Error Occurred");
                return false;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Core.DTO
{
    public class CountryDTO
    {
        /// <summary>
        /// Country ID. (country code) Ex: IND, LKA
        /// </summary>
        [JsonPropertyName("cca3")]
        public string Id           { get; set; }

        /// <summary>
        /// Country name
        /// </summary>
        public CountryNameDTO Name { get; set; }

        /// <summary>
        /// Region of the country
        /// </summary>
        public string Region       { get; set; }

        /// <summary>
        /// Population of the country
        /// </summary>
        public long Population     { get; set; }
    }
}

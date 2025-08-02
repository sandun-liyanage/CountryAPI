using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO
{
    /// <summary>
    /// Countries have multiple names. common, official, native etc.
    /// Only retrieving common name for this project
    /// </summary>
    public class CountryNameDTO
    {
        /// <summary>
        /// Commonly used name
        /// </summary>
        public string Common { get; set; }
    }
}

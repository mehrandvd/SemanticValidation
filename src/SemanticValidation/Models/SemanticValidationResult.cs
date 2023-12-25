using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SemanticValidation.Models
{
    /// <summary>
    /// The result of a semantic validation
    /// </summary>
    public class SemanticValidationResult
    {
        /// <summary>
        /// Whether the semantic validation is successful or not.
        /// </summary>
        [JsonPropertyName("success")]
        public bool IsValid { get; set; }

        /// <summary>
        /// The reason for the validation result.
        /// </summary>
        [JsonPropertyName("message")]
        public string? Reason { get; set; }
    }
}

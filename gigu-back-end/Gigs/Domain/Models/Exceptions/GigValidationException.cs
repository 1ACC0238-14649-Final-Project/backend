using System;
using System.Collections.Generic;
using System.Linq;

namespace Gigs.Domain.Models.Exceptions
{
    public class GigValidationException : Exception
    {
        public Dictionary<string, string[]> ValidationErrors { get; } = new Dictionary<string, string[]>();
        
        public GigValidationException() : base("Gig validation failed") { }

        public GigValidationException(string message) : base(message) { }

        public GigValidationException(string message, Exception innerException) 
            : base(message, innerException) { }

        public GigValidationException(Dictionary<string, string[]> validationErrors) 
            : base("Multiple validation errors occurred")
        {
            ValidationErrors = validationErrors ?? throw new ArgumentNullException(nameof(validationErrors));
        }

        public GigValidationException(string field, string error)
            : base($"Validation error for {field}: {error}")
        {
            if (string.IsNullOrWhiteSpace(field))
                throw new ArgumentException("Field name cannot be empty", nameof(field));

            if (string.IsNullOrWhiteSpace(error))
                throw new ArgumentException("Error message cannot be empty", nameof(error));

            ValidationErrors.Add(field, new[] { error });
        }

        public override string ToString()
        {
            if (ValidationErrors.Any())
            {
                var errorMessages = ValidationErrors
                    .SelectMany(kvp => kvp.Value.Select(v => $"{kvp.Key}: {v}"));
                
                return $"{Message}{Environment.NewLine}{string.Join(Environment.NewLine, errorMessages)}";
            }
            
            return Message;
        }
    }
}
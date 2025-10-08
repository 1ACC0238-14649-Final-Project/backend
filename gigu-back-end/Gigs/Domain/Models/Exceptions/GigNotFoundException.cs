using System;

namespace Gigs.Domain.Models.Exceptions
{
    public class GigNotFoundException : Exception
    {
        public GigNotFoundException() : base("Gig not found")
        {
        }

        public GigNotFoundException(int gigId) : base($"Gig with ID {gigId} not found")
        {
        }

        public GigNotFoundException(string message) : base(message)
        {
        }

        public GigNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
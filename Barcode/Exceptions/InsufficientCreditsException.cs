using System;

namespace Barcode.Exceptions
{
    public class InsufficientCreditsException : Exception
    {
        public InsufficientCreditsException() { }
        public InsufficientCreditsException(string? message) : base(message) { }
        public InsufficientCreditsException(string? message, Exception? innerException) : base(message, innerException) { }
    }
}
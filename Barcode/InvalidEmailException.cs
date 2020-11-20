using System;

namespace Barcode
{
    public class InvalidEmailException : Exception
    {
        public InvalidEmailException() : base() { }
        public InvalidEmailException(string? message) : base(message) { }
        public InvalidEmailException(string? message, Exception? innerException) : base(message, innerException) { }
    }
}
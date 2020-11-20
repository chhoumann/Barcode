using System;

namespace Barcode
{
    public class InvalidUsernameException : Exception
    {
        public InvalidUsernameException() : base() { }
        public InvalidUsernameException(string? message) : base(message) { }
        public InvalidUsernameException(string? message, Exception? innerException) : base(message, innerException) { }
    }
}
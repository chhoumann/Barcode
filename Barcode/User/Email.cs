using System.Linq;
using Barcode.Exceptions;

namespace Barcode.User
{
    public class Email
    {
        private string _emailAddress;

        public Email() { }

        public Email(string emailAddress)
        {
            EmailAddress = emailAddress;
        }

        public string EmailAddress
        {
            get => _emailAddress;
            set
            {
                if (ValidateEmail(value)) _emailAddress = value;
                else throw new InvalidEmailException($"Invalid Email Address given: {value}");
            }
        }

        /// <summary>
        /// Validates an email address.
        /// </summary>
        /// <param name="email">Email address to validate.</param>
        /// <returns>A boolean representing the validity of the email.</returns>
        private bool ValidateEmail(string email)
        {
            string[] emailParts = email.Split("@");
            bool emailHasCorrectFormat = emailParts.Length == 2 && emailParts[0].Length > 0 && emailParts[1].Length > 0;
            if (!emailHasCorrectFormat) return false;

            string domainStartAndEnd = emailParts[1].Remove(1, emailParts[1].Length - 2);
            
            bool IsDotOrSeparator(char c) => c == '.' || c == '-';

            bool localPartIsLegal = (emailParts[0].All(c => char.IsLetterOrDigit(c) || IsDotOrSeparator(c)));
            bool domainPartIsLegal = (!domainStartAndEnd.Any(IsDotOrSeparator) &&
                                      emailParts[1].All(c => char.IsLetterOrDigit(c) || IsDotOrSeparator(c)) &&
                                      emailParts[1].Contains('.'));
            
            return (localPartIsLegal && domainPartIsLegal);
        }
    }
}
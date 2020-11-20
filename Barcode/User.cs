using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Barcode
{
    public class User : IComparable
    {
        private static uint userIdTracker = 0;
        private string _emailAddress;
        private string _username;
        public uint Id { get; private set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Username
        {
            get => _username;
            set
            {
                var illegalCharacterRegex = new Regex(@"([^a-z0-9_])");
                
                if (!illegalCharacterRegex.IsMatch(value) && !string.IsNullOrWhiteSpace(value))
                    _username = value;
                else
                    throw new InvalidUsernameException($"Invalid username given: {value}");
            }
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

        public decimal Balance { get; private set; } = 0m;

        public User()
        {
            Id = userIdTracker++;
        }

        public User(string firstName, string lastName, string username, string emailAddress) : this()
        {
            FirstName = firstName;
            LastName = lastName;
            Username = username;
            EmailAddress = emailAddress;
        }

        public override string ToString()
        {
            return $"{nameof(FirstName)}: {FirstName}, {nameof(LastName)}: {LastName}, {nameof(EmailAddress)}: {EmailAddress}";
        }

        public int CompareTo(object? obj)
        {
            if (obj is User otherUser) return (int) (Id - otherUser.Id);
            return 1;
        }
    }
}
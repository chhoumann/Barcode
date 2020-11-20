using System;
using System.Text.RegularExpressions;
using Barcode.Exceptions;

namespace Barcode
{
    public class User : IComparable
    {
        private static uint userIdTracker = 0;
        private string _username;
        private Email _email;
        private string _firstName;
        private string _lastName;
        public uint Id { get; private set; }

        public string FirstName
        {
            get => _firstName;
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException("First Name cannot be null or empty.", nameof(value));
                _firstName = value;
            }
        }

        public string LastName
        {
            get => _lastName;
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException("Last Name cannot be null or empty.", nameof(value));
                _lastName = value;
            }
        }

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

        public decimal Balance { get; set; } = 0m;

        public string Email
        {
            get => _email.EmailAddress;
            set => _email.EmailAddress = value;
        }

        private User()
        {
            Id = userIdTracker++;
            _email = new Email();
        }

        public User(string firstName, string lastName, string username, string emailAddress) : this()
        {
            FirstName = firstName;
            LastName = lastName;
            Username = username;
            _email = new Email(emailAddress);
        }

        public override string ToString()
        {
            return $"{nameof(FirstName)}: {FirstName}, {nameof(LastName)}: {LastName}, {nameof(Email)}: {Email}";
        }

        public int CompareTo(object? obj)
        {
            return obj is User otherUser ? Id.CompareTo(otherUser.Id) : 1;
        }
    }
}
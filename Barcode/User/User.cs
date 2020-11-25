using System;
using System.Text.RegularExpressions;
using Barcode.Exceptions;

namespace Barcode
{
    public class User : IComparable
    {
        public static event Action<User> UserBalanceNotification;
        private static uint userIdTracker = 0;
        private const decimal balanceNotificationThreshold = 50m;
        
        private string username;
        private Email email;
        private string firstName;
        private string lastName;
        private decimal balance;

        public decimal Balance
        {
            get => balance;
            set
            {
                balance = value;
                if (balance <= balanceNotificationThreshold) UserBalanceNotification?.Invoke(this);
            }
        }

        public uint Id { get; set; }

        public string FirstName
        {
            get => firstName;
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException("First Name cannot be null or empty.", nameof(value));
                firstName = value;
            }
        }

        public string LastName
        {
            get => lastName;
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException("Last Name cannot be null or empty.", nameof(value));
                lastName = value;
            }
        }

        public string Username
        {
            get => username;
            set
            {
                var illegalCharacterRegex = new Regex(@"([^a-z0-9_])");
                
                if (!illegalCharacterRegex.IsMatch(value) && !string.IsNullOrWhiteSpace(value))
                    username = value;
                else
                    throw new InvalidUsernameException($"Invalid username given: {value}");
            }
        }


        public string Email
        {
            get => email.EmailAddress;
            set => email.EmailAddress = value;
        }

        public User()
        {
            Id = userIdTracker++;
            email = new Email();
        }

        public User(string firstName, string lastName, string username, string emailAddress) : this()
        {
            FirstName = firstName;
            LastName = lastName;
            Username = username;
            email = new Email(emailAddress);
        }

        public override string ToString()
        {
            return $"{firstName} {lastName} - {username} | #{Id}" +
                   $"\n" +
                   $"{email}" +
                   $"\n" +
                   $"Balance: {Balance} credits";
        }

        public int CompareTo(object? obj)
        {
            return obj is User otherUser ? Id.CompareTo(otherUser.Id) : 1;
        }
    }
}
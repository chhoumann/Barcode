using System;
using System.Text.RegularExpressions;
using Barcode.Exceptions;

namespace Barcode
{
    public class User : IComparable
    {
        private const decimal balanceNotificationThreshold = 50m;

        private static uint userIdTracker;
        private readonly Email email;
        private decimal balance;
        private string firstName;
        private string lastName;
        private string username;

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
                Regex illegalCharacterRegex = new Regex(@"([^a-z0-9_])");

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

        public int CompareTo(object? obj)
        {
            return obj is User otherUser ? Id.CompareTo(otherUser.Id) : 1;
        }

        public static event Action<User> UserBalanceNotification;

        public override string ToString()
        {
            return $"{FirstName} {LastName} - {Username} | #{Id}" +
                   "\n" +
                   $"{Email}" +
                   "\n" +
                   $"Balance: {Balance} credits";
        }
    }
}
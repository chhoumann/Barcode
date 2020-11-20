using System;
using System.Collections.Generic;
using Barcode.Exceptions;
using Barcode.User;
using NUnit.Framework;
using NSubstitute;

namespace BarcodeTests
{
    public class UserTests
    {
        private object[] userArgs;

        [SetUp]
        public void SetUp()
        {
           userArgs = new object[]
           {
              "Christian", "Houmann", "chbh", "christian@bagerbach.com" 
           };
        }
        

        [Test]
        public void Constructor_CreateUser_IdIncrementsOnCreate()
        {
            User user1 = Substitute.For<User>(userArgs);
            User user2 = Substitute.For<User>(userArgs);

            bool newestUserHasGreatestId = user1.Id < user2.Id; 
            
            Assert.That(newestUserHasGreatestId, Is.True);
        }

        [Test]
        public void CompareTo_SortingUsers_IsSortedById()
        {
            List<User> userList = new List<User>()
            {
                Substitute.For<User>(userArgs),
                Substitute.For<User>(userArgs),
                Substitute.For<User>(userArgs)
            };

            userList.Sort();
            
            Assert.That(userList, Is.Ordered);
        }

        [TestCase("eksempel@domain.dk")]
        [TestCase("eksempel@mit-domain.dk")]
        public void EmailAddress_SetEmail_AcceptsCorrectlyFormattedEmailAddress(string email)
        {
            User user = Substitute.For<User>(userArgs);
            user.Email= email;
            
            Assert.That(user.Email, Is.EqualTo(email));
        }

        [TestCase("eksempel(2)@-mit_domain.dk")]
        [TestCase("()@d.dk")]
        [TestCase("a@-dk")]
        [TestCase("a@dk")]
        [TestCase("a@a.")]
        [TestCase("a@-.dk")]
        [TestCase("a@.dk-")]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("@")]
        [TestCase("word")]
        public void EmailAddress_SetEmail_RejectsInvalidEmailAddress(string email)
        {
            User user = Substitute.For<User>(userArgs);

            TestDelegate setUserEmail = () => user.Email = email;
            
            Assert.Throws<InvalidEmailException>(setUserEmail, $"Got: {user.Email}");
        }

        [TestCase("christian")]
        public void Username_SetUsername_AcceptsCorrectlyFormattedUsername(string username)
        {
            User user = Substitute.For<User>(userArgs);

            user.Username = username;

            Assert.That(user.Username == username);
        }

        [TestCase("()()()")]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase(" -asd")]
        [TestCase("chri stian")]
        [TestCase("Christian")]
        public void Username_SetUsername_RejectsIllegalUsername(string username)
        {
            User user = Substitute.For<User>(userArgs);
            
            TestDelegate setUsername = () => user.Username = username;

            Assert.Throws<InvalidUsernameException>(setUsername, $"Got: {user.Username}");
        }

        [TestCase("")]
        public void FirstName_SetFirstNameToEmptyString_Fails(string firstName)
        {
            User user = Substitute.For<User>(userArgs);

            TestDelegate setFirstName = () => user.FirstName = firstName;

            Assert.Throws<ArgumentException>(setFirstName);
        }

        [TestCase("")]
        public void LastName_SetLastNameToEmptyString_Fails(string lastName)
        {
             User user = Substitute.For<User>(userArgs);
 
             TestDelegate setLastName = () => user.LastName = lastName;
 
             Assert.Throws<ArgumentException>(setLastName);
        }

        [Test]
        public void ToString_CallToStringOnUser_ReturnsString()
        {
            User user = Substitute.For<User>(userArgs);
            
            Assert.That(user.ToString(), Is.TypeOf<String>());
        }
    }
}
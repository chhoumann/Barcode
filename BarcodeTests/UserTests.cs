using System;
using System.Collections.Generic;
using Barcode;
using NUnit.Framework;
using NSubstitute;

namespace BarcodeTests
{
    public class UserTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Constructor_CreateUser_IdIncrementsOnCreate()
        {
            User user1 = Substitute.For<User>();
            User user2 = Substitute.For<User>();

            bool newestUserHasGreatestId = user1.Id < user2.Id; 
            
            Assert.That(newestUserHasGreatestId);
        }

        [Test]
        public void CompareTo_SortingUsers_IsSortedById()
        {
            List<User> userList = new List<User>()
            {
                Substitute.For<User>(),
                Substitute.For<User>(),
                Substitute.For<User>()
            };

            userList.Sort();
            bool userListIsSortedById = (userList[0].Id == 0 && userList[1].Id == 1 && userList[2].Id == 2);
            
            Assert.That(userListIsSortedById);
        }

        [Test]
        [TestCase("eksempel@domain.dk")]
        [TestCase("eksempel@mit-domain.dk")]
        public void EmailAddress_SetEmail_AcceptsCorrectlyFormattedEmailAddress(string email)
        {
            User user = Substitute.For<User>();
            user.EmailAddress = email;
            
            Assert.That(user.EmailAddress == email);
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
            User user = Substitute.For<User>();

            TestDelegate setUserEmail = () => user.EmailAddress = email;
            
            Assert.Throws<InvalidEmailException>(setUserEmail, $"Got: {user.EmailAddress}");
        }

        [TestCase("christian")]
        public void Username_SetUsername_AcceptsCorrectlyFormattedUsername(string username)
        {
            User user = Substitute.For<User>();

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
            User user = Substitute.For<User>();
            
            TestDelegate setUsername = () => user.Username = username;

            Assert.Throws<InvalidUsernameException>(setUsername, $"Got: {user.Username}");
        }
        
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UserServiceLibrary.Tests
{
    [TestClass]
    public class UserServiceTests
    {
        private static int _startId = 1000;
        private static readonly UserService UserService;

        static UserServiceTests()
        {
            UserService = new UserService(IdGenerator);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Add_NullUser_ExceptionThrown()
        {
            UserService.Add(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Add_ExistingUser_ExceptionThrown()
        {
            User user = new User
            {
                FirstName = "Alice",
                LastName = "Smith",
                DateOfBirth = new DateTime(1980, 5, 14)
            };

            UserService.Add(user);
            UserService.Add(user);
        }

        [TestMethod]
        public void Add_User()
        {
            User user = new User
            {
                FirstName = "Alice",
                LastName = "Smith",
                DateOfBirth = new DateTime(1980, 5, 14)
            };

            UserService.Add(user);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Remove_NullUser_ExceptionThrown()
        {
            UserService.Remove(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Remove_NonexistentUser_ExceptionThrown()
        {
            User user = new User
            {
                FirstName = "Alice",
                LastName = "Smith",
                DateOfBirth = new DateTime(1980, 5, 14)
            };

            UserService.Remove(user);
        }

        [TestMethod]
        public void Remove_User()
        {
            User user = new User
            {
                FirstName = "Alice",
                LastName = "Smith",
                DateOfBirth = new DateTime(1980, 5, 14)
            };

            UserService.Add(user);
            UserService.Remove(user);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Search_NullCriterion_ExceptionThrown()
        {
            UserService.Search(null);
        }

        [TestMethod]
        public void Search_ByLastName()
        {
            User user = new User
            {
                FirstName = "Alice",
                LastName = "Smith",
                DateOfBirth = new DateTime(1980, 5, 14)
            };

            UserService.Add(user);

            Predicate<User> searchByLastName = u => u == user;
            var expected = new List<User>();
            expected.Add(user);

            CollectionAssert.AreEqual(expected, UserService.Search(searchByLastName).ToList());
        }

        [TestMethod]
        public void Search_ByLasttName()
        {
            User user = new User
            {
                FirstName = "Alice",
                LastName = "Smith",
                DateOfBirth = new DateTime(1980, 5, 14)
            };

            UserService.Add(user);

            Predicate<User> predicate = u => u.LastName == user.LastName;
            var expected = new List<User>();
            expected.Add(user);

            CollectionAssert.AreEqual(expected, UserService.Search(predicate).ToList());
        }

        [TestMethod]
        public void Search_ByFirstName()
        {
            User user = new User
            {
                FirstName = "Alice",
                LastName = "Smith",
                DateOfBirth = new DateTime(1980, 5, 14)
            };

            UserService.Add(user);

            Predicate<User> predicate = u => u.FirstName == user.FirstName;
            var expected = new List<User>();
            expected.Add(user);

            CollectionAssert.AreEqual(expected, UserService.Search(predicate).ToList());
        }

        private static int IdGenerator()
        {
            return _startId++;
        }
    }
}

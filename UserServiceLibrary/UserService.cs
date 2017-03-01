using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace UserServiceLibrary
{
    /// <summary>
    /// Creates a service for users
    /// </summary>
    [Serializable]
    public class UserService
    {
        private readonly Func<int> _idAlgorithm;

        /// <summary>
        /// A logging 
        /// </summary>
        public static bool Logging { get; }

        static UserService()
        {
            Logging = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["logging"]);

            if (Logging)
            {
                var listener = new ConsoleTraceListener();
                Trace.Listeners.Add(listener);
            }
        }

        /// <summary>
        /// A list for storing users
        /// </summary>
        public List<User> Users {get;}

        /// <summary>
        /// Initializes a new instance of the UserService class
        /// </summary>
        /// <param name="idAlgorithm">An algorithm for id generating</param>
        public UserService(Func<int> idAlgorithm)
        {
            Users = new List<User>();
            _idAlgorithm = idAlgorithm;
        }

        /// <summary>
        /// Adds a new user to the collection
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// Thrown if the user is null or if any field of the user contains invalid data
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown if the user is already in database
        /// </exception>
        /// <param name="user">A user</param>
        public void Add(User user)
        {
            if (user == null)
                throw new ArgumentNullException("Null user!");

            if (string.IsNullOrEmpty(user.FirstName) || string.IsNullOrEmpty(user.LastName) || user.DateOfBirth == null)
                throw new ArgumentNullException("Null user's data!");

            if (Users.Exists(curUser => curUser.FirstName == user.FirstName &&
                                        curUser.LastName == user.LastName &&
                                        curUser.DateOfBirth == user.DateOfBirth))
                throw new ArgumentException("Duplicate user!");

            user.Id = _idAlgorithm();
            Users.Add(user);
        }

        /// <summary>
        /// Removes a specified user from the collection
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// Thrown if the user is null
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown if the user isn't in database
        /// </exception>
        /// <param name="user">A user</param>
        public void Remove(User user)
        {
            if (user == null)
                throw new ArgumentNullException();

            if (!Users.Exists(curUser => curUser.FirstName == user.FirstName &&
                                        curUser.LastName == user.LastName &&
                                        curUser.DateOfBirth == user.DateOfBirth))
                throw new ArgumentException("No such user in database!");

            Users.Remove(user);
        }

        /// <summary>
        /// Searches for users according to a specified criterion
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// Thrown if the criterion is null
        /// </exception>
        /// <param name="predicate">A criterion</param>
        /// <returns>Returns a collection of users</returns>
        public IEnumerable<User> Search(Predicate<User> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException("Null criterion!");

            IEnumerable<User> result = Users.Where(u => predicate(u));

            if (!result.Any())
                throw new ArgumentNullException("No such user!");

            return result.ToList();
        }
    }
}

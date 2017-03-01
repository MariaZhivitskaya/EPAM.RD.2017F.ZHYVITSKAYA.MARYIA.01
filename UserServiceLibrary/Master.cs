using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace UserServiceLibrary
{
    /// <summary>
    /// Creates master's service
    /// </summary>
    [Serializable]
    public class Master
    {
        /// <summary>
        /// A service for users
        /// </summary>
        public UserService UserService { get; }

        private static readonly bool Logging;

        static Master()
        {
            Logging = UserService.Logging;
        }

        /// <summary>
        /// A delegate for slaves' notifications
        /// </summary>
        /// <param name="user">A user</param>
        public delegate void MasterEventHandler(User user);

        /// <summary>
        /// An event for adding user
        /// </summary>
        public event MasterEventHandler AddUserEvent;

        /// <summary>
        /// An event for deleting user
        /// </summary>
        public event MasterEventHandler RemoveUserEvent;

        /// <summary>
        /// Initializes a new instance of the Master class 
        /// </summary>
        /// <param name="userService">An instance of the UserService class</param>
        public Master(UserService userService)
        {
            UserService = userService;
        }

        /// <summary>
        /// Adds a new user to the service
        /// </summary>
        /// <param name="user">A user</param>
        public void Add(User user)
        {
            UserService.Add(user);
            if (Logging)
            {
                Trace.WriteLine("-----------------------------");
                Trace.WriteLine("    This is a log message");
                Trace.WriteLine("Master is adding");
                Trace.WriteLine("-----------------------------\n");
            }

            AddUserEvent?.Invoke(user);
        }

        /// <summary>
        /// Removes a user from the service
        /// </summary>
        /// <param name="user">A user</param>
        public void Remove(User user)
        {
            UserService.Remove(user);
            if (Logging)
            {
                Trace.WriteLine("-----------------------------");
                Trace.WriteLine("    This is a log message");
                Trace.WriteLine("Master is removing");
                Trace.WriteLine("-----------------------------\n");
            }

            RemoveUserEvent?.Invoke(user);
        }

        /// <summary>
        /// Searches for a user in the service
        /// </summary>
        /// <param name="predicate">A search criterion</param>
        /// <returns>Returns a collection of users</returns>
        public IEnumerable<User> Search(Predicate<User> predicate)
        {
            if (Logging)
            {
                Trace.WriteLine("-----------------------------");
                Trace.WriteLine("    This is a log message");
                Trace.WriteLine("Master is searching");
                Trace.WriteLine("-----------------------------\n");
            }

            return UserService.Search(predicate).ToList();
        }
    }
}

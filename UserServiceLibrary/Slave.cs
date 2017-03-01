using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace UserServiceLibrary
{
    /// <summary>
    /// Creates slave's service
    /// </summary>
    [Serializable]
    public class Slave
    {
        private readonly List<User> _users = new List<User>();
        private static readonly bool Logging;

        static Slave()
        {
            Logging = UserService.Logging;
        }

        /// <summary>
        /// Subscribes a slave for a master
        /// </summary>
        /// <param name="master">A master</param>
        public void Subscribe(Master master)
        {
            master.AddUserEvent += AddUser;
            master.RemoveUserEvent += RemoveUser;
        }

        /// <summary>
        /// Searches for a user in the service
        /// </summary>
        /// <param name="predicate">A search criterion</param>
        /// <returns>Returns a collection of users</returns>
        public IEnumerable<User> Search(Predicate<User> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException("Null criterion!");

            if (Logging)
            {
                Trace.WriteLine("-----------------------------");
                Trace.WriteLine("    This is a log message");
                Trace.WriteLine("Slave is searching");
                Trace.WriteLine("-----------------------------\n");
            }

            return _users.Where(u => predicate(u)).ToList().ToList();
        }

        /// <summary>
        /// Adds a user to the service
        /// </summary>
        /// <param name="user">A user</param>
        private void AddUser(User user)
        {
            _users.Add(user);
            if (Logging)
            {
                Trace.WriteLine("-----------------------------");
                Trace.WriteLine("    This is a log message");
                Trace.WriteLine("Slave is adding");
                Trace.WriteLine("-----------------------------\n");
            }
        }

        /// <summary>
        /// Removes a user from the service
        /// </summary>
        /// <param name="user">A user</param>
        private void RemoveUser(User user)
        {
            _users.Remove(user);
            if (Logging)
            {
                Trace.WriteLine("-----------------------------");
                Trace.WriteLine("    This is a log message");
                Trace.WriteLine("Slave is removing");
                Trace.WriteLine("-----------------------------\n");
            }
        }
    }
}

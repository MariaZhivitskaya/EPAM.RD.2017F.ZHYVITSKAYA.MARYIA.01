using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;

namespace UserServiceLibrary
{
    /// <summary>
    /// Creates a service with domains for users
    /// </summary>
    public class DomainService : IDisposable
    {
        private readonly List<AppDomain> _domains = new List<AppDomain>();
        private readonly List<Slave> _slaves = new List<Slave>();
        private readonly Master _master;
        private static readonly bool Logging;
        private static readonly string FileName;
        
        /// <summary>
        /// A number of slaves
        /// </summary>
        public static int SlavesNumber;

        static DomainService()
        {
            SlavesNumber = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["slaves"]);
            Logging = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["logging"]);
            FileName = System.Configuration.ConfigurationManager.AppSettings["fileName"];
        }

        /// <summary>
        /// Initializes a new instance of the DomainService class
        /// </summary>
        /// <param name="service">An instance of the UserService class</param>
        public DomainService(UserService service)
        {
            _master = MasterDomain(service);

            for (int i = 0; i < SlavesNumber; i++)
                _slaves.Add(SlaveDomain(i, _master));
        }

        /// <summary>
        /// Creates a master in it's own domain
        /// </summary>
        /// <param name="service">An instance of the UserService class</param>
        /// <returns>Returns a master</returns>
        private Master MasterDomain(UserService service)
        {
            AppDomainSetup appDomainSetup = new AppDomainSetup
            {
                ApplicationBase = AppDomain.CurrentDomain.BaseDirectory,
                PrivateBinPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MasterDomain")
            };

            AppDomain domain = AppDomain.CreateDomain
                ("MasterDomain", null, appDomainSetup);
            _domains.Add(domain);

            var master = (Master)domain.CreateInstanceAndUnwrap
                ("UserServiceLibrary", typeof(Master).FullName, false, BindingFlags.CreateInstance, null, new object[] { service }, null,
                    null);

            return master;
        }

        /// <summary>
        /// Creates a slave in it's own domain
        /// </summary>
        /// <param name="id">An id of a slave</param>
        /// <param name="master">An instance of the UserService class</param>
        /// <returns>Returns a slave</returns>
        private Slave SlaveDomain(int id, Master master)
        {
            AppDomainSetup appDomainSetup = new AppDomainSetup
            {
                ApplicationBase = AppDomain.CurrentDomain.BaseDirectory,
                PrivateBinPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SlaveDomain")
            };

            AppDomain domain = AppDomain.CreateDomain
                ($"SlaveDomain{id}", null, appDomainSetup);
            _domains.Add(domain);

            var slave = (Slave)domain.CreateInstanceAndUnwrap("UserServiceLibrary", typeof(Slave).FullName);
            slave.Subscribe(master);

            return slave;
        }

        /// <summary>
        /// Unloads domains
        /// </summary>
        public void Dispose()
        {
            foreach (var appDomain in _domains)
                AppDomain.Unload(appDomain);
        }

        /// <summary>
        /// Adds a new user to the collection
        /// </summary>
        /// <param name="user">A user</param>
        public void Add(User user) => _master.Add(user);

        /// <summary>
        /// Removes a specified user from the collection
        /// </summary>
        /// <param name="user">A user</param>
        public void Remove(User user) => _master.Remove(user);

        public IEnumerable<User> SearchMaster(Predicate<User> predicate) => 
            _master.Search(predicate);

        /// <summary>
        /// Searches for users according to a specified criterion
        /// </summary>
        /// <param name="predicate">A criterion</param>
        /// <returns>Returns a collection of users</returns>
        public IEnumerable<User> SearchSlave(Predicate<User> predicate)
        {
            int slave = _slaves.Count == 1 ? 0 : new Random().Next(0, _slaves.Count - 1);
            return _slaves[slave].Search(predicate);
        }

        /// <summary>
        /// Serializes users from the service to a file with a specified FileName
        /// </summary>
        public void Serialize()
        {
            if (Logging)
            {
                Trace.WriteLine("-----------------------------");
                Trace.WriteLine("    This is a log message");
                Trace.WriteLine("Serializing info in " + FileName + "...");
            }

            var xmlSerializer = new XmlSerializer(typeof(User));

            using (FileStream fs = new FileStream(FileName, FileMode.OpenOrCreate))
            {
                foreach (var user in _master.UserService.Users)
                {
                    xmlSerializer.Serialize(fs, user);
                }
            }

            if (Logging)
            {
                Trace.WriteLine("Successfully serialized!");
                Trace.WriteLine("-----------------------------\n");
            }
        }
    }
}

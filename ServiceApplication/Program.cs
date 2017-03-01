using System;
using UserServiceLibrary;

namespace ServiceApplication
{
    class Program
    {
        public static int StartId = 2000;
       
        static void Main(string[] args)
        {
            try
            {
                User user1 = new User
                {
                    FirstName = "Alice",
                    LastName = "Smith",
                    DateOfBirth = new DateTime(1980, 12, 8)
                };
                User user2 = new User
                {
                    FirstName = "Alice",
                    LastName = "Cooper",
                    DateOfBirth = new DateTime(1960, 5, 14)
                };
                User user3 = new User
                {
                    FirstName = "John",
                    LastName = "Smith",
                    DateOfBirth = new DateTime(1991, 5, 28)
                };
                User user4 = new User
                {
                    FirstName = "John",
                    LastName = "Lennon",
                    DateOfBirth = new DateTime(1980, 12, 8)
                };

                UserService userService = new UserService(IdGenerator);
                DomainService domainService = new DomainService(userService);

                domainService.Add(user1);
                domainService.Add(user2);
                domainService.Add(user3);
                domainService.Add(user4);

                domainService.Remove(user4);

                Predicate<User> predicateSearch = u => u.LastName == user1.LastName;
                var usersFromMaster = domainService.SearchMaster(predicateSearch);
                var usersFromSlave = domainService.SearchSlave(predicateSearch);

                domainService.Serialize();

                domainService.Dispose();

                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// An ID generator
        /// </summary>
        /// <returns>Returns the next ID</returns>
        private static int IdGenerator()
        {
            return StartId++;
        }
    }
}

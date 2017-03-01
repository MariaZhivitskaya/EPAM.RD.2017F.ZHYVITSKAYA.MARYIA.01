using System;
using System.Xml.Serialization;

namespace UserServiceLibrary
{
    /// <summary>
    /// Creates a user
    /// </summary>
    [Serializable]
    public class User
    {
        /// <summary>
        /// An id
        /// </summary>
        [XmlElement("Id")]
        public int Id { get; set; }

        /// <summary>
        /// A first name
        /// </summary>
        [XmlElement("First_name")]
        public string FirstName { get; set; }

        /// <summary>
        /// A last name
        /// </summary>
        [XmlElement("Last_name")]
        public string LastName { get; set; }

        /// <summary>
        /// A date of birth
        /// </summary>
        [XmlElement("Date_of_birth", DataType = "date")]
        public DateTime DateOfBirth { get; set; }
    }
}

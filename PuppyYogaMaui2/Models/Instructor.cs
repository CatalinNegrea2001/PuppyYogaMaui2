using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PuppyYogaMaui2.Data;
using SQLiteNetExtensions.Attributes;
namespace PuppyYogaMaui2.Models
{
    public class Instructor
    {
        [PrimaryKey, AutoIncrement]
        public int InstructorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Bio { get; set; }

        public string FullName => $"{FirstName} {LastName}";
    }
}

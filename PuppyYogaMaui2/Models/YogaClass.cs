using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using SQLite;
using SQLiteNetExtensions.Attributes;
namespace PuppyYogaMaui2.Models
{
    [Preserve(AllMembers = true)]

    public class YogaClass
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime ScheduleTime { get; set; }
        public DateTime ScheduleDate { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public double Price { get; set; }
        public int MaxCapacity { get; set; }

        [SQLiteNetExtensions.Attributes.ForeignKey(typeof(Instructor))]
        public int InstructorId { get; set; }
        [Ignore]
        public string InstructorName { get; set; }

    }
}


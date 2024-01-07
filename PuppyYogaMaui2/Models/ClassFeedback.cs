using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuppyYogaMaui2.Models
{
    public class ClassFeedback
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [ForeignKey(typeof(YogaClass))]
        public int YogaClassId { get; set; }

        public int Rating { get; set; } // De exemplu, 1-5
        public string Comment { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
    }

}

using SQLite;
using System.Text.RegularExpressions;

namespace PuppyYogaMaui2.Models
{
    public class Reservation
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Indexed] // Optional, improves lookup performance on foreign keys.
        public int YogaClassId { get; set; }

        [NotNull]
        public string CustomerName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        [Ignore]
        public string ClassName { get; set; }
        [Ignore]
        public DateTime ClassDate { get; set; }
        [Ignore]
        public DateTime ClassTime { get; set; }
    }
}

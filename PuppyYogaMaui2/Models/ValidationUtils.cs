using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace PuppyYogaMaui2.Models
{
    public static class ValidationUtils
    {
        public static bool IsValidPhoneNumber(string number)
        {
            return Regex.IsMatch(number, @"^0\d{9}$");
        }

        public static bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email, @"^[^@\s]+@[^\s]+\.[^\s]+$");
        }
    }
}

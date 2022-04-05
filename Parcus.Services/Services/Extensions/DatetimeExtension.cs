using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcus.Services.Services.Extensions
{
    public static class DatetimeExtension
    {
        public static DateTime ParseGivenTime(this DateTime dateTime, string datetime)
        {
            DateTime Date;
            try
            {
                Date = DateTime.ParseExact(datetime,
                    "yyyy-MM-dd HH:mm:ss,fff",
                    System.Globalization.CultureInfo.InvariantCulture);
                return Date;    
            }
            catch (FormatException)
            {
                return dateTime;
            }
        }
    }
    
}

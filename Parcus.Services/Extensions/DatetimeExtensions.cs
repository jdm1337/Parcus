
namespace Parcus.Services.Extensions
{
    public static class DatetimeExtensions
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoMauiBlazor.Tools
{
    internal static class StringFormat
    {
        /// <summary>
        /// converts the send datetime to a string in format 'd.M.yyyy h:mm'. Adds a leading zero for minute.
        /// </summary>
        /// <param name="date">datetime to convert to string</param>
        /// <returns>datetime as a formatted string</returns>
        public static string ToDateTimeString(this DateTime date)
        {
            if (date == new DateTime())
            {
                return "";
            }
            string formattedDate = string.Empty;
            string leadingZero = (date.Minute < 10) ? "0" : string.Empty;

            formattedDate += $"{date.Day}.{date.Month}.{date.Year} ";
            formattedDate += $"{date.Hour}:{leadingZero}{date.Minute}";

            return formattedDate;
        }
    }
}

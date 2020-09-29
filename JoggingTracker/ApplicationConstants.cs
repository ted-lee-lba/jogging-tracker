using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Globalization;

namespace JoggingTracker {
    public static class ApplicationConstants {
        public static string PasswordSalt {
            get {
                return ConfigurationManager.AppSettings["PasswordSalt"] ?? string.Empty;
            }
        }
    }

    public static class DateTimeExtension {
        public static int GetWeekNumber(this DateTime Value) {
            CultureInfo ciCurr = CultureInfo.InvariantCulture;
            int weekNum = ciCurr.Calendar.GetWeekOfYear(Value, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Sunday);
            return Convert.ToInt32(string.Format("{0:0000}{1:00}", Value.Year, weekNum));
        }

        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek) {
            int diff = dt.DayOfWeek - startOfWeek;
            if (diff < 0) {
                diff += 7;
            }
            return dt.AddDays(-1 * diff).Date;
        }

        public static DateTime EndOfWeek(this DateTime dt, DayOfWeek EndOfWeek) {
            int diff = EndOfWeek - dt.DayOfWeek;
            if (diff < 0) {
                diff += 7;
            }
            return dt.AddDays(diff).Date;
        }
    }
}
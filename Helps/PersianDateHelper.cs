using System.Globalization;

namespace Helps
{
    public static class PersianDateHelper
    {
        private static readonly PersianCalendar PersianCalendar = new PersianCalendar();

        /// <summary>
        /// تبدیل تاریخ میلادی به تاریخ شمسی با فرمت پیشفرض
        /// </summary>
        public static string ToPersianDate(this DateTime dateTime)
        {
            try
            {
                var year = PersianCalendar.GetYear(dateTime);
                var month = PersianCalendar.GetMonth(dateTime);
                var day = PersianCalendar.GetDayOfMonth(dateTime);

                return $"{year:0000}/{month:00}/{day:00}";
            }
            catch
            {
                return "تاریخ نامعتبر";
            }
        }

        /// <summary>
        /// تبدیل تاریخ میلادی به تاریخ شمسی با فرمت دلخواه
        /// </summary>
        public static string ToPersianDate(this DateTime dateTime, string format)
        {
            try
            {
                var year = PersianCalendar.GetYear(dateTime);
                var month = PersianCalendar.GetMonth(dateTime);
                var day = PersianCalendar.GetDayOfMonth(dateTime);
                var hour = PersianCalendar.GetHour(dateTime);
                var minute = PersianCalendar.GetMinute(dateTime);
                var second = PersianCalendar.GetSecond(dateTime);

                return format
                    .Replace("yyyy", year.ToString("0000"))
                    .Replace("yy", (year % 100).ToString("00"))
                    .Replace("MM", month.ToString("00"))
                    .Replace("M", month.ToString())
                    .Replace("dd", day.ToString("00"))
                    .Replace("d", day.ToString())
                    .Replace("HH", hour.ToString("00"))
                    .Replace("H", hour.ToString())
                    .Replace("mm", minute.ToString("00"))
                    .Replace("m", minute.ToString())
                    .Replace("ss", second.ToString("00"))
                    .Replace("s", second.ToString());
            }
            catch
            {
                return "تاریخ نامعتبر";
            }
        }

        /// <summary>
        /// تبدیل تاریخ میلادی به تاریخ شمسی با ساعت
        /// </summary>
        public static string ToPersianDateTime(this DateTime dateTime)
        {
            try
            {
                var year = PersianCalendar.GetYear(dateTime);
                var month = PersianCalendar.GetMonth(dateTime);
                var day = PersianCalendar.GetDayOfMonth(dateTime);
                var hour = PersianCalendar.GetHour(dateTime);
                var minute = PersianCalendar.GetMinute(dateTime);

                return $"{year:0000}/{month:00}/{day:00} {hour:00}:{minute:00}";
            }
            catch
            {
                return "تاریخ نامعتبر";
            }
        }

        /// <summary>
        /// تبدیل تاریخ میلادی به متن فارسی (مثلاً: ۲۵ دی ۱۴۰۲)
        /// </summary>
        public static string ToPersianDateString(this DateTime dateTime)
        {
            try
            {
                var year = PersianCalendar.GetYear(dateTime);
                var month = PersianCalendar.GetMonth(dateTime);
                var day = PersianCalendar.GetDayOfMonth(dateTime);

                var monthName = GetPersianMonthName(month);

                return $"{ToPersianNumber(day.ToString())} {monthName} {ToPersianNumber(year.ToString())}";
            }
            catch
            {
                return "تاریخ نامعتبر";
            }
        }

        /// <summary>
        /// تبدیل تاریخ میلادی به متن فارسی با ساعت
        /// </summary>
        public static string ToPersianDateTimeString(this DateTime dateTime)
        {
            try
            {
                var year = PersianCalendar.GetYear(dateTime);
                var month = PersianCalendar.GetMonth(dateTime);
                var day = PersianCalendar.GetDayOfMonth(dateTime);
                var hour = PersianCalendar.GetHour(dateTime);
                var minute = PersianCalendar.GetMinute(dateTime);

                var monthName = GetPersianMonthName(month);

                return $"{ToPersianNumber(day.ToString())} {monthName} {ToPersianNumber(year.ToString())} - {ToPersianNumber(hour.ToString("00"))}:{ToPersianNumber(minute.ToString("00"))}";
            }
            catch
            {
                return "تاریخ نامعتبر";
            }
        }

        /// <summary>
        /// تبدیل اعداد انگلیسی به فارسی
        /// </summary>
        public static string ToPersianNumber(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            var persianNumbers = new[] { '۰', '۱', '۲', '۳', '۴', '۵', '۶', '۷', '۸', '۹' };
            var englishNumbers = new[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

            for (var i = 0; i < englishNumbers.Length; i++)
            {
                input = input.Replace(englishNumbers[i], persianNumbers[i]);
            }

            return input;
        }

        /// <summary>
        /// نام ماه شمسی
        /// </summary>
        private static string GetPersianMonthName(int month)
        {
            return month switch
            {
                1 => "فروردین",
                2 => "اردیبهشت",
                3 => "خرداد",
                4 => "تیر",
                5 => "مرداد",
                6 => "شهریور",
                7 => "مهر",
                8 => "آبان",
                9 => "آذر",
                10 => "دی",
                11 => "بهمن",
                12 => "اسفند",
                _ => "نامعتبر"
            };
        }

        /// <summary>
        /// بررسی معتبر بودن تاریخ شمسی
        /// </summary>
        public static bool IsValidPersianDate(int year, int month, int day)
        {
            try
            {
                if (year < 1 || year > 9999)
                    return false;
                if (month < 1 || month > 12)
                    return false;
                if (day < 1 || day > 31)
                    return false;

                // بررسی روزهای ماه
                if (month <= 6 && day > 31)
                    return false;
                if (month > 6 && month <= 12 && day > 30)
                    return false;
                if (month == 12 && day > 29)
                {
                    // بررسی سال کبیسه
                    if (!IsLeapYear(year) && day > 29)
                        return false;
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// بررسی سال کبیسه شمسی
        /// </summary>
        private static bool IsLeapYear(int year)
        {
            // الگوریتم ساده برای تشخیص سال کبیسه شمسی
            // سال کبیسه در تقویم هجری شمسی در چرخه‌های 33 ساله اتفاق می‌افتد
            var leapYears = new[] { 1, 5, 9, 13, 17, 22, 26, 30 };
            var remainder = year % 33;
            return leapYears.Contains(remainder);
        }

        /// <summary>
        /// تبدیل تاریخ شمسی به میلادی
        /// </summary>
        public static DateTime ToGregorianDate(int persianYear, int persianMonth, int persianDay)
        {
            try
            {
                return PersianCalendar.ToDateTime(persianYear, persianMonth, persianDay, 0, 0, 0, 0);
            }
            catch
            {
                throw new ArgumentException("تاریخ شمسی نامعتبر است");
            }
        }
    }

    public static class NullableDateTimeExtensions
    {
        /// <summary>
        /// اکستنشن متد برای DateTime?
        /// </summary>
        public static string ToPersianDate(this DateTime? dateTime)
        {
            return dateTime.HasValue ? dateTime.Value.ToPersianDate() : "تعیین نشده";
        }

        /// <summary>
        /// اکستنشن متد برای DateTime? با فرمت دلخواه
        /// </summary>
        public static string ToPersianDate(this DateTime? dateTime, string format)
        {
            return dateTime.HasValue ? dateTime.Value.ToPersianDate(format) : "تعیین نشده";
        }

        /// <summary>
        /// اکستنشن متد برای DateTime? با ساعت
        /// </summary>
        public static string ToPersianDateTime(this DateTime? dateTime)
        {
            return dateTime.HasValue ? dateTime.Value.ToPersianDateTime() : "تعیین نشده";
        }

        /// <summary>
        /// اکستنشن متد برای DateTime? به متن فارسی
        /// </summary>
        public static string ToPersianDateString(this DateTime? dateTime)
        {
            return dateTime.HasValue ? dateTime.Value.ToPersianDateString() : "تعیین نشده";
        }

        /// <summary>
        /// اکستنشن متد برای DateTime? به متن فارسی با ساعت
        /// </summary>
        public static string ToPersianDateTimeString(this DateTime? dateTime)
        {
            return dateTime.HasValue ? dateTime.Value.ToPersianDateTimeString() : "تعیین نشده";
        }
    }
}
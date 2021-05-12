using System;
using System.Globalization;

namespace API
{
    public static class Helper
    {
        public static string GetCronExpression(string time, byte days)
        {
            if (!TimeSpan.TryParse(time, CultureInfo.InvariantCulture, out TimeSpan newTime))
            {
                return null;
            }

            if (days is 0 or 255)
            {
                return null;
            }

            string result = $"{newTime.Minutes} {newTime.Hours} * * ";
            int position = 1;
            while (days != 0)
            {
                if (days % 2 == 1)
                {
                    result += $"{position},";
                }

                position++;
                days /= 2;
            }

            result = result.Remove(result.Length - 1, 1);
            return result;
        }
    }
}
using System;

namespace API
{
    public static class Helper
    {
        public static string GetCronExpression(TimeSpan time, byte days)
        {
            if (days is 0 or 255)
            {
                return null;
            }

            string result = $"{time.Minutes} {time.Hours} * * ";
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
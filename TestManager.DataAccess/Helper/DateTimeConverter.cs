namespace TestManager.DataAccess.Helper
{
    public static class DateTimeConverter
    {
        public static DateTime ConvertTimeToRequiredTimeZone(string timeZone)
        {
            DateTimeOffset convertedTime = DateTimeOffset.UtcNow;

            switch (timeZone)
            {
                case "AST":
                    {
                        // Canada/Atlantic "now" (handles DST; OS-aware ID)
                        convertedTime =
                            TimeZoneInfo.ConvertTimeBySystemTimeZoneId(
                                DateTimeOffset.UtcNow,
                                OperatingSystem.IsWindows() ? "Atlantic Standard Time" : "America/Halifax"
                            );
                        break;
                    }
                case "NST":
                    {
                        // Canada/ST_Johns "now" (handles DST; OS-aware ID)
                        convertedTime =
                            TimeZoneInfo.ConvertTimeBySystemTimeZoneId(
                                DateTimeOffset.UtcNow,
                                OperatingSystem.IsWindows() ? "Newfoundland Standard Time" : "America/St_Johns"
                            );
                        break;
                    }
                case "EST":
                    {
                        // Toronto/Eastern "now" (handles DST; OS-aware ID)
                        convertedTime =
                            TimeZoneInfo.ConvertTimeBySystemTimeZoneId(
                                DateTimeOffset.UtcNow,
                                OperatingSystem.IsWindows() ? "Eastern Standard Time" : "America/Toronto"
                            );
                        break;
                    }
                case "CST":
                    {
                        // America/Chicago "now" (handles DST; OS-aware ID)
                        convertedTime =
                            TimeZoneInfo.ConvertTimeBySystemTimeZoneId(
                                DateTimeOffset.UtcNow,
                                OperatingSystem.IsWindows() ? "Central Standard Time" : "America/Chicago"
                            );
                        break;
                    }
                case "MST":
                    {
                        // America/Chicago "now" (handles DST; OS-aware ID)
                        convertedTime =
                            TimeZoneInfo.ConvertTimeBySystemTimeZoneId(
                                DateTimeOffset.UtcNow,
                                OperatingSystem.IsWindows() ? "Mountain Standard Time" : "America/Denver"
                            );
                        break;
                    }
                case "PST":
                    {
                        // America/Los Angeles "now" (handles DST; OS-aware ID)
                        convertedTime =
                            TimeZoneInfo.ConvertTimeBySystemTimeZoneId(
                                DateTimeOffset.UtcNow,
                                OperatingSystem.IsWindows() ? "Pacific Standard Time" : "America/Los_Angeles"
                            );
                        break;
                    }
            }

            return convertedTime.DateTime;
        }
    }
}



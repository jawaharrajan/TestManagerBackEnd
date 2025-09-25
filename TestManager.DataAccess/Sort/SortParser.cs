namespace TestManager.DataAccess.Sort
{
    public static class SortParser
    {
        public static List<(TEnum Field, bool Descending)> Parse<TEnum>(string? sortString)
            where TEnum : struct, System.Enum
        {
            var result = new List<(TEnum, bool)>();

            if (string.IsNullOrWhiteSpace(sortString))
                return result;

            foreach (var part in sortString.Split(',', StringSplitOptions.RemoveEmptyEntries))
            {
                //var split = part.Trim().Split(':');
                var tokens = part.Split(':', StringSplitOptions.RemoveEmptyEntries);
                //var key = split[0].Trim();
                var fieldString = tokens[0].Trim();
                //var direction = split.Length > 1 ? split[1].Trim().ToLower() : "asc";
                var direction = tokens.Length > 1 ? tokens[1].Trim().ToLower() : "asc";

                if (System.Enum.TryParse<TEnum>(fieldString, ignoreCase: true, out var field))
                {
                    result.Add((field, direction == "desc"));
                }
            }

            return result;
        }
    }

}

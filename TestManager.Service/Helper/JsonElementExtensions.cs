using System.Text.Json;

namespace TestManager.Service.Helper
{
    public static class JsonElementExtensions
    {
        public static Dictionary<string, object?> ToDictionary(this JsonElement element)
        {
            var dict = new Dictionary<string, object?>();

            foreach (var prop in element.EnumerateObject())
            {
                dict[prop.Name] = ConvertElement(prop.Value);
            }

            return dict;
        }

        private static object? ConvertElement(JsonElement value)
        {
            return value.ValueKind switch
            {
                JsonValueKind.Object => value.ToDictionary(),
                JsonValueKind.Array => value.EnumerateArray().Select(ConvertElement).ToList(),
                JsonValueKind.String => value.GetString(),
                JsonValueKind.Number => value.TryGetInt64(out var l) ? l : value.GetDouble(),
                JsonValueKind.True => true,
                JsonValueKind.False => false,
                JsonValueKind.Null => null,
                _ => value.GetRawText()
            };
        }
    }

}

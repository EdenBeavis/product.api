using Newtonsoft.Json;

namespace product.api.test.Infrastructure
{
    public static class JsonConvertExtension
    {
        public static bool TryParseJson<T>(this string jsonString, out T result)
        {
            bool success = true;
            var settings = new JsonSerializerSettings
            {
                Error = (sender, args) => { success = false; args.ErrorContext.Handled = true; },
                MissingMemberHandling = MissingMemberHandling.Error
            };

            try
            {
                result = JsonConvert.DeserializeObject<T>(jsonString, settings);
            }
            catch
            {
                result = default;
                return false;
            }

            return success;
        }
    }
}
namespace Tanka.WebTests
{
    using global::Nancy.Testing;
    using Newtonsoft.Json;

    public static class JsonNetExtensions
    {
        public static T ToObject<T>(this BrowserResponse response)
        {
            return JsonConvert.DeserializeObject<T>(response.Body.AsString());
        }
    }
}
using Newtonsoft.Json;

namespace Api.ExtensionMethods;

public static class HttpResponseExtensionMethods
{
    public static void AddPaginationHeaders(this HttpResponse value, object meta)
    {
        value.Headers.Append("X-Pagination", JsonConvert.SerializeObject(meta));
    }
}

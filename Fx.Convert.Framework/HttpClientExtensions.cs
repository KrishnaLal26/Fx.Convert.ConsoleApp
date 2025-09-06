using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Fx.Convert.Framework
{
    public static class HttpResponseMessageExtensions
    {
        public static async Task<Response<TData>> GetAsync<TData>(this HttpClient httpClient, string url, CancellationToken cancellationToken = default)
            where TData : class
        {
            var httpResponse = await httpClient.GetAsync(url, cancellationToken);
            return await httpResponse.GetAsync<TData>();
        }
        private static async Task<Response<TData>> GetAsync<TData>(this HttpResponseMessage httpResponse)
            where TData : class
        {
            Response<TData> response = null;
            using (Stream contentStream = await httpResponse.Content.ReadAsStreamAsync())
            {
                if (contentStream != null && contentStream.CanRead)
                {
                    using (StreamReader reader = new StreamReader(contentStream))
                    {
                        if(httpResponse.IsSuccessStatusCode)
                        {
                            response = await GetResponse<TData>(reader, httpResponse);
                        }
                        else
                        {
                            var errorMsg = await reader.ReadToEndAsync();
                            response = new Response<TData>
                            {
                                Data = null,
                                ResponseCode = httpResponse.StatusCode,
                                ErrorMessage = errorMsg,
                                ResponseHeaders = httpResponse.Headers.ToDictionary(h => h.Key, h => string.Join(";", h.Value))
                            };
                        }
                    }
                }
            }
            response.ResponseHeaders = httpResponse.Headers.ToDictionary(h => h.Key, h => string.Join(";", h.Value));
            return response;
        }

        private static async Task<Response<TData>> GetResponse<TData>(StreamReader streamReader, HttpResponseMessage httpResponseMessage)
            where TData : class
        {
            var stramContent = await streamReader.ReadToEndAsync();
            TData content = JsonSerializer.Deserialize<TData>(stramContent);
            return new Response<TData>
            {
                Data = content,
                ResponseCode = httpResponseMessage.StatusCode,
            };
        }
    }
}

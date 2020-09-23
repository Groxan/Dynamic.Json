using System;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net.Http;
using System.Threading;

namespace DJson.Extensions
{
    public static class HttpClientExtension
    {
        /// <summary>
        /// Send a GET request to the specified Uri and return the response body as a dynamic JSON.
        /// </summary>
        /// <param name="client">Http client.</param>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="options">Options to control the behavior during reading.</param>
        /// <param name="cancellationToken">A token that may be used to cansel the read operation.</param>
        /// <returns></returns>
        public static async Task<dynamic> GetJsonAsync(
            this HttpClient client,
            string requestUri,
            JsonSerializerOptions options = null,
            CancellationToken cancellationToken = default)
        {
            using (var stream = await client.GetStreamAsync(requestUri))
            {
                return await DJson.ParseAsync(stream, options, cancellationToken);
            }
        }

        /// <summary>
        /// Send a GET request to the specified Uri and return the response body as a dynamic JSON.
        /// </summary>
        /// <param name="client">Http client.</param>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="options">Options to control the behavior during reading.</param>
        /// <param name="cancellationToken">A token that may be used to cansel the read operation.</param>
        /// <returns></returns>
        public static async Task<dynamic> GetJsonAsync(
            this HttpClient client,
            Uri requestUri,
            JsonSerializerOptions options = null,
            CancellationToken cancellationToken = default)
        {
            using (var stream = await client.GetStreamAsync(requestUri))
            {
                return await DJson.ParseAsync(stream, options, cancellationToken);
            }
        }

        /// <summary>
        /// Send a GET request to the specified Uri and return the response body deserialized to the specified type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="client">Http client.</param>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="options">Options to control the behavior during reading.</param>
        /// <param name="cancellationToken">A token that may be used to cansel the read operation.</param>
        /// <returns></returns>
        public static async Task<T> GetJsonAsync<T>(
            this HttpClient client,
            string requestUri,
            JsonSerializerOptions options = null,
            CancellationToken cancellationToken = default)
        {
            using (var stream = await client.GetStreamAsync(requestUri))
            {
                return await JsonSerializer.DeserializeAsync<T>(stream, options, cancellationToken);
            }
        }

        /// <summary>
        /// Send a GET request to the specified Uri and return the response body deserialized to the specified type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="client">Http client.</param>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="options">Options to control the behavior during reading.</param>
        /// <param name="cancellationToken">A token that may be used to cansel the read operation.</param>
        /// <returns></returns>
        public static async Task<T> GetJsonAsync<T>(
            this HttpClient client,
            Uri requestUri,
            JsonSerializerOptions options = null,
            CancellationToken cancellationToken = default)
        {
            using (var stream = await client.GetStreamAsync(requestUri))
            {
                return await JsonSerializer.DeserializeAsync<T>(stream, options, cancellationToken);
            }
        }
    }
}

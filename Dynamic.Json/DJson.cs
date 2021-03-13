using System;
using System.Buffers;
using System.Dynamic;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Dynamic.Json
{
    /// <summary>
    /// Dynamic JSON base class.
    /// Provides static factory methods.
    /// </summary>
    public abstract class DJson : DynamicObject
    {
        protected readonly JsonElement Element;
        protected readonly JsonSerializerOptions Options;
 
        protected DJson(JsonElement element, JsonSerializerOptions options)
        {
            Element = element;
            Options = options;
        }

        /// <summary>
        /// Return raw JSON
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Element.ValueKind == JsonValueKind.String
                ? Element.GetString()
                : Element.GetRawText();
        }

        #region create
        public static DJson Create(JsonElement element, JsonSerializerOptions options = null)
        {
            switch (element.ValueKind)
            {
                case JsonValueKind.Undefined:
                case JsonValueKind.Null:
                    return null;
                case JsonValueKind.Object:
                    return new DJsonObject(element, options);
                case JsonValueKind.Array:
                    return new DJsonArray(element, options);
                default:
                    return new DJsonValue(element, options);
            }
        }
        #endregion

        #region parse
        /// <summary>
        /// Parse text representing a single JSON string value into dynamic JSON.
        /// </summary>
        /// <param name="json">The JSON text to parse</param>
        /// <param name="options">Options to control the behavior during reading.</param>
        /// <returns></returns>
        public static dynamic Parse(string json, JsonSerializerOptions options = null)
        {
            if (json == null)
                throw new ArgumentNullException(nameof(json));

            using (var doc = JsonDocument.Parse(json, GetDocumentOptions(options)))
            {
                return Create(doc.RootElement.Clone(), options);
            }
        }

        /// <summary>
        /// Parse a Stream as UTF-8-encoded data representing a single JSON value into dynamic JSON. The stream is read to completion.
        /// </summary>
        /// <param name="json">The JSON data to parse</param>
        /// <param name="options">Options to control the behavior during reading.</param>
        /// <returns></returns>
        public static dynamic Parse(Stream json, JsonSerializerOptions options = null)
        {
            if (json == null)
                throw new ArgumentNullException(nameof(json));

            using (var doc = JsonDocument.Parse(json, GetDocumentOptions(options)))
            {
                return Create(doc.RootElement.Clone(), options);
            }
        }

        /// <summary>
        /// Parse memory as UTF-8-encoded text representing a single JSON byte value into dynamic JSON.
        /// </summary>
        /// <param name="json">The JSON text to parse</param>
        /// <param name="options">Options to control the behavior during reading.</param>
        /// <returns></returns>
        public static dynamic Parse(ReadOnlyMemory<byte> json, JsonSerializerOptions options = null)
        {
            using (var doc = JsonDocument.Parse(json, GetDocumentOptions(options)))
            {
                return Create(doc.RootElement.Clone(), options);
            }
        }

        /// <summary>
        /// Parse text representing a single JSON character value into dynamic JSON.
        /// </summary>
        /// <param name="json">The JSON text to parse</param>
        /// <param name="options">Options to control the behavior during reading.</param>
        /// <returns></returns>
        public static dynamic Parse(ReadOnlyMemory<char> json, JsonSerializerOptions options = null)
        {
            using (var doc = JsonDocument.Parse(json, GetDocumentOptions(options)))
            {
                return Create(doc.RootElement.Clone(), options);
            }
        }

        /// <summary>
        /// Parse a sequence as UTF-8-encoded text representing a single JSON byte value into dynamic JSON.
        /// </summary>
        /// <param name="json">The JSON text to parse</param>
        /// <param name="options">Options to control the behavior during reading.</param>
        /// <returns></returns>
        public static dynamic Parse(ReadOnlySequence<byte> json, JsonSerializerOptions options = null)
        {
            using (var doc = JsonDocument.Parse(json, GetDocumentOptions(options)))
            {
                return Create(doc.RootElement.Clone(), options);
            }
        }

        /// <summary>
        /// Parse a Stream as UTF-8-encoded data representing a single JSON value into dynamic JSON. The stream is read to completion.
        /// </summary>
        /// <param name="json">The JSON data to parse</param>
        /// <param name="options">Options to control the behavior during reading.</param>
        /// <param name="cancellationToken">A token that may be used to cansel the read operation.</param>
        /// <returns></returns>
        public static async Task<dynamic> ParseAsync(
            Stream json,
            JsonSerializerOptions options = null,
            CancellationToken cancellationToken = default)
        {
            if (json == null)
                throw new ArgumentNullException(nameof(json));

            using (var doc = await JsonDocument.ParseAsync(json, GetDocumentOptions(options), cancellationToken))
            {
                return Create(doc.RootElement.Clone(), options);
            }
        }
        #endregion

        #region read
        /// <summary>
        /// Read a file at the specified path as a dynamic JSON.
        /// </summary>
        /// <param name="file">A relative or absolute path to the file.</param>
        /// <param name="options">Options to control the behavior during reading.</param>
        /// <returns></returns>
        public static dynamic Read(string file, JsonSerializerOptions options = null)
        {
            if (file == null)
                throw new ArgumentNullException(nameof(file));

            using (var stream = new FileStream(file, FileMode.Open))
            using (var doc = JsonDocument.Parse(stream, GetDocumentOptions(options)))
            {
                return Create(doc.RootElement.Clone(), options);
            }
        }

        /// <summary>
        /// Read a file at the specified path as a dynamic JSON.
        /// </summary>
        /// <param name="file">A relative or absolute path to the file.</param>
        /// <param name="options">Options to control the behavior during reading.</param>
        /// <param name="cancellationToken">A token that may be used to cansel the read operation.</param>
        /// <returns></returns>
        public static async Task<dynamic> ReadAsync(
            string file,
            JsonSerializerOptions options = null,
            CancellationToken cancellationToken = default)
        {
            if (file == null)
                throw new ArgumentNullException(nameof(file));

            using (var stream = new FileStream(file, FileMode.Open))
            using (var doc = await JsonDocument.ParseAsync(stream, GetDocumentOptions(options), cancellationToken))
            {
                return Create(doc.RootElement.Clone(), options);
            }
        }
        #endregion

        #region get
        /// <summary>
        /// Send a GET request to the specified Uri and return the response body as a dynamic JSON.
        /// </summary>
        /// <param name="uri">The Uri the request is sent to.</param>
        /// <param name="options">Options to control the behavior during reading.</param>
        /// <param name="cancellationToken">A token that may be used to cansel the read operation.</param>
        /// <returns></returns>
        public static async Task<dynamic> GetAsync(
            string uri,
            JsonSerializerOptions options = null,
            CancellationToken cancellationToken = default)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            using (var client = new HttpClient())
            using (var stream = await client.GetStreamAsync(uri))
            using (var doc = await JsonDocument.ParseAsync(stream, GetDocumentOptions(options), cancellationToken))
            {
                return Create(doc.RootElement.Clone(), options);
            }
        }

        /// <summary>
        /// Send a GET request to the specified Uri and return the response body as a dynamic JSON.
        /// </summary>
        /// <param name="uri">The Uri the request is sent to.</param>
        /// <param name="options">Options to control the behavior during reading.</param>
        /// <param name="cancellationToken">A token that may be used to cansel the read operation.</param>
        /// <returns></returns>
        public static async Task<dynamic> GetAsync(
            Uri uri,
            JsonSerializerOptions options = null,
            CancellationToken cancellationToken = default)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            using (var client = new HttpClient())
            using (var stream = await client.GetStreamAsync(uri))
            using (var doc = await JsonDocument.ParseAsync(stream, GetDocumentOptions(options), cancellationToken))
            {
                return Create(doc.RootElement.Clone(), options);
            }
        }
        #endregion

        private static JsonDocumentOptions GetDocumentOptions(JsonSerializerOptions options)
        {
            return options == null ? default : new JsonDocumentOptions
            {
                AllowTrailingCommas = options.AllowTrailingCommas,
                CommentHandling = options.ReadCommentHandling,
                MaxDepth = options.MaxDepth
            };
        }
    }
}

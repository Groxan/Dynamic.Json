using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text.Json;

namespace Dynamic.Json
{
    public sealed class DJsonObject : DJson, IEnumerable<DJsonProperty>
    {
        internal DJsonObject(JsonElement element, JsonSerializerOptions options) : base(element, options) { }

        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return Element.EnumerateObject().Select(x => x.Name);
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            return TryGetMember(binder.Name, out result);
        }

        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
        {
            if (indexes.Length != 1 || !(indexes[0] is string name))
                throw new NotImplementedException();

            return TryGetMember(name, out result);
        }

        public override bool TryConvert(ConvertBinder binder, out object result)
        {
            try
            {
                result = JsonSerializer.Deserialize(Element.GetRawText(), binder.Type, Options);
                return true;
            }
            catch (JsonException ex)
            {
                throw new FormatException($"Unable to convert json object to {binder.Type.Name}", ex);
            }
        }

        public IEnumerator GetEnumerator() => Element.EnumerateObject()
            .Select(x => new DJsonProperty(x.Name, Create(x.Value, Options)))
            .GetEnumerator();

        IEnumerator<DJsonProperty> IEnumerable<DJsonProperty>.GetEnumerator() => Element.EnumerateObject()
            .Select(x => new DJsonProperty(x.Name, Create(x.Value, Options)))
            .GetEnumerator();

        bool TryGetMember(string name, out object result)
        {
            if (!Element.TryGetProperty(name, out var el) && IsPascalCase(name))
                if (!Element.TryGetProperty(PascalToSnakeCase(name), out el))
                    if (!Element.TryGetProperty(PascalToCamelCase(name), out el))
                    {
                        result = null;
                        return true;
                    }

            result = Create(el, Options);
            return true;
        }

        #region conventions
        bool IsPascalCase(string value)
        {
            return value[0] <= 90 && value[0] >= 65;
        }

        string PascalToCamelCase(string value)
        {
            var buf = new List<char>(value.Length);
            var abbreviation = 0;

            for (int i = 0; i < value.Length; i++)
            {
                if (value[i] <= 90 && value[i] >= 65)
                {
                    abbreviation++;
                    buf.Add(i == 0 || abbreviation > 1
                        ? (char)(value[i] + 32)
                        : value[i]);
                }
                else
                {
                    if (abbreviation > 1)
                        buf[buf.Count - 1] = (char)(buf[buf.Count - 1] - 32);

                    abbreviation = 0;
                    buf.Add(value[i]);
                }
            }

            return new string(buf.ToArray());
        }

        string PascalToSnakeCase(string value)
        {
            var buf = new List<char>();
            var abbreviation = 0;

            for (int i = 0; i < value.Length; i++)
            {
                if (value[i] <= 90 && value[i] >= 65)
                {
                    if (abbreviation == 0 && i > 0)
                        buf.Add('_');

                    abbreviation++;
                    buf.Add((char)(value[i] + 32));
                }
                else
                {
                    if (abbreviation > 1)
                        buf.Insert(buf.Count - 1, '_');

                    abbreviation = 0;
                    buf.Add(value[i]);
                }
            }

            return new string(buf.ToArray());
        }
        #endregion
    }
}

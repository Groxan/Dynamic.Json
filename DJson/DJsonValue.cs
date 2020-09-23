using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text.Json;

namespace DJson
{
    public sealed class DJsonValue : DJson
    {
        internal DJsonValue(JsonElement element, JsonSerializerOptions options) : base(element, options) { }

        public override bool TryConvert(ConvertBinder binder, out object result)
        {
            if (!Extractors.TryGetValue(binder.Type, out var extract) || !extract(Element, out result))
                throw new FormatException($"Unable to convert {Element.GetRawText()} to {binder.Type.Name}");

            return true;
        }

        #region extractors
        delegate bool Extractor(JsonElement element, out object result);

        static readonly Dictionary<Type, Extractor> Extractors = new Dictionary<Type, Extractor>
        {
            [typeof(bool)] = ExtractBool,
            [typeof(bool?)] = ExtractBool,
            [typeof(sbyte)] = ExtractSByte,
            [typeof(sbyte?)] = ExtractSByte,
            [typeof(byte)] = ExtractByte,
            [typeof(byte?)] = ExtractByte,
            [typeof(short)] = ExtractInt16,
            [typeof(short?)] = ExtractInt16,
            [typeof(ushort)] = ExtractUInt16,
            [typeof(ushort?)] = ExtractUInt16,
            [typeof(int)] = ExtractInt32,
            [typeof(int?)] = ExtractInt32,
            [typeof(uint)] = ExtractUInt32,
            [typeof(uint?)] = ExtractUInt32,
            [typeof(long)] = ExtractInt64,
            [typeof(long?)] = ExtractInt64,
            [typeof(ulong)] = ExtractUInt64,
            [typeof(ulong?)] = ExtractUInt64,
            [typeof(float)] = ExtractFloat,
            [typeof(float?)] = ExtractFloat,
            [typeof(double)] = ExtractDouble,
            [typeof(double?)] = ExtractDouble,
            [typeof(decimal)] = ExtractDecimal,
            [typeof(decimal?)] = ExtractDecimal,
            [typeof(Guid)] = ExtractGuid,
            [typeof(Guid?)] = ExtractGuid,
            [typeof(DateTime)] = ExtractDateTime,
            [typeof(DateTime?)] = ExtractDateTime,
            [typeof(DateTimeOffset)] = ExtractDateTimeOffset,
            [typeof(DateTimeOffset?)] = ExtractDateTimeOffset,
            [typeof(string)] = ExtractString
        };

        static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Unspecified);

        static readonly DateTimeOffset EpochOffset = new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero);

        static bool ExtractBool(JsonElement element, out object result)
        {
            if (element.ValueKind == JsonValueKind.True)
            {
                result = true;
                return true;
            }
            else if (element.ValueKind == JsonValueKind.False)
            {
                result = false;
                return true;
            }
            else if (element.ValueKind == JsonValueKind.String && bool.TryParse(element.GetString(), out var value))
            {
                result = value;
                return true;
            }
            else
            {
                result = false;
                return false;
            }
        }

        static bool ExtractByte(JsonElement element, out object result)
        {
            if (element.ValueKind == JsonValueKind.Number && element.TryGetByte(out var value))
            {
                result = value;
                return true;
            }
            else if (element.ValueKind == JsonValueKind.String && byte.TryParse(element.GetString(), out var parsed))
            {
                result = parsed;
                return true;
            }
            else
            {
                result = 0;
                return false;
            }
        }

        static bool ExtractDateTime(JsonElement element, out object result)
        {
            if (element.ValueKind == JsonValueKind.String && element.TryGetDateTime(out var value))
            {
                result = value;
                return true;
            }
            else if (element.ValueKind == JsonValueKind.Number && element.TryGetInt64(out var timestamp))
            {
                result = timestamp > 9_999_999_999 //should be updated in ~2286 :D
                    ? Epoch.AddMilliseconds(timestamp)
                    : Epoch.AddSeconds(timestamp);

                return true;
            }
            else
            {
                result = DateTime.MinValue;
                return false;
            }
        }

        static bool ExtractDateTimeOffset(JsonElement element, out object result)
        {
            if (element.ValueKind == JsonValueKind.String && element.TryGetDateTimeOffset(out var value))
            {
                result = value;
                return true;
            }
            else if (element.ValueKind == JsonValueKind.Number && element.TryGetInt64(out var timestamp))
            {
                result = timestamp > 9_999_999_999 //should be updated in ~2286 :D
                    ? EpochOffset.AddMilliseconds(timestamp)
                    : EpochOffset.AddSeconds(timestamp);

                return true;
            }
            else
            {
                result = DateTimeOffset.MinValue;
                return false;
            }
        }

        static bool ExtractDecimal(JsonElement element, out object result)
        {
            if (element.ValueKind == JsonValueKind.Number && element.TryGetDecimal(out var value))
            {
                result = value;
                return true;
            }
            else if (element.ValueKind == JsonValueKind.String && decimal.TryParse(element.GetString(), out var parsed))
            {
                result = parsed;
                return true;
            }
            else
            {
                result = 0.0M;
                return false;
            }
        }

        static bool ExtractDouble(JsonElement element, out object result)
        {
            if (element.ValueKind == JsonValueKind.Number && element.TryGetDouble(out var value))
            {
                result = value;
                return true;
            }
            else if (element.ValueKind == JsonValueKind.String && double.TryParse(element.GetString(), out var parsed))
            {
                result = parsed;
                return true;
            }
            else
            {
                result = 0.0;
                return false;
            }
        }

        static bool ExtractFloat(JsonElement element, out object result)
        {
            if (element.ValueKind == JsonValueKind.Number && element.TryGetSingle(out var value))
            {
                result = value;
                return true;
            }
            else if (element.ValueKind == JsonValueKind.String && float.TryParse(element.GetString(), out var parsed))
            {
                result = parsed;
                return true;
            }
            else
            {
                result = 0.0F;
                return false;
            }
        }

        static bool ExtractGuid(JsonElement element, out object result)
        {
            if (element.ValueKind == JsonValueKind.String && element.TryGetGuid(out var value))
            {
                result = value;
                return true;
            }
            else
            {
                result = Guid.Empty;
                return false;
            }
        }

        static bool ExtractInt16(JsonElement element, out object result)
        {
            if (element.ValueKind == JsonValueKind.Number && element.TryGetInt16(out var value))
            {
                result = value;
                return true;
            }
            else if (element.ValueKind == JsonValueKind.String && short.TryParse(element.GetString(), out var parsed))
            {
                result = parsed;
                return true;
            }
            else
            {
                result = 0;
                return false;
            }
        }

        static bool ExtractInt32(JsonElement element, out object result)
        {
            if (element.ValueKind == JsonValueKind.Number && element.TryGetInt32(out var value))
            {
                result = value;
                return true;
            }
            else if (element.ValueKind == JsonValueKind.String && int.TryParse(element.GetString(), out var parsed))
            {
                result = parsed;
                return true;
            }
            else
            {
                result = 0;
                return false;
            }
        }

        static bool ExtractInt64(JsonElement element, out object result)
        {
            if (element.ValueKind == JsonValueKind.Number && element.TryGetInt64(out var value))
            {
                result = value;
                return true;
            }
            else if (element.ValueKind == JsonValueKind.String && long.TryParse(element.GetString(), out var parsed))
            {
                result = parsed;
                return true;
            }
            else
            {
                result = 0L;
                return false;
            }
        }

        static bool ExtractSByte(JsonElement element, out object result)
        {
            if (element.ValueKind == JsonValueKind.Number && element.TryGetSByte(out var value))
            {
                result = value;
                return true;
            }
            else if (element.ValueKind == JsonValueKind.String && sbyte.TryParse(element.GetString(), out var parsed))
            {
                result = parsed;
                return true;
            }
            else
            {
                result = 0;
                return false;
            }
        }

        static bool ExtractString(JsonElement element, out object result)
        {
            if (element.ValueKind == JsonValueKind.String)
            {
                result = element.GetString();
                return true;
            }
            else
            {
                result = element.GetRawText();
                return true;
            }
        }

        static bool ExtractUInt16(JsonElement element, out object result)
        {
            if (element.ValueKind == JsonValueKind.Number && element.TryGetUInt16(out var value))
            {
                result = value;
                return true;
            }
            else if (element.ValueKind == JsonValueKind.String && ushort.TryParse(element.GetString(), out var parsed))
            {
                result = parsed;
                return true;
            }
            else
            {
                result = 0;
                return false;
            }
        }

        static bool ExtractUInt32(JsonElement element, out object result)
        {
            if (element.ValueKind == JsonValueKind.Number && element.TryGetUInt32(out var value))
            {
                result = value;
                return true;
            }
            else if (element.ValueKind == JsonValueKind.String && uint.TryParse(element.GetString(), out var parsed))
            {
                result = parsed;
                return true;
            }
            else
            {
                result = 0U;
                return false;
            }
        }

        static bool ExtractUInt64(JsonElement element, out object result)
        {
            if (element.ValueKind == JsonValueKind.Number && element.TryGetUInt64(out var value))
            {
                result = value;
                return true;
            }
            else if (element.ValueKind == JsonValueKind.String && ulong.TryParse(element.GetString(), out var parsed))
            {
                result = parsed;
                return true;
            }
            else
            {
                result = 0UL;
                return false;
            }
        }
        #endregion

        #region implicit
        public static implicit operator bool(DJsonValue json)
        {
            return ExtractBool(json.Element, out var value) ? (bool)value
                : throw new FormatException($"Unable to implicitly convert {json.Element.GetRawText()} to Boolean");
        }

        public static implicit operator byte(DJsonValue json)
        {
            return ExtractByte(json.Element, out var value) ? (byte)value
                : throw new FormatException($"Unable to implicitly convert {json.Element.GetRawText()} to Byte");
        }

        public static implicit operator DateTime(DJsonValue json)
        {
            return ExtractDateTime(json.Element, out var value) ? (DateTime)value
                : throw new FormatException($"Unable to implicitly convert {json.Element.GetRawText()} to DateTime");
        }

        public static implicit operator DateTimeOffset(DJsonValue json)
        {
            return ExtractDateTimeOffset(json.Element, out var value) ? (DateTimeOffset)value
                : throw new FormatException($"Unable to implicitly convert {json.Element.GetRawText()} to DateTimeOffset");
        }

        public static implicit operator decimal(DJsonValue json)
        {
            return ExtractDecimal(json.Element, out var value) ? (decimal)value
                : throw new FormatException($"Unable to implicitly convert {json.Element.GetRawText()} to Decimal");
        }

        public static implicit operator double(DJsonValue json)
        {
            return ExtractDouble(json.Element, out var value) ? (double)value
                : throw new FormatException($"Unable to implicitly convert {json.Element.GetRawText()} to Double");
        }

        public static implicit operator float(DJsonValue json)
        {
            return ExtractFloat(json.Element, out var value) ? (float)value
                : throw new FormatException($"Unable to implicitly convert {json.Element.GetRawText()} to Single");
        }

        public static implicit operator Guid(DJsonValue json)
        {
            return ExtractGuid(json.Element, out var value) ? (Guid)value
                : throw new FormatException($"Unable to implicitly convert {json.Element.GetRawText()} to Guid");
        }

        public static implicit operator short(DJsonValue json)
        {
            return ExtractInt16(json.Element, out var value) ? (short)value
                : throw new FormatException($"Unable to implicitly convert {json.Element.GetRawText()} to Int16");
        }

        public static implicit operator int(DJsonValue json)
        {
            return ExtractInt32(json.Element, out var value) ? (int)value
                : throw new FormatException($"Unable to implicitly convert {json.Element.GetRawText()} to Int32");
        }

        public static implicit operator long(DJsonValue json)
        {
            return ExtractInt64(json.Element, out var value) ? (long)value
                : throw new FormatException($"Unable to implicitly convert {json.Element.GetRawText()} to Int64");
        }

        public static implicit operator sbyte(DJsonValue json)
        {
            return ExtractSByte(json.Element, out var value) ? (sbyte)value
                : throw new FormatException($"Unable to implicitly convert {json.Element.GetRawText()} to SByte");
        }

        public static implicit operator string(DJsonValue json)
        {
            return ExtractString(json.Element, out var value) ? (string)value
                : throw new FormatException($"Unable to implicitly convert {json.Element.GetRawText()} to String");
        }

        public static implicit operator ushort(DJsonValue json)
        {
            return ExtractUInt16(json.Element, out var value) ? (ushort)value
                : throw new FormatException($"Unable to implicitly convert {json.Element.GetRawText()} to UInt16");
        }

        public static implicit operator uint(DJsonValue json)
        {
            return ExtractUInt32(json.Element, out var value) ? (uint)value
                : throw new FormatException($"Unable to implicitly convert {json.Element.GetRawText()} to UInt32");
        }

        public static implicit operator ulong(DJsonValue json)
        {
            return ExtractUInt64(json.Element, out var value) ? (ulong)value
                : throw new FormatException($"Unable to implicitly convert {json.Element.GetRawText()} to UInt64");
        }
        #endregion
    }
}

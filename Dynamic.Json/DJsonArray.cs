﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text.Json;

namespace Dynamic.Json
{
    public sealed class DJsonArray : DJson, IEnumerable<DJson>
    {
        internal DJsonArray(JsonElement element, JsonSerializerOptions options) : base(element, options) { }

        public int Count => Element.GetArrayLength();

        public int count => Element.GetArrayLength();

        public int Length => Element.GetArrayLength();

        public int length => Element.GetArrayLength();

        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
        {
            if (indexes.Length != 1 || !(indexes[0] is int index))
                throw new NotImplementedException();

            if (index < 0 || index >= Element.GetArrayLength())
                throw new IndexOutOfRangeException();

            result = Create(Element.EnumerateArray().Skip(index).First(), Options);
            return true;
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
                throw new FormatException($"Unable to convert json array to {binder.Type.Name}", ex);
            }
        }

        public IEnumerator GetEnumerator() => Element.EnumerateArray()
            .Select(x => Create(x, Options))
            .GetEnumerator();

        IEnumerator<DJson> IEnumerable<DJson>.GetEnumerator() => Element.EnumerateArray()
            .Select(x => Create(x, Options))
            .GetEnumerator();

        #region implicit
        public static implicit operator string(DJsonArray json)
        {
            return json.Element.GetRawText();
        }
        #endregion
    }
}

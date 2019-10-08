// Copyright (c) 2020, mParticle, Inc
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//   https://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace MP.Json
{
    /// <summary>
    /// This class is meant to support very lightweight JSON trees that use the minimum amount of memory.
    /// The footprint of the JSON trees are meant to be similar to the equivalent representation of the
    /// the JSON data as a text string.
    /// 
    /// There could be thousands of these trees in memory. Existing object trees from Newtonsoft JSON.NET,
    /// ManateeJSON and others have a lot of unnecessary overhead.
    /// 
    /// Data is stored as raw objects:
    /// Nulls are represented as DBNullValue.
    /// Strings are represented as strings but we share instances with a small lru.
    /// Booleans are stored as shared objects.
    /// Numbers are normally stored as doubles just as in Javascript, 
    ///     they are converted to doubles with possible loss of precision.
    /// Arrays are normally stored and optimized for object[]
    /// Objects are normally stored and optimized for SORTED KeyValuePair(string,object)[], 
    /// </summary>
    public struct MPJson : IEquatable<MPJson>, IEnumerable<MPJson>, IFormattable
    {
        #region Constants
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        const int MaxDepth = 1000;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static readonly MPJson True = MPJson.From(JsonParser.True);
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static readonly MPJson False = MPJson.From(JsonParser.False);
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static readonly MPJson Null = MPJson.From(DBNull.Value);
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static readonly MPJson Zero = new MPJson(0.0);
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static readonly MPJson Undefined = new MPJson();
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static Comparison<KeyValuePair<string, object>> KeyComparer = (a, b) => string.CompareOrdinal(a.Key, b.Key);
        #endregion

        #region Variables
        public object Value;
        #endregion

        #region Json

        /// <summary>
        /// Produces a JSON object for numbers with coercion to double
        /// </summary>
        public MPJson(double value)
        {
            Value = value;
        }

        /// <summary>
        /// Produce a json string object
        /// </summary>
        /// <param name="s"></param>
        public MPJson(string s)
        {
            Value = s != null ? (object)s : DBNull.Value;
        }

        /// <summary>
        /// Produces a json object for booleans
        /// </summary>
        public MPJson(bool b)
        {
            Value = b ? JsonParser.True : JsonParser.False;
        }

        /// <summary>
        /// Creates an objects from the properties
        /// </summary>
        /// <param name="array"></param>
        [DebuggerStepThrough]
        public static MPJson Object(params KeyValuePair<string, object>[] array)
        {
            // We know params was used, otherwise the other method would have been called!
            return Object(array, true);
        }

        public static MPJson Object(KeyValuePair<string, object>[] array, bool keep = false)
        {
            if (!keep)
                array = (KeyValuePair<string, object>[])array.Clone();

            System.Array.Sort(array, KeyComparer);
            return new MPJson { Value = array };
        }

        /// <summary>
        /// Creates an objects from the properties entered as flattened pairs
        /// </summary>
        /// <param name="pairs"></param>
        public static MPJson Object(params MPJson[] pairs)
        {
            if ((pairs.Length & 1) == 1)
                throw new ArgumentException("An even number of parameters must be passed", nameof(pairs));

            int length = pairs.Length >> 1;
            var kvarray = new KeyValuePair<string, object>[length];

            for (int i = 0; i < length; i++)
                kvarray[i] = new KeyValuePair<string, object>((string)pairs[i * 2], pairs[i * 2 + 1].Value);

            return Object(kvarray);
        }

        /// <summary>
        /// Empty object
        /// </summary>
        /// <returns></returns>
        public static MPJson Object() => new MPJson { Value = System.Array.Empty<KeyValuePair<string, object>>() };

        /// <summary>
        /// Construct Json from array
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static MPJson Array(params MPJson[] array)
        {
            object[] jarray = new object[array.Length];
            for (int i = 0; i < array.Length; i++)
                jarray[i] = array[i].Value;
            return new MPJson { Value = jarray };
        }

        /// <summary>
        /// Construct Json from arbitrary object
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static MPJson From(object obj) => new MPJson { Value = obj };

        #endregion

        #region Properties

        /// <summary>
        /// Returns the type of the object
        /// </summary>
        public JsonType Type => GetType(Value);

        /// <summary>
        /// Returns the element at array index, otherwise undefined for other types
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public MPJson this[int index]
            => Value is object[] array
                   && unchecked((uint)index < (uint)array.Length) ? MPJson.From(array[index]) : (default);

        /// <summary>
        /// Returns the value for key property, otherwise undefined for other types
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public MPJson this[string property]
            => Value is KeyValuePair<string, object>[] map
                ? MPJson.From(map.GetProperty(property))
                : default;

        /// <summary>
        /// Returns if Json object is undefined
        /// </summary>
        public bool IsUndefined => Value == null;

        /// <summary>
        /// Returns if Json object has value
        /// </summary>
        public bool HasValue => Value != null;

        public int Length => Value is Array array ? array.Length : 0;

        /// <summary>
        /// Shortcut for new JsonProperty
        /// </summary>
        [DebuggerStepThrough]
        public static JsonProperty Property(string keyword, MPJson json)
            => new JsonProperty(keyword, json);

        #endregion

        #region Enumerable

        /// <summary>
        /// Returns keys of objects
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> Keys
        {
            get
            {
                if (Value is KeyValuePair<string, object>[] properties)
                    foreach (var v in properties)
                        yield return v.Key;
            }
        }

        /// <summary>
        /// Returns property pairs of objects
        /// </summary>
        /// <returns></returns>
        public IEnumerable<JsonProperty> Properties
        {
            get
            {
                if (Value is KeyValuePair<string, object>[] properties)
                    foreach (var v in properties)
                        yield return new JsonProperty(v.Key, MPJson.From(v.Value));
            }
        }

        /// <summary>
        /// Returns array elements
        /// </summary>
        public IEnumerator<MPJson> GetEnumerator()
        {
            if (Value is object[] array)
                foreach (object v in array)
                    yield return MPJson.From(v);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region Conversions

        public static explicit operator string(MPJson json) => (string)json.Value;

        public static explicit operator double(MPJson json) => Convert.ToDouble(json.Value);

        public static explicit operator object[](MPJson json) => (object[])json.Value;

        public static explicit operator bool(MPJson json) => (bool)json.Value;

        public static implicit operator MPJson(string value) => new MPJson(value);

        public static implicit operator MPJson(double value) => new MPJson(value);

        public static implicit operator MPJson(bool value) => new MPJson(value);

        public static implicit operator MPJson(MPJson[] value) => MPJson.Array(value);

        #endregion

        #region Methods

        /// <summary>
        /// Parses a json string
        /// </summary>
        /// <param name="s"></param>
        /// <returns>
        /// Return json object or FormatException on failure
        /// </returns>
        public static MPJson Parse(string s) => new JsonParser().Parse(s);

        /// <summary>
        /// Parses a json reader
        /// </summary>
        /// <param name="reader"></param>
        /// <returns>
        /// Return json object or FormatException on failure
        /// </returns>
        public static MPJson Parse(TextReader reader) => new JsonParser().Parse(reader);

        /// <summary>
        /// Attempts to parse a json string
        /// </summary>
        /// <param name="reader"></param>
        /// <returns>a json object, which may be undefined on failure (IsUndefined=true, HasValue=false)</returns>
        public static bool TryParse(string s, out MPJson json) => new JsonParser().TryParse(s, out json);

        /// <summary>
        /// Attempts to parse a json reader
        /// </summary>
        /// <param name="reader"></param>
        /// <returns>a json object, which may be undefined on failure (IsUndefined=true, HasValue=false)</returns>
        public static bool TryParse(TextReader reader, out MPJson json) => new JsonParser().TryParse(reader, out json);

        #endregion

        #region Formatter

        /// <summary>
        /// Renders a string representation of the JSON
        /// </summary>
        public override string ToString() => ToString(null, null);

        /// <summary>
        /// Provides for multiple different string formatting options
        /// 
        /// This is to allow for multiline, indented json in the future
        /// </summary>
        /// <param name="format"></param>
        /// <param name="provider"></param>
        /// <returns></returns>
        public string ToString(string format, IFormatProvider provider)
        {
            var builder = new StringBuilder();
            Write(builder, Value);
            return builder.ToString();
        }

        private static void Write(StringBuilder builder, object obj)
        {
            var type = GetType(obj);
            switch (type)
            {
                case JsonType.String:
                    Write(builder, (string)obj);
                    break;

                case JsonType.Object:
                    builder.Append('{');
                    bool first = true;
                    foreach (var v in (IEnumerable<KeyValuePair<string, object>>)obj)
                    {
                        if (!first) builder.Append(',');
                        builder.Append(' ');
                        Write(builder, v.Key);
                        builder.Append(':');
                        Write(builder, v.Value);
                        first = false;
                    }
                    builder.Append(' ').Append('}');
                    break;

                case JsonType.Array:
                    builder.Append('[');
                    first = true;
                    foreach (object v in (IEnumerable<object>)obj)
                    {
                        if (!first) builder.Append(',');
                        builder.Append(' ');
                        Write(builder, v);
                        first = false;
                    }
                    builder.Append(' ').Append(']');
                    break;

                case JsonType.Boolean:
                    builder.Append(((bool)obj) ? JsonParser.TrueKeyword : JsonParser.FalseKeyword);
                    break;

                case JsonType.Null:
                    builder.Append(JsonParser.NullKeyword);
                    break;

                case JsonType.Undefined:
                    builder.Append(JsonParser.UndefinedKeyword);
                    break;

                default:
                    builder.Append(obj);
                    break;
            }
        }

        private static void Write(StringBuilder builder, string s)
        {
            builder.Append('"');
            foreach (char ch in s)
            {
                if (ch >= ' ')
                {
                    if (ch == '"' || ch == '\\')
                        builder.Append('\\');
                    builder.Append(ch);
                }
                else
                {
                    builder.Append('\\');
                    switch (ch)
                    {
                        case '\t':
                            builder.Append('t');
                            break;
                        case '\n':
                            builder.Append('n');
                            break;
                        case '\r':
                            builder.Append('r');
                            break;
                        case '\f':
                            builder.Append('f');
                            break;
                        default:
                            builder.Append('u').Append(((int)ch).ToString("x4"));
                            break;
                    }
                }
            }
            builder.Append('"');
        }

        #endregion

        #region Equality Operations

        /// <summary>
        /// Indicates whether two Json objects are equivalent
        /// </summary>
        /// <param name="obj1"></param>
        /// <param name="obj2"></param>
        /// <param name="depth">set to zero; used for catching stack overflows </param>
        /// <returns></returns>

        public static bool operator ==(MPJson object1, MPJson object2)
            => Matches(object1.Value, object2.Value);

        public static bool operator !=(MPJson object1, MPJson object2)
            => !Matches(object1.Value, object2.Value);

        public bool Equals(MPJson obj)
            => Matches(Value, obj.Value);

        public override bool Equals(object obj)
            => obj is MPJson && Equals((MPJson)obj);

        public override int GetHashCode()
        {
            return GetHashCode(Value);
        }

        private static int GetHashCode(object value)
        {
            if (value == null) return 0x12345678;

            int hashCode = unchecked((int)0xDEADBEEF);
            if (value is object[] array)
            {
                foreach (object v in array)
                    hashCode = CombineHashcode(hashCode, GetHashCode(v));
                return hashCode;
            }
            else if (value is KeyValuePair<string, object>[] obj)
            {
                foreach (var kv in obj)
                    hashCode ^= CombineHashcode(GetHashCode(kv.Key), GetHashCode(kv.Value));
                return hashCode;
            }

            return hashCode ^ value.GetHashCode();
        }

        private static int CombineHashcode(int h1, int h2)
        {
            return h1 * 3777 ^ h2;
        }


        public static bool Matches(object obj1, object obj2, int depth = 0)
        {
            if (obj1 == null || obj2 == null) return obj1 == obj2;
            if (obj1.Equals(obj2)) return true;

            if (depth >= MaxDepth)
                throw new StackOverflowException("Json too large");  // catchable stack overflow

            JsonType type1 = GetType(obj1);
            JsonType type2 = GetType(obj2);
            if (type1 != type2)
                return false;

            switch (type1)
            {
                case JsonType.Number:
                    if (GetType(obj2) != JsonType.Number) break;
                    return Convert.ToDouble(obj1) == Convert.ToDouble(obj2);

                case JsonType.Array:
                    var array1 = (IList)obj1;
                    var array2 = (IList)obj2;
                    int count = array1.Count;
                    if (count != array2.Count)
                        return false;

                    for (int i = 0; i < count; i++)
                        if (!Matches(array1[i], array2[i], depth + 1))
                            return false;
                    return true;

                case JsonType.Object:
                    // Handle JsonLite values here...
                    if (obj1 is KeyValuePair<string, object>[] kvlist1
                        && obj2 is KeyValuePair<string, object>[] kvlist2)
                    {
                        count = kvlist1.Length;
                        if (count != kvlist2.Length)
                            return false;

                        for (int i = 0; i < count; i++)
                            if (kvlist1[i].Key != kvlist2[i].Key
                                || !Matches(kvlist1[i].Value, kvlist2[i].Value, depth + 1))
                                return false;

                        return true;
                    }

                    return false;

                case JsonType.String:
                case JsonType.Boolean:
                case JsonType.Null:
                    // These cases are handled at the beginning of the function
                    break;
            }

            return false;
        }

        #endregion

        #region Conversion

        /// <summary>
        /// Gets the type of a Json object
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static JsonType GetType(object obj)
        {
            if (obj == null)
                return JsonType.Undefined;

            // Not sure which is faster
            TypeCode typeCode = System.Type.GetTypeCode(obj.GetType());
            // TypeCode typeCode = obj is IConvertible iconv ? iconv.GetTypeCode() : TypeCode.Object;

            switch (typeCode)
            {
                case TypeCode.String:
                    return JsonType.String;
                case TypeCode.Double:
                    return JsonType.Number;
                case TypeCode.DBNull:
                    return JsonType.Null;
                case TypeCode.Boolean:
                    return JsonType.Boolean;
            }

            if (obj is KeyValuePair<string, object>[])
                return JsonType.Object;

            if (obj is object[])
                return JsonType.Array;

            return JsonType.Unknown;
        }
        #endregion

        #region Functional routines

        public static object MapChildren(object obj, Func<object, object> func)
        {
            if (obj is KeyValuePair<string, object>[] map)
            {
                KeyValuePair<string, object>[] result = map;
                for (int i = 0; i < map.Length; i++)
                {
                    var kv = map[i];
                    object mappedChild = func(kv.Value);
                    if (mappedChild == null) return null;
                    if (mappedChild != kv.Value)
                    {
                        result = (KeyValuePair<string, object>[])map.Clone();
                        result[i] = new KeyValuePair<string, object>(kv.Key, mappedChild);
                        for (i++; i < map.Length; i++)
                        {
                            kv = map[i];
                            mappedChild = func(kv.Value);
                            if (mappedChild == null) return null;
                            result[i] = new KeyValuePair<string, object>(kv.Key, mappedChild);
                        }
                    }
                }
                return result;
            }
            else if (obj is object[] array)
            {
                object[] result = array;
                for (int i = 0; i < array.Length; i++)
                {
                    object child = array[i];
                    object mappedChild = func(child);
                    if (mappedChild == null) return null;
                    if (mappedChild != child)
                    {
                        result = (object[])array.Clone();
                        result[i] = mappedChild;
                        for (i++; i < array.Length; i++)
                        {
                            child = array[i];
                            mappedChild = func(child);
                            if (mappedChild == null) return null;
                            result[i] = mappedChild;
                        }
                    }
                }
                return result;
            }
            return obj;
        }

        /// <summary>
        /// Transform every object in tree while reusing any unchanged objects
        /// If func is null on any object, the whole result is null
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="func"></param>
        /// <returns>
        /// </returns>
        /// <remarks>
        /// This methods is useful for many different task.
        /// -- Searching for objects
        ///     - func returns same object if not found, or null if found
        /// -- Make modifications to objects
        /// </remarks>

        public static object MapAll(object obj, Func<object, object> func)
        {
            object result = obj;

            if (obj is KeyValuePair<string, object>[] map)
            {
                KeyValuePair<string, object>[] resultMap = map;
                for (int i = 0; i < map.Length; i++)
                {
                    var kv = map[i];
                    object mappedChild = MapAll(kv.Value, func);
                    if (mappedChild == null) return null;
                    if (mappedChild != kv.Value)
                    {
                        resultMap = (KeyValuePair<string, object>[])map.Clone();
                        resultMap[i] = new KeyValuePair<string, object>(kv.Key, mappedChild);
                        for (i++; i < map.Length; i++)
                        {
                            kv = map[i];
                            mappedChild = MapAll(kv.Value, func);
                            if (mappedChild == null) return null;
                            resultMap[i] = new KeyValuePair<string, object>(kv.Key, mappedChild);
                        }
                    }
                }
                result = resultMap;
            }
            else if (obj is object[] array)
            {
                object[] resultArray = array;
                for (int i = 0; i < array.Length; i++)
                {
                    object child = array[i];
                    object mappedChild = MapAll(child, func);
                    if (mappedChild == null) return null;
                    if (mappedChild != child)
                    {
                        resultArray = (object[])array.Clone();
                        resultArray[i] = mappedChild;
                        for (i++; i < array.Length; i++)
                        {
                            child = array[i];
                            mappedChild = MapAll(child, func);
                            if (mappedChild == null) return null;
                            resultArray[i] = mappedChild;
                        }
                    }
                }
                result = resultArray;
            }

            return func(result);
        }


        #endregion
    }
}

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
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace MP.Json.Validation
{
    public class JsonPointer
    {
        #region Variables
        static char[] delims = { '/' };
        Unit[] path = Array.Empty<Unit>();
        #endregion

        #region Constructor
        public JsonPointer()
        {
            Clear();
        }

        public JsonPointer(string pointer)
        {
            Assign(pointer);
        }

        /// <summary>
        /// Assigns a Json Pointer to here
        /// </summary>
        /// <param name="pointer"></param>
        public void Assign(string pointer)
        {
            Clear();

            int fragmentStart = pointer.IndexOf('#');
            if (fragmentStart < 0)
            {
                BaseUri = pointer;
                return;
            }

            BaseUri = pointer.Remove(fragmentStart);
            string fragment = pointer.Substring(fragmentStart + 1);
            string[] split = fragment.Split(delims, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < split.Length; i++)
            {
                string s = split[i];
                if (i == 0 && s.Length == 1 && s[0] == '#')
                    continue;

                char ch = s[0];
                if (ch <= '9' && ch >= '0' && int.TryParse(s, out int d))
                    Push(d);
                else
                    Push(Unescape(s));
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// BaseUri
        /// </summary>
        public string BaseUri { get; set; }

        /// <summary>
        /// Length of Pointer
        /// </summary>
        public int Length { get; private set; }

        /// <summary>
        /// First in of path
        /// </summary>
        public string FirstProperty
        {
            get
            {
                for (int i = 0; i < path.Length; i++)
                    if (path[i].Property != null)
                        return path[i].Property;
                return null;
            }
        }

        /// <summary>
        /// Last property in path
        /// </summary>
        public string LastProperty
        {
            get
            {
                for (int i = Length - 1; i >= 0; i--)
                    if (path[i].Property != null)
                        return path[i].Property;
                return null;
            }
        }

        /// <summary>
        /// Last keyword in schema path -- cause of error
        /// </summary>
        public Keyword Keyword
        {
            get
            {
                for (int i = Length - 1; i >= 0; i--)
                    if (path[i].Keyword >= 0)
                        return path[i].Keyword;
                return Keyword.None;
            }
        }

        /// <summary>
        /// Returns the json pointer text
        /// </summary>
        public string Text => GetText();

        public string GetText(StringBuilder builder = null)
        {
            if (builder == null) builder = new StringBuilder();
            else builder.Clear();

            builder.Append('#');
            for (int i = 0; i < Length; i++)
            {
                if (path[i].Keyword >= 0)
                {
                    builder.Append('/');
                    builder.Append(path[i].SchemaKeywordText);
                }
                else if (path[i].Index >= 0)
                {
                    builder.Append('/');
                    builder.Append(path[i].Index);
                }

                if (path[i].Property != null)
                {
                    builder.Append('/');
                    foreach (char ch in path[i].Property)
                    {
                        if (ch == '~')
                            builder.Append('~').Append('0');
                        else if (ch == '/')
                            builder.Append('~').Append('1');
                        else
                            builder.Append(ch);
                    }
                }
            }

            if (builder.Length == 1)
                builder.Append('/');

            return builder.ToString();
        }

        public static implicit operator string(JsonPointer builder) => builder.ToString();

        public override string ToString() => GetText();

        #endregion

        #region Methods

        /// <summary>
        /// Clear pointer data
        /// </summary>
        public void Clear()
        {
            Length = 0;
            BaseUri = string.Empty;
        }

        /// <summary>
        /// Pushes the property
        /// </summary>
        /// <param name="property"></param>
        public void Push(string property)
        {
            var unit = new Unit(property);
            Push(ref unit);
        }

        /// <summary>
        /// Pushes the current index
        /// </summary>
        /// <param name="index"></param>
        public void Push(int index)
        {
            Unit unit = new Unit(index);
            Push(ref unit);
        }

        /// <summary>
        /// Pushes the schema keyword
        /// </summary>
        /// <param name="schema"></param>
        public void Push(Keyword keyword)
        {
            Unit unit = new Unit(keyword);
            Push(ref unit);
        }

        /// <summary>
        /// Push Unit
        /// </summary>
        /// <param name="unit"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Push(ref Unit unit)
        {
            if (Length == path.Length)
                Array.Resize(ref path, Math.Max(4, path.Length * 2));
            path[Length++] = unit;
        }

        /// <summary>
        /// Pops the object context
        /// </summary>
        public void Pop()
        {
            Length--;
        }

        /// <summary>
        /// Find path in json
        /// </summary>
        public static object Find(MPJson json, string path)
        {
            return Find(json.Value, path);
        }

        /// <summary>
        /// Find path
        /// </summary>
        public static object Find(object root, string path)
        {
            int index = path.IndexOf('#');
            index = index < 0 ? path.Length : index + 1;

            bool hasTilde = path.IndexOf('~') >= 0;

            object node = root;
            while (index < path.Length)
            {
                int newIndex = path.IndexOf('/', index);
                if (newIndex == index)
                {
                    index++;
                    continue;
                }

                if (newIndex < 0)
                    newIndex = path.Length;

                string key = Unescape(path.Substring(index, newIndex - index));

                if (node is KeyValuePair<string, object>[] obj)
                {
                    node = obj.GetProperty(key);
                    if (node == null)
                        return null;
                }
                else if (node is object[] array)
                {
                    if (!uint.TryParse(key, out uint elemIndex) || elemIndex >= array.Length)
                        return null;
                    node = array[elemIndex];
                }
                else
                    return null;

                index = newIndex + 1;
            }

            return node;
        }

        public static string Unescape(string pathElement)
        {
            StringBuilder sb = null;
            for (int i = 0; i < pathElement.Length; i++)
            {
                char ch = pathElement[i];
                if (ch == '~' || ch == '%')
                {
                    if (sb == null)
                        sb = new StringBuilder(pathElement, 0, i, pathElement.Length);
                    if (ch == '~')
                    {
                        if (i + 1 < pathElement.Length)
                        {
                            char ch2 = pathElement[i + 1];
                            if (ch2 == '0')
                            {
                                i++;
                                sb.Append('~');
                                continue;
                            }
                            else if (ch2 == '1')
                            {
                                i++;
                                sb.Append('/');
                                continue;
                            }
                        }
                    }
                    else
                    {
                        if (i + 2 < pathElement.Length
                            && Formats.IsHexDigit(pathElement[i + 1])
                            && Formats.IsHexDigit(pathElement[i + 2]))
                        {
                            char ch2 = (char)(Formats.Hex(pathElement[i + 1]) * 16 + Formats.Hex(pathElement[i + 2]));
                            sb.Append(ch2);
                            i += 2;
                            continue;
                        }
                    }
                }
                sb?.Append(ch);
            }

            return sb?.ToString() ?? pathElement;
        }

        /// <summary>
        /// Canonicalize by removing https and trailing slashes
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string Canonicalize(string s)
        {
            if (s == null || s.Length == 0) return s;
            if (s.StartsWith("https", StringComparison.Ordinal))
                s = "http" + s.Substring(5);
            if (s[s.Length - 1] == '/')
                s = s.Substring(0, s.Length - 1);
            return s;
        }

        /// <summary>
        /// Split into base uri or relative string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>

        public static (string baseUri, string relative) Split(string value)
        {
            string baseUri, relative;
            if (string.IsNullOrWhiteSpace(value))
                return (string.Empty, string.Empty);

            int fragmentStart = value.IndexOf('#');
            if (fragmentStart < 0)
                return (Canonicalize(value), string.Empty);

            baseUri = fragmentStart == 0 ? string.Empty : value.Remove(fragmentStart);
            relative = fragmentStart == value.Length - 1 ? string.Empty : value.Substring(fragmentStart + 1);
            return (Canonicalize(baseUri), relative);
        }


        #endregion

        #region Unit
        public readonly struct Unit
        {
            #region Variables
            public readonly string Property;
            public readonly int data;
            #endregion

            #region Constructor
            public Unit(int index)
            {
                data = index >= 0 ? index + 1 : 0;
                Property = null;
            }

            public Unit(Keyword keyword)
            {
                data = ~(int)keyword;
                Property = null;
            }

            public Unit(string property)
            {
                Property = property;
                data = 0;
            }

            #endregion

            #region Schema
            public string SchemaKeywordText => KeywordUtils.GetText(Keyword);

            public Keyword Keyword
            {
                get => data < 0 ? (Keyword)~data : Keyword.None;
            }

            public int Index
            {
                get => data >= 0 ? data - 1 : -1;
            }

            #endregion
        }
        #endregion
    }
}

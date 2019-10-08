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
using System.Globalization;
using System.IO;
using System.Text;


namespace MP.Json
{
    /// <summary>
    /// Fast and memory-efficient Json parsing. This parser is not thread-safe, so create a parser for each thread.
    /// </summary>
    /// 
    /// <remarks>
    /// https://tools.ietf.org/id/draft-ietf-jsonbis-rfc7159bis-04.html
    /// Deviations from standard grammar include support for single quotes and trailing commas.
    /// Objects are represented as sorted KeyValuePair of string and object.
    /// Arrays are represented as object[].
    /// Strings are represented as String.
    /// Numbers are represented as boxed doubles.
    /// True, False are represented as boxed booleans.
    /// Null is represented by DBNull.Value.
    /// </remarks>
    public class JsonParser
    {
        #region Static
        private static int maxDepth = 1000;

        // Shared boolean instances
        public static object True = true;
        public static object False = false;

        // Keywords
        public static readonly string TrueKeyword = "true";
        public static readonly string FalseKeyword = "false";
        public static readonly string NullKeyword = "null";
        public static readonly string UndefinedKeyword = "undefined";

        // Error Messages
        private const string MissingValue = "Missing value";
        private const string EndOfInput = "End of input";
        private const string ExpectedChar = "Expected '{0}'";
        private const string ControlChar = "Control character in string";
        private const string UnterminatedString = "Unterminated string";
        private const string ExpectedEOF = "Expected end of file but found '{0}'";
        private const string ExpectedString = "Expected string";
        private const string InvalidNumber = "Invalid number";
        private const string ExpectedObject = "Expected object";
        private const string ExpectedArray = "Expected array";
        private const string BadUnicodeSequence = "Bad unicode sequence";
        private const string SyntaxError = "Syntax error";

        #endregion

        #region Variables
        private TextReader reader;
        private StringBuilder buffer = new StringBuilder();
        private Comparison<KeyValuePair<string, object>> propertyComparer = (a, b) => string.CompareOrdinal(a.Key, b.Key);
        private int depth;
        #endregion

        #region Properties

        public bool Strict { get; set; }

        public int Line { get; private set; }

        public int Column { get; private set; }

        public string ErrorMessage { get; private set; }

        #endregion

        /// <summary>
        /// Parses a json string
        /// </summary>
        /// <param name="s"></param>
        /// <returns>
        /// Return json object or FormatException on failure.
        /// </returns>
        public MPJson Parse(string json) => Parse(new StringReader(json));

        /// <summary>
        /// Parses a json reader.
        /// </summary>
        /// <param name="reader"></param>
        /// <returns>
        /// Return json object or FormatException on failure.
        /// </returns>
        public MPJson Parse(TextReader reader)
        {
            if (TryParse(reader, out MPJson result))
                return result;

            string errorMessage = ErrorMessage ?? SyntaxError;
            throw new FormatException($"{errorMessage} in Line {Line} and Column {Column}");
        }

        /// <summary>
        /// Attempts to parse a json reader.
        /// </summary>
        /// <param name="reader"></param>
        /// <returns>a json object, which may be undefined on failure (IsUndefined=true, HasValue=false).</returns>
        public bool TryParse(TextReader reader, out MPJson json)
        {
            this.Line = 1;
            this.Column = 1;
            this.reader = reader;
            this.depth = 0;
            json = default;

            var obj = ParseTerminal();
            if (obj == null)
                return false;

            int ch = SkipSpaces();
            if (ch >= 0)
            {
                ReportError(string.Format(CultureInfo.InvariantCulture,
                    ExpectedEOF,
                    new string((char)ch, 1)));
                return false;
            }

            json = MPJson.From(obj);
            return true;
        }

        /// <summary>
        /// Attempts to parse a json string
        /// </summary>
        /// <param name="text"></param>
        /// <returns>a json object, which may be undefined on failure (IsUndefined=true, HasValue=false)</returns>
        public bool TryParse(string text, out MPJson json)
        {
            return TryParse(new StringReader(text), out json);
        }

        private object ParseTerminal()
        {
            if (depth >= maxDepth)
                return ReportError($"Maximum object/array depth of {maxDepth} exceeded.");

            int ch = SkipSpaces();
            switch (ch)
            {
                // object
                case '{':
                    depth++;
                    var result = ParseObject();
                    depth--;
                    return result;

                // Array
                case '[':
                    depth++;
                    result = ParseArray();
                    depth--;
                    return result;

                // String
                case '\'':
                case '"':
                    return ParseString();

                // TODO: Change code to return Invalid identifiers
                // when nonstandard ones are found.

                // null
                case 'n':
                    if (Accept(NullKeyword))
                        return DBNull.Value;
                    return null;

                // true
                case 't':
                    if (Accept(TrueKeyword))
                        return True;
                    return null;

                // false
                case 'f':
                    if (Accept(FalseKeyword))
                        return False;
                    return null;

                // Numbers
                case '-':
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    return ParseNumber();

                default:
                    return ReportError(MissingValue);
            }
        }

        /// <summary>
        /// Parses an object
        /// </summary>
        /// <returns></returns>
        private object ParseObject()
        {
            if (!Accept('{'))
                return ReportError(ExpectedObject);

            var list = new List<KeyValuePair<string, object>>();

            if (!Check('}', advance: false))
            {
                do
                {
                    string property = ParseString();
                    if (property == null) return null;

                    SkipSpaces();
                    if (!Accept(':')) return null;

                    object value = ParseTerminal();
                    if (value == null) return null;

                    list.Add(new KeyValuePair<string, object>(property, value));
                }
                while (Check(','));
            }

            list.Sort(propertyComparer);
            return Accept('}') ? list.ToArray() : null;
        }

        /// <summary>
        /// Parses an array.
        /// </summary>
        /// <returns></returns>
        private object ParseArray()
        {
            if (!Accept('['))
                return ReportError(ExpectedArray);

            var list = new List<object>();

            if (!Check(']', advance: false))
                do
                {
                    object value = ParseTerminal();
                    if (value == null) return null;
                    list.Add(value);
                }
                while (Check(','));

            return Accept(']') ? list.ToArray() : null;
        }

        /// <summary>
        /// Parses strings
        /// 
        /// Deviations from standard grammar include support for single quotes.
        /// </summary>
        /// <returns></returns>
        private string ParseString()
        {
            int ch = SkipSpaces();

            // Newtonsoft accepts both ' (nonstandard) and ".
            if (ch != '"' && (ch != '\'' || Strict))
                return ReportError(ExpectedString);

            int startLine = Line;
            int startColumn = Column;
            int quote = Read();
            buffer.Clear();

            while (true)
            {
                ch = reader.Read();

                if (ch == quote)
                {
                    var result = buffer.ToString();
                    return string.IsInterned(result) ?? result;
                }

                if (ch < ' ')
                {
                    return ch < 0
                        ? ReportError(UnterminatedString, startLine, startColumn)
                        : ReportError(ControlChar);
                }

                if (ch == '\\')
                {
                    ch = reader.Read();
                    switch (ch)
                    {
                        case 'b':
                            ch = '\b';
                            break;
                        case 'f':
                            ch = '\f';
                            break;
                        case 'n':
                            ch = '\n';
                            break;
                        case 'r':
                            ch = '\r';
                            break;
                        case 't':
                            ch = '\t';
                            break;
                        case 'u':
                            ch = ReadUnicodeCharacter();
                            if (ch < 0) return ReportError(BadUnicodeSequence);
                            break;

                        case '0': // Nonstandard
                            ch = '\0';
                            break;
                        case 'v': // Nonstandard
                            ch = '\v';
                            break;

                        case '\\':
                        case '/':
                        case '"':
                        case '\'': // Nonstandard, but used internally for unit tests
                            break;

                        default:
                            return ReportError($"Nonstandard escape character '{(char)ch}'");
                    }
                }

                buffer.Append((char)ch);
            }
        }

        private int ReadUnicodeCharacter()
        {
            int b1 = ReadHexidecimalDigit();
            if (b1 < 0) return -1;

            int b2 = ReadHexidecimalDigit();
            if (b2 < 0) return -1;

            int b3 = ReadHexidecimalDigit();
            if (b3 < 0) return -1;

            int b4 = ReadHexidecimalDigit();
            if (b4 < 0) return -1;

            return (b1 << 12) + (b2 << 8) + (b3 << 4) + b4;
        }

        private int ReadHexidecimalDigit()
        {
            int ch = Read();
            if (ch <= '9')
                return ch - '0';

            if (ch >= 'a' && ch <= 'f')
                return ch - 'a' + 10;

            if (ch >= 'A' && ch <= 'F')
                return ch - 'A' + 10;

            ReportError($"Invalid hexidecimal character '{(char)ch}'");
            return -1;
        }


        private double? ParseNumber()
        {
            double result = 0;
            string error = null;

            buffer.Clear();

            int ch = SkipSpaces();
            if (ch == '-')
            {
                buffer.Append((char)Read());
                ch = reader.Peek();
            }

            if (ch >= '0' && ch <= '9')
            {
                if (Check('0'))
                {
                    // Leading zeros are not permitted
                    buffer.Append('0');
                    ch = reader.Peek();
                    if (ch >= '0' && ch <= '9')
                    {
                        error = "Leading zeros in {0} are invalid JSON";
                    }
                }
                else
                {
                    while (ch >= '0' && ch <= '9')
                    {
                        buffer.Append((char)Read());
                        ch = reader.Peek();
                    }
                }

                if (ch == '.')
                {
                    buffer.Append((char)Read());
                    ch = reader.Peek();

                    if (ch < '0' || ch > '9')
                        error = "Missing fraction in '{0}'";

                    while (ch >= '0' && ch <= '9')
                    {
                        buffer.Append((char)Read());
                        ch = reader.Peek();
                    }
                }

                if (ch == 'e' || ch == 'E')
                {
                    buffer.Append((char)Read());
                    ch = reader.Peek();

                    if (ch == '+' || ch == '-')
                    {
                        buffer.Append((char)Read());
                        ch = reader.Peek();
                    }

                    if (ch < '0' || ch > '9')
                        error = "Missing exponent in '{0}'";

                    while (ch >= '0' && ch <= '9')
                    {
                        buffer.Append((char)Read());
                        ch = reader.Peek();
                    }
                }
            }

            string number = buffer.ToString();
            if (error == null && !double.TryParse(number, out result))
                error = InvalidNumber;

            if (error == null)
                return result;

            ReportError(string.Format(CultureInfo.InvariantCulture, error, number));
            return null;
        }

        private bool Accept(int ch)
        {
            int read = Read();
            if (read == ch) return true;
            ReportError(string.Format(CultureInfo.InvariantCulture, ExpectedChar, (char)ch));
            return false;
        }

        private bool Check(int chAccept, bool advance = true)
        {
            int ch = SkipSpaces();
            if (ch == chAccept)
            {
                if (advance) Read();
                return true;
            }
            return false;
        }

        private bool Accept(string s)
        {
            foreach (int ch in s)
            {
                var read = reader.Read();
                if (read != ch)
                {
                    ReportError(string.Format(CultureInfo.InvariantCulture, ExpectedChar, s));
                    return false;
                }
            }
            return true;
        }

        private int SkipSpaces()
        {
            while (true)
            {
                int ch = reader.Peek();
                switch (ch)
                {
                    default:
                        return ch;
                    case ' ':
                    case '\t':
                    case '\r':
                    case '\n':
                        Read();
                        continue;
                }
            }
        }

        private int Read()
        {
            int ch = reader.Read();
            switch (ch)
            {
                default:
                    Column++;
                    return ch;
                case '\n':
                    Line++;
                    Column = 1;
                    return '\n';
                case '\r':
                    if (reader.Peek() != '\n')
                    {
                        Line++;
                        Column = 1;
                    }
                    return '\r';
            }
        }

        /// <summary>
        /// Reports the errors
        /// </summary>
        /// <param name="message"></param>
        /// <param name="line"></param>
        /// <param name="column"></param>
        /// <returns>always returns null, which is useful for writing return ReportError(...) </returns>
        private string ReportError(string message, int line = 0, int column = 0)
        {
            Line = line != 0 ? line : this.Line;
            Column = column != 0 ? column : this.Column;
            ErrorMessage = reader.Peek() >= 0 ? message : Combine(EndOfInput, message);
            return null;
        }

        private string Combine(string s1, string s2)
        {
            if (string.IsNullOrEmpty(s1)) return s2;
            if (string.IsNullOrEmpty(s2)) return s1;
            return s1 + ", " + s2;
        }
    }
}

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
using System.Globalization;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

namespace MP.Json.Validation
{
    public static class Formats
    {

        public static bool? IsValueOfFormat(string value, string format)
        {
            switch (format)
            {
                // Draft 4
                case "date-time":
                    return IsDateTime(value);

                case "email":
                    return IsEmail(value);

                case "hostname":
                    return IsHostname(value);

                case "ipv4":
                    return IsIpv4(value);

                case "ipv6":
                    return IsIpv6(value);

                case "iri":
                case "uri":
                    return IsUri(value);

                // Draft 6
                case "json-pointer":
                    return IsJsonPointer(value);

                case "iri-reference":
                    return IsUriReference(value, true);

                case "uri-reference":
                    return IsUriReference(value);

                case "uri-template":
                    return IsUriTemplate(value);

                // Draft 7
                case "time":
                    return IsTime(value);

                case "date":
                    return IsDate(value);

                case "relative-json-pointer":
                    return IsRelativeJsonPointer(value);

                case "regex":
                    return IsRegex(value);

                case "duration":
                    return IsDuration(value);

                case "uuid":
                    return IsUuid(value);
            }

            return null;
        }

        public static bool IsRegex(string value)
        {
            try
            {
                // TODO: Very expensive call
                new Regex(value);
                return true;
            }
            catch
            {
                return false;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsDigit(char ch) => unchecked((uint)(ch - '0') < 10);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Digit(char ch) => IsDigit(ch) ? ch - '0' : -1;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsLetter(char ch) => unchecked((uint)(ch - 'A' & ~32)) <= 'Z' - 'A';

        public static bool IsDate(string s, int index = 0, int count = -1)
        {
            if (count == -1) count = s.Length - index;
            // \d{4}-(0\d{1}|1[0-2]{1})-(3[01]|0\d|[12]\d)
            // 0000-00-00

            unchecked
            {
                if (count != 10
                    || (uint)Digit(s[index + 0]) > 9
                    || (uint)Digit(s[index + 1]) > 9
                    || (uint)Digit(s[index + 2]) > 9
                    || (uint)Digit(s[index + 3]) > 9
                    || s[index + 4] != '-'
                    || (uint)Digit(s[index + 5]) > 1
                    || (uint)Digit(s[index + 6]) > 9
                    || s[index + 7] != '-'
                    || (uint)Digit(s[index + 8]) > 3
                    || (uint)Digit(s[index + 9]) > 9
                    )
                    return false;

                // Check Month
                int month = Digit(s[index + 5]) * 10 + Digit(s[index + 6]);
                int day = Digit(s[index + 8]) * 10 + Digit(s[index + 9]);

                if (month < 1 || month > 12 || day < 1 || day > 31) return false;

                switch (month)
                {
                    case 2:
                        return day <= 29 && DateTime.TryParse(s.Substring(0, count), out _);
                    case 4:
                    case 6:
                    case 9:
                    case 11:
                        return day <= 30;
                }

                return true;
            }
        }

        public static bool IsTime(string s, int index = 0, int count = -1)
        {
            if (count == -1) count = s.Length - index;

            // (2[0-3]|[01]\d):([0-5]\d):(60|[0-5]\d)(\.\d+)?(Z|[+-](2[0-3]|[01]\d):([0-5]\d))?
            unchecked
            {
                if (count < 8
                    || (uint)Digit(s[index + 0]) > 9
                    || (uint)Digit(s[index + 1]) > 9
                    || s[index + 2] != ':'
                    || (uint)Digit(s[index + 3]) > 9
                    || (uint)Digit(s[index + 4]) > 9
                    || s[index + 5] != ':'
                    || (uint)Digit(s[index + 6]) > 9
                    || (uint)Digit(s[index + 7]) > 9)
                    return false;

                var hour = Digit(s[index + 0]) * 10 + Digit(s[index + 1]);
                var min = Digit(s[index + 3]) * 10 + Digit(s[index + 4]);
                var sec = Digit(s[index + 6]) * 10 + Digit(s[index + 7]);

                // NOTE: Seconds is from 00-60. 60 is a leap second.
                if (hour >= 24 || min >= 60 || sec >= 61)
                    return false;

                // Handle subseconds
                int pos = 8;
                if (pos < count && s[index + pos] == '.')
                {
                    pos++;
                    while (pos < count && IsDigit(s[index + pos]))
                        pos++;

                    // Fail if there was no digits
                    if (pos == 9) return false;
                }

                // Time zone information
                if (pos < count)
                {
                    var ch = s[index + pos];
                    pos++;
                    if ((ch & ~32) != 'Z') // case-insensitive Z test
                    {
                        if (ch != '+' && ch != '-'
                            || count - pos != 5
                            || (uint)Digit(s[index + pos + 0]) > 9
                            || (uint)Digit(s[index + pos + 1]) > 9
                            || s[index + pos + 2] != ':'
                            || (uint)Digit(s[index + pos + 3]) > 9
                            || (uint)Digit(s[index + pos + 4]) > 9)
                            return false;

                        var tzhour = Digit(s[index + pos + 0]) * 10 + Digit(s[index + pos + 1]);
                        var tzmin = Digit(s[index + pos + 3]) * 10 + Digit(s[index + pos + 4]);
                        if (tzhour >= 24 || tzmin >= 60)
                            return false;

                        pos += 5;
                    }
                }

                return pos == count;
            }
        }

        public static bool IsDateTime(string s) 
            => s.Length >= 10
                && IsDate(s, 0, 10)
                && (s.Length == 10 
                    || ((s[10] & ~32) == 'T') // case-insensitive T test
                        && IsTime(s, 11, s.Length - 11));
        //&& DateTimeOffset.TryParse(value,
        //    CultureInfo.InvariantCulture,
        //    DateTimeStyles.None, out _);

        public static bool IsDuration(string s)
            => FormatRegexes.Duration.IsMatch(s);


        public static bool IsBoolean(string s) =>
            s.Length == 4 && (s[0] & ~32) == 'T' && (s[1] & ~32) == 'R' && (s[2] & ~32) == 'U' && (s[3] & ~32) == 'E'
            || s.Length == 5 && (s[0] & ~32) == 'F' && (s[1] & ~32) == 'A' && (s[2] & ~32) == 'L' && (s[3] & ~32) == 'S' && (s[4] & ~32) == 'E';

        public static bool IsNumber(string s) => double.TryParse(s, NumberStyles.Float, CultureInfo.InvariantCulture, out _);

        public static bool ValidateInteger(string s)
        {
            if (string.IsNullOrEmpty(s)) return false;
            int start = s[0] != '-' ? 0 : 1;
            int pos = start;
            while (pos < start && IsDigit(s[pos])) pos++;
            return pos == s.Length && pos > start;
        }

        public static bool IsEmail(string value) => FormatRegexes.Email.IsMatch(value);

        public static bool IsHostname(string value) => !string.IsNullOrEmpty(value) && value.Length < 254 && FormatRegexes.Hostname.IsMatch(value);

        public static bool IsIpv4(string value) => FormatRegexes.Ipv4.IsMatch(value);

        public static bool IsIpv6(string value)
            => IPAddress.TryParse(value, out IPAddress ip)
                && ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6;

        public static bool IsJsonPointer(string value) => FormatRegexes.JsonPointer.IsMatch(value);

        public static bool IsRelativeJsonPointer(string value) => FormatRegexes.RelativeJsonPointer.IsMatch(value);

        public static bool IsUri(string value)
        {
            bool good = Uri.IsWellFormedUriString(value, UriKind.Absolute);
            if (good || !IsInternational(value))
                return good;

            // .NET regression in Uri.IsWellFormedUriString()
            // International strings don't handle Uri encoding well

            if (!AreUriCharactersValid(value, true))
                return false;

            string reencoded = Reencode(value);
            return Uri.IsWellFormedUriString(reencoded, UriKind.Absolute);
        }

        public static string Reencode(string s)
        {
            int index = s.IndexOf('%');
            if (index < 0) return s;

            var sb = new StringBuilder(s.Length);
            for (int i = 0; i < s.Length; i++)
            {
                char ch = s[i];

                // https://github.com/dotnet/corefx/issues/19630

                if (s[i] == '%' && i + 2 < s.Length && s[i + 1] == '2' && s[i + 2] == '0')
                {
                    int hex1 = Hex(s[i + 1]);
                    int hex2 = Hex(s[i + 2]);

                    if (hex1 >= 0 && hex2 >= 0)
                    {
                        char hex = (char)(hex1 * 16 + hex2);
                        switch (hex)
                        {
                            case ' ':
                                sb.Append('+');
                                i += 2;
                                break;
                            case '/':
                            case '?':
                            case '#':
                                break;
                            default:
                                if (!IsUriCharacterValid(ch)) break;
                                sb.Append(ch);
                                i += 2;
                                continue;
                        }
                    }

                    i += 1;
                    sb.Append('+');
                    continue;
                }
                sb.Append(ch);
            }

            return sb.ToString();
        }

        public static bool IsInternational(string value)
        {
            foreach (char ch in value)
                if (ch >= 0x80)
                    return true;
            return false;
        }

        public static bool IsUriReference(string value, bool international = false)
            => FormatRegexes.UriReference.IsMatch(value)
                && Formats.AreUriCharactersValid(value, international);

        /// <summary>
        /// Is string a UriTemplate (https://tools.ietf.org/html/rfc6570)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsUriTemplate(string value)
        {
            var sb = new StringBuilder(value.Length);

            int start = 0;
            while (start < value.Length)
            {
                int index = value.IndexOf('{', start);
                int index2 = value.IndexOf('}', start);

                if (index < 0)
                {
                    if (index2 >= 0) return false;
                    sb.Append(value, start, value.Length - start);
                    break;
                }

                // This handles both missing close (index2 < 0) and close before open (index2 < index)
                if (index2 < index) return false;

                sb.Append(value, start, index - start);
                sb.Append('x');
                start = index2 + 1;

                index++;
                if ("+#./;?&=,!@!".IndexOf(value[index]) >= 0)
                    index++;


                // Check for {var*} and {var:maxLength}
                if (index2 > 0 && value[index2 - 1] == '*')
                {
                    // Exploding term
                    index2--;
                }
                else
                {
                    int colonIndex = value.IndexOf(':', index, index2 - index);
                    if (colonIndex > index)
                    {
                        colonIndex++;
                        if (!int.TryParse(value.Substring(colonIndex, index2 - colonIndex), out _))
                            return false;
                        index2 = colonIndex - 1;
                    }
                }

                // Ignore zero length terms
                if (index2 - index <= 0)
                    return false;


                string terms = value.Substring(index + 1, index2 - (index + 1));
                if (terms.IndexOf('{') >= 0)
                    return false;

                string[] split = terms.Split(',');
                foreach (string term in split)
                {
                    if (term.Length == 0) return false;
                    for (int i = 0; i < term.Length; i++)
                    {
                        char ch = term[i];
                        if (!char.IsLetterOrDigit(term[i]) && ch != '_')
                        {
                            if (ch == '%' && i + 2 < term.Length
                                                          && IsHexDigit(term[i + 1])
                                                          && IsHexDigit(term[i + 2]))
                                i += 2;
                            else if (!(ch == '.' && (i == 0 || term[i - 1] != '.')))
                                return false;
                        }
                    }
                }
            }

            return Uri.IsWellFormedUriString(sb.ToString(), UriKind.RelativeOrAbsolute);
        }

        public static bool IsUuid(string value)
        {
            return Regex.IsMatch(value,
                @"^[\dA-F]{8}\-[\dA-F]{4}\-[\dA-F]{4}\-[\dA-F]{4}\-[\dA-F]{12}$",
                RegexOptions.IgnoreCase | RegexOptions.Compiled);
        }

        public static bool AreUriCharactersValid(string uri, bool international = false)
        {
            for (int i = 0; i < uri.Length; i++)
            {
                char ch = uri[i];
                if (!IsUriCharacterValid(ch, international))
                    return false;

                if (ch == '%')
                {
                    if (i + 2 >= uri.Length || !IsHexDigit(uri[i + 1]) || !IsHexDigit(uri[i + 2]))
                        return false;
                    i += 2;
                }
            }
            return true;
        }

        public static bool IsUriCharacterValid(char ch, bool international = false)
        {
            if (ch <= 32 || ch >= 127 && (!international || ch == 127))
                return false;

            switch (ch)
            {
                case '"':
                case '^':
                case '`':
                case '|':
                case '\\':
                case '{':
                case '}':
                case '<':
                case '>':
                    return false;
            }
            return true;
        }

        public static bool IsHexDigit(char ch)
        {
            return ch >= '0' && ch <= '9' || ch >= 'a' && ch <= 'f' || ch >= 'A' && ch <= 'F';
        }

        public static int Hex(char ch)
        {
            if (ch >= '0' && ch <= '9')
                return ch - '0';

            ch = (char)(ch & ~32);
            if (ch >= 'A' && ch <= 'F')
                return ch - 'A';

            return -1;
        }

    }
}

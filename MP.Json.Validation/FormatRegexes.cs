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
using System.Text;
using System.Text.RegularExpressions;

namespace MP.Json.Validation
{
    /// <summary>
    /// Standard format regexes, kept in a separate class to prevent initialization if not used.
    /// </summary>

    public class FormatRegexes
    {
        private const RegexOptions StandardOptions =
            0 // RegexOptions.Compiled
            | RegexOptions.CultureInvariant
            | RegexOptions.ExplicitCapture
            | RegexOptions.IgnoreCase
            | RegexOptions.IgnorePatternWhitespace
            | RegexOptions.Singleline;

        public static readonly Regex Email = new Regex(@"^([A-Z\d+.\-])+\@([A-Z\d\-.])+\.[A-Z\d\-]+$", StandardOptions);

        public static readonly Regex Hostname = new Regex(@"^([A-Z\d][A-Z\d-]{0,63}\.)*([A-Z\d][A-Z\d-]{1,63})$", StandardOptions);

        public static readonly Regex Ipv4 = new Regex(@"^((25[0-5]|2[0-4]\d|[01]?\d\d?)\.){3}(25[0-5]|2[0-4]\d|[01]?\d\d?)$", StandardOptions);

        public static readonly Regex Uuid = new Regex(@"^[\dA-F]{8}\-[\dA-F]{4}\-[\dA-F]{4}\-[\dA-F]{4}\-[\dA-F]{12}$", StandardOptions);

        public static readonly Regex UriReference = new Regex(@"^(([^:/?#]+):)?(//([^/?#]*))?([^?#]*)(\?([^#]*))?(\#(.*))?", StandardOptions);

        public static readonly Regex Duration = new Regex(@"^(-?)P(?=\d|T\d)((\d+)Y)?((\d+)M)?((\d+)([DW]))?(T((\d+)H)?((\d+)M)?((\d+(\.\d+)?)S)?)?$", StandardOptions);

        public static readonly Regex JsonPointer = new Regex(@"^(/([^~]|~[01])*)?$", StandardOptions);

        public static readonly Regex RelativeJsonPointer = new Regex(@"^[0-9]+(\#|/([^#~]|~[01])*)?$", StandardOptions);
    }
}

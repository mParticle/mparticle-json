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
    public static class SchemaRegexCache
    {
        private const int DefaultCapacity = 200;
        private const string NumberRegex = @"^-?\d+(\.\d+)?([eE][+-]?\d+)?$";
        private const string BooleanRegex = "^([Tt][Rr][Uu][Ee]|[Ff][Aa][Ll][Ss][Ee])$";

        private static LruCache<string, SchemaRegex> cache = new LruCache<string, SchemaRegex>(DefaultCapacity)
        {
            [NumberRegex] = new SchemaRegex(NumberRegex, SchemaRegex.RegexType.Number),
            [BooleanRegex] = new SchemaRegex(BooleanRegex, SchemaRegex.RegexType.Boolean),
        };

        public static SchemaRegex Lookup(string pattern)
        {
            if (pattern == null) return null;

            lock (cache)
            {
                SchemaRegex regex = cache[pattern];
                if (regex != null)
                    return regex;
                cache[pattern] = regex = new SchemaRegex(pattern);
                return regex;
            }
        }
    }
}

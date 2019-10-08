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
using System.Runtime.Serialization;
using System.Text;

namespace MP.Json.Validation
{
    /// <summary>
    /// Standard schema Keywords recognized by the Schema object
    /// </summary>
    public static class KeywordUtils
    {
        #region Variables
        private static readonly Dictionary<Keyword, string> keyword2Name
            = new Dictionary<Keyword, string>()
            {
                [Keyword.Metadata] = "<metadata>",
                [Keyword.None] = "<none>"
            };

        private static readonly Dictionary<string, Keyword> name2Keyword
            = name2Keyword = new Dictionary<string, Keyword>()
            {
                ["additionalItems"] = Keyword.AdditionalItems,
                ["additionalProperties"] = Keyword.AdditionalProperties,
                ["allOf"] = Keyword.AllOf,
                ["anyOf"] = Keyword.AnyOf,
                ["$comment"] = Keyword._Comment,
                ["const"] = Keyword.Const,
                ["contains"] = Keyword.Contains,
                ["contentEncoding"] = Keyword.ContentEncoding,
                ["contentMediaType"] = Keyword.ContentMediaType,
                ["contentSchema"] = Keyword.ContentSchema,
                ["default"] = Keyword.Default,
                ["definitions"] = Keyword.Definitions,
                ["$defs"] = Keyword._Defs,
                ["dependencies"] = Keyword.Dependencies,
                ["dependentRequired"] = Keyword.DependentRequired,
                ["dependentSchemas"] = Keyword.DependentSchemas,
                ["deprecated"] = Keyword.Deprecated,
                ["description"] = Keyword.Description,
                ["else"] = Keyword.Else,
                ["enum"] = Keyword.Enum,
                ["examples"] = Keyword.Examples,
                ["exclusiveMaximum"] = Keyword.ExclusiveMaximum,
                ["exclusiveMinimum"] = Keyword.ExclusiveMinimum,
                ["format"] = Keyword.Format,
                ["id"] = Keyword.Id,
                ["$id"] = Keyword._Id,
                ["if"] = Keyword.If,
                ["items"] = Keyword.Items,
                ["maxContains"] = Keyword.MaxContains,
                ["maximum"] = Keyword.Maximum,
                ["maxLength"] = Keyword.MaxLength,
                ["maxProperties"] = Keyword.MaxProperties,
                ["maxItems"] = Keyword.MaxItems,
                ["minContains"] = Keyword.MinContains,
                ["minimum"] = Keyword.Minimum,
                ["minItems"] = Keyword.MinItems,
                ["minLength"] = Keyword.MinLength,
                ["minProperties"] = Keyword.MinProperties,
                ["multipleOf"] = Keyword.MultipleOf,
                ["not"] = Keyword.Not,
                ["oneOf"] = Keyword.OneOf,
                ["pattern"] = Keyword.Pattern,
                ["patternProperties"] = Keyword.PatternProperties,
                ["properties"] = Keyword.Properties,
                ["propertyNames"] = Keyword.PropertyNames,
                ["readOnly"] = Keyword.ReadOnly,
                ["$recursiveAnchor"] = Keyword._RecursiveAnchor,
                ["$recursiveRef"] = Keyword._RecursiveRef,
                ["$ref"] = Keyword._Ref,
                ["required"] = Keyword.Required,
                ["$schema"] = Keyword._Schema,
                ["then"] = Keyword.Then,
                ["title"] = Keyword.Title,
                ["type"] = Keyword.Type,
                ["uniqueItems"] = Keyword.UniqueItems,
                ["writeOnly"] = Keyword.WriteOnly,
            };
        #endregion

        #region Constructor
        static KeywordUtils()
        {
            foreach (var v in name2Keyword)
                keyword2Name[v.Value] = v.Key;
        }
        #endregion


        public static string GetText(this Keyword keyword, SchemaVersion draft=0)
        {
            return GetTextCore(keyword, draft)
                ?? KeywordCase(keyword.ToString());
        }

        public static string GetText(this ErrorType keyword, SchemaVersion draft = 0)
        {
            return GetTextCore((Keyword)keyword, draft) 
                ?? KeywordCase(keyword.ToString());
        }

        private static string GetTextCore(Keyword keyword, SchemaVersion draft = 0)
        {
            if (draft == 0)
            {
                // We default to Draft7, 
                // since later drafts introduces/replaces a number of keywords
                // and might not readable by earlier versions
                // but earlier drafts may be readable by later drafts
                draft = SchemaVersion.Draft7;
            }

            switch (keyword)
            {
                case Keyword.DependentSchemas:
                case Keyword.DependentRequired:
                    if (draft < SchemaVersion.Draft201909)
                        return "dependencies";
                    break;
            }

            keyword2Name.TryGetValue(keyword, out string result);
            return result;
        }

        private static string KeywordCase(string keyword)
        {
            var sb = new StringBuilder(keyword);

            if (sb[0] != '_')
                sb[0] = char.ToLower(sb[0]);
            else
            {
                sb[0] = '$';
                sb[1] = char.ToLower(sb[1]);
            }
            return sb.ToString();
        }


        public static string GetTypeText(JsonType type)
        {
            switch(type)
            {
                case JsonType.Array: return "array";
                case JsonType.Boolean: return "boolean";
                case JsonType.Number: return "number";
                case JsonType.Null: return "null";
                case JsonType.Object: return "object";
                case JsonType.String: return "string";
                case JsonType.Undefined: return "undefined";
            }
            return "unknown";
        }

        public static Keyword ParseKeyword(string text)
            => name2Keyword.TryGetValue(text, out Keyword result) ? result : Keyword.None;


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static TypeFlags SchemaFlagsToTypeFlags(SchemaFlags flags)
        {
            return (TypeFlags)((long)(flags & SchemaFlags.TypeAll) >> 56);
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static SchemaFlags TypeFlagsToSchemaFlags(TypeFlags flags)
        {
            return (SchemaFlags)((long)(flags) << 56);
        }


    }
}

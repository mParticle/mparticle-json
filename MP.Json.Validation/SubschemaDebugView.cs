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

using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using static MP.Json.Validation.BitUtils;

namespace MP.Json.Validation
{
    [DebuggerDisplay("{ToString(),nq}")]
    public partial class Subschema
    {
        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        internal DebugNode[] DebuggerKeywords
        {
            get
            {
                var list = new List<DebugNode>();
                var stored = Flags & SchemaFlags.StoredProperties;

                var types = (Flags & SchemaFlags.TypeAll);
                if (types != SchemaFlags.TypeAll && types != 0)
                {
                    list.Add(new DebugNode
                    {
                        Key = "type",
                        Value = string.Join(",", GetValidTypes())
                    });
                }

                for (SchemaFlags flags = stored & ~SchemaFlags.Metadata;
                    flags != 0;
                    flags = (SchemaFlags)RemoveLowestBit((long)flags))
                {
                    Keyword keyword = (Keyword)IndexOfLowestBit((long)flags);
                    object value = GetData(keyword);

                    list.Add(new DebugNode
                    {
                        Key = keyword.GetText() ?? string.Empty,
                        Value = value
                    });
                }

                var metadata = Metadata;
                if (metadata != null)
                {
                    foreach(var v in metadata)
                        list.Add(new DebugNode { Key = v.Key ?? string.Empty, Value = v.Value });
                }

                list.Sort((a, b) =>
                {
                    if (a?.Key == b?.Key) return 0;
                    if (a?.Key == null) return -1;
                    if (b?.Key == null) return 1;
                    return a.Key.CompareTo(b.Key);
                });
                return list.ToArray();
            }
        }

        [DebuggerDisplay("{ValueText,nq}", Name = "{Key,nq}", Type = "{Value?.GetType().FullName,nq}" )]
        internal class DebugNode
        {
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            public string Key;
            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public object Value;
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            public string ValueText
            {
                get
                {
                    var value = Value;
                    if (value == null)
                        return "null";
                    if (value is System.Array array)
                        return $"Count = {array.Length}";
                    return value.ToString();
                }
            }
        }
    }
}

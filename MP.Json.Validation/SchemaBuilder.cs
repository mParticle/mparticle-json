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
using System.Linq;
using System.Text;
using static MP.Json.Validation.BitUtils;
using Pair = System.Collections.Generic.KeyValuePair<string, object>;

namespace MP.Json.Validation
{
    internal class SchemaBuilder
    {
        public MPJson Json;
        public MPSchema Document;
        public Dictionary<string, object> Ids = new Dictionary<string, object>();
        public Dictionary<string, SchemaReference> Definitions = new Dictionary<string, SchemaReference>();
        public HashSet<string> References = new HashSet<string>();
        public Subschema Root;
        public Dictionary<string, Subschema> ids;

        public SchemaBuilder(MPSchema doc, MPJson json)
        {
            this.Document = doc;
            this.Json = json;

            // Find references
            GatherIdsAndReferences(json.Value);

            // Build schemas
            Root = From(json.Value, doc);

            string rootReference = doc.ConvertReference("");
            References.Remove(rootReference);
            Definitions[rootReference] = new SchemaReference(rootReference, Root);

            foreach (string r in References)
            {
                if (!Ids.TryGetValue(r, out object value))
                    value = JsonPointer.Find(json, r);
                Definitions[r] = new SchemaReference(r, value != null ? From(value, doc) : null)
                {
                    Id = Definitions.Count
                };
            }

            // Fix up references
            foreach (var reference in Definitions.Values)
                if (reference.Schema != null)
                    ReplaceReferences(reference.Schema);

            // Compression
        }

        private void ReplaceReferences(Subschema schema)
        {
            var origRef = schema.Ref;
            if (origRef != null && !origRef.Resolved)
            {
                if (Definitions.TryGetValue(origRef.Uri, out SchemaReference newRef)
                    && newRef.Schema != schema)
                {
                    schema.Ref = newRef;

                    // We will init this anyway, if any references still exist in memory
                    origRef.Schema = newRef.Schema;
                    origRef.Version = newRef.Version;
                }
                origRef.Resolved = true;
            }

            foreach (var subschema in schema.GetChildSchemas())
                ReplaceReferences(subschema);
        }

        private void GatherIdsAndReferences(object json)
        {
            if (json is KeyValuePair<string, object>[] obj)
            {
                foreach (var kv in obj)
                {
                    if (kv.Value is string relReference)
                    {
                        if (kv.Key == "$id" || kv.Key == "id")
                        {
                            string reference = Document.ConvertReference(relReference);
                            Ids[reference] = obj;
                        }
                        else if (kv.Key == "$ref")
                        {
                            string reference = Document.ConvertReference(relReference);
                            References.Add(reference);
                        }
                    }
                    else
                    {
                        GatherIdsAndReferences(kv.Value);
                    }
                }
            }
            else if (json is object[] array)
            {
                foreach (object elem in array)
                    GatherIdsAndReferences(elem);
            }
        }


        private void AddId(string uri, Subschema schema)
        {
            if (ids == null)
                ids = new Dictionary<string, Subschema>();
            ids[uri] = schema;
        }

        /// <summary>
        /// Builds a schema document from a JsonLite object tree
        /// </summary>
        /// <param name="schema"></param>
        /// <param name="doc"></param>
        /// <returns></returns>
        public Subschema From(object json, MPSchema doc)
        {
            var map = json as KeyValuePair<string, object>[];
            if (map == null || map.Length == 0)
            {
                if (map != null)
                    return SchemaConstants.Everything;
                if (json is bool b)
                    return b ? SchemaConstants.Everything : SchemaConstants.Nothing;
                return null;
            }

            var subschema = new Subschema
            {
                data = new object[(int)Keyword.MAX_STORED],
                Flags = SchemaFlags.TypeAll | SchemaFlags.Uncompressed
            };

            bool exclusiveMinimum = false;
            bool exclusiveMaximum = false;
            bool isNothing = false;
            List<Pair> metadata = null;
            foreach (var kv in map)
            {
                var k = KeywordUtils.ParseKeyword(kv.Key);
                object v = kv.Value;
                Subschema child;
                Subschema[] list;
                string name;
                object[] array;
                string[] stringList;
                double d;

                switch (k)
                {
                    case Keyword.AllOf:
                        list = ReadSchemaList(v, doc);
                        if (list == null || list.Length < 1) return null;
                        subschema.AllOf = list;
                        break;

                    case Keyword.AnyOf:
                        list = ReadSchemaList(v, doc);
                        if (list == null || list.Length < 1) return null;
                        subschema.AnyOf = list;
                        break;

                    case Keyword.OneOf:
                        list = ReadSchemaList(v, doc);
                        if (list == null || list.Length < 1) return null;
                        subschema.OneOf = list;
                        break;

                    case Keyword.Const:
                        if (!doc.LimitVersion(SchemaVersion.Draft6, true)) return null;
                        subschema.Const = MPJson.From(v);
                        break;

                    case Keyword.Enum:
                        array = v as object[];
                        if (array == null || array.Length < 1) return null;
                        subschema.Enum = MPJson.From(array);
                        break;

                    case Keyword.Not:
                        child = From(v, doc);
                        if (child == null) return null;
                        subschema.Not = child;
                        if (map.Length == 1)
                        {
                            if (child.Type == TypeFlags.None)
                                return SchemaConstants.Everything;
                            if (child == SchemaConstants.Everything)
                                return SchemaConstants.Nothing;
                        }
                        break;

                    case Keyword.Type:
                        var flags = subschema.Flags & ~SchemaFlags.TypeAll;
                        if (v is string)
                        {
                            flags = ToTypeFlag((string)v);
                            if (flags == 0)
                                return null;
                        }
                        else
                        {
                            stringList = ReadStringList(v);
                            if (stringList == null) return null;
                            foreach (string type in stringList)
                            {
                                var flag = ToTypeFlag(type);
                                if (flag == 0)
                                    return null;
                                flags |= flag;
                            }
                        }

                        subschema.Flags = (subschema.Flags & ~SchemaFlags.TypeAll) | flags;
                        break;

                    case Keyword.If:
                        if (!doc.LimitVersion(SchemaVersion.Draft7, true)) return null;
                        child = From(v, doc);
                        if (child == null) return null;
                        subschema.If = child;
                        break;

                    case Keyword.Then:
                        if (!doc.LimitVersion(SchemaVersion.Draft7, true)) return null;
                        child = From(v, doc);
                        if (child == null) return null;
                        subschema.Then = child;
                        break;

                    case Keyword.Else:
                        if (!doc.LimitVersion(SchemaVersion.Draft7, true)) return null;
                        child = From(v, doc);
                        if (child == null) return null;
                        subschema.Else = child;
                        break;

                    // Number attributes
                    case Keyword.ExclusiveMaximum:
                        if (v is double && doc.LimitVersion(SchemaVersion.Draft6, true))
                            subschema.ExclusiveMaximum = (double)v;
                        else if (v is bool && doc.LimitVersion(SchemaVersion.Draft4, false))
                            exclusiveMaximum = (bool)v;
                        else
                            return null;
                        break;

                    case Keyword.ExclusiveMinimum:
                        if (v is double && doc.LimitVersion(SchemaVersion.Draft6, true))
                            subschema.ExclusiveMinimum = (double)v;
                        else if (v is bool && doc.LimitVersion(SchemaVersion.Draft4, false))
                            exclusiveMinimum = (bool)v;
                        else
                            return null;
                        break;

                    case Keyword.Maximum:
                        if (!(v is double)) return null;
                        if (exclusiveMaximum)
                            subschema.ExclusiveMaximum = (double)v;
                        else
                            subschema.Maximum = (double)v;
                        break;

                    case Keyword.Minimum:
                        if (!(v is double)) return null;
                        if (exclusiveMinimum)
                            subschema.ExclusiveMinimum = (double)v;
                        else
                            subschema.Minimum = (double)v;
                        break;

                    case Keyword.MultipleOf:
                        if (!(v is double) || (d = (double)v) <= 0) return null;
                        subschema.MultipleOf = d;
                        break;

                    // String attributes
                    case Keyword.Format:
                        name = v as string;
                        if (name == null) return null;
                        subschema.Format = name;
                        break;

                    case Keyword.MaxLength:
                        if (!IsNonnegativeInteger(v)) return null;
                        subschema.MaxLength = (double)v;
                        break;

                    case Keyword.MinLength:
                        if (!IsNonnegativeInteger(v)) return null;
                        subschema.MinLength = (double)v;
                        break;

                    case Keyword.Pattern:
                        name = v as string;
                        var regex = SchemaRegexCache.Lookup(name);
                        if (regex == null) return null;
                        subschema.Pattern = regex;
                        break;

                    // Object attributes
                    case Keyword.AdditionalProperties:
                        child = From(v, doc);
                        if (child == null) return null;
                        subschema.AdditionalProperties = child;
                        break;

                    case Keyword.Dependencies:
                        KeyValuePair<string, string[]>[] depRequired;
                        KeyValuePair<string, Subschema>[] schemas;

                        if (!ReadDependencies(v, doc, out schemas, out depRequired)) return null;
                        if (depRequired != null)
                            subschema.DependentRequired = depRequired;
                        if (schemas != null)
                            subschema.DependentSchemas = schemas;
                        break;

                    case Keyword.DependentRequired:
                        if (!doc.LimitVersion(SchemaVersion.Draft201909, true)) return null;
                        if (!ReadDependencies(v, doc, out schemas, out depRequired)) return null;
                        if (schemas != null) return null;
                        subschema.DependentRequired = depRequired;
                        break;

                    case Keyword.DependentSchemas:
                        if (!doc.LimitVersion(SchemaVersion.Draft201909, true)) return null;
                        schemas = ReadPropertySchemas(v, doc);
                        if (schemas == null) return null;
                        subschema.DependentSchemas = schemas;
                        break;

                    case Keyword.PatternProperties:
                        var patternSchemas = ReadPatternSchemas(v, doc);
                        if (patternSchemas == null) return null;
                        subschema.PatternProperties = patternSchemas;
                        break;

                    case Keyword.Properties:
                        schemas = ReadPropertySchemas(v, doc);
                        if (schemas == null) return null;
                        subschema.Properties = schemas;
                        break;

                    case Keyword.MaxProperties:
                        if (!IsNonnegativeInteger(v)) return null;
                        subschema.MaxProperties = (double)v;
                        break;

                    case Keyword.MinProperties:
                        if (!IsNonnegativeInteger(v)) return null;
                        subschema.MinProperties = (double)v;
                        break;

                    case Keyword.PropertyNames:
                        if (!doc.LimitVersion(SchemaVersion.Draft6, true)) return null;
                        child = From(v, doc);
                        if (child == null) return null;
                        subschema.PropertyNames = child;
                        break;

                    case Keyword.Required:
                        stringList = ReadStringList(v);
                        if (stringList == null) return null;
                        subschema.Required = stringList;
                        break;

                    // Array attributes
                    case Keyword.AdditionalItems:
                        child = From(v, doc);
                        if (child == null) return null;
                        subschema.AdditionalItems = child;
                        break;

                    case Keyword.Contains:
                        if (!doc.LimitVersion(SchemaVersion.Draft6, true)) return null;
                        child = From(v, doc);
                        if (child == null) return null;
                        subschema.Contains = child;
                        break;

                    case Keyword.Items:
                        object result = v is object[]? ReadSchemaList(v, doc) : (object)From(v, doc);
                        if (result == null) return null;
                        subschema.Items = result;
                        break;

                    case Keyword.MaxContains:
                        if (!IsNonnegativeInteger(v)) return null;
                        subschema.MaxContains = (double)v;
                        break;

                    case Keyword.MinContains:
                        if (!IsNonnegativeInteger(v)) return null;
                        subschema.MinContains = (double)v;
                        break;

                    case Keyword.MaxItems:
                        if (!IsNonnegativeInteger(v)) return null;
                        subschema.MaxItems = (double)v;
                        break;

                    case Keyword.MinItems:
                        if (!IsNonnegativeInteger(v)) return null;
                        subschema.MinItems = (double)v;
                        break;

                    case Keyword.UniqueItems:
                        if (!(v is bool)) return null;
                        if ((bool)v) subschema.UniqueItems = true;
                        break;

                    // MORE
                    case Keyword._Ref:
                        name = v as string;
                        if (name == null) return null;
                        string reference = doc.ConvertReference(name);
                        var r = subschema.Ref = new SchemaReference(reference);
                        break;

                    case Keyword.Id:
                        if (!doc.LimitVersion(SchemaVersion.Draft4, false))
                            return null;
                        goto DoId;

                    case Keyword._Id:
                        if (!doc.LimitVersion(SchemaVersion.Draft6, true)) return null;

                        DoId:
                        name = v as string;
                        if (name == null) return null;
                        AddMetadata(ref metadata, kv.Key, v);
                        break;

                    case Keyword.ContentSchema:
                    case Keyword.Default:
                        AddMetadata(ref metadata, kv.Key, v);
                        break;

                    case Keyword.Title:
                    case Keyword.Description:
                    case Keyword._Schema:
                        name = v as string;
                        if (name == null) return null;
                        AddMetadata(ref metadata, kv.Key, v);
                        break;

                    case Keyword.Examples:
                        if (!(v is object[])) return null;
                        AddMetadata(ref metadata, kv.Key, v);
                        break;

                    case Keyword._Comment:
                    case Keyword.ContentEncoding:
                    case Keyword.ContentMediaType:
                        if (!doc.LimitVersion(SchemaVersion.Draft7, true)) return null;
                        name = v as string;
                        if (name == null) return null;
                        AddMetadata(ref metadata, kv.Key, v);
                        break;

                    case Keyword.Deprecated:
                    case Keyword.ReadOnly:
                    case Keyword.WriteOnly:
                        if (!doc.LimitVersion(SchemaVersion.Draft7, true)) return null;
                        if (!(v is bool)) return null;
                        if ((bool)v) AddMetadata(ref metadata, kv.Key, v);
                        break;

                    case Keyword._Defs:
                        if (!doc.LimitVersion(SchemaVersion.Draft201909, true)) return null;
                        goto case Keyword.Definitions;

                    case Keyword.Definitions:
                        schemas = ReadPropertySchemas(v, doc);
                        if (schemas == null) return null;
                        AddMetadata(ref metadata, kv.Key, schemas);
                        break;
                }
            }

            // Ignore all other keywords if ref is present
            // Draft8 might have changed this,
            // but it is not reflected in the standard test suite
            if ((subschema.Flags & SchemaFlags._Ref) != 0)
            {
                subschema.Flags &= ~SchemaFlags.StoredProperties;
                subschema.Flags |= SchemaFlags.TypeAll | SchemaFlags._Ref;
            }

            if (metadata != null)
                AssignMetadata(subschema, metadata);

            CompressData(subschema);
            return subschema;
        }

        private void AssignMetadata(Subschema schema, List<Pair> metadata)
        {
            metadata.Sort((a, b) => a.Key.CompareTo(b.Key));
            schema.Metadata = metadata.ToArray();
        }

        private void CompressData(Subschema schema)
        {
            object[] uncompressedData = (object[])schema.data;
            object data = null;

            long storedBits = (long)(schema.Flags & SchemaFlags.StoredProperties);

            int bitCount = BitCount(storedBits);
            if (bitCount > 1)
            {
                int index = 0;
                object[] compressedData = new object[bitCount];
                while (storedBits != 0)
                {
                    int lowestBit = IndexOfLowestBit(storedBits);
                    compressedData[index++] = uncompressedData[lowestBit];
                    storedBits -= 1L << lowestBit;
                }
                data = compressedData;
            }
            else if (bitCount == 1)
            {
                data = uncompressedData[IndexOfLowestBit(storedBits)];
            }

            schema.data = data;
            schema.Flags &= ~SchemaFlags.Uncompressed;
        }

        private static bool IsNonnegativeInteger(object obj)
        {
            return obj is double d && d > 0 && Math.Floor(d) == d;
        }

        private static bool AddMetadata(ref List<KeyValuePair<string, object>> list, string name, object value)
        {
            if (value == null) return false;
            if (list == null) list = new List<KeyValuePair<string, object>>();
            list.Add(new KeyValuePair<string, object>(name, value));
            return true;
        }

        private bool ReadDependencies(object v, MPSchema doc,
            out KeyValuePair<string, Subschema>[] schemaMap, out KeyValuePair<string, string[]>[] requiredMap)
        {
            var map = (KeyValuePair<string, object>[])v;

            List<KeyValuePair<string, Subschema>> schemaList = null;
            List<KeyValuePair<string, string[]>> requiredList = null;

            schemaMap = null;
            requiredMap = null;

            for (int i = 0; i < map.Length; i++)
            {
                string key = map[i].Key;
                object value = map[i].Value;
                if (value is object[])
                {
                    string[] v2 = ReadStringList(map[i].Value);
                    if (v2 == null) return false;
                    if (requiredList == null) requiredList = new List<KeyValuePair<string, string[]>>();
                    requiredList.Add(new KeyValuePair<string, string[]>(key, v2));
                }
                else
                {
                    var v2 = From(map[i].Value, doc);
                    if (v2 == null) return false;
                    if (schemaList == null) schemaList = new List<KeyValuePair<string, Subschema>>();
                    schemaList.Add(new KeyValuePair<string, Subschema>(key, v2));
                }
            }

            if (schemaList != null)
                schemaMap = schemaList.ToArray();

            if (requiredList != null)
                requiredMap = requiredList.ToArray();
            else if (schemaList == null)
                requiredMap = Array.Empty<KeyValuePair<string, string[]>>();

            return true;
        }

        private KeyValuePair<string, Subschema>[] ReadPropertySchemas(object v, MPSchema doc)
        {
            var map = (KeyValuePair<string, object>[])v;
            var result = new KeyValuePair<string, Subschema>[map.Length];
            for (int i = 0; i < map.Length; i++)
            {
                string key = map[i].Key;
                var v2 = From(map[i].Value, doc);
                if (v2 == null) return null;
                result[i] = new KeyValuePair<string, Subschema>(key, v2);
            }
            return result;
        }

        private KeyValuePair<SchemaRegex, Subschema>[] ReadPatternSchemas(object v, MPSchema doc)
        {
            var map = (KeyValuePair<string, object>[])v;
            var result = new KeyValuePair<SchemaRegex, Subschema>[map.Length];
            for (int i = 0; i < map.Length; i++)
            {
                string key = map[i].Key;
                var regex = SchemaRegexCache.Lookup(key);
                if (regex == null) return null;

                var v2 = From(map[i].Value, doc);
                if (v2 == null) return null;
                result[i] = new KeyValuePair<SchemaRegex, Subschema>(regex, v2);
            }
            return result;
        }

        private static string[] ReadStringList(object v)
        {
            object[] array = v as object[];
            if (array == null) return null;

            string[] result = new string[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                string elem = array[i] as string;
                if (elem == null) return null;
                result[i] = elem;
            }

            // Strings must be unique
            if (result.Length > 1 && new HashSet<string>(result).Count != result.Length)
                return null;

            return result;
        }

        private Subschema[] ReadSchemaList(object v, MPSchema doc)
        {
            object[] array = v as object[];
            if (array == null)
                return null;

            var list = new Subschema[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                Subschema child = From(array[i], doc);
                if (child == null)
                    return null;
                list[i] = child;
            }

            return list;
        }

        private static SchemaFlags ToTypeFlag(string s)
        {
            switch (s)
            {
                case "string": return SchemaFlags.TypeString;
                case "number": return SchemaFlags.TypeNumber;
                case "integer": return SchemaFlags.TypeInteger;
                case "boolean": return SchemaFlags.TypeBoolean;
                case "null": return SchemaFlags.TypeNull;
                case "object": return SchemaFlags.TypeObject;
                case "array": return SchemaFlags.TypeArray;
            }
            return 0;
        }
    }
}

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
using System.Diagnostics;
using System.Runtime.CompilerServices;
using static MP.Json.Validation.BitUtils;

namespace MP.Json.Validation
{
    /// <summary>
    /// Represents a subschema
    /// </summary>
    public partial class Subschema
    {
        #region Variables
        /// <summary>
        /// Contains flags indicating which keywords are stored in the data
        /// </summary>
        internal SchemaFlags Flags;

        /// <summary>
        /// The data hold only the properties that are stored in the keyword flags
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal object data;
        #endregion

        #region Setters and Getters
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetIndex(Keyword keyword)
        {
            long flags = (long)(this.Flags & SchemaFlags.StoredProperties);
            long bit = (flags & 1L << (int)keyword);
            if (bit == 0) return -2;

            // If is only one bit, then data is object
            // Otherwise, data is an object
            return (flags != bit) ? BitCount(flags & bit - 1) : -1;
        }

        public object GetData(Keyword keyword)
        {
            // The Uncompressed bit is also the sign bit (bit 63)
            if (Flags >= 0) // Same as (Flags & MPSchemaFlags.UncompressedBit)==0
            {
                // Compressed data
                int index = GetIndex(keyword);
                return index >= 0
                    ? (data as object[])[index]
                    : index == -1
                    ? data : null;
            }
            else
            {
                // Uncompressed data
                long flags = (long)(this.Flags & SchemaFlags.StoredProperties);
                long bit = (flags & 1L << (int)keyword);
                return bit != 0 ? (data as object[])[(int)keyword] : null;
            }
        }

        private void SetData(Keyword keyword, object value)
        {
            // The Uncompressed bit is also the sign bit (bit 63)
            if (Flags < 0) // Same as (Flags & MPSchemaFlags.UncompressedBit)!=0
            {
                // Uncompressed data
                Flags |= (SchemaFlags)(1L << (int)keyword);
                (data as object[])[(int)keyword] = value;
            }
            else
            {
                // Compressed data
                int index = GetIndex(keyword);
                if (index >= 0)
                    (data as object[])[index] = value;
                else if (index == -1)
                    data = value;
                else
                    throw new InvalidOperationException("No space allocated");
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets any metadata attached to the schema by name
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public MPJson GetMetadata(string key)
        {
            object result = Metadata?.GetProperty(key);
            return result != null ? MPJson.From(result) : MPJson.Undefined;
        }

        /// <summary>
        /// Used Keywords
        /// </summary>
        public KeywordSet Keywords => KeywordSet.Raw((long)(Flags & SchemaFlags.StoredProperties));


        /// <summary>
        /// Type Flags
        /// </summary>
        public TypeFlags Type => KeywordUtils.SchemaFlagsToTypeFlags(Flags);

        /// <summary>
        /// Const
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public MPJson Const
        {
            get => MPJson.From(GetData(Keyword.Const));
            set => SetData(Keyword.Const, value.Value);
        }

        /// <summary>
        /// Enum
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public MPJson Enum
        {
            get => MPJson.From(GetData(Keyword.Enum));
            set => SetData(Keyword.Enum, (object[])value.Value);
        }

        // Number attributes

        /// <summary>
        /// ExclusiveMinimum
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public double? ExclusiveMinimum
        {
            get => (double?)GetData(Keyword.ExclusiveMinimum);
            set => SetData(Keyword.ExclusiveMinimum, value);
        }

        /// <summary>
        /// ExclusiveMaximum
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public double? ExclusiveMaximum
        {
            get => (double?)GetData(Keyword.ExclusiveMaximum);
            set => SetData(Keyword.ExclusiveMaximum, value);
        }

        /// <summary>
        /// Minimum
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public double Minimum
        {
            get => (double?)GetData(Keyword.Minimum) ?? 0;
            set => SetData(Keyword.Minimum, value);
        }

        /// <summary>
        /// Maximum
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public double Maximum
        {
            get => (double?)GetData(Keyword.Maximum) ?? int.MaxValue;
            set => SetData(Keyword.Maximum, value);
        }

        /// <summary>
        /// MultipleOf
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public double? MultipleOf
        {
            get => (double?)GetData(Keyword.MultipleOf);
            set => SetData(Keyword.MultipleOf, value);
        }

        // String attributes

        /// <summary>
        /// Format
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public string Format
        {
            get => (string)GetData(Keyword.Format);
            set => SetData(Keyword.Format, value);
        }

        /// <summary>
        /// MinLength
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public double MinLength
        {
            get => (double?)GetData(Keyword.MinLength) ?? 0;
            set => SetData(Keyword.MinLength, value);
        }

        /// <summary>
        /// MaxLength
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public double MaxLength
        {
            get => (double?)GetData(Keyword.MaxLength) ?? int.MaxValue;
            set => SetData(Keyword.MaxLength, value);
        }

        /// <summary>
        /// Pattern
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public SchemaRegex Pattern
        {
            get => (SchemaRegex)GetData(Keyword.Pattern);
            set => SetData(Keyword.Pattern, value);
        }

        // Object attributes

        /// <summary>
        /// AdditionalProperties
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public Subschema AdditionalProperties
        {
            get => (Subschema)GetData(Keyword.AdditionalProperties);
            set => SetData(Keyword.AdditionalProperties, value);
        }

        /// <summary>
        /// DependentSchemas
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public KeyValuePair<string, Subschema>[] DependentSchemas
        {
            get => (KeyValuePair<string, Subschema>[])GetData(Keyword.DependentSchemas);
            set => SetData(Keyword.DependentSchemas, value);
        }

        /// <summary>
        /// DependentRequired
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public KeyValuePair<string, string[]>[] DependentRequired
        {
            get => (KeyValuePair<string, string[]>[])GetData(Keyword.DependentRequired);
            set => SetData(Keyword.DependentRequired, value);
        }

        /// <summary>
        /// MinProperties
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public double MinProperties
        {
            get => (double?)GetData(Keyword.MinProperties) ?? 0;
            set => SetData(Keyword.MinProperties, value);
        }

        /// <summary>
        /// MaxProperties
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public double MaxProperties
        {
            get => (double?)GetData(Keyword.MaxProperties) ?? int.MaxValue;
            set => SetData(Keyword.MaxProperties, value);
        }

        /// <summary>
        /// Properties
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public KeyValuePair<string, Subschema>[] Properties
        {
            get => (KeyValuePair<string, Subschema>[])GetData(Keyword.Properties);
            set => SetData(Keyword.Properties, value);
        }

        /// <summary>
        /// PatternProperties
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public KeyValuePair<SchemaRegex, Subschema>[] PatternProperties
        {
            get => (KeyValuePair<SchemaRegex, Subschema>[])GetData(Keyword.PatternProperties);
            set => SetData(Keyword.PatternProperties, value);
        }

        /// <summary>
        /// PropertyNames
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public Subschema PropertyNames
        {
            get => (Subschema)GetData(Keyword.PropertyNames);
            set => SetData(Keyword.PropertyNames, value);
        }

        /// <summary>
        /// Required
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public string[] Required
        {
            get => (string[])GetData(Keyword.Required);
            set => SetData(Keyword.Required, value);
        }

        // Array attributes

        /// <summary>
        /// AdditionalItems
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public Subschema AdditionalItems
        {
            get => (Subschema)GetData(Keyword.AdditionalItems);
            set => SetData(Keyword.AdditionalItems, value);
        }

        /// <summary>
        /// Contains
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public Subschema Contains
        {
            get => (Subschema)GetData(Keyword.Contains);
            set => SetData(Keyword.Contains, value);
        }

        /// <summary>
        /// The type of Items is either SchemaElement or SchemaElement[] for tuple validation
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public object Items
        {
            get => GetData(Keyword.Items);
            set => SetData(Keyword.Items, value);
        }

        /// <summary>
        /// MinItems
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public double MinItems
        {
            get => (double?)GetData(Keyword.MinItems) ?? 0;
            set => SetData(Keyword.MinItems, value);
        }

        /// <summary>
        /// MaxItems
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public double MaxItems
        {
            get => (double?)GetData(Keyword.MaxItems) ?? int.MaxValue;
            set => SetData(Keyword.MaxItems, value);
        }

        /// <summary>
        /// MinItems
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public double MinContains
        {
            get => (double?)GetData(Keyword.MinContains) ?? 1;
            set => SetData(Keyword.MinContains, value);
        }

        /// <summary>
        /// MaxItems
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public double MaxContains
        {
            get => (double?)GetData(Keyword.MaxContains) ?? int.MaxValue;
            set => SetData(Keyword.MaxContains, value);
        }


        /// <summary>
        /// UniqueItems
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool UniqueItems
        {
            get => (bool?)GetData(Keyword.UniqueItems) != false;
            set => SetData(Keyword.UniqueItems, value);
        }

        // Logical operations

        /// <summary>
        /// AnyOf
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public Subschema[] AnyOf
        {
            get => (Subschema[])GetData(Keyword.AnyOf);
            set => SetData(Keyword.AnyOf, value);
        }

        /// <summary>
        /// AllOf
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public Subschema[] AllOf
        {
            get => (Subschema[])GetData(Keyword.AllOf);
            set => SetData(Keyword.AllOf, value);
        }

        /// <summary>
        /// OneOf
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public Subschema[] OneOf
        {
            get => (Subschema[])GetData(Keyword.OneOf);
            set => SetData(Keyword.OneOf, value);
        }

        /// <summary>
        /// If
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public Subschema If
        {
            get => (Subschema)GetData(Keyword.If);
            set => SetData(Keyword.If, value);
        }

        /// <summary>
        /// Else
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public Subschema Else
        {
            get => (Subschema)GetData(Keyword.Else) ?? SchemaConstants.Everything;
            set => SetData(Keyword.Else, value);
        }

        /// <summary>
        /// Then
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public Subschema Then
        {
            get => (Subschema)GetData(Keyword.Then) ?? SchemaConstants.Everything;
            set => SetData(Keyword.Then, value);
        }

        /// <summary>
        /// Not
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public Subschema Not
        {
            get => (Subschema)GetData(Keyword.Not);
            set => SetData(Keyword.Not, value);
        }

        // Miscellaneous

        /// <summary>
        /// Id
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public string Id
        {
            get => (string)GetData(Keyword._Id);
            set => SetData(Keyword._Id, value);
        }

        /// <summary>
        /// Schema
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public string Schema
        {
            get => (string)GetData(Keyword._Schema);
            set => SetData(Keyword._Schema, value);
        }

        /// <summary>
        /// Ref
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public SchemaReference Ref
        {
            get => (SchemaReference)GetData(Keyword._Ref);
            set => SetData(Keyword._Ref, value);
        }

        /// <summary>
        /// Metadata
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public KeyValuePair<string, object>[] Metadata
        {
            get => (KeyValuePair<string, object>[])GetData(Keyword.Metadata);
            set => SetData(Keyword.Metadata, value);
        }
        #endregion

        #region Navigation

        /// <summary>
        /// Obtain a collection of child schemas
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Subschema> GetChildSchemas()
        {
            if (data is object[] array)
            {
                foreach (object obj in array)
                {
                    if (obj is Subschema singleSchema)
                        yield return singleSchema;
                    else if (obj is Subschema[] schemaArray)
                    {
                        foreach (var schema in schemaArray)
                            yield return schema;
                    }
                    else if (obj is KeyValuePair<string, Subschema>[] schemaMap)
                    {
                        foreach (var kv in schemaMap)
                            yield return kv.Value;
                    }
                }
            }
            else if (data != null)
            {
                if (data is Subschema singleSchema)
                    yield return singleSchema;
                else if (data is Subschema[] schemaArray)
                {
                    foreach (var schema in schemaArray)
                        yield return schema;
                }
                else if (data is KeyValuePair<string, Subschema>[] schemaMap)
                {
                    foreach (var kv in schemaMap)
                        yield return kv.Value;
                }
            }
        }

        /// <summary>
        /// Walk through each subschema
        /// </summary>
        /// <param name="action"></param>
        public void ForEach(Action<Subschema> action)
        {
            if (data is object[] array)
            {
                foreach (object obj in array)
                {
                    if (obj is Subschema singleSchema)
                        action(singleSchema);
                    else if (obj is Subschema[] schemaArray)
                    {
                        foreach (var schema in schemaArray)
                            action(schema);
                    }
                    else if (obj is KeyValuePair<string, Subschema>[] schemaMap)
                    {
                        foreach (var kv in schemaMap)
                            action(kv.Value);
                    }
                }
            }
            else if (data != null)
            {
                if (data is Subschema singleSchema)
                    action(singleSchema);
                else if (data is Subschema[] schemaArray)
                {
                    foreach (var schema in schemaArray)
                        action(schema);
                }
                else if (data is KeyValuePair<string, Subschema>[] schemaMap)
                {
                    foreach (var kv in schemaMap)
                        action(kv.Value);
                }
            }
        }
        #endregion

        #region Renderer
        public string[] GetValidTypes()
        {
            int count = BitCount((long)(Flags & SchemaFlags.TypeAll));
            string[] types = new string[count];
            int pos = 0;
            if ((Flags & SchemaFlags.TypeArray) != 0) types[pos++] = "array";
            if ((Flags & SchemaFlags.TypeBoolean) != 0) types[pos++] = "boolean";
            if ((Flags & SchemaFlags.TypeInteger) != 0) types[pos++] = "integer";
            if ((Flags & SchemaFlags.TypeNull) != 0) types[pos++] = "null";
            if ((Flags & SchemaFlags.TypeNumber) != 0) types[pos++] = "number";
            if ((Flags & SchemaFlags.TypeObject) != 0) types[pos++] = "object";
            if ((Flags & SchemaFlags.TypeString) != 0) types[pos++] = "string";
            return types;
        }

        public MPJson ToJson()
        {
            if ((Flags & SchemaFlags.StoredProperties) == 0)
            {
                if ((Flags & SchemaFlags.TypeAll) == 0)
                    return false;
                if ((Flags & SchemaFlags.TypeAll) == SchemaFlags.TypeAll)
                    return true;
            }

            var list = new List<KeyValuePair<string, object>>();

            if ((Flags & SchemaFlags.TypeAll) != SchemaFlags.TypeAll)
            {
                string[] types = GetValidTypes();
                AddProperty(list, Keyword.Type, MPJson.From(types));
            }

            for (SchemaFlags mask = Flags & SchemaFlags.StoredProperties;
                    mask != 0;
                    mask = (SchemaFlags)RemoveLowestBit((long)mask))
            {
                Keyword keyword = (Keyword)IndexOfLowestBit((long)mask);
                switch (keyword)
                {
                    case Keyword.Const:
                    case Keyword.Enum:
                    case Keyword.ExclusiveMinimum:
                    case Keyword.ExclusiveMaximum:
                    case Keyword.Format:
                    case Keyword.MaxContains:
                    case Keyword.MaxItems:
                    case Keyword.MaxLength:
                    case Keyword.MaxProperties:
                    case Keyword.MinContains:
                    case Keyword.MinItems:
                    case Keyword.MinLength:
                    case Keyword.MinProperties:
                    case Keyword.MultipleOf:
                    case Keyword.Pattern:
                    case Keyword.UniqueItems:
                        AddProperty(list, keyword, MPJson.From(GetData(keyword)));
                        break;

                    case Keyword.AdditionalItems:
                    case Keyword.AdditionalProperties:
                    case Keyword.PropertyNames:
                    case Keyword.Contains:
                    case Keyword.If:
                    case Keyword.Then:
                    case Keyword.Else:
                    case Keyword.Not:
                        AddProperty(list, keyword,
                            ((Subschema)GetData(keyword)).ToJson());
                        break;

                    case Keyword.AllOf:
                    case Keyword.AnyOf:
                    case Keyword.OneOf:
                        var schemaArray = (Subschema[])GetData(keyword);
                        AddProperty(list, keyword,
                            MPJson.Array(Array.ConvertAll(schemaArray, x => x.ToJson())));
                        break;

                    case Keyword.Properties:
                    case Keyword.DependentSchemas:
                        AddProperty(list, keyword,
                            (KeyValuePair<string, Subschema>[])GetData(keyword), x => x.ToJson());
                        break;

                    case Keyword.PatternProperties:
                        AddProperty(list, keyword, PatternProperties, x => x.ToJson());
                        break;

                    case Keyword.DependentRequired:
                        AddProperty(list, keyword,
                            (KeyValuePair<string, string[]>[])GetData(keyword),
                            x => MPJson.From(x));
                        break;

                    case Keyword.Items:
                        object items = GetData(keyword);
                        schemaArray = items as Subschema[];
                        AddProperty(list, keyword,
                            schemaArray != null
                            ? MPJson.Array(Array.ConvertAll(schemaArray, x => x.ToJson()))
                            : ((Subschema)items).ToJson());
                        break;

                    case Keyword.CustomValidation:
                        break;

                    case Keyword._Ref:
                        var r = Ref;
                        AddProperty(list, keyword, r.Uri);
                        break;

                    case Keyword.Metadata:
                        foreach(var v in Metadata)
                            list.Add( new KeyValuePair<string, object>(v.Key, 
                                ConvertObject(v.Value)));
                        break;
                }
            }

            return MPJson.Object(list.ToArray(), true);
        }

        private static object ConvertObject(object value)
        {
            var type = MPJson.GetType(value);
            if (type != JsonType.Unknown)
                return value;

            if (value is Subschema schema)
                return schema.ToJson().Value;

            if (value is object[] schemaArray)
                return Array.ConvertAll(schemaArray, ConvertObject);

            if (value is KeyValuePair<string, Subschema>[] schemaPairs)
                return Array.ConvertAll(schemaPairs,
                    kv => new KeyValuePair<string, object>(
                        kv.Key, kv.Value.ToJson().Value));

            return value;
        }


        public static void AddProperty(List<KeyValuePair<string, object>> list,
            Keyword keyword, MPJson json)
        {
            list.Add(new KeyValuePair<string, object>(keyword.GetText(), json.Value));
        }

        public static void AddProperty<K, T>(List<KeyValuePair<string, object>> list,
            Keyword keyword,
            KeyValuePair<K, T>[] array,
            Func<T, object> func)
        {
            var props = new KeyValuePair<string, object>[array.Length];

            for (int i = 0; i < array.Length; i++)
                props[i] = new KeyValuePair<string, object>(array[i].Key.ToString(), func(array[i].Value));

            list.Add(new KeyValuePair<string, object>(keyword.GetText(), MPJson.Object(props)));
        }

        public override string ToString()
        {
            MPJson json = ToJson();
            return json.ToString();
        }
        #endregion
    }
}

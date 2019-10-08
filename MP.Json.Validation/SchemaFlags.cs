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
using System.Runtime.Serialization;
using System.Text;

namespace MP.Json.Validation
{
    /// <summary>
    /// Schema flags
    /// </summary>
    [Flags]
    internal enum SchemaFlags : long
    {
        None = 0,

        Const = 1L << Keyword.Const,
        Enum = 1L << Keyword.Enum,

        // Number attributes
        ExclusiveMaximum = 1L << Keyword.ExclusiveMaximum,       // boolean in v4, number in v6+
        ExclusiveMinimum = 1L << Keyword.ExclusiveMinimum,       // boolean in v4, number in v6+
        Maximum = 1L << Keyword.Maximum,
        Minimum = 1L << Keyword.Minimum,
        MultipleOf = 1L << Keyword.MultipleOf,

        // String attributes
        Format = 1L << Keyword.Format,
        MaxLength = 1L << Keyword.MaxLength,
        MinLength = 1L << Keyword.MinLength,
        Pattern = 1L << Keyword.Pattern,

        // Object attributes
        AdditionalProperties = 1L << Keyword.AdditionalProperties,
        DependentSchemas = 1L << Keyword.DependentSchemas,       // v8
        DependentRequired = 1L << Keyword.DependentRequired,      // v8
        MaxProperties = 1L << Keyword.MaxProperties,
        MinProperties = 1L << Keyword.MinProperties,
        PatternProperties = 1L << Keyword.PatternProperties,
        Properties = 1L << Keyword.Properties,
        PropertyNames = 1L << Keyword.PropertyNames,          // v6
        Required = 1L << Keyword.Required,               // v6 

        // Array attributes
        AdditionalItems = 1L << Keyword.AdditionalItems,
        Contains = 1L << Keyword.Contains,               // v6
        Items = 1L << Keyword.Items,
        MaxItems = 1L << Keyword.MaxItems,
        MinItems = 1L << Keyword.MinItems,
        UniqueItems = 1L << Keyword.UniqueItems,            // boolean

        // Logical operations -- these are the slowest, so we look at them last
        AllOf = 1L << Keyword.AllOf,
        AnyOf = 1L << Keyword.AnyOf,
        If = 1L << Keyword.If,                     // v7
        Not = 1L << Keyword.Not,
        OneOf = 1L << Keyword.OneOf,

        _Ref = 1L << Keyword._Ref,                   // other keys allowed alongside it in v8+
        CustomValidation = 1L << Keyword.CustomValidation,       // Not a keyword
        Metadata = 1L << Keyword.Metadata,               // Not a keyword

        // METADATA

        Then = 1L << Keyword.Then,  // v7
        Else = 1L << Keyword.Else,                   // v7
        MaxContains = 1L << Keyword.MaxContains,            // v8, array property
        MinContains = 1L << Keyword.MinContains,            // v8, array property

        ArrayProperties = 0
            | (1L << Keyword.AdditionalItems)
            | (1L << Keyword.Contains)
            | (1L << Keyword.Items)
            | (1L << Keyword.MinItems)
            | (1L << Keyword.MaxItems)
            | (1L << Keyword.UniqueItems)
            | (1L << Keyword.MaxContains)
            | (1L << Keyword.MinContains)
            ,

        ObjectProperties = 0
            | (1L << Keyword.AdditionalProperties)
            | (1L << Keyword.Properties)
            | (1L << Keyword.Required)
            | (1L << Keyword.PropertyNames)
            | (1L << Keyword.MinProperties)
            | (1L << Keyword.MaxProperties)
            | (1L << Keyword.DependentRequired)
            | (1L << Keyword.DependentSchemas)
            | (1L << Keyword.PatternProperties)
            ,

        NumberProperties = 0
            | (1L << (int)Keyword.MultipleOf)
            | (1L << (int)Keyword.Minimum)
            | (1L << (int)Keyword.Maximum)
            | (1L << (int)Keyword.ExclusiveMinimum)
            | (1L << (int)Keyword.ExclusiveMaximum)
            ,

        StringProperties = 0
            | (1L << (int)Keyword.MinLength)
            | (1L << (int)Keyword.MaxLength)
            | (1L << (int)Keyword.Pattern)
            | (1L << (int)Keyword.Format)
            ,

        // This will eventually include the boolean properties UniqueItems, ExclusiveMinimum, ExclusiveMaximum
        UnstoredProperties = 0,
        StoredProperties = (1L << (int)Keyword.MAX_STORED) - 1 & ~UnstoredProperties,

        // TYPE FLAGS - used for quickly detecting which types are allowed in the schema
        TypeArray = 1L << 56,
        TypeBoolean = 1L << 57,
        TypeInteger = 1L << 58,
        TypeNull = 1L << 59,
        TypeNumber = 1L << 60,
        TypeObject = 1L << 61,
        TypeString = 1L << 62,
        TypeAll = 0
            | TypeArray
            | TypeBoolean
            | TypeInteger
            | TypeNull
            | TypeNumber
            | TypeObject
            | TypeString,


        // This is the sign bit, so that we can quickly test flag by checking for whether flags is negative
        Uncompressed = 1L << 63,
        AllProperties = (1L << (int)Keyword.MAX_VALIDATION) - 1,
        GenericProperties = StoredProperties
            & ~ArrayProperties
            & ~StringProperties
            & ~ObjectProperties
            & ~NumberProperties
            ,
    }
}

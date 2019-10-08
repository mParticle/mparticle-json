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
    /// Standard schema Keywords recognized by the Schema object
    /// </summary>
    public enum Keyword
    {
        None = -1,

        Const,                  // v6
        Enum,

        // Number attributes
        ExclusiveMaximum,       // boolean in v4, number in v6+
        ExclusiveMinimum,       // boolean in v4, number in v6+
        Maximum,
        Minimum,
        MultipleOf,

        // String attributes
        Format,
        MaxLength,
        MinLength,
        Pattern,

        // Object attributes
        AdditionalProperties,
        // Dependencies,        // v6, changed to dependentSchemas and dependentRequired in v8
        DependentSchemas,       // v8
        DependentRequired,      // v8
        MaxProperties,
        MinProperties,
        PatternProperties,
        Properties,
        PropertyNames,          // v6
        Required,               // v6 

        // Array attributes
        AdditionalItems,
        Contains,               // v6
        Items,
        MaxItems,
        MinItems,
        UniqueItems,            // boolean

        // Logical operations -- these are the slowest, so we look at them last
        AllOf,
        AnyOf,
        If,                     // v7
        Not,
        OneOf,

        _Ref,                   // other keys allowed alongside it in v8+
        CustomValidation,       // Not a keyword
        Metadata,               // Not a keyword

        MAX_VALIDATION,         // Keywords above are examined during the main validation loop

        // METADATA

        Then = MAX_VALIDATION,  // v7
        Else,                   // v7
        MaxContains,            // v8, array property
        MinContains,            // v8, array property

        MAX_STORED,

        _Id = MAX_STORED,         // Id in v4, $id in v6+
        _Schema,
        Type,
        Dependencies,
        Definitions,
        Default,

        // It probably makes sense to get them out of this enum and put elsewhere

        // ANNOTATIONS
        // To save bits we don't store in mask
        // Anything unknown with a dollar sign is extension data that must be ignored

        Title,
        Description,
        Examples,  // v6

        Id,         // v4

        // DRAFT 7

        ReadOnly,  // v7
        WriteOnly, // v7
        _Comment,  // v7
        ContentEncoding,
        ContentMediaType,

        // DRAFT 8

        _Anchor,
        _RecursiveAnchor,
        _RecursiveRef,
        _Vocabulary,
        ContentSchema,
        Deprecated,   
        _Defs,

        // Keywords that don't use any bits
        MAX,
    }
}

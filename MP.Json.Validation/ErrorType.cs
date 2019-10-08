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
    /// Error types for schema construction and schema validation
    /// </summary>
    public enum ErrorType
    {
        None = -1,

        Const = Keyword.Const,
        Enum = Keyword.Enum,

        // Number attributes
        ExclusiveMaximum = Keyword.ExclusiveMaximum,
        ExclusiveMinimum = Keyword.ExclusiveMinimum,
        Maximum = Keyword.Maximum,
        Minimum = Keyword.Minimum,
        MultipleOf = Keyword.MultipleOf,

        // String attributes
        Format = Keyword.Format,
        MaxLength = Keyword.MaxLength,
        MinLength = Keyword.MinLength,
        Pattern = Keyword.Pattern,

        // Object attributes
        AdditionalProperties = Keyword.AdditionalProperties,
        DependentSchemas = Keyword.DependentSchemas,
        DependentRequired = Keyword.DependentRequired,
        MaxProperties = Keyword.MaxProperties,
        MinProperties = Keyword.MinProperties,
        PatternProperties = Keyword.PatternProperties,
        Properties = Keyword.Properties,
        PropertyNames = Keyword.PropertyNames,
        Required = Keyword.Required, 

        // Array attributes
        AdditionalItems = Keyword.AdditionalItems,
        Contains = Keyword.Contains,
        Items = Keyword.Items,
        MaxItems = Keyword.MaxItems,
        MinItems = Keyword.MinItems,
        UniqueItems = Keyword.UniqueItems,

        // Logical operations -- these are the slowest, so we look at them last
        AllOf = Keyword.AllOf,
        AnyOf = Keyword.AnyOf,
        If = Keyword.If,
        Not = Keyword.Not,
        OneOf = Keyword.OneOf,

        Ref = Keyword._Ref,
        Validator = Keyword.CustomValidation,

        // METADATA
        Then = Keyword.Then,
        Else = Keyword.Else,
        MaxContains = Keyword.MaxContains,
        MinContains = Keyword.MinContains,

        // Miscellaneous Errors
        Type = Keyword.Type,
        Dependencies = Keyword.Dependencies,
        
        // Non-keyword errors
        TimeLimit = 100,
    }
}

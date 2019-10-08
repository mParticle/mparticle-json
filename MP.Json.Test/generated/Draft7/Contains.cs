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
using MP.Json;
using MP.Json.Validation;
using Xunit;

namespace JsonSchemaTestSuite.Draft7
{
    public class Contains
    {

        /// <summary>
        ///     1 - contains keyword validation
        /// </summary>

        [Theory]
        [InlineData(
           "array with item matching schema (5) is valid",
           "[ 3, 4, 5 ]",
           true
           )]

        [InlineData(
           "array with item matching schema (6) is valid",
           "[ 3, 4, 6 ]",
           true
           )]

        [InlineData(
           "array with two items matching schema (5, 6) is valid",
           "[ 3, 4, 5, 6 ]",
           true
           )]

        [InlineData(
           "array without items matching schema is invalid",
           "[ 2, 3, 4 ]",
           false
           )]

        [InlineData(
           "empty array is invalid",
           "[ ]",
           false
           )]

        [InlineData(
           "not array is valid",
           "{ }",
           true
           )]

        public void ContainsKeywordValidation(string desc, string data, bool expected)
        {
            // contains keyword validation
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'contains':{ 'minimum':5 } }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft7 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     2 - contains keyword with const keyword
        /// </summary>

        [Theory]
        [InlineData(
           "array with item 5 is valid",
           "[ 3, 4, 5 ]",
           true
           )]

        [InlineData(
           "array with two items 5 is valid",
           "[ 3, 4, 5, 5 ]",
           true
           )]

        [InlineData(
           "array without item 5 is invalid",
           "[ 1, 2, 3, 4 ]",
           false
           )]

        public void ContainsKeywordWithConstKeyword(string desc, string data, bool expected)
        {
            // contains keyword with const keyword
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'contains':{ 'const':5 } }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft7 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     3 - contains keyword with boolean schema true
        /// </summary>

        [Theory]
        [InlineData(
           "any non-empty array is valid",
           "[ 'foo' ]",
           true
           )]

        [InlineData(
           "empty array is invalid",
           "[ ]",
           false
           )]

        public void ContainsKeywordWithBooleanSchemaTrue(string desc, string data, bool expected)
        {
            // contains keyword with boolean schema true
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'contains':true }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft7 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     4 - contains keyword with boolean schema false
        /// </summary>

        [Theory]
        [InlineData(
           "any non-empty array is invalid",
           "[ 'foo' ]",
           false
           )]

        [InlineData(
           "empty array is invalid",
           "[ ]",
           false
           )]

        [InlineData(
           "non-arrays are valid",
           "'contains does not apply to strings'",
           true
           )]

        public void ContainsKeywordWithBooleanSchemaFalse(string desc, string data, bool expected)
        {
            // contains keyword with boolean schema false
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'contains':false }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft7 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


    }
}

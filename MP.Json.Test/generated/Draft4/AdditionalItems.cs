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

namespace JsonSchemaTestSuite.Draft4
{
    public class AdditionalItems
    {

        /// <summary>
        ///     1 - additionalItems as schema
        /// </summary>

        [Theory]
        [InlineData(
           "additional items match schema",
           "[ null, 2, 3, 4 ]",
           true
           )]

        [InlineData(
           "additional items do not match schema",
           "[ null, 2, 3, 'foo' ]",
           false
           )]

        public void AdditionalItemsAsSchema(string desc, string data, bool expected)
        {
            // additionalItems as schema
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'additionalItems':{ 'type':'integer' }, 'items':[ { } ] }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft4 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     2 - items is schema, no additionalItems
        /// </summary>

        [Theory]
        [InlineData(
           "all items match schema",
           "[ 1, 2, 3, 4, 5 ]",
           true
           )]

        public void ItemsIsSchemaNoAdditionalItems(string desc, string data, bool expected)
        {
            // items is schema, no additionalItems
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'additionalItems':false, 'items':{ } }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft4 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     3 - array of items with no additionalItems
        /// </summary>

        [Theory]
        [InlineData(
           "fewer number of items present",
           "[ 1, 2 ]",
           true
           )]

        [InlineData(
           "equal number of items present",
           "[ 1, 2, 3 ]",
           true
           )]

        [InlineData(
           "additional items are not permitted",
           "[ 1, 2, 3, 4 ]",
           false
           )]

        public void ArrayOfItemsWithNoAdditionalItems(string desc, string data, bool expected)
        {
            // array of items with no additionalItems
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'additionalItems':false, 'items':[ { }, { }, { } ] }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft4 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     4 - additionalItems as false without items
        /// </summary>

        [Theory]
        [InlineData(
           "items defaults to empty schema so everything is valid",
           "[ 1, 2, 3, 4, 5 ]",
           true
           )]

        [InlineData(
           "ignores non-arrays",
           "{ 'foo':'bar' }",
           true
           )]

        public void AdditionalItemsAsFalseWithoutItems(string desc, string data, bool expected)
        {
            // additionalItems as false without items
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'additionalItems':false }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft4 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     5 - additionalItems are allowed by default
        /// </summary>

        [Theory]
        [InlineData(
           "only the first item is validated",
           "[ 1, 'foo', false ]",
           true
           )]

        public void AdditionalItemsAreAllowedByDefault(string desc, string data, bool expected)
        {
            // additionalItems are allowed by default
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'items':[ { 'type':'integer' } ] }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft4 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


    }
}

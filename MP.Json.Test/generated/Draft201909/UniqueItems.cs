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

namespace JsonSchemaTestSuite.Draft201909
{
    public class UniqueItems
    {

        /// <summary>
        ///     1 - uniqueItems validation
        /// </summary>

        [Theory]
        [InlineData(
           "unique array of integers is valid",
           "[ 1, 2 ]",
           true
           )]

        [InlineData(
           "non-unique array of integers is invalid",
           "[ 1, 1 ]",
           false
           )]

        [InlineData(
           "numbers are unique if mathematically unequal",
           "[ 1, 1, 1 ]",
           false
           )]

        [InlineData(
           "false is not equal to zero",
           "[ 0, false ]",
           true
           )]

        [InlineData(
           "true is not equal to one",
           "[ 1, true ]",
           true
           )]

        [InlineData(
           "unique array of objects is valid",
           "[ { 'foo':'bar' }, { 'foo':'baz' } ]",
           true
           )]

        [InlineData(
           "non-unique array of objects is invalid",
           "[ { 'foo':'bar' }, { 'foo':'bar' } ]",
           false
           )]

        [InlineData(
           "unique array of nested objects is valid",
           "[ { 'foo':{ 'bar':{ 'baz':true } } }, { 'foo':{ 'bar':{ 'baz':false } } } ]",
           true
           )]

        [InlineData(
           "non-unique array of nested objects is invalid",
           "[ { 'foo':{ 'bar':{ 'baz':true } } }, { 'foo':{ 'bar':{ 'baz':true } } } ]",
           false
           )]

        [InlineData(
           "unique array of arrays is valid",
           "[ [ 'foo' ], [ 'bar' ] ]",
           true
           )]

        [InlineData(
           "non-unique array of arrays is invalid",
           "[ [ 'foo' ], [ 'foo' ] ]",
           false
           )]

        [InlineData(
           "1 and true are unique",
           "[ 1, true ]",
           true
           )]

        [InlineData(
           "0 and false are unique",
           "[ 0, false ]",
           true
           )]

        [InlineData(
           "unique heterogeneous types are valid",
           "[ { }, [ 1 ], true, null, 1 ]",
           true
           )]

        [InlineData(
           "non-unique heterogeneous types are invalid",
           "[ { }, [ 1 ], true, null, { }, 1 ]",
           false
           )]

        public void UniqueItemsValidation(string desc, string data, bool expected)
        {
            // uniqueItems validation
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'uniqueItems':true }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft201909 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     2 - uniqueItems with an array of items
        /// </summary>

        [Theory]
        [InlineData(
           "[false, true] from items array is valid",
           "[ false, true ]",
           true
           )]

        [InlineData(
           "[true, false] from items array is valid",
           "[ true, false ]",
           true
           )]

        [InlineData(
           "[false, false] from items array is not valid",
           "[ false, false ]",
           false
           )]

        [InlineData(
           "[true, true] from items array is not valid",
           "[ true, true ]",
           false
           )]

        [InlineData(
           "unique array extended from [false, true] is valid",
           "[ false, true, 'foo', 'bar' ]",
           true
           )]

        [InlineData(
           "unique array extended from [true, false] is valid",
           "[ true, false, 'foo', 'bar' ]",
           true
           )]

        [InlineData(
           "non-unique array extended from [false, true] is not valid",
           "[ false, true, 'foo', 'foo' ]",
           false
           )]

        [InlineData(
           "non-unique array extended from [true, false] is not valid",
           "[ true, false, 'foo', 'foo' ]",
           false
           )]

        public void UniqueItemsWithAnArrayOfItems(string desc, string data, bool expected)
        {
            // uniqueItems with an array of items
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'items':[ { 'type':'boolean' }, { 'type':'boolean' } ], 'uniqueItems':true }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft201909 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     3 - uniqueItems with an array of items and additionalItems=false
        /// </summary>

        [Theory]
        [InlineData(
           "[false, true] from items array is valid",
           "[ false, true ]",
           true
           )]

        [InlineData(
           "[true, false] from items array is valid",
           "[ true, false ]",
           true
           )]

        [InlineData(
           "[false, false] from items array is not valid",
           "[ false, false ]",
           false
           )]

        [InlineData(
           "[true, true] from items array is not valid",
           "[ true, true ]",
           false
           )]

        [InlineData(
           "extra items are invalid even if unique",
           "[ false, true, null ]",
           false
           )]

        public void UniqueItemsWithAnArrayOfItemsAndAdditionalItemsFalse(string desc, string data, bool expected)
        {
            // uniqueItems with an array of items and additionalItems=false
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'additionalItems':false, 'items':[ { 'type':'boolean' }, { 'type':'boolean' } ], 'uniqueItems':true }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft201909 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


    }
}

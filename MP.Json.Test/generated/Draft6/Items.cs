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

namespace JsonSchemaTestSuite.Draft6
{
    public class Items
    {

        /// <summary>
        ///     1 - a schema given for items
        /// </summary>

        [Theory]
        [InlineData(
           "valid items",
           "[ 1, 2, 3 ]",
           true
           )]

        [InlineData(
           "wrong type of items",
           "[ 1, 'x' ]",
           false
           )]

        [InlineData(
           "ignores non-arrays",
           "{ 'foo':'bar' }",
           true
           )]

        [InlineData(
           "JavaScript pseudo-array is valid",
           "{ '0':'invalid', 'length':1 }",
           true
           )]

        public void ASchemaGivenForItems(string desc, string data, bool expected)
        {
            // a schema given for items
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'items':{ 'type':'integer' } }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft6 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     2 - an array of schemas for items
        /// </summary>

        [Theory]
        [InlineData(
           "correct types",
           "[ 1, 'foo' ]",
           true
           )]

        [InlineData(
           "wrong types",
           "[ 'foo', 1 ]",
           false
           )]

        [InlineData(
           "incomplete array of items",
           "[ 1 ]",
           true
           )]

        [InlineData(
           "array with additional items",
           "[ 1, 'foo', true ]",
           true
           )]

        [InlineData(
           "empty array",
           "[ ]",
           true
           )]

        [InlineData(
           "JavaScript pseudo-array is valid",
           "{ '0':'invalid', '1':'valid', 'length':2 }",
           true
           )]

        public void AnArrayOfSchemasForItems(string desc, string data, bool expected)
        {
            // an array of schemas for items
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'items':[ { 'type':'integer' }, { 'type':'string' } ] }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft6 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     3 - items with boolean schema (true)
        /// </summary>

        [Theory]
        [InlineData(
           "any array is valid",
           "[ 1, 'foo', true ]",
           true
           )]

        [InlineData(
           "empty array is valid",
           "[ ]",
           true
           )]

        public void ItemsWithBooleanSchemaTrue(string desc, string data, bool expected)
        {
            // items with boolean schema (true)
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'items':true }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft6 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     4 - items with boolean schema (false)
        /// </summary>

        [Theory]
        [InlineData(
           "any non-empty array is invalid",
           "[ 1, 'foo', true ]",
           false
           )]

        [InlineData(
           "empty array is valid",
           "[ ]",
           true
           )]

        public void ItemsWithBooleanSchemaFalse(string desc, string data, bool expected)
        {
            // items with boolean schema (false)
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'items':false }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft6 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     5 - items with boolean schemas
        /// </summary>

        [Theory]
        [InlineData(
           "array with one item is valid",
           "[ 1 ]",
           true
           )]

        [InlineData(
           "array with two items is invalid",
           "[ 1, 'foo' ]",
           false
           )]

        [InlineData(
           "empty array is valid",
           "[ ]",
           true
           )]

        public void ItemsWithBooleanSchemas(string desc, string data, bool expected)
        {
            // items with boolean schemas
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'items':[ true, false ] }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft6 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     6 - items and subitems
        /// </summary>

        [Theory]
        [InlineData(
           "valid items",
           "[ [ { 'foo':null }, { 'foo':null } ], [ { 'foo':null }, { 'foo':null } ], [ { 'foo':null }, { 'foo':null } ] ]",
           true
           )]

        [InlineData(
           "too many items",
           "[ [ { 'foo':null }, { 'foo':null } ], [ { 'foo':null }, { 'foo':null } ], [ { 'foo':null }, { 'foo':null } ], [ { 'foo':null }, { 'foo':null } ] ]",
           false
           )]

        [InlineData(
           "too many sub-items",
           "[ [ { 'foo':null }, { 'foo':null }, { 'foo':null } ], [ { 'foo':null }, { 'foo':null } ], [ { 'foo':null }, { 'foo':null } ] ]",
           false
           )]

        [InlineData(
           "wrong item",
           "[ { 'foo':null }, [ { 'foo':null }, { 'foo':null } ], [ { 'foo':null }, { 'foo':null } ] ]",
           false
           )]

        [InlineData(
           "wrong sub-item",
           "[ [ { }, { 'foo':null } ], [ { 'foo':null }, { 'foo':null } ], [ { 'foo':null }, { 'foo':null } ] ]",
           false
           )]

        [InlineData(
           "fewer items is valid",
           "[ [ { 'foo':null } ], [ { 'foo':null } ] ]",
           true
           )]

        public void ItemsAndSubitems(string desc, string data, bool expected)
        {
            // items and subitems
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'additionalItems':false, 'definitions':{ 'item':{ 'additionalItems':false, 'items':[ { '$ref':'#/definitions/sub-item' }, { '$ref':'#/definitions/sub-item' } ], 'type':'array' }, 'sub-item':{ 'required':[ 'foo' ], 'type':'object' } }, 'items':[ { '$ref':'#/definitions/item' }, { '$ref':'#/definitions/item' }, { '$ref':'#/definitions/item' } ], 'type':'array' }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft6 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }

        /// <summary>
        ///     7 - nested items
        /// </summary>

        [Theory]
        [InlineData(
           "valid nested array",
           "[ [ [ [ 1 ] ], [ [ 2 ], [ 3 ] ] ], [ [ [ 4 ], [ 5 ], [ 6 ] ] ] ]",
           true
           )]

        [InlineData(
           "nested array with invalid type",
           "[ [ [ [ '1' ] ], [ [ 2 ], [ 3 ] ] ], [ [ [ 4 ], [ 5 ], [ 6 ] ] ] ]",
           false
           )]

        [InlineData(
           "not deep enough",
           "[ [ [ 1 ], [ 2 ], [ 3 ] ], [ [ 4 ], [ 5 ], [ 6 ] ] ]",
           false
           )]

        public void NestedItems(string desc, string data, bool expected)
        {
            // nested items
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'items':{ 'items':{ 'items':{ 'items':{ 'type':'number' }, 'type':'array' }, 'type':'array' }, 'type':'array' }, 'type':'array' }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft6 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


    }
}

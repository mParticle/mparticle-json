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
    public class Type
    {

        /// <summary>
        ///     1 - integer type matches integers
        /// </summary>

        [Theory]
        [InlineData(
           "an integer is an integer",
           "1",
           true
           )]

        [InlineData(
           "a float is not an integer",
           "1.1",
           false
           )]

        [InlineData(
           "a string is not an integer",
           "'foo'",
           false
           )]

        [InlineData(
           "a string is still not an integer, even if it looks like one",
           "'1'",
           false
           )]

        [InlineData(
           "an object is not an integer",
           "{ }",
           false
           )]

        [InlineData(
           "an array is not an integer",
           "[ ]",
           false
           )]

        [InlineData(
           "a boolean is not an integer",
           "true",
           false
           )]

        [InlineData(
           "null is not an integer",
           "null",
           false
           )]

        public void IntegerTypeMatchesIntegers(string desc, string data, bool expected)
        {
            // integer type matches integers
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'type':'integer' }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft4 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     2 - number type matches numbers
        /// </summary>

        [Theory]
        [InlineData(
           "an integer is a number",
           "1",
           true
           )]

        [InlineData(
           "a float is a number",
           "1.1",
           true
           )]

        [InlineData(
           "a string is not a number",
           "'foo'",
           false
           )]

        [InlineData(
           "a string is still not a number, even if it looks like one",
           "'1'",
           false
           )]

        [InlineData(
           "an object is not a number",
           "{ }",
           false
           )]

        [InlineData(
           "an array is not a number",
           "[ ]",
           false
           )]

        [InlineData(
           "a boolean is not a number",
           "true",
           false
           )]

        [InlineData(
           "null is not a number",
           "null",
           false
           )]

        public void NumberTypeMatchesNumbers(string desc, string data, bool expected)
        {
            // number type matches numbers
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'type':'number' }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft4 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     3 - string type matches strings
        /// </summary>

        [Theory]
        [InlineData(
           "1 is not a string",
           "1",
           false
           )]

        [InlineData(
           "a float is not a string",
           "1.1",
           false
           )]

        [InlineData(
           "a string is a string",
           "'foo'",
           true
           )]

        [InlineData(
           "a string is still a string, even if it looks like a number",
           "'1'",
           true
           )]

        [InlineData(
           "an empty string is still a string",
           "''",
           true
           )]

        [InlineData(
           "an object is not a string",
           "{ }",
           false
           )]

        [InlineData(
           "an array is not a string",
           "[ ]",
           false
           )]

        [InlineData(
           "a boolean is not a string",
           "true",
           false
           )]

        [InlineData(
           "null is not a string",
           "null",
           false
           )]

        public void StringTypeMatchesStrings(string desc, string data, bool expected)
        {
            // string type matches strings
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'type':'string' }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft4 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     4 - object type matches objects
        /// </summary>

        [Theory]
        [InlineData(
           "an integer is not an object",
           "1",
           false
           )]

        [InlineData(
           "a float is not an object",
           "1.1",
           false
           )]

        [InlineData(
           "a string is not an object",
           "'foo'",
           false
           )]

        [InlineData(
           "an object is an object",
           "{ }",
           true
           )]

        [InlineData(
           "an array is not an object",
           "[ ]",
           false
           )]

        [InlineData(
           "a boolean is not an object",
           "true",
           false
           )]

        [InlineData(
           "null is not an object",
           "null",
           false
           )]

        public void ObjectTypeMatchesObjects(string desc, string data, bool expected)
        {
            // object type matches objects
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'type':'object' }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft4 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     5 - array type matches arrays
        /// </summary>

        [Theory]
        [InlineData(
           "an integer is not an array",
           "1",
           false
           )]

        [InlineData(
           "a float is not an array",
           "1.1",
           false
           )]

        [InlineData(
           "a string is not an array",
           "'foo'",
           false
           )]

        [InlineData(
           "an object is not an array",
           "{ }",
           false
           )]

        [InlineData(
           "an array is an array",
           "[ ]",
           true
           )]

        [InlineData(
           "a boolean is not an array",
           "true",
           false
           )]

        [InlineData(
           "null is not an array",
           "null",
           false
           )]

        public void ArrayTypeMatchesArrays(string desc, string data, bool expected)
        {
            // array type matches arrays
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'type':'array' }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft4 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     6 - boolean type matches booleans
        /// </summary>

        [Theory]
        [InlineData(
           "an integer is not a boolean",
           "1",
           false
           )]

        [InlineData(
           "zero is not a boolean",
           "0",
           false
           )]

        [InlineData(
           "a float is not a boolean",
           "1.1",
           false
           )]

        [InlineData(
           "a string is not a boolean",
           "'foo'",
           false
           )]

        [InlineData(
           "an empty string is not a boolean",
           "''",
           false
           )]

        [InlineData(
           "an object is not a boolean",
           "{ }",
           false
           )]

        [InlineData(
           "an array is not a boolean",
           "[ ]",
           false
           )]

        [InlineData(
           "true is a boolean",
           "true",
           true
           )]

        [InlineData(
           "false is a boolean",
           "false",
           true
           )]

        [InlineData(
           "null is not a boolean",
           "null",
           false
           )]

        public void BooleanTypeMatchesBooleans(string desc, string data, bool expected)
        {
            // boolean type matches booleans
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'type':'boolean' }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft4 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     7 - null type matches only the null object
        /// </summary>

        [Theory]
        [InlineData(
           "an integer is not null",
           "1",
           false
           )]

        [InlineData(
           "a float is not null",
           "1.1",
           false
           )]

        [InlineData(
           "zero is not null",
           "0",
           false
           )]

        [InlineData(
           "a string is not null",
           "'foo'",
           false
           )]

        [InlineData(
           "an empty string is not null",
           "''",
           false
           )]

        [InlineData(
           "an object is not null",
           "{ }",
           false
           )]

        [InlineData(
           "an array is not null",
           "[ ]",
           false
           )]

        [InlineData(
           "true is not null",
           "true",
           false
           )]

        [InlineData(
           "false is not null",
           "false",
           false
           )]

        [InlineData(
           "null is null",
           "null",
           true
           )]

        public void NullTypeMatchesOnlyTheNullObject(string desc, string data, bool expected)
        {
            // null type matches only the null object
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'type':'null' }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft4 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     8 - multiple types can be specified in an array
        /// </summary>

        [Theory]
        [InlineData(
           "an integer is valid",
           "1",
           true
           )]

        [InlineData(
           "a string is valid",
           "'foo'",
           true
           )]

        [InlineData(
           "a float is invalid",
           "1.1",
           false
           )]

        [InlineData(
           "an object is invalid",
           "{ }",
           false
           )]

        [InlineData(
           "an array is invalid",
           "[ ]",
           false
           )]

        [InlineData(
           "a boolean is invalid",
           "true",
           false
           )]

        [InlineData(
           "null is invalid",
           "null",
           false
           )]

        public void MultipleTypesCanBeSpecifiedInAnArray(string desc, string data, bool expected)
        {
            // multiple types can be specified in an array
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'type':[ 'integer', 'string' ] }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft4 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     9 - type as array with one item
        /// </summary>

        [Theory]
        [InlineData(
           "string is valid",
           "'foo'",
           true
           )]

        [InlineData(
           "number is invalid",
           "123",
           false
           )]

        public void TypeAsArrayWithOneItem(string desc, string data, bool expected)
        {
            // type as array with one item
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'type':[ 'string' ] }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft4 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     10 - type: array or object
        /// </summary>

        [Theory]
        [InlineData(
           "array is valid",
           "[ 1, 2, 3 ]",
           true
           )]

        [InlineData(
           "object is valid",
           "{ 'foo':123 }",
           true
           )]

        [InlineData(
           "number is invalid",
           "123",
           false
           )]

        [InlineData(
           "string is invalid",
           "'foo'",
           false
           )]

        [InlineData(
           "null is invalid",
           "null",
           false
           )]

        public void TypeArrayOrObject(string desc, string data, bool expected)
        {
            // type: array or object
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'type':[ 'array', 'object' ] }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft4 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     11 - type: array, object or null
        /// </summary>

        [Theory]
        [InlineData(
           "array is valid",
           "[ 1, 2, 3 ]",
           true
           )]

        [InlineData(
           "object is valid",
           "{ 'foo':123 }",
           true
           )]

        [InlineData(
           "null is valid",
           "null",
           true
           )]

        [InlineData(
           "number is invalid",
           "123",
           false
           )]

        [InlineData(
           "string is invalid",
           "'foo'",
           false
           )]

        public void TypeArrayObjectOrNull(string desc, string data, bool expected)
        {
            // type: array, object or null
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'type':[ 'array', 'object', 'null' ] }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft4 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


    }
}

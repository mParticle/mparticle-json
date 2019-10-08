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
    public class Enum
    {

        /// <summary>
        ///     1 - simple enum validation
        /// </summary>

        [Theory]
        [InlineData(
           "one of the enum is valid",
           "1",
           true
           )]

        [InlineData(
           "something else is invalid",
           "4",
           false
           )]

        public void SimpleEnumValidation(string desc, string data, bool expected)
        {
            // simple enum validation
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'enum':[ 1, 2, 3 ] }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft201909 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     2 - heterogeneous enum validation
        /// </summary>

        [Theory]
        [InlineData(
           "one of the enum is valid",
           "[ ]",
           true
           )]

        [InlineData(
           "something else is invalid",
           "null",
           false
           )]

        [InlineData(
           "objects are deep compared",
           "{ 'foo':false }",
           false
           )]

        public void HeterogeneousEnumValidation(string desc, string data, bool expected)
        {
            // heterogeneous enum validation
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'enum':[ 6, 'foo', [ ], true, { 'foo':12 } ] }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft201909 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     3 - enums in properties
        /// </summary>

        [Theory]
        [InlineData(
           "both properties are valid",
           "{ 'bar':'bar', 'foo':'foo' }",
           true
           )]

        [InlineData(
           "missing optional property is valid",
           "{ 'bar':'bar' }",
           true
           )]

        [InlineData(
           "missing required property is invalid",
           "{ 'foo':'foo' }",
           false
           )]

        [InlineData(
           "missing all properties is invalid",
           "{ }",
           false
           )]

        public void EnumsInProperties(string desc, string data, bool expected)
        {
            // enums in properties
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'properties':{ 'bar':{ 'enum':[ 'bar' ] }, 'foo':{ 'enum':[ 'foo' ] } }, 'required':[ 'bar' ], 'type':'object' }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft201909 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     4 - enum with escaped characters
        /// </summary>

        [Theory]
        [InlineData(
           "member 1 is valid",
           @"'foo\nbar'",
           true
           )]

        [InlineData(
           "member 2 is valid",
           @"'foo\rbar'",
           true
           )]

        [InlineData(
           "another string is invalid",
           "'abc'",
           false
           )]

        public void EnumWithEscapedCharacters(string desc, string data, bool expected)
        {
            // enum with escaped characters
            Console.Error.WriteLine(desc);
            string schemaData = @"{ 'enum':[ 'foo\nbar', 'foo\rbar' ] }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft201909 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     5 - enum with false does not match 0
        /// </summary>

        [Theory]
        [InlineData(
           "false is valid",
           "false",
           true
           )]

        [InlineData(
           "integer zero is invalid",
           "0",
           false
           )]

        [InlineData(
           "float zero is invalid",
           "0",
           false
           )]

        public void EnumWithFalseDoesNotMatch0(string desc, string data, bool expected)
        {
            // enum with false does not match 0
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'enum':[ false ] }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft201909 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     6 - enum with true does not match 1
        /// </summary>

        [Theory]
        [InlineData(
           "true is valid",
           "true",
           true
           )]

        [InlineData(
           "integer one is invalid",
           "1",
           false
           )]

        [InlineData(
           "float one is invalid",
           "1",
           false
           )]

        public void EnumWithTrueDoesNotMatch1(string desc, string data, bool expected)
        {
            // enum with true does not match 1
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'enum':[ true ] }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft201909 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     7 - enum with 0 does not match false
        /// </summary>

        [Theory]
        [InlineData(
           "false is invalid",
           "false",
           false
           )]

        [InlineData(
           "integer zero is valid",
           "0",
           true
           )]

        [InlineData(
           "float zero is valid",
           "0",
           true
           )]

        public void EnumWith0DoesNotMatchFalse(string desc, string data, bool expected)
        {
            // enum with 0 does not match false
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'enum':[ 0 ] }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft201909 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     8 - enum with 1 does not match true
        /// </summary>

        [Theory]
        [InlineData(
           "true is invalid",
           "true",
           false
           )]

        [InlineData(
           "integer one is valid",
           "1",
           true
           )]

        [InlineData(
           "float one is valid",
           "1",
           true
           )]

        public void EnumWith1DoesNotMatchTrue(string desc, string data, bool expected)
        {
            // enum with 1 does not match true
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'enum':[ 1 ] }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft201909 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


    }
}

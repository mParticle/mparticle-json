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
    public class Const
    {

        /// <summary>
        ///     1 - const validation
        /// </summary>

        [Theory]
        [InlineData(
           "same value is valid",
           "2",
           true
           )]

        [InlineData(
           "another value is invalid",
           "5",
           false
           )]

        [InlineData(
           "another type is invalid",
           "'a'",
           false
           )]

        public void ConstValidation(string desc, string data, bool expected)
        {
            // const validation
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'const':2 }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft201909 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     2 - const with object
        /// </summary>

        [Theory]
        [InlineData(
           "same object is valid",
           "{ 'baz':'bax', 'foo':'bar' }",
           true
           )]

        [InlineData(
           "same object with different property order is valid",
           "{ 'baz':'bax', 'foo':'bar' }",
           true
           )]

        [InlineData(
           "another object is invalid",
           "{ 'foo':'bar' }",
           false
           )]

        [InlineData(
           "another type is invalid",
           "[ 1, 2 ]",
           false
           )]

        public void ConstWithObject(string desc, string data, bool expected)
        {
            // const with object
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'const':{ 'baz':'bax', 'foo':'bar' } }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft201909 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     3 - const with array
        /// </summary>

        [Theory]
        [InlineData(
           "same array is valid",
           "[ { 'foo':'bar' } ]",
           true
           )]

        [InlineData(
           "another array item is invalid",
           "[ 2 ]",
           false
           )]

        [InlineData(
           "array with additional items is invalid",
           "[ 1, 2, 3 ]",
           false
           )]

        public void ConstWithArray(string desc, string data, bool expected)
        {
            // const with array
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'const':[ { 'foo':'bar' } ] }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft201909 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     4 - const with null
        /// </summary>

        [Theory]
        [InlineData(
           "null is valid",
           "null",
           true
           )]

        [InlineData(
           "not null is invalid",
           "0",
           false
           )]

        public void ConstWithNull(string desc, string data, bool expected)
        {
            // const with null
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'const':null }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft201909 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     5 - const with false does not match 0
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

        public void ConstWithFalseDoesNotMatch0(string desc, string data, bool expected)
        {
            // const with false does not match 0
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'const':false }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft201909 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     6 - const with true does not match 1
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

        public void ConstWithTrueDoesNotMatch1(string desc, string data, bool expected)
        {
            // const with true does not match 1
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'const':true }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft201909 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     7 - const with 0 does not match false
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

        public void ConstWith0DoesNotMatchFalse(string desc, string data, bool expected)
        {
            // const with 0 does not match false
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'const':0 }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft201909 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     8 - const with 1 does not match true
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

        public void ConstWith1DoesNotMatchTrue(string desc, string data, bool expected)
        {
            // const with 1 does not match true
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'const':1 }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft201909 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


    }
}

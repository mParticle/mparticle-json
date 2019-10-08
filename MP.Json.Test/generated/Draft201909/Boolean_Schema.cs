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
    public class Boolean_Schema
    {

        /// <summary>
        ///     1 - boolean schema 'true'
        /// </summary>

        [Theory]
        [InlineData(
           "number is valid",
           "1",
           true
           )]

        [InlineData(
           "string is valid",
           "'foo'",
           true
           )]

        [InlineData(
           "boolean true is valid",
           "true",
           true
           )]

        [InlineData(
           "boolean false is valid",
           "false",
           true
           )]

        [InlineData(
           "null is valid",
           "null",
           true
           )]

        [InlineData(
           "object is valid",
           "{ 'foo':'bar' }",
           true
           )]

        [InlineData(
           "empty object is valid",
           "{ }",
           true
           )]

        [InlineData(
           "array is valid",
           "[ 'foo' ]",
           true
           )]

        [InlineData(
           "empty array is valid",
           "[ ]",
           true
           )]

        public void BooleanSchemaTrue(string desc, string data, bool expected)
        {
            // boolean schema 'true'
            Console.Error.WriteLine(desc);
            string schemaData = "true";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft201909 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     2 - boolean schema 'false'
        /// </summary>

        [Theory]
        [InlineData(
           "number is invalid",
           "1",
           false
           )]

        [InlineData(
           "string is invalid",
           "'foo'",
           false
           )]

        [InlineData(
           "boolean true is invalid",
           "true",
           false
           )]

        [InlineData(
           "boolean false is invalid",
           "false",
           false
           )]

        [InlineData(
           "null is invalid",
           "null",
           false
           )]

        [InlineData(
           "object is invalid",
           "{ 'foo':'bar' }",
           false
           )]

        [InlineData(
           "empty object is invalid",
           "{ }",
           false
           )]

        [InlineData(
           "array is invalid",
           "[ 'foo' ]",
           false
           )]

        [InlineData(
           "empty array is invalid",
           "[ ]",
           false
           )]

        public void BooleanSchemaFalse(string desc, string data, bool expected)
        {
            // boolean schema 'false'
            Console.Error.WriteLine(desc);
            string schemaData = "false";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft201909 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


    }
}

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
    public class Required
    {

        /// <summary>
        ///     1 - required validation
        /// </summary>

        [Theory]
        [InlineData(
           "present required property is valid",
           "{ 'foo':1 }",
           true
           )]

        [InlineData(
           "non-present required property is invalid",
           "{ 'bar':1 }",
           false
           )]

        [InlineData(
           "ignores arrays",
           "[ ]",
           true
           )]

        [InlineData(
           "ignores strings",
           "''",
           true
           )]

        [InlineData(
           "ignores other non-objects",
           "12",
           true
           )]

        public void RequiredValidation(string desc, string data, bool expected)
        {
            // required validation
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'properties':{ 'bar':{ }, 'foo':{ } }, 'required':[ 'foo' ] }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft4 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     2 - required default validation
        /// </summary>

        [Theory]
        [InlineData(
           "not required by default",
           "{ }",
           true
           )]

        public void RequiredDefaultValidation(string desc, string data, bool expected)
        {
            // required default validation
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'properties':{ 'foo':{ } } }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft4 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     3 - required with escaped characters
        /// </summary>

        [Theory]
        [InlineData(
           "object with all properties present is valid",
           @"{ 'foo\tbar':1, 'foo\nbar':1, 'foo\fbar':1, 'foo\rbar':1, 'foo\'bar':1, 'foo\\bar':1 }",
           true
           )]

        [InlineData(
           "object with some properties missing is invalid",
           @"{ 'foo\nbar':'1', 'foo\'bar':'1' }",
           false
           )]

        public void RequiredWithEscapedCharacters(string desc, string data, bool expected)
        {
            // required with escaped characters
            Console.Error.WriteLine(desc);
            string schemaData = @"{ 'required':[ 'foo\nbar', 'foo\'bar', 'foo\\bar', 'foo\rbar', 'foo\tbar', 'foo\fbar' ] }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft4 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


    }
}

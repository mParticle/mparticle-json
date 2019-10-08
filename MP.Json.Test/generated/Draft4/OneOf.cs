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
    public class OneOf
    {

        /// <summary>
        ///     1 - oneOf
        /// </summary>

        [Theory]
        [InlineData(
           "first oneOf valid",
           "1",
           true
           )]

        [InlineData(
           "second oneOf valid",
           "2.5",
           true
           )]

        [InlineData(
           "both oneOf valid",
           "3",
           false
           )]

        [InlineData(
           "neither oneOf valid",
           "1.5",
           false
           )]

        public void OneOfTest(string desc, string data, bool expected)
        {
            // oneOf
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'oneOf':[ { 'type':'integer' }, { 'minimum':2 } ] }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft4 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     2 - oneOf with base schema
        /// </summary>

        [Theory]
        [InlineData(
           "mismatch base schema",
           "3",
           false
           )]

        [InlineData(
           "one oneOf valid",
           "'foobar'",
           true
           )]

        [InlineData(
           "both oneOf valid",
           "'foo'",
           false
           )]

        public void OneOfWithBaseSchema(string desc, string data, bool expected)
        {
            // oneOf with base schema
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'oneOf':[ { 'minLength':2 }, { 'maxLength':4 } ], 'type':'string' }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft4 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     3 - oneOf complex types
        /// </summary>

        [Theory]
        [InlineData(
           "first oneOf valid (complex)",
           "{ 'bar':2 }",
           true
           )]

        [InlineData(
           "second oneOf valid (complex)",
           "{ 'foo':'baz' }",
           true
           )]

        [InlineData(
           "both oneOf valid (complex)",
           "{ 'bar':2, 'foo':'baz' }",
           false
           )]

        [InlineData(
           "neither oneOf valid (complex)",
           "{ 'bar':'quux', 'foo':2 }",
           false
           )]

        public void OneOfComplexTypes(string desc, string data, bool expected)
        {
            // oneOf complex types
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'oneOf':[ { 'properties':{ 'bar':{ 'type':'integer' } }, 'required':[ 'bar' ] }, { 'properties':{ 'foo':{ 'type':'string' } }, 'required':[ 'foo' ] } ] }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft4 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     4 - oneOf with empty schema
        /// </summary>

        [Theory]
        [InlineData(
           "one valid - valid",
           "'foo'",
           true
           )]

        [InlineData(
           "both valid - invalid",
           "123",
           false
           )]

        public void OneOfWithEmptySchema(string desc, string data, bool expected)
        {
            // oneOf with empty schema
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'oneOf':[ { 'type':'number' }, { } ] }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft4 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     5 - oneOf with required
        /// </summary>

        [Theory]
        [InlineData(
           "both invalid - invalid",
           "{ 'bar':2 }",
           false
           )]

        [InlineData(
           "first valid - valid",
           "{ 'bar':2, 'foo':1 }",
           true
           )]

        [InlineData(
           "second valid - valid",
           "{ 'baz':3, 'foo':1 }",
           true
           )]

        [InlineData(
           "both valid - invalid",
           "{ 'bar':2, 'baz':3, 'foo':1 }",
           false
           )]

        public void OneOfWithRequired(string desc, string data, bool expected)
        {
            // oneOf with required
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'oneOf':[ { 'required':[ 'foo', 'bar' ] }, { 'required':[ 'foo', 'baz' ] } ], 'type':'object' }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft4 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


    }
}

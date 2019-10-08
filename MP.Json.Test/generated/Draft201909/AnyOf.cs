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
    public class AnyOf
    {

        /// <summary>
        ///     1 - anyOf
        /// </summary>

        [Theory]
        [InlineData(
           "first anyOf valid",
           "1",
           true
           )]

        [InlineData(
           "second anyOf valid",
           "2.5",
           true
           )]

        [InlineData(
           "both anyOf valid",
           "3",
           true
           )]

        [InlineData(
           "neither anyOf valid",
           "1.5",
           false
           )]

        public void AnyOfTest(string desc, string data, bool expected)
        {
            // anyOf
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'anyOf':[ { 'type':'integer' }, { 'minimum':2 } ] }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft201909 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     2 - anyOf with base schema
        /// </summary>

        [Theory]
        [InlineData(
           "mismatch base schema",
           "3",
           false
           )]

        [InlineData(
           "one anyOf valid",
           "'foobar'",
           true
           )]

        [InlineData(
           "both anyOf invalid",
           "'foo'",
           false
           )]

        public void AnyOfWithBaseSchema(string desc, string data, bool expected)
        {
            // anyOf with base schema
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'anyOf':[ { 'maxLength':2 }, { 'minLength':4 } ], 'type':'string' }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft201909 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     3 - anyOf with boolean schemas, all true
        /// </summary>

        [Theory]
        [InlineData(
           "any value is valid",
           "'foo'",
           true
           )]

        public void AnyOfWithBooleanSchemasAllTrue(string desc, string data, bool expected)
        {
            // anyOf with boolean schemas, all true
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'anyOf':[ true, true ] }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft201909 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     4 - anyOf with boolean schemas, some true
        /// </summary>

        [Theory]
        [InlineData(
           "any value is valid",
           "'foo'",
           true
           )]

        public void AnyOfWithBooleanSchemasSomeTrue(string desc, string data, bool expected)
        {
            // anyOf with boolean schemas, some true
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'anyOf':[ true, false ] }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft201909 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     5 - anyOf with boolean schemas, all false
        /// </summary>

        [Theory]
        [InlineData(
           "any value is invalid",
           "'foo'",
           false
           )]

        public void AnyOfWithBooleanSchemasAllFalse(string desc, string data, bool expected)
        {
            // anyOf with boolean schemas, all false
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'anyOf':[ false, false ] }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft201909 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     6 - anyOf complex types
        /// </summary>

        [Theory]
        [InlineData(
           "first anyOf valid (complex)",
           "{ 'bar':2 }",
           true
           )]

        [InlineData(
           "second anyOf valid (complex)",
           "{ 'foo':'baz' }",
           true
           )]

        [InlineData(
           "both anyOf valid (complex)",
           "{ 'bar':2, 'foo':'baz' }",
           true
           )]

        [InlineData(
           "neither anyOf valid (complex)",
           "{ 'bar':'quux', 'foo':2 }",
           false
           )]

        public void AnyOfComplexTypes(string desc, string data, bool expected)
        {
            // anyOf complex types
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'anyOf':[ { 'properties':{ 'bar':{ 'type':'integer' } }, 'required':[ 'bar' ] }, { 'properties':{ 'foo':{ 'type':'string' } }, 'required':[ 'foo' ] } ] }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft201909 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     7 - anyOf with one empty schema
        /// </summary>

        [Theory]
        [InlineData(
           "string is valid",
           "'foo'",
           true
           )]

        [InlineData(
           "number is valid",
           "123",
           true
           )]

        public void AnyOfWithOneEmptySchema(string desc, string data, bool expected)
        {
            // anyOf with one empty schema
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'anyOf':[ { 'type':'number' }, { } ] }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft201909 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     8 - nested anyOf, to check validation semantics
        /// </summary>

        [Theory]
        [InlineData(
           "null is valid",
           "null",
           true
           )]

        [InlineData(
           "anything non-null is invalid",
           "123",
           false
           )]

        public void NestedAnyOfToCheckValidationSemantics(string desc, string data, bool expected)
        {
            // nested anyOf, to check validation semantics
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'anyOf':[ { 'anyOf':[ { 'type':'null' } ] } ] }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft201909 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


    }
}

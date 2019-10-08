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

namespace JsonSchemaTestSuite.Draft7
{
    public class AllOf
    {

        /// <summary>
        ///     1 - allOf
        /// </summary>

        [Theory]
        [InlineData(
           "allOf",
           "{ 'bar':2, 'foo':'baz' }",
           true
           )]

        [InlineData(
           "mismatch second",
           "{ 'foo':'baz' }",
           false
           )]

        [InlineData(
           "mismatch first",
           "{ 'bar':2 }",
           false
           )]

        [InlineData(
           "wrong type",
           "{ 'bar':'quux', 'foo':'baz' }",
           false
           )]

        public void AllOfTest(string desc, string data, bool expected)
        {
            // allOf
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'allOf':[ { 'properties':{ 'bar':{ 'type':'integer' } }, 'required':[ 'bar' ] }, { 'properties':{ 'foo':{ 'type':'string' } }, 'required':[ 'foo' ] } ] }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft7 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     2 - allOf with base schema
        /// </summary>

        [Theory]
        [InlineData(
           "valid",
           "{ 'bar':2, 'baz':null, 'foo':'quux' }",
           true
           )]

        [InlineData(
           "mismatch base schema",
           "{ 'baz':null, 'foo':'quux' }",
           false
           )]

        [InlineData(
           "mismatch first allOf",
           "{ 'bar':2, 'baz':null }",
           false
           )]

        [InlineData(
           "mismatch second allOf",
           "{ 'bar':2, 'foo':'quux' }",
           false
           )]

        [InlineData(
           "mismatch both",
           "{ 'bar':2 }",
           false
           )]

        public void AllOfWithBaseSchema(string desc, string data, bool expected)
        {
            // allOf with base schema
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'allOf':[ { 'properties':{ 'foo':{ 'type':'string' } }, 'required':[ 'foo' ] }, { 'properties':{ 'baz':{ 'type':'null' } }, 'required':[ 'baz' ] } ], 'properties':{ 'bar':{ 'type':'integer' } }, 'required':[ 'bar' ] }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft7 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     3 - allOf simple types
        /// </summary>

        [Theory]
        [InlineData(
           "valid",
           "25",
           true
           )]

        [InlineData(
           "mismatch one",
           "35",
           false
           )]

        public void AllOfSimpleTypes(string desc, string data, bool expected)
        {
            // allOf simple types
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'allOf':[ { 'maximum':30 }, { 'minimum':20 } ] }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft7 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     4 - allOf with boolean schemas, all true
        /// </summary>

        [Theory]
        [InlineData(
           "any value is valid",
           "'foo'",
           true
           )]

        public void AllOfWithBooleanSchemasAllTrue(string desc, string data, bool expected)
        {
            // allOf with boolean schemas, all true
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'allOf':[ true, true ] }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft7 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     5 - allOf with boolean schemas, some false
        /// </summary>

        [Theory]
        [InlineData(
           "any value is invalid",
           "'foo'",
           false
           )]

        public void AllOfWithBooleanSchemasSomeFalse(string desc, string data, bool expected)
        {
            // allOf with boolean schemas, some false
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'allOf':[ true, false ] }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft7 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     6 - allOf with boolean schemas, all false
        /// </summary>

        [Theory]
        [InlineData(
           "any value is invalid",
           "'foo'",
           false
           )]

        public void AllOfWithBooleanSchemasAllFalse(string desc, string data, bool expected)
        {
            // allOf with boolean schemas, all false
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'allOf':[ false, false ] }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft7 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     7 - allOf with one empty schema
        /// </summary>

        [Theory]
        [InlineData(
           "any data is valid",
           "1",
           true
           )]

        public void AllOfWithOneEmptySchema(string desc, string data, bool expected)
        {
            // allOf with one empty schema
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'allOf':[ { } ] }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft7 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     8 - allOf with two empty schemas
        /// </summary>

        [Theory]
        [InlineData(
           "any data is valid",
           "1",
           true
           )]

        public void AllOfWithTwoEmptySchemas(string desc, string data, bool expected)
        {
            // allOf with two empty schemas
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'allOf':[ { }, { } ] }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft7 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     9 - allOf with the first empty schema
        /// </summary>

        [Theory]
        [InlineData(
           "number is valid",
           "1",
           true
           )]

        [InlineData(
           "string is invalid",
           "'foo'",
           false
           )]

        public void AllOfWithTheFirstEmptySchema(string desc, string data, bool expected)
        {
            // allOf with the first empty schema
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'allOf':[ { }, { 'type':'number' } ] }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft7 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     10 - allOf with the last empty schema
        /// </summary>

        [Theory]
        [InlineData(
           "number is valid",
           "1",
           true
           )]

        [InlineData(
           "string is invalid",
           "'foo'",
           false
           )]

        public void AllOfWithTheLastEmptySchema(string desc, string data, bool expected)
        {
            // allOf with the last empty schema
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'allOf':[ { 'type':'number' }, { } ] }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft7 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


    }
}

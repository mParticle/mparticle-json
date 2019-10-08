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
    public class Not
    {

        /// <summary>
        ///     1 - not
        /// </summary>

        [Theory]
        [InlineData(
           "allowed",
           "'foo'",
           true
           )]

        [InlineData(
           "disallowed",
           "1",
           false
           )]

        public void NotTest(string desc, string data, bool expected)
        {
            // not
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'not':{ 'type':'integer' } }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft4 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     2 - not multiple types
        /// </summary>

        [Theory]
        [InlineData(
           "valid",
           "'foo'",
           true
           )]

        [InlineData(
           "mismatch",
           "1",
           false
           )]

        [InlineData(
           "other mismatch",
           "true",
           false
           )]

        public void NotMultipleTypes(string desc, string data, bool expected)
        {
            // not multiple types
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'not':{ 'type':[ 'integer', 'boolean' ] } }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft4 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     3 - not more complex schema
        /// </summary>

        [Theory]
        [InlineData(
           "match",
           "1",
           true
           )]

        [InlineData(
           "other match",
           "{ 'foo':1 }",
           true
           )]

        [InlineData(
           "mismatch",
           "{ 'foo':'bar' }",
           false
           )]

        public void NotMoreComplexSchema(string desc, string data, bool expected)
        {
            // not more complex schema
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'not':{ 'properties':{ 'foo':{ 'type':'string' } }, 'type':'object' } }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft4 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     4 - forbidden property
        /// </summary>

        [Theory]
        [InlineData(
           "property present",
           "{ 'bar':2, 'foo':1 }",
           false
           )]

        [InlineData(
           "property absent",
           "{ 'bar':1, 'baz':2 }",
           true
           )]

        public void ForbiddenProperty(string desc, string data, bool expected)
        {
            // forbidden property
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'properties':{ 'foo':{ 'not':{ } } } }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft4 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


    }
}

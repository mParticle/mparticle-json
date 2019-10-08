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
    public class Default
    {

        /// <summary>
        ///     1 - invalid type for default
        /// </summary>

        [Theory]
        [InlineData(
           "valid when property is specified",
           "{ 'foo':13 }",
           true
           )]

        [InlineData(
           "still valid when the invalid default is used",
           "{ }",
           true
           )]

        public void InvalidTypeForDefault(string desc, string data, bool expected)
        {
            // invalid type for default
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'properties':{ 'foo':{ 'default':[ ], 'type':'integer' } } }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft6 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     2 - invalid string value for default
        /// </summary>

        [Theory]
        [InlineData(
           "valid when property is specified",
           "{ 'bar':'good' }",
           true
           )]

        [InlineData(
           "still valid when the invalid default is used",
           "{ }",
           true
           )]

        public void InvalidStringValueForDefault(string desc, string data, bool expected)
        {
            // invalid string value for default
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'properties':{ 'bar':{ 'default':'bad', 'minLength':4, 'type':'string' } } }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft6 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


    }
}

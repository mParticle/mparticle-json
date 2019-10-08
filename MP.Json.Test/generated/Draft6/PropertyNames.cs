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
    public class PropertyNames
    {

        /// <summary>
        ///     1 - propertyNames validation
        /// </summary>

        [Theory]
        [InlineData(
           "all property names valid",
           "{ 'f':{ }, 'foo':{ } }",
           true
           )]

        [InlineData(
           "some property names invalid",
           "{ 'foo':{ }, 'foobar':{ } }",
           false
           )]

        [InlineData(
           "object without properties is valid",
           "{ }",
           true
           )]

        [InlineData(
           "ignores arrays",
           "[ 1, 2, 3, 4 ]",
           true
           )]

        [InlineData(
           "ignores strings",
           "'foobar'",
           true
           )]

        [InlineData(
           "ignores other non-objects",
           "12",
           true
           )]

        public void PropertyNamesValidation(string desc, string data, bool expected)
        {
            // propertyNames validation
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'propertyNames':{ 'maxLength':3 } }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft6 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     2 - propertyNames with boolean schema true
        /// </summary>

        [Theory]
        [InlineData(
           "object with any properties is valid",
           "{ 'foo':1 }",
           true
           )]

        [InlineData(
           "empty object is valid",
           "{ }",
           true
           )]

        public void PropertyNamesWithBooleanSchemaTrue(string desc, string data, bool expected)
        {
            // propertyNames with boolean schema true
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'propertyNames':true }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft6 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     3 - propertyNames with boolean schema false
        /// </summary>

        [Theory]
        [InlineData(
           "object with any properties is invalid",
           "{ 'foo':1 }",
           false
           )]

        [InlineData(
           "empty object is valid",
           "{ }",
           true
           )]

        public void PropertyNamesWithBooleanSchemaFalse(string desc, string data, bool expected)
        {
            // propertyNames with boolean schema false
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'propertyNames':false }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft6 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


    }
}

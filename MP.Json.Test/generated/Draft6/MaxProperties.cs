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
    public class MaxProperties
    {

        /// <summary>
        ///     1 - maxProperties validation
        /// </summary>

        [Theory]
        [InlineData(
           "shorter is valid",
           "{ 'foo':1 }",
           true
           )]

        [InlineData(
           "exact length is valid",
           "{ 'bar':2, 'foo':1 }",
           true
           )]

        [InlineData(
           "too long is invalid",
           "{ 'bar':2, 'baz':3, 'foo':1 }",
           false
           )]

        [InlineData(
           "ignores arrays",
           "[ 1, 2, 3 ]",
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

        public void MaxPropertiesValidation(string desc, string data, bool expected)
        {
            // maxProperties validation
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'maxProperties':2 }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft6 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


    }
}

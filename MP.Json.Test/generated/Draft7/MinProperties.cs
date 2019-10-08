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
    public class MinProperties
    {

        /// <summary>
        ///     1 - minProperties validation
        /// </summary>

        [Theory]
        [InlineData(
           "longer is valid",
           "{ 'bar':2, 'foo':1 }",
           true
           )]

        [InlineData(
           "exact length is valid",
           "{ 'foo':1 }",
           true
           )]

        [InlineData(
           "too short is invalid",
           "{ }",
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

        public void MinPropertiesValidation(string desc, string data, bool expected)
        {
            // minProperties validation
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'minProperties':1 }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft7 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


    }
}

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
    public class Minimum
    {

        /// <summary>
        ///     1 - minimum validation
        /// </summary>

        [Theory]
        [InlineData(
           "above the minimum is valid",
           "2.6",
           true
           )]

        [InlineData(
           "boundary point is valid",
           "1.1",
           true
           )]

        [InlineData(
           "below the minimum is invalid",
           "0.6",
           false
           )]

        [InlineData(
           "ignores non-numbers",
           "'x'",
           true
           )]

        public void MinimumValidation(string desc, string data, bool expected)
        {
            // minimum validation
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'minimum':1.1 }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft6 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     2 - minimum validation with signed integer
        /// </summary>

        [Theory]
        [InlineData(
           "negative above the minimum is valid",
           "-1",
           true
           )]

        [InlineData(
           "positive above the minimum is valid",
           "0",
           true
           )]

        [InlineData(
           "boundary point is valid",
           "-2",
           true
           )]

        [InlineData(
           "below the minimum is invalid",
           "-3",
           false
           )]

        [InlineData(
           "ignores non-numbers",
           "'x'",
           true
           )]

        public void MinimumValidationWithSignedInteger(string desc, string data, bool expected)
        {
            // minimum validation with signed integer
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'minimum':-2 }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft6 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


    }
}

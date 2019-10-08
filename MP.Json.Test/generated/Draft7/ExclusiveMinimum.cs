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
    public class ExclusiveMinimum
    {

        /// <summary>
        ///     1 - exclusiveMinimum validation
        /// </summary>

        [Theory]
        [InlineData(
           "above the exclusiveMinimum is valid",
           "1.2",
           true
           )]

        [InlineData(
           "boundary point is invalid",
           "1.1",
           false
           )]

        [InlineData(
           "below the exclusiveMinimum is invalid",
           "0.6",
           false
           )]

        [InlineData(
           "ignores non-numbers",
           "'x'",
           true
           )]

        public void ExclusiveMinimumValidation(string desc, string data, bool expected)
        {
            // exclusiveMinimum validation
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'exclusiveMinimum':1.1 }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft7 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


    }
}

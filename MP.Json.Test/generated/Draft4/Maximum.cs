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
    public class Maximum
    {

        /// <summary>
        ///     1 - maximum validation
        /// </summary>

        [Theory]
        [InlineData(
           "below the maximum is valid",
           "2.6",
           true
           )]

        [InlineData(
           "boundary point is valid",
           "3",
           true
           )]

        [InlineData(
           "above the maximum is invalid",
           "3.5",
           false
           )]

        [InlineData(
           "ignores non-numbers",
           "'x'",
           true
           )]

        public void MaximumValidation(string desc, string data, bool expected)
        {
            // maximum validation
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'maximum':3 }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft4 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     2 - maximum validation (explicit false exclusivity)
        /// </summary>

        [Theory]
        [InlineData(
           "below the maximum is valid",
           "2.6",
           true
           )]

        [InlineData(
           "boundary point is valid",
           "3",
           true
           )]

        [InlineData(
           "above the maximum is invalid",
           "3.5",
           false
           )]

        [InlineData(
           "ignores non-numbers",
           "'x'",
           true
           )]

        public void MaximumValidationExplicitFalseExclusivity(string desc, string data, bool expected)
        {
            // maximum validation (explicit false exclusivity)
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'exclusiveMaximum':false, 'maximum':3 }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft4 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     3 - exclusiveMaximum validation
        /// </summary>

        [Theory]
        [InlineData(
           "below the maximum is still valid",
           "2.2",
           true
           )]

        [InlineData(
           "boundary point is invalid",
           "3",
           false
           )]

        public void ExclusiveMaximumValidation(string desc, string data, bool expected)
        {
            // exclusiveMaximum validation
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'exclusiveMaximum':true, 'maximum':3 }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft4 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


    }
}

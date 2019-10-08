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
    public class Pattern
    {

        /// <summary>
        ///     1 - pattern validation
        /// </summary>

        [Theory]
        [InlineData(
           "a matching pattern is valid",
           "'aaa'",
           true
           )]

        [InlineData(
           "a non-matching pattern is invalid",
           "'abc'",
           false
           )]

        [InlineData(
           "ignores non-strings",
           "true",
           true
           )]

        public void PatternValidation(string desc, string data, bool expected)
        {
            // pattern validation
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'pattern':'^a*$' }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft4 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     2 - pattern is not anchored
        /// </summary>

        [Theory]
        [InlineData(
           "matches a substring",
           "'xxaayy'",
           true
           )]

        public void PatternIsNotAnchored(string desc, string data, bool expected)
        {
            // pattern is not anchored
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'pattern':'a+' }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft4 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


    }
}

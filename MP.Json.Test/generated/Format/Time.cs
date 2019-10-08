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

namespace JsonSchemaTestSuite.Format
{
    public class Time
    {

        /// <summary>
        ///     1 - validation of time strings
        /// </summary>

        [Theory]
        [InlineData(
           "a valid time string",
           "'08:30:06.283185Z'",
           true
           )]

        [InlineData(
           "an invalid time string",
           "'08:30:06 PST'",
           false
           )]

        [InlineData(
           "only RFC3339 not all of ISO 8601 are valid",
           "'01:01:01,1111'",
           false
           )]

        public void ValidationOfTimeStrings(string desc, string data, bool expected)
        {
            // validation of time strings
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'format':'time' }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft7 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


    }
}

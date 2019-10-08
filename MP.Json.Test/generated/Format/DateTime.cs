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
    public class DateTime
    {

        /// <summary>
        ///     1 - validation of date-time strings
        /// </summary>

        [Theory]
        [InlineData(
           "a valid date-time string",
           "'1963-06-19T08:30:06.283185Z'",
           true
           )]

        [InlineData(
           "a valid date-time string without second fraction",
           "'1963-06-19T08:30:06Z'",
           true
           )]

        [InlineData(
           "a valid date-time string with plus offset",
           "'1937-01-01T12:00:27.87+00:20'",
           true
           )]

        [InlineData(
           "a valid date-time string with minus offset",
           "'1990-12-31T15:59:50.123-08:00'",
           true
           )]

        [InlineData(
           "a invalid day in date-time string",
           "'1990-02-31T15:59:60.123-08:00'",
           false
           )]

        [InlineData(
           "an invalid offset in date-time string",
           "'1990-12-31T15:59:60-24:00'",
           false
           )]

        [InlineData(
           "an invalid date-time string",
           "'06/19/1963 08:30:06 PST'",
           false
           )]

        [InlineData(
           "case-insensitive T and Z",
           "'1963-06-19t08:30:06.283185z'",
           true
           )]

        [InlineData(
           "only RFC3339 not all of ISO 8601 are valid",
           "'2013-350T01:01:01'",
           false
           )]

        public void ValidationOfDateTimeStrings(string desc, string data, bool expected)
        {
            // validation of date-time strings
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'format':'date-time' }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft7 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


    }
}

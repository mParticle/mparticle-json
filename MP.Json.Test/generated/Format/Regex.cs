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
    public class Regex
    {

        /// <summary>
        ///     1 - validation of regular expressions
        /// </summary>

        [Theory]
        [InlineData(
           "a valid regular expression",
           @"'([abc])+\\s+$'",
           true
           )]

        [InlineData(
           "a regular expression with unclosed parens is invalid",
           "'^(abc]'",
           false
           )]

        public void ValidationOfRegularExpressions(string desc, string data, bool expected)
        {
            // validation of regular expressions
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'format':'regex' }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft7 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


    }
}

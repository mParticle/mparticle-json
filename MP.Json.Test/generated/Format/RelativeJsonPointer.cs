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
    public class RelativeJsonPointer
    {

        /// <summary>
        ///     1 - validation of Relative JSON Pointers (RJP)
        /// </summary>

        [Theory]
        [InlineData(
           "a valid upwards RJP",
           "'1'",
           true
           )]

        [InlineData(
           "a valid downwards RJP",
           "'0/foo/bar'",
           true
           )]

        [InlineData(
           "a valid up and then down RJP, with array index",
           "'2/0/baz/1/zip'",
           true
           )]

        [InlineData(
           "a valid RJP taking the member or index name",
           "'0#'",
           true
           )]

        [InlineData(
           "an invalid RJP that is a valid JSON Pointer",
           "'/foo/bar'",
           false
           )]

        public void ValidationOfRelativeJSONPointersRJP(string desc, string data, bool expected)
        {
            // validation of Relative JSON Pointers (RJP)
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'format':'relative-json-pointer' }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft7 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


    }
}

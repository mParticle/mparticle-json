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
    public class JsonPointer
    {

        /// <summary>
        ///     1 - validation of JSON-pointers (JSON String Representation)
        /// </summary>

        [Theory]
        [InlineData(
           "a valid JSON-pointer",
           "'/foo/bar~0/baz~1/%a'",
           true
           )]

        [InlineData(
           "not a valid JSON-pointer (~ not escaped)",
           "'/foo/bar~'",
           false
           )]

        [InlineData(
           "valid JSON-pointer with empty segment",
           "'/foo//bar'",
           true
           )]

        [InlineData(
           "valid JSON-pointer with the last empty segment",
           "'/foo/bar/'",
           true
           )]

        [InlineData(
           "valid JSON-pointer as stated in RFC 6901 #1",
           "''",
           true
           )]

        [InlineData(
           "valid JSON-pointer as stated in RFC 6901 #2",
           "'/foo'",
           true
           )]

        [InlineData(
           "valid JSON-pointer as stated in RFC 6901 #3",
           "'/foo/0'",
           true
           )]

        [InlineData(
           "valid JSON-pointer as stated in RFC 6901 #4",
           "'/'",
           true
           )]

        [InlineData(
           "valid JSON-pointer as stated in RFC 6901 #5",
           "'/a~1b'",
           true
           )]

        [InlineData(
           "valid JSON-pointer as stated in RFC 6901 #6",
           "'/c%d'",
           true
           )]

        [InlineData(
           "valid JSON-pointer as stated in RFC 6901 #7",
           "'/e^f'",
           true
           )]

        [InlineData(
           "valid JSON-pointer as stated in RFC 6901 #8",
           "'/g|h'",
           true
           )]

        [InlineData(
           "valid JSON-pointer as stated in RFC 6901 #9",
           @"'/i\\j'",
           true
           )]

        [InlineData(
           "valid JSON-pointer as stated in RFC 6901 #10",
           @"'/k\'l'",
           true
           )]

        [InlineData(
           "valid JSON-pointer as stated in RFC 6901 #11",
           "'/ '",
           true
           )]

        [InlineData(
           "valid JSON-pointer as stated in RFC 6901 #12",
           "'/m~0n'",
           true
           )]

        [InlineData(
           "valid JSON-pointer used adding to the last array position",
           "'/foo/-'",
           true
           )]

        [InlineData(
           "valid JSON-pointer (- used as object member name)",
           "'/foo/-/bar'",
           true
           )]

        [InlineData(
           "valid JSON-pointer (multiple escaped characters)",
           "'/~1~0~0~1~1'",
           true
           )]

        [InlineData(
           "valid JSON-pointer (escaped with fraction part) #1",
           "'/~1.1'",
           true
           )]

        [InlineData(
           "valid JSON-pointer (escaped with fraction part) #2",
           "'/~0.1'",
           true
           )]

        [InlineData(
           "not a valid JSON-pointer (URI Fragment Identifier) #1",
           "'#'",
           false
           )]

        [InlineData(
           "not a valid JSON-pointer (URI Fragment Identifier) #2",
           "'#/'",
           false
           )]

        [InlineData(
           "not a valid JSON-pointer (URI Fragment Identifier) #3",
           "'#a'",
           false
           )]

        [InlineData(
           "not a valid JSON-pointer (some escaped, but not all) #1",
           "'/~0~'",
           false
           )]

        [InlineData(
           "not a valid JSON-pointer (some escaped, but not all) #2",
           "'/~0/~'",
           false
           )]

        [InlineData(
           "not a valid JSON-pointer (wrong escape character) #1",
           "'/~2'",
           false
           )]

        [InlineData(
           "not a valid JSON-pointer (wrong escape character) #2",
           "'/~-1'",
           false
           )]

        [InlineData(
           "not a valid JSON-pointer (multiple characters not escaped)",
           "'/~~'",
           false
           )]

        [InlineData(
           "not a valid JSON-pointer (isn't empty nor starts with /) #1",
           "'a'",
           false
           )]

        [InlineData(
           "not a valid JSON-pointer (isn't empty nor starts with /) #2",
           "'0'",
           false
           )]

        [InlineData(
           "not a valid JSON-pointer (isn't empty nor starts with /) #3",
           "'a/a'",
           false
           )]

        public void ValidationOfJSONPointersJSONStringRepresentation(string desc, string data, bool expected)
        {
            // validation of JSON-pointers (JSON String Representation)
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'format':'json-pointer' }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft7 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


    }
}

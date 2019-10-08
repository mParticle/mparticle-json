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
    public class Iri
    {
        /// <summary>
        ///     1 - validation of IRIs
        /// </summary>

        [Theory]
        [InlineData(
           "a valid IRI with anchor tag",
           "'http://ƒøø.ßår/?∂éœ=πîx#πîüx'",
           true
           )]

        [InlineData(
           "a valid IRI with anchor tag and parantheses",
           "'http://ƒøø.com/blah_(wîkïpédiå)_blah#ßité-1'",
           true
           )]

        [InlineData(
           "a valid IRI with URL-encoded stuff",
           "'http://ƒøø.ßår/?q=Test%20URL-encoded%20stuff'",
           true
           )]

        [InlineData(
           "a valid IRI with many special characters",
           @"""http://-.~_!$&'()*+,;=:%40:80%2f::::::@example.com""",
           true
           )]

        [InlineData(
           "a valid IRI based on IPv6",
           "'http://[2001:0db8:85a3:0000:0000:8a2e:0370:7334]'",
           true
           )]

        [InlineData(
           "an invalid IRI based on IPv6",
           "'http://2001:0db8:85a3:0000:0000:8a2e:0370:7334'",
           false
           )]

        [InlineData(
           "an invalid relative IRI Reference",
           "'/abc'",
           false
           )]

        [InlineData(
           "an invalid IRI",
           @"'\\\\WINDOWS\\filëßåré'",
           false
           )]

        [InlineData(
           "an invalid IRI though valid IRI reference",
           "'âππ'",
           false
           )]

        public void ValidationOfIRIs(string desc, string data, bool expected)
        {
            // validation of IRIs
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'format':'iri' }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft7 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }
    }
}

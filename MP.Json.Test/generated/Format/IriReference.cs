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
    public class IriReference
    {

        /// <summary>
        ///     1 - validation of IRI References
        /// </summary>

        [Theory]
        [InlineData(
           "a valid IRI",
           "'http://ƒøø.ßår/?∂éœ=πîx#πîüx'",
           true
           )]

        [InlineData(
           "a valid protocol-relative IRI Reference",
           "'//ƒøø.ßår/?∂éœ=πîx#πîüx'",
           true
           )]

        [InlineData(
           "a valid relative IRI Reference",
           "'/âππ'",
           true
           )]

        [InlineData(
           "an invalid IRI Reference",
           @"'\\\\WINDOWS\\filëßåré'",
           false
           )]

        [InlineData(
           "a valid IRI Reference",
           "'âππ'",
           true
           )]

        [InlineData(
           "a valid IRI fragment",
           "'#ƒrägmênt'",
           true
           )]

        [InlineData(
           "an invalid IRI fragment",
           @"'#ƒräg\\mênt'",
           false
           )]

        public void ValidationOfIRIReferences(string desc, string data, bool expected)
        {
            // validation of IRI References
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'format':'iri-reference' }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft7 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


    }
}

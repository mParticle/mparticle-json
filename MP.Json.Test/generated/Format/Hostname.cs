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
    public class Hostname
    {

        /// <summary>
        ///     1 - validation of host names
        /// </summary>

        [Theory]
        [InlineData(
           "a valid host name",
           "'www.example.com'",
           true
           )]

        [InlineData(
           "a valid punycoded IDN hostname",
           "'xn--4gbwdl.xn--wgbh1c'",
           true
           )]

        [InlineData(
           "a host name starting with an illegal character",
           "'-a-host-name-that-starts-with--'",
           false
           )]

        [InlineData(
           "a host name containing illegal characters",
           "'not_a_valid_host_name'",
           false
           )]

        [InlineData(
           "a host name with a component too long",
           "'a-vvvvvvvvvvvvvvvveeeeeeeeeeeeeeeerrrrrrrrrrrrrrrryyyyyyyyyyyyyyyy-long-host-name-component'",
           false
           )]

        public void ValidationOfHostNames(string desc, string data, bool expected)
        {
            // validation of host names
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'format':'hostname' }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft7 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


    }
}

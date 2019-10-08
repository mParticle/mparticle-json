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

namespace JsonSchemaTestSuite.Draft7
{
    public class Definitions
    {

        /// <summary>
        ///     1 - valid definition
        /// </summary>

        [Theory]
        [InlineData(
           "valid definition schema",
           "{ 'definitions':{ 'foo':{ 'type':'integer' } } }",
           true
           )]

        public void ValidDefinition(string desc, string data, bool expected)
        {
            // valid definition
            Console.Error.WriteLine(desc);
            string schemaData = "{ '$ref':'http://json-schema.org/draft-07/schema#' }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft7 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     2 - invalid definition
        /// </summary>

        [Theory]
        [InlineData(
           "invalid definition schema",
           "{ 'definitions':{ 'foo':{ 'type':1 } } }",
           false
           )]

        public void InvalidDefinition(string desc, string data, bool expected)
        {
            // invalid definition
            Console.Error.WriteLine(desc);
            string schemaData = "{ '$ref':'http://json-schema.org/draft-07/schema#' }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = new MPSchema(schemaJson);
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft7 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


    }
}

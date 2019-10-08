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

namespace JsonSchemaTestSuite.Draft6
{
    public class MultipleOf
    {

        /// <summary>
        ///     1 - by int
        /// </summary>

        [Theory]
        [InlineData(
           "int by int",
           "10",
           true
           )]

        [InlineData(
           "int by int fail",
           "7",
           false
           )]

        [InlineData(
           "ignores non-numbers",
           "'foo'",
           true
           )]

        public void ByInt(string desc, string data, bool expected)
        {
            // by int
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'multipleOf':2 }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft6 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     2 - by number
        /// </summary>

        [Theory]
        [InlineData(
           "zero is multiple of anything",
           "0",
           true
           )]

        [InlineData(
           "4.5 is multiple of 1.5",
           "4.5",
           true
           )]

        [InlineData(
           "35 is not multiple of 1.5",
           "35",
           false
           )]

        public void ByNumber(string desc, string data, bool expected)
        {
            // by number
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'multipleOf':1.5 }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft6 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     3 - by small number
        /// </summary>

        [Theory]
        [InlineData(
           "0.0075 is multiple of 0.0001",
           "0.0075",
           true
           )]

        [InlineData(
           "0.00751 is not multiple of 0.0001",
           "0.00751",
           false
           )]

        public void BySmallNumber(string desc, string data, bool expected)
        {
            // by small number
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'multipleOf':0.0001 }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft6 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


    }
}

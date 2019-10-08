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

namespace JsonSchemaTestSuite.Draft201909
{
    public class IfThenElse
    {

        /// <summary>
        ///     1 - ignore if without then or else
        /// </summary>

        [Theory]
        [InlineData(
           "valid when valid against lone if",
           "0",
           true
           )]

        [InlineData(
           "valid when invalid against lone if",
           "'hello'",
           true
           )]

        public void IgnoreIfWithoutThenOrElse(string desc, string data, bool expected)
        {
            // ignore if without then or else
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'if':{ 'const':0 } }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft201909 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     2 - ignore then without if
        /// </summary>

        [Theory]
        [InlineData(
           "valid when valid against lone then",
           "0",
           true
           )]

        [InlineData(
           "valid when invalid against lone then",
           "'hello'",
           true
           )]

        public void IgnoreThenWithoutIf(string desc, string data, bool expected)
        {
            // ignore then without if
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'then':{ 'const':0 } }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft201909 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     3 - ignore else without if
        /// </summary>

        [Theory]
        [InlineData(
           "valid when valid against lone else",
           "0",
           true
           )]

        [InlineData(
           "valid when invalid against lone else",
           "'hello'",
           true
           )]

        public void IgnoreElseWithoutIf(string desc, string data, bool expected)
        {
            // ignore else without if
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'else':{ 'const':0 } }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft201909 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     4 - if and then without else
        /// </summary>

        [Theory]
        [InlineData(
           "valid through then",
           "-1",
           true
           )]

        [InlineData(
           "invalid through then",
           "-100",
           false
           )]

        [InlineData(
           "valid when if test fails",
           "3",
           true
           )]

        public void IfAndThenWithoutElse(string desc, string data, bool expected)
        {
            // if and then without else
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'if':{ 'exclusiveMaximum':0 }, 'then':{ 'minimum':-10 } }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft201909 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }

        /// <summary>
        ///     5 - if and else without then
        /// </summary>

        [Theory]
        [InlineData(
           "valid when if test passes",
           "-1",
           true
           )]

        [InlineData(
           "valid through else",
           "4",
           true
           )]

        [InlineData(
           "invalid through else",
           "3",
           false
           )]

        public void IfAndElseWithoutThen(string desc, string data, bool expected)
        {
            // if and else without then
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'else':{ 'multipleOf':2 }, 'if':{ 'exclusiveMaximum':0 } }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft201909 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     6 - validate against correct branch, then vs else
        /// </summary>

        [Theory]
        [InlineData(
           "valid through then",
           "-1",
           true
           )]

        [InlineData(
           "invalid through then",
           "-100",
           false
           )]

        [InlineData(
           "valid through else",
           "4",
           true
           )]

        [InlineData(
           "invalid through else",
           "3",
           false
           )]

        public void ValidateAgainstCorrectBranchThenVsElse(string desc, string data, bool expected)
        {
            // validate against correct branch, then vs else
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'else':{ 'multipleOf':2 }, 'if':{ 'exclusiveMaximum':0 }, 'then':{ 'minimum':-10 } }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft201909 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     7 - non-interference across combined schemas
        /// </summary>

        [Theory]
        [InlineData(
           "valid, but would have been invalid through then",
           "-100",
           true
           )]

        [InlineData(
           "valid, but would have been invalid through else",
           "3",
           true
           )]

        public void NonInterferenceAcrossCombinedSchemas(string desc, string data, bool expected)
        {
            // non-interference across combined schemas
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'allOf':[ { 'if':{ 'exclusiveMaximum':0 } }, { 'then':{ 'minimum':-10 } }, { 'else':{ 'multipleOf':2 } } ] }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft201909 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


    }
}

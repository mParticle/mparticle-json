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

namespace JsonSchemaTestSuite.Draft4
{
    public class AdditionalProperties
    {

        /// <summary>
        ///     1 - additionalProperties being false does not allow other properties
        /// </summary>

        [Theory]
        [InlineData(
           "no additional properties is valid",
           "{ 'foo':1 }",
           true
           )]

        [InlineData(
           "an additional property is invalid",
           "{ 'bar':2, 'foo':1, 'quux':'boom' }",
           false
           )]

        [InlineData(
           "ignores arrays",
           "[ 1, 2, 3 ]",
           true
           )]

        [InlineData(
           "ignores strings",
           "'foobarbaz'",
           true
           )]

        [InlineData(
           "ignores other non-objects",
           "12",
           true
           )]

        [InlineData(
           "patternProperties are not additional properties",
           "{ 'foo':1, 'vroom':2 }",
           true
           )]

        public void AdditionalPropertiesBeingFalseDoesNotAllowOtherProperties(string desc, string data, bool expected)
        {
            // additionalProperties being false does not allow other properties
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'additionalProperties':false, 'patternProperties':{ '^v':{ } }, 'properties':{ 'bar':{ }, 'foo':{ } } }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft4 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     2 - non-ASCII pattern with additionalProperties
        /// </summary>

        [Theory]
        [InlineData(
           "matching the pattern is valid",
           "{ 'ármányos':2 }",
           true
           )]

        [InlineData(
           "not matching the pattern is invalid",
           "{ 'élmény':2 }",
           false
           )]

        public void NonASCIIPatternWithAdditionalProperties(string desc, string data, bool expected)
        {
            // non-ASCII pattern with additionalProperties
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'additionalProperties':false, 'patternProperties':{ '^á':{ } } }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft4 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     3 - additionalProperties allows a schema which should validate
        /// </summary>

        [Theory]
        [InlineData(
           "no additional properties is valid",
           "{ 'foo':1 }",
           true
           )]

        [InlineData(
           "an additional valid property is valid",
           "{ 'bar':2, 'foo':1, 'quux':true }",
           true
           )]

        [InlineData(
           "an additional invalid property is invalid",
           "{ 'bar':2, 'foo':1, 'quux':12 }",
           false
           )]

        public void AdditionalPropertiesAllowsASchemaWhichShouldValidate(string desc, string data, bool expected)
        {
            // additionalProperties allows a schema which should validate
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'additionalProperties':{ 'type':'boolean' }, 'properties':{ 'bar':{ }, 'foo':{ } } }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft4 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     4 - additionalProperties can exist by itself
        /// </summary>

        [Theory]
        [InlineData(
           "an additional valid property is valid",
           "{ 'foo':true }",
           true
           )]

        [InlineData(
           "an additional invalid property is invalid",
           "{ 'foo':1 }",
           false
           )]

        public void AdditionalPropertiesCanExistByItself(string desc, string data, bool expected)
        {
            // additionalProperties can exist by itself
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'additionalProperties':{ 'type':'boolean' } }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft4 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     5 - additionalProperties are allowed by default
        /// </summary>

        [Theory]
        [InlineData(
           "additional properties are allowed",
           "{ 'bar':2, 'foo':1, 'quux':true }",
           true
           )]

        public void AdditionalPropertiesAreAllowedByDefault(string desc, string data, bool expected)
        {
            // additionalProperties are allowed by default
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'properties':{ 'bar':{ }, 'foo':{ } } }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft4 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     6 - additionalProperties should not look in applicators
        /// </summary>

        [Theory]
        [InlineData(
           "properties defined in allOf are not allowed",
           "{ 'bar':true, 'foo':1 }",
           false
           )]

        public void AdditionalPropertiesShouldNotLookInApplicators(string desc, string data, bool expected)
        {
            // additionalProperties should not look in applicators
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'additionalProperties':{ 'type':'boolean' }, 'allOf':[ { 'properties':{ 'foo':{ } } } ] }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft4 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


    }
}

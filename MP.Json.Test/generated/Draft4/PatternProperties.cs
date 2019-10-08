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
    public class PatternProperties
    {

        /// <summary>
        ///     1 - patternProperties validates properties matching a regex
        /// </summary>

        [Theory]
        [InlineData(
           "a single valid match is valid",
           "{ 'foo':1 }",
           true
           )]

        [InlineData(
           "multiple valid matches is valid",
           "{ 'foo':1, 'foooooo':2 }",
           true
           )]

        [InlineData(
           "a single invalid match is invalid",
           "{ 'foo':'bar', 'fooooo':2 }",
           false
           )]

        [InlineData(
           "multiple invalid matches is invalid",
           "{ 'foo':'bar', 'foooooo':'baz' }",
           false
           )]

        [InlineData(
           "ignores arrays",
           "[ ]",
           true
           )]

        [InlineData(
           "ignores strings",
           "''",
           true
           )]

        [InlineData(
           "ignores other non-objects",
           "12",
           true
           )]

        public void PatternPropertiesValidatesPropertiesMatchingARegex(string desc, string data, bool expected)
        {
            // patternProperties validates properties matching a regex
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'patternProperties':{ 'f.*o':{ 'type':'integer' } } }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft4 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     2 - multiple simultaneous patternProperties are validated
        /// </summary>

        [Theory]
        [InlineData(
           "a single valid match is valid",
           "{ 'a':21 }",
           true
           )]

        [InlineData(
           "a simultaneous match is valid",
           "{ 'aaaa':18 }",
           true
           )]

        [InlineData(
           "multiple matches is valid",
           "{ 'a':21, 'aaaa':18 }",
           true
           )]

        [InlineData(
           "an invalid due to one is invalid",
           "{ 'a':'bar' }",
           false
           )]

        [InlineData(
           "an invalid due to the other is invalid",
           "{ 'aaaa':31 }",
           false
           )]

        [InlineData(
           "an invalid due to both is invalid",
           "{ 'aaa':'foo', 'aaaa':31 }",
           false
           )]

        public void MultipleSimultaneousPatternPropertiesAreValidated(string desc, string data, bool expected)
        {
            // multiple simultaneous patternProperties are validated
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'patternProperties':{ 'a*':{ 'type':'integer' }, 'aaa*':{ 'maximum':20 } } }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft4 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     3 - regexes are not anchored by default and are case sensitive
        /// </summary>

        [Theory]
        [InlineData(
           "non recognized members are ignored",
           "{ 'answer 1':'42' }",
           true
           )]

        [InlineData(
           "recognized members are accounted for",
           "{ 'a31b':null }",
           false
           )]

        [InlineData(
           "regexes are case sensitive",
           "{ 'a_x_3':3 }",
           true
           )]

        [InlineData(
           "regexes are case sensitive, 2",
           "{ 'a_X_3':3 }",
           false
           )]

        public void RegexesAreNotAnchoredByDefaultAndAreCaseSensitive(string desc, string data, bool expected)
        {
            // regexes are not anchored by default and are case sensitive
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'patternProperties':{ 'X_':{ 'type':'string' }, '[0-9]{2,}':{ 'type':'boolean' } } }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft4 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


    }
}

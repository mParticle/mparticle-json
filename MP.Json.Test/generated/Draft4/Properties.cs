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
    public class Properties
    {

        /// <summary>
        ///     1 - object properties validation
        /// </summary>

        [Theory]
        [InlineData(
           "both properties present and valid is valid",
           "{ 'bar':'baz', 'foo':1 }",
           true
           )]

        [InlineData(
           "one property invalid is invalid",
           "{ 'bar':{ }, 'foo':1 }",
           false
           )]

        [InlineData(
           "both properties invalid is invalid",
           "{ 'bar':{ }, 'foo':[ ] }",
           false
           )]

        [InlineData(
           "doesn't invalidate other properties",
           "{ 'quux':[ ] }",
           true
           )]

        [InlineData(
           "ignores arrays",
           "[ ]",
           true
           )]

        [InlineData(
           "ignores other non-objects",
           "12",
           true
           )]

        public void ObjectPropertiesValidation(string desc, string data, bool expected)
        {
            // object properties validation
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'properties':{ 'bar':{ 'type':'string' }, 'foo':{ 'type':'integer' } } }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft4 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     2 - properties, patternProperties, additionalProperties interaction
        /// </summary>

        [Theory]
        [InlineData(
           "property validates property",
           "{ 'foo':[ 1, 2 ] }",
           true
           )]

        [InlineData(
           "property invalidates property",
           "{ 'foo':[ 1, 2, 3, 4 ] }",
           false
           )]

        [InlineData(
           "patternProperty invalidates property",
           "{ 'foo':[ ] }",
           false
           )]

        [InlineData(
           "patternProperty validates nonproperty",
           "{ 'fxo':[ 1, 2 ] }",
           true
           )]

        [InlineData(
           "patternProperty invalidates nonproperty",
           "{ 'fxo':[ ] }",
           false
           )]

        [InlineData(
           "additionalProperty ignores property",
           "{ 'bar':[ ] }",
           true
           )]

        [InlineData(
           "additionalProperty validates others",
           "{ 'quux':3 }",
           true
           )]

        [InlineData(
           "additionalProperty invalidates others",
           "{ 'quux':'foo' }",
           false
           )]

        public void PropertiesPatternPropertiesAdditionalPropertiesInteraction(string desc, string data, bool expected)
        {
            // properties, patternProperties, additionalProperties interaction
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'additionalProperties':{ 'type':'integer' }, 'patternProperties':{ 'f.o':{ 'minItems':2 } }, 'properties':{ 'bar':{ 'type':'array' }, 'foo':{ 'maxItems':3, 'type':'array' } } }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft4 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     3 - properties with escaped characters
        /// </summary>

        [Theory]
        [InlineData(
           "object with all numbers is valid",
           @"{ 'foo\tbar':1, 'foo\nbar':1, 'foo\fbar':1, 'foo\rbar':1, 'foo\'bar':1, 'foo\\bar':1 }",
           true
           )]

        [InlineData(
           "object with strings is invalid",
           @"{ 'foo\tbar':'1', 'foo\nbar':'1', 'foo\fbar':'1', 'foo\rbar':'1', 'foo\'bar':'1', 'foo\\bar':'1' }",
           false
           )]

        public void PropertiesWithEscapedCharacters(string desc, string data, bool expected)
        {
            // properties with escaped characters
            Console.Error.WriteLine(desc);
            string schemaData = @"{ 'properties':{ 'foo\tbar':{ 'type':'number' }, 'foo\nbar':{ 'type':'number' }, 'foo\fbar':{ 'type':'number' }, 'foo\rbar':{ 'type':'number' }, 'foo\'bar':{ 'type':'number' }, 'foo\\bar':{ 'type':'number' } } }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft4 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


    }
}

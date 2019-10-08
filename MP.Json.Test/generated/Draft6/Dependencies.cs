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
    public class Dependencies
    {

        /// <summary>
        ///     1 - dependencies
        /// </summary>

        [Theory]
        [InlineData(
           "neither",
           "{ }",
           true
           )]

        [InlineData(
           "nondependant",
           "{ 'foo':1 }",
           true
           )]

        [InlineData(
           "with dependency",
           "{ 'bar':2, 'foo':1 }",
           true
           )]

        [InlineData(
           "missing dependency",
           "{ 'bar':2 }",
           false
           )]

        [InlineData(
           "ignores arrays",
           "[ 'bar' ]",
           true
           )]

        [InlineData(
           "ignores strings",
           "'foobar'",
           true
           )]

        [InlineData(
           "ignores other non-objects",
           "12",
           true
           )]

        public void DependenciesTest(string desc, string data, bool expected)
        {
            // dependencies
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'dependencies':{ 'bar':[ 'foo' ] } }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft6 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     2 - dependencies with empty array
        /// </summary>

        [Theory]
        [InlineData(
           "empty object",
           "{ }",
           true
           )]

        [InlineData(
           "object with one property",
           "{ 'bar':2 }",
           true
           )]

        public void DependenciesWithEmptyArray(string desc, string data, bool expected)
        {
            // dependencies with empty array
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'dependencies':{ 'bar':[ ] } }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft6 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     3 - multiple dependencies
        /// </summary>

        [Theory]
        [InlineData(
           "neither",
           "{ }",
           true
           )]

        [InlineData(
           "nondependants",
           "{ 'bar':2, 'foo':1 }",
           true
           )]

        [InlineData(
           "with dependencies",
           "{ 'bar':2, 'foo':1, 'quux':3 }",
           true
           )]

        [InlineData(
           "missing dependency",
           "{ 'foo':1, 'quux':2 }",
           false
           )]

        [InlineData(
           "missing other dependency",
           "{ 'bar':1, 'quux':2 }",
           false
           )]

        [InlineData(
           "missing both dependencies",
           "{ 'quux':1 }",
           false
           )]

        public void MultipleDependencies(string desc, string data, bool expected)
        {
            // multiple dependencies
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'dependencies':{ 'quux':[ 'foo', 'bar' ] } }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft6 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     4 - multiple dependencies subschema
        /// </summary>

        [Theory]
        [InlineData(
           "valid",
           "{ 'bar':2, 'foo':1 }",
           true
           )]

        [InlineData(
           "no dependency",
           "{ 'foo':'quux' }",
           true
           )]

        [InlineData(
           "wrong type",
           "{ 'bar':2, 'foo':'quux' }",
           false
           )]

        [InlineData(
           "wrong type other",
           "{ 'bar':'quux', 'foo':2 }",
           false
           )]

        [InlineData(
           "wrong type both",
           "{ 'bar':'quux', 'foo':'quux' }",
           false
           )]

        public void MultipleDependenciesSubschema(string desc, string data, bool expected)
        {
            // multiple dependencies subschema
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'dependencies':{ 'bar':{ 'properties':{ 'bar':{ 'type':'integer' }, 'foo':{ 'type':'integer' } } } } }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft6 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     5 - dependencies with boolean subschemas
        /// </summary>

        [Theory]
        [InlineData(
           "object with property having schema true is valid",
           "{ 'foo':1 }",
           true
           )]

        [InlineData(
           "object with property having schema false is invalid",
           "{ 'bar':2 }",
           false
           )]

        [InlineData(
           "object with both properties is invalid",
           "{ 'bar':2, 'foo':1 }",
           false
           )]

        [InlineData(
           "empty object is valid",
           "{ }",
           true
           )]

        public void DependenciesWithBooleanSubschemas(string desc, string data, bool expected)
        {
            // dependencies with boolean subschemas
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'dependencies':{ 'bar':false, 'foo':true } }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft6 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     6 - empty array of dependencies
        /// </summary>

        [Theory]
        [InlineData(
           "object with property is valid",
           "{ 'foo':1 }",
           true
           )]

        [InlineData(
           "empty object is valid",
           "{ }",
           true
           )]

        [InlineData(
           "non-object is valid",
           "1",
           true
           )]

        public void EmptyArrayOfDependencies(string desc, string data, bool expected)
        {
            // empty array of dependencies
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'dependencies':{ 'foo':[ ] } }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft6 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     7 - dependencies with escaped characters
        /// </summary>

        [Theory]
        [InlineData(
           "valid object 1",
           @"{ 'foo\nbar':1, 'foo\rbar':2 }",
           true
           )]

        [InlineData(
           "valid object 2",
           @"{ 'a':2, 'b':3, 'c':4, 'foo\tbar':1 }",
           true
           )]

        [InlineData(
           "valid object 3",
           @"{ ""foo\""bar"":2, ""foo'bar"":1 }",
           true
           )]

        [InlineData(
           "invalid object 1",
           @"{ 'foo':2, 'foo\nbar':1 }",
           false
           )]

        [InlineData(
           "invalid object 2",
           @"{ 'a':2, 'foo\tbar':1 }",
           false
           )]

        [InlineData(
           "invalid object 3",
           @"{ ""foo'bar"":1 }",
           false
           )]

        [InlineData(
           "invalid object 4",
           @"{ 'foo\'bar':2 }",
           false
           )]

        public void DependenciesWithEscapedCharacters(string desc, string data, bool expected)
        {
            // dependencies with escaped characters
            Console.Error.WriteLine(desc);
            string schemaData = @"{ ""dependencies"":{ ""foo\tbar"":{ ""minProperties"":4 }, ""foo\nbar"":[ ""foo\rbar"" ], ""foo\""bar"":[ ""foo'bar"" ], ""foo'bar"":{ ""required"":[ ""foo\""bar"" ] } } }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft6 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


    }
}

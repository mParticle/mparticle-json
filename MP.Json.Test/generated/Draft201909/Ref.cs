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
    public class Ref
    {

        /// <summary>
        ///     1 - root pointer ref
        /// </summary>

        [Theory]
        [InlineData(
           "match",
           "{ 'foo':false }",
           true
           )]

        [InlineData(
           "recursive match",
           "{ 'foo':{ 'foo':false } }",
           true
           )]

        [InlineData(
           "mismatch",
           "{ 'bar':false }",
           false
           )]

        [InlineData(
           "recursive mismatch",
           "{ 'foo':{ 'bar':false } }",
           false
           )]

        public void RootPointerRef(string desc, string data, bool expected)
        {
            // root pointer ref
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'additionalProperties':false, 'properties':{ 'foo':{ '$ref':'#' } } }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft201909 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     2 - relative pointer ref to object
        /// </summary>

        [Theory]
        [InlineData(
           "match",
           "{ 'bar':3 }",
           true
           )]

        [InlineData(
           "mismatch",
           "{ 'bar':true }",
           false
           )]

        public void RelativePointerRefToObject(string desc, string data, bool expected)
        {
            // relative pointer ref to object
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'properties':{ 'bar':{ '$ref':'#/properties/foo' }, 'foo':{ 'type':'integer' } } }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft201909 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     3 - relative pointer ref to array
        /// </summary>

        [Theory]
        [InlineData(
           "match array",
           "[ 1, 2 ]",
           true
           )]

        [InlineData(
           "mismatch array",
           "[ 1, 'foo' ]",
           false
           )]

        public void RelativePointerRefToArray(string desc, string data, bool expected)
        {
            // relative pointer ref to array
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'items':[ { 'type':'integer' }, { '$ref':'#/items/0' } ] }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft201909 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     4 - escaped pointer ref
        /// </summary>

        [Theory]
        [InlineData(
           "slash invalid",
           "{ 'slash':'aoeu' }",
           false
           )]

        [InlineData(
           "tilda invalid",
           "{ 'tilda':'aoeu' }",
           false
           )]

        [InlineData(
           "percent invalid",
           "{ 'percent':'aoeu' }",
           false
           )]

        [InlineData(
           "slash valid",
           "{ 'slash':123 }",
           true
           )]

        [InlineData(
           "tilda valid",
           "{ 'tilda':123 }",
           true
           )]

        [InlineData(
           "percent valid",
           "{ 'percent':123 }",
           true
           )]

        public void EscapedPointerRef(string desc, string data, bool expected)
        {
            // escaped pointer ref
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'percent%field':{ 'type':'integer' }, 'properties':{ 'percent':{ '$ref':'#/percent%25field' }, 'slash':{ '$ref':'#/slash~1field' }, 'tilda':{ '$ref':'#/tilda~0field' } }, 'slash/field':{ 'type':'integer' }, 'tilda~field':{ 'type':'integer' } }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft201909 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     5 - nested refs
        /// </summary>

        [Theory]
        [InlineData(
           "nested ref valid",
           "5",
           true
           )]

        [InlineData(
           "nested ref invalid",
           "'a'",
           false
           )]

        public void NestedRefs(string desc, string data, bool expected)
        {
            // nested refs
            Console.Error.WriteLine(desc);
            string schemaData = "{ '$defs':{ 'a':{ 'type':'integer' }, 'b':{ '$ref':'#/$defs/a' }, 'c':{ '$ref':'#/$defs/b' } }, '$ref':'#/$defs/c' }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft201909 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     6 - ref overrides any sibling keywords
        /// </summary>

        [Theory]
        [InlineData(
           "ref valid",
           "{ 'foo':[ ] }",
           true
           )]

        [InlineData(
           "ref valid, maxItems ignored",
           "{ 'foo':[ 1, 2, 3 ] }",
           true
           )]

        [InlineData(
           "ref invalid",
           "{ 'foo':'string' }",
           false
           )]

        public void RefOverridesAnySiblingKeywords(string desc, string data, bool expected)
        {
            // ref overrides any sibling keywords
            Console.Error.WriteLine(desc);
            string schemaData = "{ '$defs':{ 'reffed':{ 'type':'array' } }, 'properties':{ 'foo':{ '$ref':'#/$defs/reffed', 'maxItems':2 } } }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft201909 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     7 - remote ref, containing refs itself
        /// </summary>

        [Theory]
        [InlineData(
           "remote ref valid",
           "{ 'minLength':1 }",
           true
           )]

        [InlineData(
           "remote ref invalid",
           "{ 'minLength':-1 }",
           false
           )]

        public void RemoteRefContainingRefsItself(string desc, string data, bool expected)
        {
            // remote ref, containing refs itself
            Console.Error.WriteLine(desc);
            string schemaData = "{ '$ref':'https://json-schema.org/draft/2019-09/schema' }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft201909 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     8 - property named $ref that is not a reference
        /// </summary>

        [Theory]
        [InlineData(
           "property named $ref valid",
           "{ '$ref':'a' }",
           true
           )]

        [InlineData(
           "property named $ref invalid",
           "{ '$ref':2 }",
           false
           )]

        public void PropertyNamedRefThatIsNotAReference(string desc, string data, bool expected)
        {
            // property named $ref that is not a reference
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'properties':{ '$ref':{ 'type':'string' } } }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft201909 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     9 - $ref to boolean schema true
        /// </summary>

        [Theory]
        [InlineData(
           "any value is valid",
           "'foo'",
           true
           )]

        public void RefToBooleanSchemaTrue(string desc, string data, bool expected)
        {
            // $ref to boolean schema true
            Console.Error.WriteLine(desc);
            string schemaData = "{ '$defs':{ 'bool':true }, '$ref':'#/$defs/bool' }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft201909 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     10 - $ref to boolean schema false
        /// </summary>

        [Theory]
        [InlineData(
           "any value is invalid",
           "'foo'",
           false
           )]

        public void RefToBooleanSchemaFalse(string desc, string data, bool expected)
        {
            // $ref to boolean schema false
            Console.Error.WriteLine(desc);
            string schemaData = "{ '$defs':{ 'bool':false }, '$ref':'#/$defs/bool' }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft201909 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     11 - Recursive references between schemas
        /// </summary>

        [Theory]
        [InlineData(
           "valid tree",
           "{ 'meta':'root', 'nodes':[ { 'subtree':{ 'meta':'child', 'nodes':[ { 'value':1.1 }, { 'value':1.2 } ] }, 'value':1 }, { 'subtree':{ 'meta':'child', 'nodes':[ { 'value':2.1 }, { 'value':2.2 } ] }, 'value':2 } ] }",
           true
           )]

        [InlineData(
           "invalid tree",
           "{ 'meta':'root', 'nodes':[ { 'subtree':{ 'meta':'child', 'nodes':[ { 'value':'string is invalid' }, { 'value':1.2 } ] }, 'value':1 }, { 'subtree':{ 'meta':'child', 'nodes':[ { 'value':2.1 }, { 'value':2.2 } ] }, 'value':2 } ] }",
           false
           )]

        public void RecursiveReferencesBetweenSchemas(string desc, string data, bool expected)
        {
            // Recursive references between schemas
            Console.Error.WriteLine(desc);
            string schemaData = "{ '$defs':{ 'node':{ '$id':'http://localhost:1234/node', 'description':'node', 'properties':{ 'subtree':{ '$ref':'tree' }, 'value':{ 'type':'number' } }, 'required':[ 'value' ], 'type':'object' } }, '$id':'http://localhost:1234/tree', 'description':'tree of nodes', 'properties':{ 'meta':{ 'type':'string' }, 'nodes':{ 'items':{ '$ref':'node' }, 'type':'array' } }, 'required':[ 'meta', 'nodes' ], 'type':'object' }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft201909 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     12 - refs with quote
        /// </summary>

        [Theory]
        [InlineData(
           "object with numbers is valid",
           @"{ 'foo\""bar':1 }",
           true
           )]

        [InlineData(
           "object with strings is invalid",
           @"{ 'foo\""bar':'1' }",
           false
           )]

        public void RefsWithQuote(string desc, string data, bool expected)
        {
            // refs with quote
            Console.Error.WriteLine(desc);
            string schemaData = @"{ '$defs':{ 'foo\""bar':{ 'type':'number' } }, 'properties':{ 'foo\""bar':{ '$ref':'#/$defs/foo%22bar' } } }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft201909 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     13 - Location-independent identifier
        /// </summary>

        [Theory]
        [InlineData(
           "match",
           "1",
           true
           )]

        [InlineData(
           "mismatch",
           "'a'",
           false
           )]

        public void LocationIndependentIdentifier(string desc, string data, bool expected)
        {
            // Location-independent identifier
            Console.Error.WriteLine(desc);
            string schemaData = "{ '$defs':{ 'A':{ '$id':'#foo', 'type':'integer' } }, 'allOf':[ { '$ref':'#foo' } ] }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft201909 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     14 - Location-independent identifier with absolute URI
        /// </summary>

        [Theory]
        [InlineData(
           "match",
           "1",
           true
           )]

        [InlineData(
           "mismatch",
           "'a'",
           false
           )]

        public void LocationIndependentIdentifierWithAbsoluteURI(string desc, string data, bool expected)
        {
            // Location-independent identifier with absolute URI
            Console.Error.WriteLine(desc);
            string schemaData = "{ '$defs':{ 'A':{ '$id':'http://localhost:1234/bar#foo', 'type':'integer' } }, 'allOf':[ { '$ref':'http://localhost:1234/bar#foo' } ] }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft201909 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     15 - Location-independent identifier with base URI change in subschema
        /// </summary>

        [Theory(Skip ="Work in Progress")]
        [InlineData(
           "match",
           "1",
           true
           )]

        [InlineData(
           "mismatch",
           "'a'",
           false
           )]

        public void LocationIndependentIdentifierWithBaseURIChangeInSubschema(string desc, string data, bool expected)
        {
            // Location-independent identifier with base URI change in subschema
            Console.Error.WriteLine(desc);
            string schemaData = "{ '$defs':{ 'A':{ '$defs':{ 'B':{ '$id':'#foo', 'type':'integer' } }, '$id':'nested.json' } }, '$id':'http://localhost:1234/root', 'allOf':[ { '$ref':'http://localhost:1234/nested.json#foo' } ] }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft201909 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


    }
}

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
    public class RefRemote
    {

        /// <summary>
        ///     1 - remote ref
        /// </summary>

        [Theory(Skip = "NYI")]
        [InlineData(
           "remote ref valid",
           "1",
           true
           )]

        [InlineData(
           "remote ref invalid",
           "'a'",
           false
           )]

        public void RemoteRef(string desc, string data, bool expected)
        {
            // remote ref
            Console.Error.WriteLine(desc);
            string schemaData = "{ '$ref':'http://localhost:1234/integer.json' }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft6 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     2 - fragment within remote ref
        /// </summary>

        [Theory(Skip = "NYI")]
        [InlineData(
           "remote fragment valid",
           "1",
           true
           )]

        [InlineData(
           "remote fragment invalid",
           "'a'",
           false
           )]

        public void FragmentWithinRemoteRef(string desc, string data, bool expected)
        {
            // fragment within remote ref
            Console.Error.WriteLine(desc);
            string schemaData = "{ '$ref':'http://localhost:1234/subSchemas.json#/integer' }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft6 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     3 - ref within remote ref
        /// </summary>

        [Theory(Skip = "NYI")]
        [InlineData(
           "ref within ref valid",
           "1",
           true
           )]

        [InlineData(
           "ref within ref invalid",
           "'a'",
           false
           )]

        public void RefWithinRemoteRef(string desc, string data, bool expected)
        {
            // ref within remote ref
            Console.Error.WriteLine(desc);
            string schemaData = "{ '$ref':'http://localhost:1234/subSchemas.json#/refToInteger' }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft6 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     4 - base URI change
        /// </summary>

        [Theory(Skip = "NYI")]
        [InlineData(
           "base URI change ref valid",
           "[ [ 1 ] ]",
           true
           )]

        [InlineData(
           "base URI change ref invalid",
           "[ [ 'a' ] ]",
           false
           )]

        public void BaseURIChange(string desc, string data, bool expected)
        {
            // base URI change
            Console.Error.WriteLine(desc);
            string schemaData = "{ '$id':'http://localhost:1234/', 'items':{ '$id':'folder/', 'items':{ '$ref':'folderInteger.json' } } }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft6 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     5 - base URI change - change folder
        /// </summary>

        [Theory(Skip = "NYI")]
        [InlineData(
           "number is valid",
           "{ 'list':[ 1 ] }",
           true
           )]

        [InlineData(
           "string is invalid",
           "{ 'list':[ 'a' ] }",
           false
           )]

        public void BaseURIChangeChangeFolder(string desc, string data, bool expected)
        {
            // base URI change - change folder
            Console.Error.WriteLine(desc);
            string schemaData = "{ '$id':'http://localhost:1234/scope_change_defs1.json', 'definitions':{ 'baz':{ '$id':'folder/', 'items':{ '$ref':'folderInteger.json' }, 'type':'array' } }, 'properties':{ 'list':{ '$ref':'#/definitions/baz' } }, 'type':'object' }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft6 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     6 - base URI change - change folder in subschema
        /// </summary>

        [Theory(Skip = "NYI")]
        [InlineData(
           "number is valid",
           "{ 'list':[ 1 ] }",
           true
           )]

        [InlineData(
           "string is invalid",
           "{ 'list':[ 'a' ] }",
           false
           )]

        public void BaseURIChangeChangeFolderInSubschema(string desc, string data, bool expected)
        {
            // base URI change - change folder in subschema
            Console.Error.WriteLine(desc);
            string schemaData = "{ '$id':'http://localhost:1234/scope_change_defs2.json', 'definitions':{ 'baz':{ '$id':'folder/', 'definitions':{ 'bar':{ 'items':{ '$ref':'folderInteger.json' }, 'type':'array' } } } }, 'properties':{ 'list':{ '$ref':'#/definitions/baz/definitions/bar' } }, 'type':'object' }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft6 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     7 - root ref in remote ref
        /// </summary>

        [Theory(Skip = "NYI")]
        [InlineData(
           "string is valid",
           "{ 'name':'foo' }",
           true
           )]

        [InlineData(
           "null is valid",
           "{ 'name':null }",
           true
           )]

        [InlineData(
           "object is invalid",
           "{ 'name':{ 'name':null } }",
           false
           )]

        public void RootRefInRemoteRef(string desc, string data, bool expected)
        {
            // root ref in remote ref
            Console.Error.WriteLine(desc);
            string schemaData = "{ '$id':'http://localhost:1234/object', 'properties':{ 'name':{ '$ref':'name.json#/definitions/orNull' } }, 'type':'object' }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft6 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


    }
}

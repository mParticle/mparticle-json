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
    public class Format
    {

        /// <summary>
        ///     1 - validation of e-mail addresses
        /// </summary>

        [Theory]
        [InlineData(
           "ignores integers",
           "12",
           true
           )]

        [InlineData(
           "ignores floats",
           "13.7",
           true
           )]

        [InlineData(
           "ignores objects",
           "{ }",
           true
           )]

        [InlineData(
           "ignores arrays",
           "[ ]",
           true
           )]

        [InlineData(
           "ignores booleans",
           "false",
           true
           )]

        [InlineData(
           "ignores null",
           "null",
           true
           )]

        public void ValidationOfEMailAddresses(string desc, string data, bool expected)
        {
            // validation of e-mail addresses
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'format':'email' }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft201909 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     2 - validation of IDN e-mail addresses
        /// </summary>

        [Theory]
        [InlineData(
           "ignores integers",
           "12",
           true
           )]

        [InlineData(
           "ignores floats",
           "13.7",
           true
           )]

        [InlineData(
           "ignores objects",
           "{ }",
           true
           )]

        [InlineData(
           "ignores arrays",
           "[ ]",
           true
           )]

        [InlineData(
           "ignores booleans",
           "false",
           true
           )]

        [InlineData(
           "ignores null",
           "null",
           true
           )]

        public void ValidationOfIDNEMailAddresses(string desc, string data, bool expected)
        {
            // validation of IDN e-mail addresses
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'format':'idn-email' }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft201909 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     3 - validation of regexes
        /// </summary>

        [Theory]
        [InlineData(
           "ignores integers",
           "12",
           true
           )]

        [InlineData(
           "ignores floats",
           "13.7",
           true
           )]

        [InlineData(
           "ignores objects",
           "{ }",
           true
           )]

        [InlineData(
           "ignores arrays",
           "[ ]",
           true
           )]

        [InlineData(
           "ignores booleans",
           "false",
           true
           )]

        [InlineData(
           "ignores null",
           "null",
           true
           )]

        public void ValidationOfRegexes(string desc, string data, bool expected)
        {
            // validation of regexes
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'format':'regex' }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft201909 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     4 - validation of IP addresses
        /// </summary>

        [Theory]
        [InlineData(
           "ignores integers",
           "12",
           true
           )]

        [InlineData(
           "ignores floats",
           "13.7",
           true
           )]

        [InlineData(
           "ignores objects",
           "{ }",
           true
           )]

        [InlineData(
           "ignores arrays",
           "[ ]",
           true
           )]

        [InlineData(
           "ignores booleans",
           "false",
           true
           )]

        [InlineData(
           "ignores null",
           "null",
           true
           )]

        public void ValidationOfIPAddresses(string desc, string data, bool expected)
        {
            // validation of IP addresses
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'format':'ipv4' }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft201909 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     5 - validation of IPv6 addresses
        /// </summary>

        [Theory]
        [InlineData(
           "ignores integers",
           "12",
           true
           )]

        [InlineData(
           "ignores floats",
           "13.7",
           true
           )]

        [InlineData(
           "ignores objects",
           "{ }",
           true
           )]

        [InlineData(
           "ignores arrays",
           "[ ]",
           true
           )]

        [InlineData(
           "ignores booleans",
           "false",
           true
           )]

        [InlineData(
           "ignores null",
           "null",
           true
           )]

        public void ValidationOfIPv6Addresses(string desc, string data, bool expected)
        {
            // validation of IPv6 addresses
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'format':'ipv6' }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft201909 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     6 - validation of IDN hostnames
        /// </summary>

        [Theory]
        [InlineData(
           "ignores integers",
           "12",
           true
           )]

        [InlineData(
           "ignores floats",
           "13.7",
           true
           )]

        [InlineData(
           "ignores objects",
           "{ }",
           true
           )]

        [InlineData(
           "ignores arrays",
           "[ ]",
           true
           )]

        [InlineData(
           "ignores booleans",
           "false",
           true
           )]

        [InlineData(
           "ignores null",
           "null",
           true
           )]

        public void ValidationOfIDNHostnames(string desc, string data, bool expected)
        {
            // validation of IDN hostnames
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'format':'idn-hostname' }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft201909 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     7 - validation of hostnames
        /// </summary>

        [Theory]
        [InlineData(
           "ignores integers",
           "12",
           true
           )]

        [InlineData(
           "ignores floats",
           "13.7",
           true
           )]

        [InlineData(
           "ignores objects",
           "{ }",
           true
           )]

        [InlineData(
           "ignores arrays",
           "[ ]",
           true
           )]

        [InlineData(
           "ignores booleans",
           "false",
           true
           )]

        [InlineData(
           "ignores null",
           "null",
           true
           )]

        public void ValidationOfHostnames(string desc, string data, bool expected)
        {
            // validation of hostnames
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'format':'hostname' }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft201909 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     8 - validation of date strings
        /// </summary>

        [Theory]
        [InlineData(
           "ignores integers",
           "12",
           true
           )]

        [InlineData(
           "ignores floats",
           "13.7",
           true
           )]

        [InlineData(
           "ignores objects",
           "{ }",
           true
           )]

        [InlineData(
           "ignores arrays",
           "[ ]",
           true
           )]

        [InlineData(
           "ignores booleans",
           "false",
           true
           )]

        [InlineData(
           "ignores null",
           "null",
           true
           )]

        public void ValidationOfDateStrings(string desc, string data, bool expected)
        {
            // validation of date strings
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'format':'date' }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft201909 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     9 - validation of date-time strings
        /// </summary>

        [Theory]
        [InlineData(
           "ignores integers",
           "12",
           true
           )]

        [InlineData(
           "ignores floats",
           "13.7",
           true
           )]

        [InlineData(
           "ignores objects",
           "{ }",
           true
           )]

        [InlineData(
           "ignores arrays",
           "[ ]",
           true
           )]

        [InlineData(
           "ignores booleans",
           "false",
           true
           )]

        [InlineData(
           "ignores null",
           "null",
           true
           )]

        public void ValidationOfDateTimeStrings(string desc, string data, bool expected)
        {
            // validation of date-time strings
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'format':'date-time' }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft201909 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     10 - validation of time strings
        /// </summary>

        [Theory]
        [InlineData(
           "ignores integers",
           "12",
           true
           )]

        [InlineData(
           "ignores floats",
           "13.7",
           true
           )]

        [InlineData(
           "ignores objects",
           "{ }",
           true
           )]

        [InlineData(
           "ignores arrays",
           "[ ]",
           true
           )]

        [InlineData(
           "ignores booleans",
           "false",
           true
           )]

        [InlineData(
           "ignores null",
           "null",
           true
           )]

        public void ValidationOfTimeStrings(string desc, string data, bool expected)
        {
            // validation of time strings
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'format':'time' }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft201909 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     11 - validation of JSON pointers
        /// </summary>

        [Theory]
        [InlineData(
           "ignores integers",
           "12",
           true
           )]

        [InlineData(
           "ignores floats",
           "13.7",
           true
           )]

        [InlineData(
           "ignores objects",
           "{ }",
           true
           )]

        [InlineData(
           "ignores arrays",
           "[ ]",
           true
           )]

        [InlineData(
           "ignores booleans",
           "false",
           true
           )]

        [InlineData(
           "ignores null",
           "null",
           true
           )]

        public void ValidationOfJSONPointers(string desc, string data, bool expected)
        {
            // validation of JSON pointers
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'format':'json-pointer' }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft201909 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     12 - validation of relative JSON pointers
        /// </summary>

        [Theory]
        [InlineData(
           "ignores integers",
           "12",
           true
           )]

        [InlineData(
           "ignores floats",
           "13.7",
           true
           )]

        [InlineData(
           "ignores objects",
           "{ }",
           true
           )]

        [InlineData(
           "ignores arrays",
           "[ ]",
           true
           )]

        [InlineData(
           "ignores booleans",
           "false",
           true
           )]

        [InlineData(
           "ignores null",
           "null",
           true
           )]

        public void ValidationOfRelativeJSONPointers(string desc, string data, bool expected)
        {
            // validation of relative JSON pointers
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'format':'relative-json-pointer' }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft201909 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     13 - validation of IRIs
        /// </summary>

        [Theory]
        [InlineData(
           "ignores integers",
           "12",
           true
           )]

        [InlineData(
           "ignores floats",
           "13.7",
           true
           )]

        [InlineData(
           "ignores objects",
           "{ }",
           true
           )]

        [InlineData(
           "ignores arrays",
           "[ ]",
           true
           )]

        [InlineData(
           "ignores booleans",
           "false",
           true
           )]

        [InlineData(
           "ignores null",
           "null",
           true
           )]

        public void ValidationOfIRIs(string desc, string data, bool expected)
        {
            // validation of IRIs
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'format':'iri' }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft201909 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     14 - validation of IRI references
        /// </summary>

        [Theory]
        [InlineData(
           "ignores integers",
           "12",
           true
           )]

        [InlineData(
           "ignores floats",
           "13.7",
           true
           )]

        [InlineData(
           "ignores objects",
           "{ }",
           true
           )]

        [InlineData(
           "ignores arrays",
           "[ ]",
           true
           )]

        [InlineData(
           "ignores booleans",
           "false",
           true
           )]

        [InlineData(
           "ignores null",
           "null",
           true
           )]

        public void ValidationOfIRIReferences(string desc, string data, bool expected)
        {
            // validation of IRI references
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'format':'iri-reference' }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft201909 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     15 - validation of URIs
        /// </summary>

        [Theory]
        [InlineData(
           "ignores integers",
           "12",
           true
           )]

        [InlineData(
           "ignores floats",
           "13.7",
           true
           )]

        [InlineData(
           "ignores objects",
           "{ }",
           true
           )]

        [InlineData(
           "ignores arrays",
           "[ ]",
           true
           )]

        [InlineData(
           "ignores booleans",
           "false",
           true
           )]

        [InlineData(
           "ignores null",
           "null",
           true
           )]

        public void ValidationOfURIs(string desc, string data, bool expected)
        {
            // validation of URIs
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'format':'uri' }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft201909 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     16 - validation of URI references
        /// </summary>

        [Theory]
        [InlineData(
           "ignores integers",
           "12",
           true
           )]

        [InlineData(
           "ignores floats",
           "13.7",
           true
           )]

        [InlineData(
           "ignores objects",
           "{ }",
           true
           )]

        [InlineData(
           "ignores arrays",
           "[ ]",
           true
           )]

        [InlineData(
           "ignores booleans",
           "false",
           true
           )]

        [InlineData(
           "ignores null",
           "null",
           true
           )]

        public void ValidationOfURIReferences(string desc, string data, bool expected)
        {
            // validation of URI references
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'format':'uri-reference' }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft201909 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///     17 - validation of URI templates
        /// </summary>

        [Theory]
        [InlineData(
           "ignores integers",
           "12",
           true
           )]

        [InlineData(
           "ignores floats",
           "13.7",
           true
           )]

        [InlineData(
           "ignores objects",
           "{ }",
           true
           )]

        [InlineData(
           "ignores arrays",
           "[ ]",
           true
           )]

        [InlineData(
           "ignores booleans",
           "false",
           true
           )]

        [InlineData(
           "ignores null",
           "null",
           true
           )]

        public void ValidationOfURITemplates(string desc, string data, bool expected)
        {
            // validation of URI templates
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'format':'uri-template' }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft201909 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }
    }
}

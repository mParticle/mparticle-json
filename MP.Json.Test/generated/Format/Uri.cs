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

namespace JsonSchemaTestSuite.Format
{
    public class Uri
    {

        /// <summary>
        ///     1 - validation of URIs
        /// </summary>

        [Theory]
        [InlineData(
           "a valid URL with anchor tag",
           "'http://foo.bar/?baz=qux#quux'",
           true
           )]

        [InlineData(
           "a valid URL with anchor tag and parantheses",
           "'http://foo.com/blah_(wikipedia)_blah#cite-1'",
           true
           )]

        [InlineData(
           "a valid URL with URL-encoded stuff",
           "'http://foo.bar/?q=Test%20URL-encoded%20stuff'",
           true
           )]

        [InlineData(
           "a valid puny-coded URL ",
           "'http://xn--nw2a.xn--j6w193g/'",
           true
           )]

        [InlineData(
           "a valid URL with many special characters",
           @"""http://-.~_!$&'()*+,;=:%40:80%2f::::::@example.com""",
           true
           )]

        [InlineData(
           "a valid URL based on IPv4",
           "'http://223.255.255.254'",
           true
           )]

        [InlineData(
           "a valid URL with ftp scheme",
           "'ftp://ftp.is.co.za/rfc/rfc1808.txt'",
           true
           )]

        [InlineData(
           "a valid URL for a simple text file",
           "'http://www.ietf.org/rfc/rfc2396.txt'",
           true
           )]

        [InlineData(
           "a valid URL ",
           "'ldap://[2001:db8::7]/c=GB?objectClass?one'",
           true
           )]

        [InlineData(
           "a valid mailto URI",
           "'mailto:John.Doe@example.com'",
           true
           )]

        [InlineData(
           "a valid newsgroup URI",
           "'news:comp.infosystems.www.servers.unix'",
           true
           )]

        [InlineData(
           "a valid tel URI",
           "'tel:+1-816-555-1212'",
           true
           )]

        [InlineData(
           "a valid URN",
           "'urn:oasis:names:specification:docbook:dtd:xml:4.1.2'",
           true
           )]

        [InlineData(
           "an invalid protocol-relative URI Reference",
           "'//foo.bar/?baz=qux#quux'",
           false
           )]

        [InlineData(
           "an invalid relative URI Reference",
           "'/abc'",
           false
           )]

        [InlineData(
           "an invalid URI",
           @"'\\\\WINDOWS\\fileshare'",
           false
           )]

        [InlineData(
           "an invalid URI though valid URI reference",
           "'abc'",
           false
           )]

        [InlineData(
           "an invalid URI with spaces",
           "'http:// shouldfail.com'",
           false
           )]

        [InlineData(
           "an invalid URI with spaces and missing scheme",
           "':// should fail'",
           false
           )]

        public void ValidationOfURIs(string desc, string data, bool expected)
        {
            // validation of URIs
            Console.Error.WriteLine(desc);
            string schemaData = "{ 'format':'uri' }";
            MPJson schemaJson = MPJson.Parse(schemaData);
            MPJson json = MPJson.Parse(data);
            MPSchema schema = schemaJson;
            var validator = new JsonValidator { Strict = true, Version = SchemaVersion.Draft7 };
            bool actual = validator.Validate(schema, json);
            Assert.Equal(expected, actual);
        }


    }
}

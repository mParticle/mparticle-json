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
using System.Text;
using Xunit;

namespace MP.Json.Validation.Test
{
    public class StringTests
    {

        [Fact]
        public void StringFormatTest()
        {
            var schema = SimpleSchema("format", "unknown");

            Assert.True(schema.IsValid);
            Assert.True(schema.Validate("xyz"));
            Assert.True(schema.Validate(123));

            Assert.False(SimpleSchema("format", 1).IsValid);
            Assert.False(SimpleSchema("format", MPJson.Array()).IsValid);
        }

        [Fact]
        public void StringMaxLengthTest()
        {
            var schema = SimpleSchema("maxLength", 3);

            Assert.True(schema.IsValid);
            Assert.True(schema.Validate(""));
            Assert.True(schema.Validate("ab"));
            Assert.True(schema.Validate("abc"));
            Assert.False(schema.Validate("abcd"));
            Assert.True(schema.Validate(0));
            Assert.False(SimpleSchema("maxLength", 1.5).IsValid);
            Assert.False(SimpleSchema("maxLength", "1").IsValid);
            Assert.False(SimpleSchema("maxLength", MPJson.Array()).IsValid);
        }

        [Fact]
        public void StringMinLengthTest()
        {
            var schema = SimpleSchema("minLength", 3);

            Assert.True(schema.IsValid);
            Assert.False(schema.Validate(""));
            Assert.False(schema.Validate("ab"));
            Assert.True(schema.Validate("abc"));
            Assert.True(schema.Validate("abcd"));
            Assert.True(schema.Validate(0));
            Assert.False(SimpleSchema("minLength", 1.5).IsValid);
            Assert.False(SimpleSchema("minLength", "1").IsValid);
            Assert.False(SimpleSchema("minLength", MPJson.Array()).IsValid);
        }

        [Fact]
        public void StringPatternTest()
        {
            var schema = SimpleSchema("pattern", @"\d+");

            Assert.True(schema.IsValid);
            Assert.True(schema.Validate(0));
            Assert.True(schema.Validate("1"));
            Assert.True(schema.Validate("12"));
            Assert.False(schema.Validate(""));
            Assert.False(SimpleSchema("pattern", 1.5).IsValid);
            Assert.False(SimpleSchema("pattern", MPJson.Array()).IsValid);

        }

        #region Helpers
        MPSchema SimpleSchema(string keyword, MPJson json)
        {
            return new MPSchema(MPJson.Object(MPJson.Property(keyword, json)));
        }
        #endregion
    }
}

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
    public class NumberTests
    {
        [Fact]
        public void MaximumTest()
        {
            var schema = SimpleSchema("maximum", 3);
            Assert.True(schema.IsValid);
            Assert.False(SimpleSchema("minimum", MPJson.Array()).IsValid);
            Assert.True(schema.Validate(3));
            Assert.False(schema.Validate(4));
            Assert.False(schema.Validate(4.5));
            Assert.True(schema.Validate("123"));
            Assert.True(schema.Validate(1));
            Assert.True(schema.Validate(0));
            Assert.True(schema.Validate(-1));
        }

        [Fact]
        public void MinimumTest()
        {
            var schema = SimpleSchema("minimum", 3);
            Assert.True(schema.IsValid);
            Assert.False(SimpleSchema("minimum", false).IsValid);
            Assert.True(schema.Validate(3));
            Assert.True(schema.Validate(4));
            Assert.True(schema.Validate(4.5));
            Assert.True(schema.Validate("123"));
            Assert.False(schema.Validate(1));
            Assert.False(schema.Validate(0));
            Assert.False(schema.Validate(-1));
        }

        [Fact]
        public void ExclusiveMaximumTest()
        {
            var schema = SimpleSchema("exclusiveMaximum", 3);
            Assert.True(schema.IsValid);
            Assert.False(SimpleSchema("exclusiveMinimum", "3").IsValid);
            Assert.False(schema.Validate(3));
            Assert.False(schema.Validate(4));
            Assert.False(schema.Validate(4.5));
            Assert.True(schema.Validate("123"));
            Assert.True(schema.Validate(1));
            Assert.True(schema.Validate(0));
            Assert.True(schema.Validate(-1));
        }

        [Fact]
        public void ExclusiveMinimumTest()
        {
            var schema = SimpleSchema("exclusiveMinimum", 3);
            Assert.True(schema.IsValid);
            Assert.False(SimpleSchema("minimum", MPJson.Object()).IsValid);
            Assert.False(schema.Validate(3));
            Assert.True(schema.Validate(4));
            Assert.True(schema.Validate(4.5));
            Assert.True(schema.Validate("123"));
            Assert.False(schema.Validate(1));
            Assert.False(schema.Validate(0));
            Assert.False(schema.Validate(-1));
        }

        [Fact]
        public void MultipleOfTest()
        {
            var schema = SimpleSchema("multipleOf", 3);
            Assert.True(schema.IsValid);
            Assert.False(SimpleSchema("multipleOf", 0).IsValid);
            Assert.True(schema.Validate(9));
            Assert.True(schema.Validate(0));
            Assert.True(schema.Validate(-3));
            Assert.True(schema.Validate(3));
            Assert.False(schema.Validate(1));
            Assert.False(schema.Validate(2));
            Assert.False(schema.Validate(32));
        }

        [Fact]
        public void MultipleOfDoubleTest()
        {
            var schema = SimpleSchema("multipleOf", 1.2);
            Assert.True(schema.IsValid);
            Assert.True(schema.Validate(6));
            Assert.True(schema.Validate(-2.4));
            Assert.True(schema.Validate(1.2));
            Assert.False(schema.Validate(1.200000001));
            Assert.True(schema.Validate(0));
            Assert.False(schema.Validate(1));
            Assert.False(schema.Validate(2));
            Assert.False(schema.Validate(32));
        }

        public MPSchema SimpleSchema(string keyword, MPJson json)
        {
            return new MPSchema(MPJson.Object(MPJson.Property(keyword, json)));
        }

    }
}

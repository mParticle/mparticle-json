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
    public class ObjectTests
    {
        JsonProperty a1 = MPJson.Property("a", 1);
        JsonProperty b2 = MPJson.Property("b", 2);
        JsonProperty c3 = MPJson.Property("c", 3);
        JsonProperty d4 = MPJson.Property("d", 4);

        [Fact]
        public void MaxPropertiesTest()
        {
            var schema = SingletonSchema("maxProperties", 3);

            Assert.True(schema.IsValid);
            Assert.True(schema.Validate(MPJson.Object()));
            Assert.True(schema.Validate(MPJson.Object(a1, b2)));
            Assert.True(schema.Validate(MPJson.Object(a1, b2, c3)));
            Assert.False(schema.Validate(MPJson.Object(a1, b2, c3, d4)));

            // Any non-object should validate
            Assert.True(schema.Validate(0));

            Assert.False(SingletonSchema("maxProperties", -1).IsValid);
            Assert.False(SingletonSchema("maxProperties", 1.5).IsValid);
            Assert.False(SingletonSchema("maxProperties", "1").IsValid);
            Assert.False(SingletonSchema("maxProperties", MPJson.Array()).IsValid);
        }

        [Fact]
        public void MinItemsTest()
        {
            var schema = SingletonSchema("minProperties", 3);

            Assert.True(schema.IsValid);
            Assert.False(schema.Validate(MPJson.Object()));
            Assert.False(schema.Validate(MPJson.Object(a1, b2)));
            Assert.True(schema.Validate(MPJson.Object(a1, b2, c3)));
            Assert.True(schema.Validate(MPJson.Object(a1, b2, c3, d4)));

            // Any non-object should validate
            Assert.True(schema.Validate(0));

            Assert.False(SingletonSchema("minProperties", -1).IsValid);
            Assert.False(SingletonSchema("minProperties", 1.5).IsValid);
            Assert.False(SingletonSchema("minProperties", "1").IsValid);
            Assert.False(SingletonSchema("minProperties", MPJson.Array()).IsValid);
        }

        [Fact]
        public void RequiredTest()
        {
            var schema = SingletonSchema("required", MPJson.Array("a", "c"));

            Assert.True(schema.IsValid);
            Assert.False(schema.Validate(MPJson.Object()));
            Assert.False(schema.Validate(MPJson.Object(a1)));
            Assert.False(schema.Validate(MPJson.Object(b2, c3, d4)));
            Assert.True(schema.Validate(MPJson.Object(a1, c3)));
            Assert.True(schema.Validate(MPJson.Object(a1, b2, c3, d4)));
            Assert.False(SingletonSchema("required", "true").IsValid);
            Assert.False(SingletonSchema("required", MPJson.Object()).IsValid);
        }

        [Fact]
        public void PropertyNamesTest()
        {
            var schema = SingletonSchema("propertyNames",
                MPJson.Object(
                    MPJson.Property("pattern", "a|c")
                ));

            Assert.True(schema.IsValid);
            Assert.True(schema.Validate(MPJson.Object()));
            Assert.True(schema.Validate(MPJson.Object(a1)));
            Assert.False(schema.Validate(MPJson.Object(b2, c3, d4)));
            Assert.True(schema.Validate(MPJson.Object(a1, c3)));
            Assert.False(schema.Validate(MPJson.Object(a1, b2, c3, d4)));
            Assert.False(SingletonSchema("propertyNames", "true").IsValid);
            Assert.False(SingletonSchema("propertyNames", MPJson.Array()).IsValid);
        }

        [Fact]
        public void PropertiesTest()
        {
            var m2 = MPJson.Object("multipleOf", 2);
            var m3 = MPJson.Object("multipleOf", 3);
            var m5 = MPJson.Object("multipleOf", 5);
            MPSchema schema = MPJson.Object("properties", MPJson.Object("m2", m2, "m3", m3, "m5", m5));

            Assert.True(schema.IsValid);
            Assert.True(schema.Validate(MPJson.Object()));
            Assert.True(schema.Validate(MPJson.Object("m2", 2)));
            Assert.True(schema.Validate(MPJson.Object("m5", 5)));
            Assert.True(schema.Validate(MPJson.Object("m2", 2, "m3", 3)));
            Assert.True(schema.Validate(MPJson.Object("m2", 4, "m3", 9, "m5", 25)));
            Assert.False(schema.Validate(MPJson.Object("m2", 3)));
            Assert.False(schema.Validate(MPJson.Object("m2", 2, "m3", 3, "m5", 7)));
        }

        [Fact]
        public void AdditionalPropertiesTest()
        {
            var m2 = MPJson.Object("multipleOf", 2);
            var m3 = MPJson.Object("multipleOf", 3);
            var m5 = MPJson.Object("multipleOf", 5);
            MPSchema schema = MPJson.Object(
                "properties", MPJson.Object("m2", m2, "m3", m3),
                "additionalProperties", m5);

            Assert.True(schema.IsValid);
            Assert.True(schema.Validate(MPJson.Object()));
            Assert.True(schema.Validate(MPJson.Object("m2", 2)));
            Assert.True(schema.Validate(MPJson.Object("m5", 5)));
            Assert.True(schema.Validate(MPJson.Object("m2", 2, "m3", 3)));
            Assert.True(schema.Validate(MPJson.Object("m2", 4, "m3", 9, "m5", 25)));
            Assert.False(schema.Validate(MPJson.Object("m2", 3)));
            Assert.False(schema.Validate(MPJson.Object("m2", 2, "m3", 3, "m5", 7)));
            Assert.False(schema.Validate(MPJson.Object("m7", 7)));
        }

        [Fact]
        public void PatternPropertiesTest()
        {
            var m2 = MPJson.Object("multipleOf", 2);
            var m3 = MPJson.Object("multipleOf", 3);
            var m5 = MPJson.Object("multipleOf", 5);
            MPSchema schema = MPJson.Object(
                "patternProperties", MPJson.Object("2", m2, "3", m3),
                "additionalProperties", m5);

            Assert.True(schema.IsValid);
            Assert.True(schema.Validate(MPJson.Object()));
            Assert.True(schema.Validate(MPJson.Object("m2", 2)));
            Assert.True(schema.Validate(MPJson.Object("m5", 5)));
            Assert.True(schema.Validate(MPJson.Object("m2", 2, "m3", 3)));
            Assert.True(schema.Validate(MPJson.Object("m2", 4, "m3", 9, "m5", 25)));
            Assert.True(schema.Validate(MPJson.Object("m23", 6)));
            Assert.True(schema.Validate(MPJson.Object("m23", 6, "m5", 5)));
            Assert.False(schema.Validate(MPJson.Object("m23", 5, "m5", 5)));
            Assert.False(schema.Validate(MPJson.Object("m2", 3)));
            Assert.False(schema.Validate(MPJson.Object("m2", 2, "m3", 3, "m5", 7)));
            Assert.False(schema.Validate(MPJson.Object("m7", 7)));
        }

        [Theory]
        [InlineData("dependencies")]
        [InlineData("dependentSchemas")]
        public void DependentSchemasTest(string keyword)
        {
            var m2 = MPJson.Object("multipleOf", 2);
            var m3 = MPJson.Object("multipleOf", 3);
            MPSchema schema = MPJson.Object(
                "properties", MPJson.Object("m2", m2, "m3", m3),
                keyword, MPJson.Object("m5", MPJson.Object("required", MPJson.Array("m3"))));

            Assert.True(schema.IsValid);
            Assert.True(schema.Validate(MPJson.Object()));
            Assert.True(schema.Validate(MPJson.Object("m2", 2)));
            Assert.True(schema.Validate(MPJson.Object("m2", 2, "m3", 3)));
            Assert.True(schema.Validate(MPJson.Object("m2", 4, "m3", 9, "m5", 25)));
            Assert.True(schema.Validate(MPJson.Object("m5", 5, "m3", 3)));
            Assert.True(schema.Validate(MPJson.Object("m2", 2, "m3", 3, "m5", 7)));
            Assert.True(schema.Validate(MPJson.Object("m7", 7)));
            Assert.False(schema.Validate(MPJson.Object("m2", 3)));
            Assert.False(schema.Validate(MPJson.Object("m5", 5, "m2", 2)));
        }

        [Theory]
        [InlineData("dependencies")]
        [InlineData("dependentRequired")]
        public void DependentRequiredTest(string keyword)
        {
            var m2 = MPJson.Object("multipleOf", 2);
            var m3 = MPJson.Object("multipleOf", 3);
            MPSchema schema = MPJson.Object(
                "properties", MPJson.Object("m2", m2, "m3", m3),
                keyword, MPJson.Object("m5", MPJson.Array("m3")));

            Assert.True(schema.IsValid);
            Assert.True(schema.Validate(MPJson.Object()));
            Assert.True(schema.Validate(MPJson.Object("m2", 2)));
            Assert.True(schema.Validate(MPJson.Object("m2", 2, "m3", 3)));
            Assert.True(schema.Validate(MPJson.Object("m2", 4, "m3", 9, "m5", 25)));
            Assert.True(schema.Validate(MPJson.Object("m5", 5, "m3", 3)));
            Assert.True(schema.Validate(MPJson.Object("m2", 2, "m3", 3, "m5", 7)));
            Assert.True(schema.Validate(MPJson.Object("m7", 7)));
            Assert.False(schema.Validate(MPJson.Object("m2", 3)));
            Assert.False(schema.Validate(MPJson.Object("m5", 5, "m2", 2)));
        }

        #region Helpers
        MPSchema SingletonSchema(string keyword, MPJson json)
        {
            return MPJson.Object(MPJson.Property(keyword, json));
        }
        #endregion

    }
}

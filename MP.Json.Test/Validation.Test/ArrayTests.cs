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
    public class ArrayTests
    {
        [Fact]
        public void MaxItemsTest()
        {
            var schema = SingletonSchema("maxItems", 3);

            Assert.True(schema.IsValid);
            Assert.True(schema.Validate(MPJson.Array()));
            Assert.True(schema.Validate(MPJson.Array(1, 2)));
            Assert.True(schema.Validate(MPJson.Array(1, 2, 3)));
            Assert.False(schema.Validate(MPJson.Array(1, 2, 3, 4)));
            Assert.True(schema.Validate(0));
            Assert.False(SingletonSchema("maxItems", -1).IsValid);
            Assert.False(SingletonSchema("maxItems", 1.5).IsValid);
            Assert.False(SingletonSchema("maxItems", "1").IsValid);
            Assert.False(SingletonSchema("maxItems", MPJson.Array()).IsValid);
        }

        [Fact]
        public void MinItemsTest()
        {
            var schema = SingletonSchema("minItems", 3);

            Assert.True(schema.IsValid);
            Assert.False(schema.Validate(MPJson.Array()));
            Assert.False(schema.Validate(MPJson.Array(1, 2)));
            Assert.True(schema.Validate(MPJson.Array(1, 2, 3)));
            Assert.True(schema.Validate(MPJson.Array(1, 2, 3, 4)));
            Assert.True(schema.Validate(0));
            Assert.False(SingletonSchema("minItems", 1.5).IsValid);
            Assert.False(SingletonSchema("minItems", "1").IsValid);
            Assert.False(SingletonSchema("minItems", MPJson.Array()).IsValid);
        }

        [Fact]
        public void UniqueItemsTest()
        {
            var unique = SingletonSchema("uniqueItems", true);

            Assert.True(unique.IsValid);
            Assert.True(unique.Validate(MPJson.Array()));
            Assert.True(unique.Validate(MPJson.Array(1)));
            Assert.True(unique.Validate(MPJson.Array(1, 2)));
            Assert.True(unique.Validate(MPJson.Array(3, 4, 5)));
            Assert.False(unique.Validate(MPJson.Array(3, 4, 4)));
            Assert.False(unique.Validate(MPJson.Array(5, 5)));
            Assert.False(SingletonSchema("uniqueItems", "true").IsValid);
            Assert.False(SingletonSchema("uniqueItems", MPJson.Array()).IsValid);
        }

        [Fact]
        public void NonuniqueItemsTest()
        {
            var nonunique = SingletonSchema("uniqueItems", false);
            Assert.True(nonunique.IsValid);
            Assert.True(nonunique.Validate(MPJson.Array()));
            Assert.True(nonunique.Validate(MPJson.Array(1)));
            Assert.True(nonunique.Validate(MPJson.Array(1, 2)));
            Assert.True(nonunique.Validate(MPJson.Array(3, 4, 5)));
            Assert.True(nonunique.Validate(MPJson.Array(3, 4, 4)));
            Assert.True(nonunique.Validate(MPJson.Array(5, 5)));

            Assert.False(SingletonSchema("uniqueItems", "false").IsValid);
            Assert.False(SingletonSchema("uniqueItems", MPJson.Array()).IsValid);
        }

        [Fact]
        public void ContainsTest()
        {
            var schema = SingletonSchema("contains", MPJson.Object(MPJson.Property("type", "number")));

            Assert.True(schema.IsValid);
            Assert.True(schema.Validate(MPJson.Array(1)));
            Assert.True(schema.Validate(MPJson.Array(3, 4, 4)));
            Assert.True(schema.Validate(MPJson.Array("a", 2)));
            Assert.True(schema.Validate(MPJson.Array(1, "b")));
            Assert.False(schema.Validate(MPJson.Array()));
            Assert.False(schema.Validate(MPJson.Array("a")));
            Assert.False(schema.Validate(MPJson.Array("a", "b", "c")));

            Assert.True(SingletonSchema("contains", true).IsValid);
            Assert.False(SingletonSchema("contains", "true").IsValid);
        }

        [Fact]
        public void MaxContainsTest()
        {
            MPSchema schema = MPJson.Object("contains", MPJson.Object(MPJson.Property("type", "number")),
                                       "maxContains", 2);

            Assert.True(schema.IsValid);
            Assert.True(schema.Validate(MPJson.Array(1)));
            Assert.True(schema.Validate(MPJson.Array(1,2)));
            Assert.True(schema.Validate(MPJson.Array("a", 2)));
            Assert.True(schema.Validate(MPJson.Array("a", 2, 3)));
            Assert.True(schema.Validate(MPJson.Array(1, "b")));
            Assert.False(schema.Validate(MPJson.Array(1, "a", 2, 3)));
            Assert.False(schema.Validate(MPJson.Array(3, 4, 4)));
            Assert.False(schema.Validate(MPJson.Array()));
            Assert.False(schema.Validate(MPJson.Array("a")));
            Assert.False(schema.Validate(MPJson.Array("a", "b", "c")));
        }

        [Fact]
        public void MinContainsTest()
        {
            MPSchema schema = MPJson.Object("contains", MPJson.Object(MPJson.Property("type", "number")),
                                       "minContains", 2);

            Assert.True(schema.IsValid);
            Assert.False(schema.Validate(MPJson.Array(1)));
            Assert.True(schema.Validate(MPJson.Array(1, 2)));
            Assert.False(schema.Validate(MPJson.Array("a", 2)));
            Assert.True(schema.Validate(MPJson.Array("a", 2, 3)));
            Assert.False(schema.Validate(MPJson.Array(1, "b")));
            Assert.True(schema.Validate(MPJson.Array(1, "a", 2, 3)));
            Assert.True(schema.Validate(MPJson.Array(3, 4, 4)));
            Assert.False(schema.Validate(MPJson.Array()));
            Assert.False(schema.Validate(MPJson.Array("a")));
            Assert.False(schema.Validate(MPJson.Array("a", "b", "c")));
        }

        [Fact]
        public void ItemsListValidationTest()
        {
            var schema = SingletonSchema("items", MPJson.Object(MPJson.Property("type", "number")));

            Assert.True(schema.IsValid);
            Assert.True(schema.Validate(MPJson.Array()));
            Assert.True(schema.Validate(MPJson.Array(1)));
            Assert.True(schema.Validate(MPJson.Array(1, 2)));
            Assert.False(schema.Validate(MPJson.Array(1, "2")));
            Assert.False(schema.Validate(MPJson.Array("2")));

            Assert.False(SingletonSchema("items", 1).IsValid);
            Assert.True(SingletonSchema("items", true).IsValid);
            Assert.True(SingletonSchema("items", false).IsValid);
        }
        
        [Fact]
        public void ItemsTupleValidationTest()
        {
            var schema = SingletonSchema("items",
                MPJson.Array(
                    MPJson.Object(MPJson.Property("type", "number")),
                    MPJson.Object(MPJson.Property("type", "string")),
                    MPJson.Object(MPJson.Property("type", "boolean"))
                    ));

            Assert.True(schema.IsValid);
            Assert.True(schema.Validate(MPJson.Array(0)));
            Assert.True(schema.Validate(MPJson.Array(2, "b")));
            Assert.True(schema.Validate(MPJson.Array(1, "a", true)));
            Assert.True(schema.Validate(MPJson.Array(3, "c", false, "x")));
            Assert.True(schema.Validate(MPJson.Array(3, "c", false, MPJson.Null)));
            Assert.False(schema.Validate(MPJson.Array("a")));
            Assert.False(schema.Validate(MPJson.Array(2, 3)));
            Assert.False(schema.Validate(MPJson.Array(1, "a", 2)));
            Assert.False(schema.Validate(MPJson.Array("a", "c", false, "x")));
        }

        [Fact]
        public void AdditionalItemsFalseValidationTest()
        {
            MPSchema schema = MPJson.Object(
                MPJson.Property("items",
                    MPJson.Array(
                        MPJson.Object(MPJson.Property("type", "number")),
                        MPJson.Object(MPJson.Property("type", "string")),
                        MPJson.Object(MPJson.Property("type", "boolean"))
                        )),
                MPJson.Property("additionalItems", 
                    false)
                );

            Assert.True(schema.IsValid);
            Assert.True(schema.Validate(MPJson.Array(0)));
            Assert.True(schema.Validate(MPJson.Array(2, "b")));
            Assert.True(schema.Validate(MPJson.Array(1, "a", true)));
            Assert.False(schema.Validate(MPJson.Array(3, "c", false, "x")));
            Assert.False(schema.Validate(MPJson.Array(3, "c", false, MPJson.Null)));
        }

        [Fact]
        public void AdditionalItemsValidationTest()
        {
            MPSchema schema = MPJson.Object(
                MPJson.Property("items",
                    MPJson.Array(
                        MPJson.Object(MPJson.Property("type", "number")),
                        MPJson.Object(MPJson.Property("type", "string")),
                        MPJson.Object(MPJson.Property("type", "boolean"))
                        )),
                MPJson.Property("additionalItems",
                    MPJson.Object(MPJson.Property("type", "null"))
                ));

            Assert.True(schema.IsValid);
            Assert.True(schema.Validate(MPJson.Array(0)));
            Assert.True(schema.Validate(MPJson.Array(2, "b")));
            Assert.True(schema.Validate(MPJson.Array(1, "a", true)));
            Assert.False(schema.Validate(MPJson.Array(3, "c", false, "x")));
            Assert.True(schema.Validate(MPJson.Array(3, "c", false, MPJson.Null)));
            Assert.True(schema.Validate(MPJson.Array(3, "c", false, MPJson.Null, MPJson.Null)));
            Assert.False(schema.Validate(MPJson.Array(3, "c", false, MPJson.Null, MPJson.Null, "x")));

            Assert.False(SingletonSchema("additionalItems", "hi").IsValid);
        }

        #region Helpers
        MPSchema SingletonSchema(string keyword, MPJson json)
        {
            return MPJson.Object(MPJson.Property(keyword, json));
        }
        #endregion
    }
}

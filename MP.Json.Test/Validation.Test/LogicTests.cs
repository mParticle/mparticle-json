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
    public class LogicTests
    {
        private MPJson m2 = MPJson.Object(MPJson.Property("multipleOf", 2));
        private MPJson m3 = MPJson.Object(MPJson.Property("multipleOf", 3));
        private MPJson m5 = MPJson.Object(MPJson.Property("multipleOf", 5));
        private MPJson m7 = MPJson.Object(MPJson.Property("multipleOf", 7));


        [Fact]
        private void LogicKeywordsRequiredNonEmptyArrayTest()
        {
            Assert.False(SimpleSchema("allOf", true).IsValid);
            Assert.False(SimpleSchema("anyOf", true).IsValid);
            Assert.False(SimpleSchema("oneOf", true).IsValid);

            Assert.False(SimpleSchema("allOf", MPJson.Array()).IsValid);
            Assert.False(SimpleSchema("anyOf", MPJson.Array()).IsValid);
            Assert.False(SimpleSchema("oneOf", MPJson.Array()).IsValid);
        }

        [Fact]
        public void AllOfTest()
        {
            var schema = SimpleSchema("allOf", MPJson.Array(m2, m3, m5));

            Assert.True(schema.IsValid);
            Assert.True(schema.Validate(0));
            Assert.False(schema.Validate(1));
            Assert.False(schema.Validate(2));
            Assert.False(schema.Validate(15));
            Assert.False(schema.Validate(21));
            Assert.True(schema.Validate(30));
            Assert.True(schema.Validate(60));
            Assert.True(schema.Validate("Not a number"));
        }

        [Fact]
        public void AnyOfTest()
        {
            var schema = SimpleSchema("anyOf", MPJson.Array(m2, m3, m5));

            Assert.True(schema.IsValid);
            Assert.True(schema.Validate(0));
            Assert.False(schema.Validate(1));
            Assert.True(schema.Validate(2));
            Assert.True(schema.Validate(15));
            Assert.True(schema.Validate(21));
            Assert.True(schema.Validate(30));
            Assert.True(schema.Validate(60));
            Assert.True(schema.Validate("Not a number"));
        }

        [Fact]
        public void OneOfTest()
        {
            var schema = SimpleSchema("oneOf", MPJson.Array(m2, m3, m5));

            Assert.True(schema.IsValid);
            Assert.False(schema.Validate(0));
            Assert.False(schema.Validate(1));
            Assert.True(schema.Validate(2));
            Assert.False(schema.Validate(15));
            Assert.True(schema.Validate(21));
            Assert.False(schema.Validate(30));
            Assert.False(schema.Validate(60));

            // Wow! This is surprising but true!!
            Assert.False(schema.Validate("Not a number"));
        }

        [Fact]
        public void NotTest()
        {
            var schema = SimpleSchema("not", m3);

            Assert.True(schema.IsValid);
            Assert.False(schema.Validate(0));
            Assert.True(schema.Validate(1));
            Assert.True(schema.Validate(2));
            Assert.False(schema.Validate(15));
            Assert.False(schema.Validate(21));
            Assert.False(schema.Validate(30));
            Assert.False(schema.Validate(60));

            // Wow! This is surprising but true!!
            Assert.False(schema.Validate("Not a number"));
        }

        [Fact]
        public void IfThenElseTest()
        {
            MPSchema schema = new MPSchema(
                MPJson.Object(
                    MPJson.Property("if", m2),
                    MPJson.Property("then", m3),
                    MPJson.Property("else", m5)
                    ));

            Assert.True(schema.IsValid);

            // if T then T else T
            Assert.True(schema.Validate(0));
            Assert.True(schema.Validate(30));
            Assert.True(schema.Validate(60));
            Assert.True(schema.Validate("Not a number"));

            // if T then T else F
            Assert.True(schema.Validate(6));

            // if T then F else T
            Assert.False(schema.Validate(10));

            // if T then F else F
            Assert.False(schema.Validate(2));

            // if F then T else T
            Assert.True(schema.Validate(15));

            // if F then T else F
            Assert.False(schema.Validate(21));

            // if F then T else F
            Assert.False(schema.Validate(3));

            // if F then F else F
            Assert.False(schema.Validate(1));

        }

        #region Helpers
        MPSchema SimpleSchema(string keyword, MPJson json)
        {
            return new MPSchema(MPJson.Object(MPJson.Property(keyword, json)));
        }
        #endregion
    }
}

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
    public class TypeTests
    {

        [Fact]
        public void EveythingSchemaTests()
        {
            var schema = new MPSchema(true);
            Assert.True(schema.IsValid);
            Assert.True(schema.IsValid);
            Assert.True(schema.Validate(true));
            Assert.True(schema.Validate(123));
            Assert.True(schema.Validate(MPJson.Object()));
            Assert.True(schema.Validate(MPJson.Array()));
        }

        [Fact]
        public void NothingSchemaTests()
        {
            var schema = new MPSchema(false);
            Assert.True(schema.IsValid);
            Assert.True(schema.IsValid);
            Assert.False(schema.Validate(true));
            Assert.False(schema.Validate("string"));
            Assert.False(schema.Validate(123));
            Assert.False(schema.Validate(MPJson.Object()));
            Assert.False(schema.Validate(MPJson.Array()));
        }

        [Fact]
        public void TypeNullSchemaTests()
        {
            var schema = new MPSchema(
                MPJson.Object(
                    MPJson.Property("type", MPJson.Array("null"))
                    )
                );

            Assert.True(schema.IsValid);
            Assert.True(schema.Validate(MPJson.Null));
            Assert.False(schema.Validate("x"));
            Assert.False(schema.Validate(123));
            Assert.False(schema.Validate(true));
            Assert.False(schema.Validate(MPJson.Object()));
            Assert.False(schema.Validate(MPJson.Array()));
        }

        [Fact]
        public void TypeBooleanSchemaTests()
        {
            var schema = new MPSchema(
                MPJson.Object(
                    MPJson.Property("type", MPJson.Array("boolean"))
                    )
                );

            Assert.True(schema.IsValid);
            Assert.True(schema.Validate(false));
            Assert.True(schema.Validate(true));
            Assert.False(schema.Validate(MPJson.Null));
            Assert.False(schema.Validate("x"));
            Assert.False(schema.Validate(123));
            Assert.False(schema.Validate(MPJson.Object()));
            Assert.False(schema.Validate(MPJson.Array()));
        }


        [Fact]
        public void TypeNumberSchemaTests()
        {
            var schema = new MPSchema(
                MPJson.Object(
                    MPJson.Property("type", MPJson.Array("number"))
                    )
                );

            Assert.True(schema.IsValid);
            Assert.True(schema.Validate(123.2));
            Assert.True(schema.Validate(54));
            Assert.False(schema.Validate(false));
            Assert.False(schema.Validate(true));
            Assert.False(schema.Validate(MPJson.Null));
            Assert.False(schema.Validate("x"));
            Assert.False(schema.Validate(MPJson.Object()));
            Assert.False(schema.Validate(MPJson.Array()));
        }

        [Fact]
        public void TypeIntegerSchemaTests()
        {
            var schema = new MPSchema(
                MPJson.Object(
                    MPJson.Property("type", MPJson.Array("integer"))
                    )
                );

            Assert.True(schema.IsValid);
            Assert.False(schema.Validate(123.2));
            Assert.True(schema.Validate(54));
            Assert.False(schema.Validate(false));
            Assert.False(schema.Validate(true));
            Assert.False(schema.Validate(MPJson.Null));
            Assert.False(schema.Validate("x"));
            Assert.False(schema.Validate(MPJson.Object()));
            Assert.False(schema.Validate(MPJson.Array()));
        }

        [Fact]
        public void TypeArraySchemaTests()
        {
            var schema = new MPSchema(
                MPJson.Object(
                    MPJson.Property("type", MPJson.Array("array"))
                    )
                );

            Assert.True(schema.IsValid);
            Assert.False(schema.Validate(123.2));
            Assert.False(schema.Validate(54));
            Assert.False(schema.Validate(false));
            Assert.False(schema.Validate(true));
            Assert.False(schema.Validate(MPJson.Null));
            Assert.False(schema.Validate("x"));
            Assert.False(schema.Validate(MPJson.Object()));
            Assert.True(schema.Validate(MPJson.Array()));
        }

        [Fact]
        public void TypeObjectSchemaTests()
        {
            var schema = new MPSchema(MPJson.Object(MPJson.Property("type", MPJson.Array("object"))));

            Assert.True(schema.IsValid);
            Assert.False(schema.Validate(123.2));
            Assert.False(schema.Validate(54));
            Assert.False(schema.Validate(false));
            Assert.False(schema.Validate(true));
            Assert.False(schema.Validate(MPJson.Null));
            Assert.False(schema.Validate("x"));
            Assert.True(schema.Validate(MPJson.Object()));
            Assert.False(schema.Validate(MPJson.Array()));
        }


        [Fact]
        public void TypeCombinedSchemaTests()
        {
            var schema = new MPSchema(
                MPJson.Object(
                    MPJson.Property("type", MPJson.Array("integer", "array", "string"))
                    )
                );

            Assert.True(schema.IsValid);
            Assert.False(schema.Validate(123.2));
            Assert.True(schema.Validate(54));
            Assert.False(schema.Validate(false));
            Assert.False(schema.Validate(true));
            Assert.False(schema.Validate(MPJson.Null));
            Assert.True(schema.Validate("x"));
            Assert.False(schema.Validate(MPJson.Object()));
            Assert.True(schema.Validate(MPJson.Array()));
        }

        [Fact]
        public void SchemaConstTest()
        {
            var schema = new MPSchema(
                MPJson.Object(
                    MPJson.Property("const", true)
                    )
                );

            Assert.True(schema.IsValid);
            Assert.False(schema.Validate(123.2));
            Assert.False(schema.Validate(54));
            Assert.False(schema.Validate(false));
            Assert.True(schema.Validate(true));
            Assert.False(schema.Validate(MPJson.Null));
            Assert.False(schema.Validate("x"));
            Assert.False(schema.Validate(MPJson.Object()));
            Assert.False(schema.Validate(MPJson.Array()));
        }

        [Fact]
        public void SchemaConstArrayTest()
        {
            var schema = new MPSchema(
                MPJson.Object(
                    MPJson.Property("const", MPJson.Array(true, 54))
                    )
                );

            Assert.True(schema.IsValid);
            Assert.True(schema.Validate(MPJson.Array(true, 54)));
            Assert.False(schema.Validate(123.2));
            Assert.False(schema.Validate(54));
            Assert.False(schema.Validate(false));
            Assert.False(schema.Validate(true));
            Assert.False(schema.Validate(MPJson.Null));
            Assert.False(schema.Validate("x"));
            Assert.False(schema.Validate(MPJson.Object()));
            Assert.False(schema.Validate(MPJson.Array()));
        }

        [Fact]
        public void SchemaEnumTest()
        {
            var schema = new MPSchema(
                MPJson.Object(
                    MPJson.Property("enum", MPJson.Array(true, 54, MPJson.Array()))
                    )
                );

            Assert.True(schema.IsValid);
            Assert.False(schema.Validate(123.2));
            Assert.True(schema.Validate(54));
            Assert.False(schema.Validate(false));
            Assert.True(schema.Validate(true));
            Assert.False(schema.Validate(MPJson.Null));
            Assert.False(schema.Validate("x"));
            Assert.False(schema.Validate(MPJson.Object()));
            Assert.True(schema.Validate(MPJson.Array()));
        }

        [Fact]
        public void SchemaEnumEmptyArrayIsInvalidTest()
        {
            var schema = new MPSchema(
                MPJson.Object(
                    MPJson.Property("enum", MPJson.Array())
                    )
                );

            Assert.False(schema.IsValid);
            Assert.False(schema.Validate(true));
        }

        [Fact]
        public void SchemaEnumWrongTypeIsInvalidTest()
        {
            var schema = new MPSchema(
                MPJson.Object(
                    MPJson.Property("enum", "Hello")
                    )
                );

            Assert.False(schema.IsValid);
            Assert.False(schema.Validate("Hello"));
        }
    }
}

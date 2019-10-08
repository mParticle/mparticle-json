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
    public class KeywordUtilsTest
    {

        [Fact]
        public void GetKeywordTextTest()
        {
            Assert.Equal("minLength", Keyword.MinLength.GetText());
            Assert.Equal("minLength", Keyword.MinLength.GetText());
            Assert.Equal("$ref", Keyword._Ref.GetText());
        }

        [Fact]
        public void GetErrorTypeTextTest()
        {
            Assert.Equal("minLength", ErrorType.MinLength.GetText());
            Assert.Equal("minLength", ErrorType.MinLength.GetText());
            Assert.Equal("$ref", ErrorType.Ref.GetText());
        }


        [Fact]
        public void SchemaFlagsToTypeFlagsTest()
        {
            Assert.Equal(TypeFlags.None, KeywordUtils.SchemaFlagsToTypeFlags(0));
            Assert.Equal(TypeFlags.All, KeywordUtils.SchemaFlagsToTypeFlags(SchemaFlags.TypeAll));
            Assert.Equal(TypeFlags.Array, KeywordUtils.SchemaFlagsToTypeFlags(SchemaFlags.TypeArray));
            Assert.Equal(TypeFlags.Boolean, KeywordUtils.SchemaFlagsToTypeFlags(SchemaFlags.TypeBoolean));
            Assert.Equal(TypeFlags.Integer, KeywordUtils.SchemaFlagsToTypeFlags(SchemaFlags.TypeInteger));
            Assert.Equal(TypeFlags.Null, KeywordUtils.SchemaFlagsToTypeFlags(SchemaFlags.TypeNull));
            Assert.Equal(TypeFlags.Number, KeywordUtils.SchemaFlagsToTypeFlags(SchemaFlags.TypeNumber));
            Assert.Equal(TypeFlags.Object, KeywordUtils.SchemaFlagsToTypeFlags(SchemaFlags.TypeObject));
            Assert.Equal(TypeFlags.String, KeywordUtils.SchemaFlagsToTypeFlags(SchemaFlags.TypeString));
        }

        [Fact]
        public void TypeFlagsToSchemaFlagsTest()
        {
            Assert.Equal(SchemaFlags.None, KeywordUtils.TypeFlagsToSchemaFlags(0));
            Assert.Equal(SchemaFlags.TypeAll, KeywordUtils.TypeFlagsToSchemaFlags(TypeFlags.All));
            Assert.Equal(SchemaFlags.TypeArray, KeywordUtils.TypeFlagsToSchemaFlags(TypeFlags.Array));
            Assert.Equal(SchemaFlags.TypeBoolean, KeywordUtils.TypeFlagsToSchemaFlags(TypeFlags.Boolean));
            Assert.Equal(SchemaFlags.TypeInteger, KeywordUtils.TypeFlagsToSchemaFlags(TypeFlags.Integer));
            Assert.Equal(SchemaFlags.TypeNull, KeywordUtils.TypeFlagsToSchemaFlags(TypeFlags.Null));
            Assert.Equal(SchemaFlags.TypeNumber, KeywordUtils.TypeFlagsToSchemaFlags(TypeFlags.Number));
            Assert.Equal(SchemaFlags.TypeObject, KeywordUtils.TypeFlagsToSchemaFlags(TypeFlags.Object));
            Assert.Equal(SchemaFlags.TypeString, KeywordUtils.TypeFlagsToSchemaFlags(TypeFlags.String));
        }


    }
}

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
    public class SubschemaTests
    {

        [Fact]
        public void KeywordsTest()
        {
            // NOTE:
            // 1) type doesn't show up in keywords
            // 2) Dependencies is split into DependentSchemas and DependentKeywords


            Assert.Empty(SchemaConstants.Nothing.Keywords);
            Assert.Empty(SchemaConstants.Everything.Keywords);

            MPSchema schema = MPJson.Parse("{ 'minLength' : 1, 'maxLength' : 2 }");
            Assert.Equal(new KeywordSet { Keyword.MinLength, Keyword.MaxLength }, schema.Root.Keywords);

            schema = MPJson.Parse("{ 'enum' : [ false ] }");
            Assert.Equal(new KeywordSet { Keyword.Enum }, schema.Root.Keywords);


            schema = MPJson.Parse("{ 'type' : [ 'string' ] }");
            Assert.Empty(schema.Root.Keywords);


            schema = MPJson.Parse("{ 'dependencies' : { 'a' : [ ] } } ");
            Assert.Equal(new KeywordSet { Keyword.DependentRequired }, schema.Root.Keywords);

        }



        [Fact]
        public void TypeFlagsTest()
        {

            MPSchema schema = MPJson.Parse("{ } ");
            Assert.Equal(TypeFlags.All, schema.Root.Type);

            schema = MPJson.Parse("{ 'type' : 'array' } ");
            Assert.Equal(TypeFlags.Array, schema.Root.Type);

            schema = MPJson.Parse("{ 'type' : 'integer' } ");
            Assert.Equal(TypeFlags.Integer, schema.Root.Type);

            schema = MPJson.Parse("{ 'type' : 'null' } ");
            Assert.Equal(TypeFlags.Null, schema.Root.Type);

            schema = MPJson.Parse("{ 'type' : 'number' } ");
            Assert.Equal(TypeFlags.Number, schema.Root.Type);

            schema = MPJson.Parse("{ 'type' : 'object' } ");
            Assert.Equal(TypeFlags.Object, schema.Root.Type);

            schema = MPJson.Parse("{ 'type' : 'string' } ");
            Assert.Equal(TypeFlags.String, schema.Root.Type);

            schema = MPJson.Parse("{ 'type' : ['string', 'number'] } ");
            Assert.Equal(TypeFlags.String|TypeFlags.Number, schema.Root.Type);


            Assert.Equal(TypeFlags.None, SchemaConstants.Nothing.Type);

            Assert.Equal(TypeFlags.All, SchemaConstants.Everything.Type);

        }

    }
}

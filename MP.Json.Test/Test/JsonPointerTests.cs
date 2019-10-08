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
using MP.Json.Validation;

namespace MP.Json.Test
{
    public class JsonPointerTests
    {
        [Fact]
        public void EmptyJsonPointerTest()
        {
            var root = new JsonPointer();
            Assert.Equal("#/", root.Text);
            Assert.Equal(root.Text, root.ToString());
        }

        [Fact]
        public void KeywordTest()
        {
            var root = new JsonPointer();
            root.Push(Keyword.AdditionalProperties);
            Assert.Equal("#/additionalProperties", root.Text);
            Assert.Equal(root.Text, root.ToString());

            root.Push(Keyword._Ref);
            Assert.Equal("#/additionalProperties/$ref", root.Text);
            Assert.Equal(root.Text, root.ToString());
        }

        [Fact]
        public void IndexTest()
        {
            var root = new JsonPointer();
            root.Push(1);
            Assert.Equal("#/1", root.Text);
            Assert.Equal(root.Text, root.ToString());

            root.Push(2);
            Assert.Equal("#/1/2", root.Text);
            Assert.Equal(root.Text, root.ToString());

            root.Push(0);
            Assert.Equal("#/1/2/0", root.Text);
            Assert.Equal(root.Text, root.ToString());
        }

        [Fact]
        public void PropertiesTest()
        {
            var root = new JsonPointer();
            root.Push("alpha");
            Assert.Equal("#/alpha", root.Text);
            Assert.Equal(root.Text, root.ToString());

            root.Push("beta");
            Assert.Equal("#/alpha/beta", root.Text);
            Assert.Equal(root.Text, root.ToString());
        }

        [Fact]
        public void PopTest()
        {
            var root = new JsonPointer();
            root.Push(1);
            Assert.Equal("#/1", root.Text);
            Assert.Equal(root.Text, root.ToString());

            root.Push(2);
            Assert.Equal("#/1/2", root.Text);
            Assert.Equal(root.Text, root.ToString());

            root.Push(0);
            Assert.Equal("#/1/2/0", root.Text);
            Assert.Equal(root.Text, root.ToString());

            root.Pop();
            Assert.Equal("#/1/2", root.Text);
            Assert.Equal(root.Text, root.ToString());

            root.Pop();
            root.Push(3);
            Assert.Equal("#/1/3", root.Text);
            Assert.Equal(root.Text, root.ToString());

            root.Pop();
            root.Pop();
            Assert.Equal("#/", root.Text);
            Assert.Equal(root.Text, root.ToString());
        }

        [Fact]
        public void MixTest()
        {
            var root = new JsonPointer();
            root.Push(Keyword.Items);
            Assert.Equal("#/items", root.Text);
            Assert.Equal(root.Text, root.ToString());

            root.Push(2);
            Assert.Equal("#/items/2", root.Text);
            Assert.Equal(root.Text, root.ToString());

            root.Push(Keyword.Properties);
            Assert.Equal("#/items/2/properties", root.Text);
            Assert.Equal(root.Text, root.ToString());

            root.Push("Name");
            Assert.Equal("#/items/2/properties/Name", root.Text);
            Assert.Equal(root.Text, root.ToString());
        }

        [Fact]
        public void EscapeTest()
        {
            var root = new JsonPointer();
            root.Push("~/bin");
            Assert.Equal("#/~0~1bin", root.Text);
            Assert.Equal(root.Text, root.ToString());
            Assert.Equal("~/bin", root.FirstProperty);

            root.Push("sh.exe");
            Assert.Equal("#/~0~1bin/sh.exe", root.Text);
            Assert.Equal(root.Text, root.ToString());
        }

        [Fact]
        public void PropertyTest()
        {
            var root = new JsonPointer();
            root.Push("FirstProperty");
            root.Push(Keyword.Properties);
            root.Push("MidProperty");
            root.Push("LastProperty");
            root.Push(Keyword.Items);
            root.Push(0);
            Assert.Equal("#/FirstProperty/properties/MidProperty/LastProperty/items/0", root.Text);
            Assert.Equal(Keyword.Items, root.Keyword);
            Assert.Equal("FirstProperty", root.FirstProperty);
            Assert.Equal("LastProperty", root.LastProperty);
        }

        [Fact]
        public void EmptyPropertyTest()
        {
            var root = new JsonPointer();
            Assert.Equal(Keyword.None, root.Keyword);
            Assert.Null(root.FirstProperty);
            Assert.Null(root.LastProperty);
        }

        [Fact]
        public void AssignTest()
        {
            var path = new JsonPointer("#/First~1~0Property/properties/MidProperty/LastProperty/items/0");
            Assert.Equal(6, path.Length);
            Assert.Equal("First/~Property", path.FirstProperty);
            Assert.Equal("items", path.LastProperty);
        }

        [Fact]
        public void UnescapeTest()
        {
            Assert.Equal("~/~", JsonPointer.Unescape("~0~1~0"));
        }

    }
}

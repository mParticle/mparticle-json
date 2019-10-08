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
using System.Linq;
using System.Text;
using Xunit;

namespace MP.Json.Test
{
    public class JsonBasicTests
    {
        [Fact]
        public void UndefinedTest()
        {
            Assert.Null(default(MPJson).Value);
            Assert.Equal(JsonType.Undefined, default(MPJson).Type);
            Assert.Equal(JsonType.Undefined, MPJson.From(null).Type);
            Assert.True(default(MPJson).IsUndefined);
            Assert.False(default(MPJson).HasValue);

            Assert.Null(MPJson.Undefined.Value);
            Assert.Equal(JsonType.Undefined, MPJson.Undefined.Type);
            Assert.True(MPJson.Undefined.IsUndefined);
            Assert.False(MPJson.Undefined.HasValue);
            Assert.Equal("undefined", MPJson.Undefined.ToString());
        }

        [Fact]
        public void NullTest()
        {
            Assert.Null(MPJson.From(null).Value);
            Assert.Equal(DBNull.Value, MPJson.Null.Value);
            Assert.Equal(JsonType.Null, MPJson.Null.Type);
            Assert.True(MPJson.Null.HasValue);
            Assert.False(MPJson.Null.IsUndefined);
            Assert.Equal("null", MPJson.Null.ToString());
            Assert.Equal(JsonType.Null, new MPJson(null).Type);
        }

        [Fact]
        public void FalseTest()
        {
            Assert.Equal(false, MPJson.False.Value);
            Assert.NotEqual(true, MPJson.False.Value);
            Assert.Equal((MPJson)false, MPJson.False);
            Assert.Equal(JsonType.Boolean, ((MPJson)false).Type);
            Assert.Equal(JsonType.Boolean, MPJson.False.Type);
            Assert.True(MPJson.False.HasValue);
            Assert.False(MPJson.False.IsUndefined);
            Assert.Equal("false", MPJson.False.ToString());
        }

        [Fact]
        public void TrueTest()
        {
            Assert.NotEqual(false, MPJson.True.Value);
            Assert.Equal(true, MPJson.True.Value);
            Assert.Equal((MPJson)true, MPJson.True);
            Assert.Equal(JsonType.Boolean, ((MPJson)true).Type);
            Assert.Equal(JsonType.Boolean, MPJson.True.Type);
            Assert.True(MPJson.True.HasValue);
            Assert.False(MPJson.True.IsUndefined);
            Assert.Equal("true", MPJson.True.ToString());
        }

        [Fact]
        public void NumberTest()
        {
            Assert.NotEqual(false, MPJson.Zero.Value);
            Assert.Equal(0.0, MPJson.Zero.Value);
            Assert.Equal((MPJson)0, MPJson.Zero);
            Assert.Equal(JsonType.Number, ((MPJson)0).Type);
            Assert.Equal(JsonType.Number, MPJson.Zero.Type);
            Assert.True(MPJson.Zero.HasValue);
            Assert.False(MPJson.Zero.IsUndefined);
            Assert.Equal(JsonType.Undefined, MPJson.Zero[0].Type);
            Assert.Equal("0", MPJson.Zero.ToString());
        }

        [Fact]
        public void StringTest()
        {
            string str = "Hello";
            var json = (MPJson)str;
            Assert.NotEqual(0, json.Value);
            Assert.NotEqual(str, MPJson.Zero.Value);
            Assert.Equal(json, new MPJson(str));
            Assert.Equal(JsonType.String, json.Type);
            Assert.True(json.HasValue);
            Assert.False(json.IsUndefined);
            Assert.Equal(JsonType.Undefined, json[0].Type);
            Assert.Equal(JsonType.Undefined, json["x"].Type);
            Assert.Equal("\"Hello\"", json.ToString());
        }

        [Fact]
        public void ObjectTest()
        {
            MPJson obj = MPJson.Object(
                MPJson.Property("a", 1),
                MPJson.Property("b", 2),
                MPJson.Property("c", 3)
                );

            Assert.Equal(JsonType.Object, obj.Type);
            Assert.True(obj.HasValue);
            Assert.False(obj.IsUndefined);
            Assert.Equal(1.0, obj["a"]);
            Assert.Equal(2.0, obj["b"]);
            Assert.Equal(3.0, obj["c"]);
            Assert.Equal(JsonType.Undefined, obj[0].Type);
            Assert.Equal(JsonType.Undefined, obj["d"].Type);
            Assert.Equal("{ \"a\":1, \"b\":2, \"c\":3 }", obj.ToString());
            Assert.Equal(3, obj.Keys.Count());
        }

        [Fact]
        public void ArrayTest()
        {
            object[] array = new object[] { 1.0, 2.0, 3.0 };
            MPJson json1 = MPJson.From(array);
            MPJson json2 = MPJson.From(array.Clone());
            MPJson ja = MPJson.Array(1.0, 2.0, 3.0);

            Assert.Equal(JsonType.Array, json1.Type);
            Assert.Equal(JsonType.Array, json2.Type);
            Assert.Equal(JsonType.Array, ja.Type);
            Assert.True(json1.HasValue);
            Assert.False(json1.IsUndefined);
            Assert.Equal(json1, json2);
            Assert.Equal(json1, ja);
            Assert.Equal(1.0, ja[0]);
            Assert.Equal(2.0, ja[1]);
            Assert.Equal(3.0, ja[2]);
            Assert.Equal(JsonType.Undefined, json1["x"].Type);
            Assert.Equal("[ 1, 2, 3 ]", ja.ToString());
            Assert.Equal(3, ja.Count());
            Assert.Empty(ja.Keys);
        }

        [Fact]
        public void JsonMatchesValuesTest()
        {
            Assert.True(MPJson.Matches(null, null));
            Assert.False(MPJson.Matches(null, "x"));
            Assert.True(MPJson.Matches(1, 1));

            Assert.True(MPJson.Matches("a", "a"));
            Assert.False(MPJson.Matches(1, 2));
            Assert.False(MPJson.Matches("a", "b"));

            Assert.False(MPJson.Matches("a", 1));
        }

        [Fact]
        public void JsonMatchesArraysTest()
        {
            object[] array0 = new object[] { };
            object[] array1 = new object[] { true };
            object[] array3 = new object[] { 1.0, 2.0, "x" };
            object[] array3b = new object[] { 1.0, 3.0, "x" };
            object[] array2 = new object[] { 1.0, 2.0 };

            Assert.False(MPJson.Matches(null, array0));
            Assert.True(MPJson.Matches(array3, array3));
            Assert.True(MPJson.Matches(array2, array2.ToArray()));
            Assert.False(MPJson.Matches(array1, array2));
            Assert.False(MPJson.Matches(array3, array3b));
            Assert.False(MPJson.Matches(array2, array3));
        }

        [Fact]
        public void JsonMatchesObjectsTest()
        {
            MPJson j1 = MPJson.Object();
            MPJson j3 = MPJson.Object(
                    MPJson.Property("a", 1),
                    MPJson.Property("b", 2),
                    MPJson.Property("c", 3)
                );
            MPJson j3b = MPJson.Object(
                    MPJson.Property("c", 3),
                    MPJson.Property("b", 2),
                    MPJson.Property("a", 1)
                );
            MPJson j3c = MPJson.Object(
                    MPJson.Property("c", 1),
                    MPJson.Property("b", 2),
                    MPJson.Property("a", 3)
                );

            Assert.NotEqual(j1, j3);
            Assert.NotEqual(j3, j3c);
            Assert.Equal(j3, j3b);
        }
    }
}

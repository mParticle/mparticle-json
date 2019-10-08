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
using System.IO;
using System.Text;
using Xunit;

namespace MP.Json.Test
{
    public class JsonParserTests
    {

        [Fact]
        public void NullTest()
        {
            Assert.Equal(MPJson.Null, MPJson.Parse("null"));
        }

        [Fact]
        public void BooleanTest()
        {
            Assert.Equal(MPJson.True, MPJson.Parse("true"));
            Assert.Equal(MPJson.False, MPJson.Parse("false"));
        }

        [Fact]
        public void NumberTest()
        {
            Assert.Equal(new MPJson(1), MPJson.Parse("1"));
            Assert.Equal(new MPJson(0), MPJson.Parse("0"));
            Assert.Equal(new MPJson(-2), MPJson.Parse("-2"));
            Assert.Equal(new MPJson(-3.5), MPJson.Parse("-3.5"));
            Assert.Equal(new MPJson(1e300), MPJson.Parse("1e300"));
            Assert.Equal(new MPJson(1e-300), MPJson.Parse("1e-300"));
            Assert.Equal(new MPJson(1.5e300), MPJson.Parse("1.5e300"));
            Assert.Equal(new MPJson(1.5e-300), MPJson.Parse("1.5e-300"));
            Assert.Equal(new MPJson(-1.53e300), MPJson.Parse("-1.53e+300"));
        }

        [Fact]
        public void StringTest()
        {
            Assert.Equal(new MPJson(""), MPJson.Parse("\"\""));
            Assert.Equal(new MPJson("123"), MPJson.Parse("\"123\""));
            Assert.Equal(new MPJson("\""), MPJson.Parse(@"""\"""""));
            Assert.Equal(new MPJson("\b\f\t\r\n"), MPJson.Parse(@"""\b\f\t\r\n"""));

            Assert.Equal(new MPJson(""), MPJson.Parse("''"));
            Assert.Equal(new MPJson("123"), MPJson.Parse("'123'"));
            Assert.Equal(new MPJson("\\"), MPJson.Parse(@"'\\'"));
            Assert.Equal(new MPJson("/"), MPJson.Parse(@"'\/'"));
            Assert.Equal(new MPJson("\b\f\t\r\n"), MPJson.Parse(@"'\b\f\t\r\n'"));
            Assert.Equal(new MPJson("\u0041"), MPJson.Parse(@"'\u0041'"));
            Assert.Equal(new MPJson("\u000c"), MPJson.Parse(@"'\u000c'"));

            Assert.False(MPJson.TryParse("\"", out _));
        }


        [Fact]
        public void FormatterControlTest()
        {
            var parsed = MPJson.Parse(@"'\u000c'");
            var json = parsed.ToString();
        }

        [Fact]
        public void ObjectTest()
        {
            var data = @" { 'a':1, 'b':2, 'c': 3 } ";

            MPJson obj = MPJson.Object(
                MPJson.Property("a", 1),
                MPJson.Property("b", 2),
                MPJson.Property("c", 3)
                );

            var parsed = MPJson.Parse(data);

            MPJson empty = MPJson.Parse("{}");
            MPJson singleton = MPJson.Parse(" { 'a':1 } ");


            Assert.Equal(3, obj.Length);
            Assert.Equal(3, parsed.Length);
            Assert.Equal(1, parsed["a"]);
            Assert.Equal(2, parsed["b"]);
            Assert.Equal(3, parsed["c"]);
            Assert.Equal(MPJson.Undefined, parsed["d"]);
            Assert.Equal(obj, parsed);
            Assert.Equal(1, singleton.Length);
            Assert.Equal(0, empty.Length);
            Assert.Equal(JsonType.Object, empty.Type);
        }

        [Fact]
        public void ArrayTest()
        {
            var data = @" [ 1, 2, 3 ] ";

            MPJson array = MPJson.Array(1, 2, 3);
            var parsed = MPJson.Parse(data);

            Assert.Equal(3, array.Length);
            Assert.Equal(3, parsed.Length);
            Assert.Equal(1, parsed[0]);
            Assert.Equal(2, parsed[1]);
            Assert.Equal(3, parsed[2]);
            Assert.Equal(MPJson.Undefined, parsed[3]);
            Assert.Equal(array, parsed);
        }

        [Theory]
        [InlineData("schema.json")]
        [InlineData("schema_noref.json")]
        public void LargeFileTest(string file)
        {
            string filepath = Path.Combine(AppContext.BaseDirectory, "sampledata/batch-level/" + file);

            using (var reader = File.OpenText(filepath))
            {
                MPJson json = MPJson.Parse(reader);
                Assert.True(json.HasValue);
            }
        }


        [Fact]
        public void ExpectedStringErrorTest()
        {
            var data = " { 'x': 1, } ";

            var ex = Record.Exception(() => MPJson.Parse(data));
            Assert.NotNull(ex);
            Assert.True(ex is FormatException);
            Assert.Contains("Expected string", ex.Message);
        }
    }
}

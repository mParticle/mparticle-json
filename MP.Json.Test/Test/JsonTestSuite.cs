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
using System.Linq;
using System.Text;
using Xunit;

// Remove unused parameter
#pragma warning disable xUnit1026 
#pragma warning disable IDE0060 

namespace MP.Json.JsonTestSuite
{
    public class TestSuite
    {

        [Theory]
        [MemberData(nameof(ReadFiles), "y_*")]
        public void ValidSpecTest(string file, JsonSpecData spec)
        {
            var json = TryParse(spec.Json);
            if (spec.Valid == true)
                Assert.True(json.HasValue);
            else if (spec.Valid == false)
                Assert.False(json.HasValue);
        }

        [Theory]
        [MemberData(nameof(ReadFiles), "y_array*")]
        public void ValidArrayTest(string file, JsonSpecData spec)
        {
            var json = TryParse(spec.Json);
            if (spec.Valid == true)
                Assert.True(json.HasValue);
            else if (spec.Valid == false)
                Assert.False(json.HasValue);
        }

        [Theory]
        [MemberData(nameof(ReadFiles), "y_*number*")]
        public void ValidNumberTest(string file, JsonSpecData spec)
        {
            var json = TryParse(spec.Json);
            if (spec.Valid == true)
                Assert.True(json.HasValue);
            else if (spec.Valid == false)
                Assert.False(json.HasValue);
        }

        [Theory]
        [MemberData(nameof(ReadFiles), "y_object*")]
        public void ValidObjectTest(string file, JsonSpecData spec)
        {
            if (file.Contains("single_quote"))
                return;

            var json = TryParse(spec.Json);
            if (spec.Valid == true)
                Assert.True(json.HasValue);
            else if (spec.Valid == false)
                Assert.False(json.HasValue);
        }

        [Theory]
        [MemberData(nameof(ReadFiles), "y_string*")]
        public void ValidStringTest(string file, JsonSpecData spec)
        {
            if (file.Contains("single_quote"))
                return;

            var json = TryParse(spec.Json);
            if (spec.Valid == true)
                Assert.True(json.HasValue);
            else if (spec.Valid == false)
                Assert.False(json.HasValue);
        }

        [Theory]
        [MemberData(nameof(ReadFiles), "y_structure*")]
        public void ValidStructureTest(string file, JsonSpecData spec)
        {
            var json = TryParse(spec.Json);
            if (spec.Valid == true)
                Assert.True(json.HasValue);
            else if (spec.Valid == false)
                Assert.False(json.HasValue);
        }

        [Theory]
        [MemberData(nameof(ReadFiles), "n_*")]
        public void InvalidSpecTest(string file, JsonSpecData spec)
        {
            if (file.Contains("single_quote"))
                return;

            var json = TryParse(spec.Json);
            if (spec.Valid == true)
                Assert.True(json.HasValue);
            else if (spec.Valid == false)
                Assert.False(json.HasValue);
        }

        [Theory]
        [MemberData(nameof(ReadFiles), "n_array*")]
        public void InvalidArrayTest(string file, JsonSpecData spec)
        {
            var json = TryParse(spec.Json);
            if (spec.Valid == true)
                Assert.True(json.HasValue);
            else if (spec.Valid == false)
                Assert.False(json.HasValue);
        }

        [Theory]
        [MemberData(nameof(ReadFiles), "n_incomplete*")]
        public void InvalidKeywordTest(string file, JsonSpecData spec)
        {
            var json = TryParse(spec.Json);
            if (spec.Valid == true)
                Assert.True(json.HasValue);
            else if (spec.Valid == false)
                Assert.False(json.HasValue);
        }

        [Theory]
        [MemberData(nameof(ReadFiles), "n_*number*")]
        public void InvalidNumberTest(string file, JsonSpecData spec)
        {
            var json = TryParse(spec.Json);
            if (spec.Valid == true)
                Assert.True(json.HasValue);
            else if (spec.Valid == false)
                Assert.False(json.HasValue);
        }

        [Theory]
        [MemberData(nameof(ReadFiles), "n_object*")]
        public void InvalidObjectTest(string file, JsonSpecData spec)
        {
            if (file.Contains("single_quote"))
                return;

            var json = TryParse(spec.Json);
            if (spec.Valid == true)
                Assert.True(json.HasValue);
            else if (spec.Valid == false)
                Assert.False(json.HasValue);
        }

        [Theory]
        [MemberData(nameof(ReadFiles), "n_string*")]
        public void InvalidStringTest(string file, JsonSpecData spec)
        {
            if (file.Contains("single_quote"))
                return;

            var json = TryParse(spec.Json);
            if (spec.Valid == true)
                Assert.True(json.HasValue);
            else if (spec.Valid == false)
                Assert.False(json.HasValue);
        }

        [Theory]
        [MemberData(nameof(ReadFiles), "n_structure*")]
        public void InvalidStructureTest(string file, JsonSpecData spec)
        {
            var json = TryParse(spec.Json);
            if (spec.Valid == true)
                Assert.True(json.HasValue);
            else if (spec.Valid == false)
                Assert.False(json.HasValue);
        }


        static MPJson TryParse(string jsonText)
        { 
            var parser = new JsonParser() { Strict = true };
            parser.TryParse(jsonText, out MPJson json);
            return json;
        }

        public static IEnumerable<object[]> ReadFiles(string pattern)
        {
            string baseDir = Path.Combine(AppContext.BaseDirectory, "testsuites/JSONTestSuite/test_parsing");
            foreach(string filePath in Directory.EnumerateFiles(baseDir, pattern))
            {
                string filename = Path.GetFileName(filePath);
                string text = File.ReadAllText(filePath);
                var data = new JsonSpecData
                {
                    Json = text,
                    Path = filePath,
                    FileName = filename,
                    Valid = filename[0] == 'y' ? true : (filename[0] == 'n' ? false : default(bool?)),
                };
                yield return new object[] { filename, data };
            }
        }

        public class JsonSpecData
        {
            public string Json;
            public string Path;
            public string FileName;
            public bool? Valid;
        }
    }
}

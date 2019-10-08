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

namespace MP.Json.Validation.Test
{
    public class RefTests
    {
        [Theory]
        [InlineData("sampledata/batch-level", "schema.json", "goodbatch.json", true)]
        [InlineData("sampledata/batch-level", "schema.json", "badbatch.json", false)]
        [InlineData("sampledata/batch-level", "schema_noref.json", "goodbatch.json", true)]
        [InlineData("sampledata/batch-level", "schema_noref.json", "badbatch.json", false)]
        [InlineData("sampledata/event-level", "commerce_event_schema.json", "commerce_event.json", true)]
        [InlineData("sampledata/event-level", "commerce_event_schema.json", "commerce_event_bad.json", false)]
        [InlineData("sampledata/event-level", "custom_event_schema.json", "custom_event.json", true)]
        [InlineData("sampledata/event-level", "custom_event_schema.json", "custom_event_bad.json", false)]
        public void TestSchemaFilesWithRef(string root, string schemaFile, string jsonFile, bool valid)
        {
            string schemaPath = Path.Combine(AppContext.BaseDirectory, root, schemaFile);
            string jsonPath = Path.Combine(AppContext.BaseDirectory, root, jsonFile);

            MPJson json = MPJson.Parse(File.ReadAllText(jsonPath));
            Assert.True(json.HasValue);
            
            MPSchema schema = MPJson.Parse(File.ReadAllText(schemaPath));
            Assert.True(schema.IsValid);

            var validated = schema.Validate(json);
            Assert.Equal(valid, validated);
        }



    }
}

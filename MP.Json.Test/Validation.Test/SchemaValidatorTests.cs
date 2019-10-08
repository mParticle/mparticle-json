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

using MP.Json.Validation;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace MP.Json.Test
{
    public class SchemaValidatorTests
    {
        [Fact]
        public void CheckStringCoercesToNumber()
        {
            MPSchema schema = MPJson.Object("type", "number", "maximum", 3);

            var jsonValidator = new JsonValidator() { CoerceStringsToValues = true };
            Assert.True(jsonValidator.Validate(schema, "1"));
            Assert.True(jsonValidator.Validate(schema, 1));
            Assert.True(jsonValidator.Validate(schema, "3"));
            Assert.True(jsonValidator.Validate(schema, 3));
            Assert.False(jsonValidator.Validate(schema, "4"));
            Assert.False(jsonValidator.Validate(schema, 4));
            Assert.False(jsonValidator.Validate(schema, "fails"));
        }

        [Fact]
        public void CheckStringCoercionDisabled()
        {
            MPSchema schema = MPJson.Object("type", "number", "maximum", 3);

            var jsonValidator = new JsonValidator();
            Assert.False(jsonValidator.Validate(schema, "1"));
            Assert.True(jsonValidator.Validate(schema, 1));
            Assert.False(jsonValidator.Validate(schema, "3"));
            Assert.True(jsonValidator.Validate(schema, 3));
            Assert.False(jsonValidator.Validate(schema, "4"));
            Assert.False(jsonValidator.Validate(schema, 4));
            Assert.False(jsonValidator.Validate(schema, "fails"));
        }

        [Fact]
        public void CheckStringCoercesToBoolean()
        {
            MPSchema schema = MPJson.Object("type", "boolean", "maximum", 3);

            var jsonValidator = new JsonValidator() { CoerceStringsToValues = true };

            Assert.True(jsonValidator.Validate(schema, true));
            Assert.True(jsonValidator.Validate(schema, false));
            Assert.True(jsonValidator.Validate(schema, "true"));
            Assert.True(jsonValidator.Validate(schema, "false"));
            Assert.True(jsonValidator.Validate(schema, "True"));
            Assert.True(jsonValidator.Validate(schema, "False"));

            Assert.False(jsonValidator.Validate(schema, "1"));
            Assert.False(jsonValidator.Validate(schema, 1));
            Assert.False(jsonValidator.Validate(schema, "3"));
            Assert.False(jsonValidator.Validate(schema, 3));
            Assert.False(jsonValidator.Validate(schema, "4"));
            Assert.False(jsonValidator.Validate(schema, 4));
            Assert.False(jsonValidator.Validate(schema, "fails"));
        }


    }
}

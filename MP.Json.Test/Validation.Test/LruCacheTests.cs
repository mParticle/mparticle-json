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

namespace MP.Json.Validation.Test
{
    public class LruCacheTests
    {
        [Fact]
        public void AddTest()
        {
            var lru = new LruCache<string, string>(4);
            Assert.Null(lru["foo"]);
            Assert.Empty(lru);

            lru["foo"] = "bar";
            Assert.Single(lru);

            lru["foo"] = "xyz";
            Assert.Single(lru);
            Assert.Equal("xyz", lru["foo"]);

            lru["1"] = "1";
            Assert.Equal(2, lru.Count);
            lru["2"] = "2";
            Assert.Equal(3, lru.Count);
            lru["1"] = "1b";
            Assert.Equal(3, lru.Count);
            lru["3"] = "3";
            Assert.Equal(4, lru.Count);
            lru["4"] = "4";
            Assert.Equal(4, lru.Count);

            Assert.Null(lru["foo"]);
            Assert.NotNull(lru["1"]);
            Assert.NotNull(lru["2"]);
            Assert.NotNull(lru["3"]);
            Assert.NotNull(lru["4"]);

            Assert.Equal("1b", lru["1"]);
            Assert.Equal("2", lru["2"]);
            Assert.Equal("3", lru["3"]);
            Assert.Equal("4", lru["4"]);
        }

        [Fact]
        public void RetrieveTest()
        {
            var lru = new LruCache<string, string>(2);
            lru["foo"] = "bar";
            Assert.Equal("bar", lru["foo"]);
            
            // Foo2 is added

            lru["foo2"] = "bar2";
            Assert.Equal("bar", lru["foo"]);
            Assert.Equal("bar2", lru["foo2"]);
            Assert.Equal("bar", lru["foo"]);
            
            // Foo3 is added, Foo2 is at end and is removed

            lru["foo3"] = "bar3";
            Assert.Equal("bar3", lru["foo3"]);
            Assert.Null(lru["foo2"]);
            Assert.Equal("bar", lru["foo"]);
            Assert.Equal("bar3", lru["foo3"]);
        }

        [Fact]
        public void CountTest()
        {
            var lru = new LruCache<string, string>(0);
            lru["foo"] = "bar";
            Assert.Empty(lru);

            lru = new LruCache<string, string>(1);
            lru["foo"] = "bar";
            Assert.Single(lru);
            lru["foo2"] = "bar2";
            Assert.Single(lru);

            lru = new LruCache<string, string>(2);
            lru["foo"] = "bar";
            Assert.Single(lru);
            lru["foo2"] = "bar2";
            Assert.Equal(2, lru.Count);
            lru["foo3"] = "bar3";
            Assert.Equal(2, lru.Count);
        }
    }
}

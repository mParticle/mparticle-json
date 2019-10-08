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

using System.Linq;
using Xunit;

namespace MP.Json.Validation.Test
{
    public class KeywordSetTests
    {
        [Fact]
        public void CountsAreAccurate()
        {
            Assert.Empty(default(KeywordSet));
            Assert.Empty(new KeywordSet { });
            Assert.True(new KeywordSet().IsEmpty);

            Assert.Equal((int)Keyword.MAX_STORED, KeywordSet.All.Count);
            Assert.False(KeywordSet.All.IsEmpty);

            Assert.Single(new KeywordSet { Keyword.Properties });
            Assert.Single(new KeywordSet { Keyword.Properties, Keyword.Properties });
            Assert.Equal(2, new KeywordSet { Keyword.Properties, Keyword.Items }.Count);

            Assert.False(new KeywordSet { Keyword.Properties }.IsEmpty);
            Assert.False(new KeywordSet { Keyword.Properties, Keyword.Properties }.IsEmpty);
            Assert.Equal(2, new KeywordSet { Keyword.Properties, Keyword.Items }.Count);
        }

        [Fact]
        public void IsEmptyIsAccurate()
        {
            Assert.True(new KeywordSet().IsEmpty);
            Assert.False(KeywordSet.All.IsEmpty);
            Assert.False(new KeywordSet { Keyword.Properties }.IsEmpty);
            Assert.False(new KeywordSet { Keyword.Properties, Keyword.Properties }.IsEmpty);
        }

        [Fact]
        public void RemoveRemovesItems()
        {
            var set = new KeywordSet();
            Assert.Empty(set);

            set.Add(Keyword.Properties);
            Assert.Single(set);
            Assert.Equal(Keyword.Properties, set.Single());

            Assert.False(set.Remove(Keyword.Items));
            Assert.Single(set);
            Assert.True(set.Remove(Keyword.Properties));
            Assert.Empty(set);

            var all = KeywordSet.All;
            var prevCount = all.Count;
            Assert.True(all.Remove(Keyword.Properties));
            Assert.Equal(prevCount-1, all.Count);
            Assert.False(all.Remove(Keyword.Properties));
        }

        [Fact]
        public void ContainsTest()
        {
            Assert.Contains(Keyword.Properties, KeywordSet.All);
            Assert.Contains(Keyword.Items, KeywordSet.All);
            Assert.Contains(Keyword.Properties, new KeywordSet { Keyword.Properties });
            Assert.Contains(Keyword.Maximum, new KeywordSet { Keyword.Properties, Keyword.Maximum });

            Assert.DoesNotContain(Keyword.Properties, new KeywordSet());
            Assert.DoesNotContain(Keyword.Items, new KeywordSet());
            Assert.DoesNotContain(Keyword.Pattern, new KeywordSet { Keyword.Properties });
            Assert.DoesNotContain(Keyword.Format, new KeywordSet { Keyword.Properties, Keyword.Items });
        }

        [Fact]
        public void ClearTest()
        {
            var set = new KeywordSet { Keyword.Properties, Keyword.Items };
            Assert.Equal(2, set.Count);
            set.Clear();
            Assert.Empty(set);

            set = KeywordSet.All;
            Assert.NotEmpty(set);
            set.Clear();
            Assert.Empty(set);
        }

        [Fact]
        public void TestEnumerationAndCopyTo()
        {
            var set = KeywordSet.All;
            var array = new Keyword[set.Count];

            set.CopyTo(array, 0);
            Assert.Equal(set.ToArray(), array);

            Assert.Empty(new KeywordSet().ToArray());
        }


        [Fact]
        public void IsSupersetTest()
        {
            var empty = KeywordSet.None;
            var universe = KeywordSet.All;
            var a = new KeywordSet { Keyword.AllOf };
            var ac = new KeywordSet { Keyword.AllOf, Keyword.Const };
            var ad = new KeywordSet { Keyword.AllOf, Keyword.DependentRequired};
            var p = new KeywordSet { Keyword.Properties };

            Assert.True(empty.IsSupersetOf(empty));
            Assert.False(empty.IsSupersetOf(a));
            Assert.False(empty.IsSupersetOf(ac));
            Assert.False(empty.IsSupersetOf(ad));
            Assert.False(empty.IsSupersetOf(universe));

            Assert.True(a.IsSupersetOf(empty));
            Assert.True(a.IsSupersetOf(a));
            Assert.False(a.IsSupersetOf(ac));
            Assert.False(a.IsSupersetOf(ad));
            Assert.False(a.IsSupersetOf(p));
            Assert.False(a.IsSupersetOf(universe));

            Assert.True(ac.IsSupersetOf(empty));
            Assert.True(ac.IsSupersetOf(a));
            Assert.True(ac.IsSupersetOf(ac));
            Assert.False(ac.IsSupersetOf(ad));
            Assert.False(ac.IsSupersetOf(universe));

            Assert.True(universe.IsSupersetOf(empty));
            Assert.True(universe.IsSupersetOf(a));
            Assert.True(universe.IsSupersetOf(ac));
            Assert.True(universe.IsSupersetOf(ad));
            Assert.True(universe.IsSupersetOf(universe));
        }

        [Fact]
        public void IsSubsetTest()
        {
            var empty = KeywordSet.None;
            var universe = KeywordSet.All;
            var a = new KeywordSet { Keyword.AllOf };
            var ac = new KeywordSet { Keyword.AllOf, Keyword.Const };
            var ad = new KeywordSet { Keyword.AllOf, Keyword.DependentRequired };
            var p = new KeywordSet { Keyword.Properties };

            Assert.True(empty.IsSubsetOf(empty));
            Assert.True(empty.IsSubsetOf(a));
            Assert.True(empty.IsSubsetOf(ac));
            Assert.True(empty.IsSubsetOf(ad));
            Assert.True(empty.IsSubsetOf(universe));

            Assert.False(a.IsSubsetOf(empty));
            Assert.True(a.IsSubsetOf(a));
            Assert.True(a.IsSubsetOf(ac));
            Assert.True(a.IsSubsetOf(ad));
            Assert.False(a.IsSubsetOf(p));
            Assert.True(a.IsSubsetOf(universe));

            Assert.False(ac.IsSubsetOf(empty));
            Assert.False(ac.IsSubsetOf(a));
            Assert.True(ac.IsSubsetOf(ac));
            Assert.False(ac.IsSubsetOf(ad));
            Assert.True(ac.IsSubsetOf(universe));

            Assert.False(universe.IsSubsetOf(empty));
            Assert.False(universe.IsSubsetOf(a));
            Assert.False(universe.IsSubsetOf(ac));
            Assert.False(universe.IsSubsetOf(ad));
            Assert.True(universe.IsSubsetOf(universe));
        }

        [Fact]
        public void OverlapsTest()
        {
            var empty = KeywordSet.None;
            var universe = KeywordSet.All;
            var a = new KeywordSet { Keyword.AllOf };
            var ac = new KeywordSet { Keyword.AllOf, Keyword.Const };
            var ad = new KeywordSet { Keyword.AllOf, Keyword.DependentRequired };
            var p = new KeywordSet { Keyword.Properties };

            Assert.False(empty.Overlaps(empty));
            Assert.False(empty.Overlaps(a));
            Assert.False(empty.Overlaps(ac));
            Assert.False(empty.Overlaps(ad));
            Assert.False(empty.Overlaps(universe));

            Assert.False(a.Overlaps(empty));
            Assert.True(a.Overlaps(a));
            Assert.True(a.Overlaps(ac));
            Assert.True(a.Overlaps(ad));
            Assert.True(a.Overlaps(universe));

            Assert.False(ac.Overlaps(empty));
            Assert.True(ac.Overlaps(a));
            Assert.True(ac.Overlaps(ac));
            Assert.True(ac.Overlaps(ad));
            Assert.True(ac.Overlaps(universe));

            Assert.False(universe.Overlaps(empty));
            Assert.True(universe.Overlaps(a));
            Assert.True(universe.Overlaps(ac));
            Assert.True(universe.Overlaps(ad));
            Assert.True(universe.Overlaps(universe));

            Assert.False(p.Overlaps(empty));
            Assert.False(p.Overlaps(a));
            Assert.False(p.Overlaps(ac));
            Assert.False(p.Overlaps(ad));
            Assert.True(p.Overlaps(universe));
        }


        [Fact]
        public void EqualsTest()
        {
            var empty = KeywordSet.None;
            var universe = KeywordSet.All;
            var a = new KeywordSet { Keyword.AllOf };
            var ad = new KeywordSet { Keyword.AllOf, Keyword.DependentRequired };

            Assert.True(empty.Equals(empty));
            Assert.True(a.Equals(a));
            Assert.True(ad.Equals(ad));
            Assert.True(universe.Equals(universe));

            Assert.False(a.Equals(empty));
            Assert.False(a.Equals(ad));
            Assert.False(a.Equals(universe));
        }

        [Fact]
        public void StringKeywordsTest()
        {
            var set = KeywordSet.StringKeywords;
            Assert.Contains(Keyword.Pattern, set);
            Assert.Contains(Keyword.Format, set);
            Assert.Contains(Keyword.MinLength, set);
            Assert.Contains(Keyword.MaxLength, set);

            Assert.True(set.IsSubsetOf(KeywordSet.All));
            Assert.False(set.Overlaps(KeywordSet.NumberKeywords));
            Assert.False(set.Overlaps(KeywordSet.ObjectKeywords));
            Assert.False(set.Overlaps(KeywordSet.ArrayKeywords));
            Assert.False(set.Overlaps(KeywordSet.GenericKeywords));
        }

        [Fact]
        public void NumberKeywordsTest()
        {
            var set = KeywordSet.NumberKeywords;
            Assert.Contains(Keyword.Maximum, set);
            Assert.Contains(Keyword.Minimum, set);
            Assert.Contains(Keyword.ExclusiveMaximum, set);
            Assert.Contains(Keyword.ExclusiveMinimum, set);
            Assert.Contains(Keyword.MultipleOf, set);

            Assert.True(set.IsSubsetOf(KeywordSet.All));
            Assert.False(set.Overlaps(KeywordSet.StringKeywords));
            Assert.False(set.Overlaps(KeywordSet.ObjectKeywords));
            Assert.False(set.Overlaps(KeywordSet.ArrayKeywords));
            Assert.False(set.Overlaps(KeywordSet.GenericKeywords));
        }

        [Fact]
        public void ArrayKeywordsTest()
        {
            var set = KeywordSet.ArrayKeywords;
            Assert.Contains(Keyword.MaxItems, set);
            Assert.Contains(Keyword.MinItems, set);
            Assert.Contains(Keyword.Items, set);
            Assert.Contains(Keyword.AdditionalItems, set);
            Assert.Contains(Keyword.UniqueItems, set);
            Assert.Contains(Keyword.Contains, set);
            Assert.Contains(Keyword.MaxContains, set);
            Assert.Contains(Keyword.MinContains, set);

            Assert.True(set.IsSubsetOf(KeywordSet.All));
            Assert.False(set.Overlaps(KeywordSet.StringKeywords));
            Assert.False(set.Overlaps(KeywordSet.ObjectKeywords));
            Assert.False(set.Overlaps(KeywordSet.NumberKeywords));
            Assert.False(set.Overlaps(KeywordSet.GenericKeywords));
        }

        [Fact]
        public void ObjectKeywordsTest()
        {
            var set = KeywordSet.ObjectKeywords;
            Assert.Contains(Keyword.MaxProperties, set);
            Assert.Contains(Keyword.MinProperties, set);
            Assert.Contains(Keyword.Properties, set);
            Assert.Contains(Keyword.AdditionalProperties, set);
            Assert.Contains(Keyword.DependentRequired, set);
            Assert.Contains(Keyword.DependentSchemas, set);
            Assert.Contains(Keyword.Required, set);
            Assert.Contains(Keyword.PatternProperties, set);
            Assert.Contains(Keyword.PropertyNames, set);

            Assert.True(set.IsSubsetOf(KeywordSet.All));
            Assert.False(set.Overlaps(KeywordSet.StringKeywords));
            Assert.False(set.Overlaps(KeywordSet.ArrayKeywords));
            Assert.False(set.Overlaps(KeywordSet.NumberKeywords));
            Assert.False(set.Overlaps(KeywordSet.GenericKeywords));
        }

        [Fact]
        public void GenericKeywordsTest()
        {
            var set = KeywordSet.GenericKeywords;
            Assert.Contains(Keyword.AllOf, set);
            Assert.Contains(Keyword.AnyOf, set);
            Assert.Contains(Keyword.OneOf, set);
            Assert.Contains(Keyword.Not, set);
            Assert.Contains(Keyword.If, set);
            Assert.Contains(Keyword.Then, set);
            Assert.Contains(Keyword.Else, set);
            Assert.Contains(Keyword.Const, set);
            Assert.Contains(Keyword.Enum, set);
            Assert.Contains(Keyword._Ref, set);

            Assert.True(set.IsSubsetOf(KeywordSet.All));
            Assert.False(set.Overlaps(KeywordSet.StringKeywords));
            Assert.False(set.Overlaps(KeywordSet.ObjectKeywords));
            Assert.False(set.Overlaps(KeywordSet.NumberKeywords));
            Assert.False(set.Overlaps(KeywordSet.ArrayKeywords));
        }
    }
}

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
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using static MP.Json.Validation.BitUtils;

namespace MP.Json.Validation
{
    /// <summary>
    /// Contains a set of Keywords. Each operation is a constant-time and constant-space operation with extremely low overhead.
    /// </summary>
    /// <remarks>
    /// Construction: var set = new KeywordSet { Keyword.Properties, Keyword.AdditionalProperties };
    /// Add: set.Add(Keyword.Properties);
    /// Remove: set.Remove(Keyword.Properties);
    /// LINQ: Linq operations are supported, but LINQ expressions refer to immutable copy not a view of the multable original
    /// </remarks>
    public struct KeywordSet : ICollection<Keyword>, IEquatable<KeywordSet>
    {
        long collection;

        public static readonly KeywordSet None = default;

        public static readonly KeywordSet All = new KeywordSet() { collection = (long)SchemaFlags.StoredProperties };

        public static readonly KeywordSet StringKeywords = new KeywordSet() { collection = (long)SchemaFlags.StringProperties };

        public static readonly KeywordSet ArrayKeywords = new KeywordSet() { collection = (long)SchemaFlags.ArrayProperties };

        public static readonly KeywordSet NumberKeywords = new KeywordSet() { collection = (long)SchemaFlags.NumberProperties };

        public static readonly KeywordSet ObjectKeywords = new KeywordSet() { collection = (long)SchemaFlags.ObjectProperties };

        public static readonly KeywordSet GenericKeywords = new KeywordSet() { collection = (long)SchemaFlags.GenericProperties };

        /// <summary>
        /// Create a keyword set from raw flags
        /// </summary>
        /// <param name="flags"></param>
        /// <returns></returns>
        internal static KeywordSet Raw(long flags)
        {
            return new KeywordSet { collection = flags };
        }

        /// <summary>
        /// Gets the number of message types in the set
        /// </summary>
        public int Count => BitCount(collection);

        /// <summary>
        /// Superfast test for emptiness
        /// </summary>
        public bool IsEmpty => collection == 0;

        bool ICollection<Keyword>.IsReadOnly => false;

        /// <summary>
        /// Ensures that the message type value is within the capacity of the set
        /// </summary>
        /// <param name="index"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ValidateIndex(Keyword index)
        {
            if (unchecked((ulong)index >= 64)) throw new IndexOutOfRangeException();
        }

        /// <summary>
        /// Adds a new Keyword to the set
        /// </summary>
        /// <param name="item"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(Keyword item)
        {
            ValidateIndex(item);
            collection |= 1L << (int)item;
        }

        /// <summary>
        /// Clears all elements from the set
        /// </summary>
        /// <param name="item"></param>
        public void Clear() => collection = 0;

        /// <summary>
        /// Indicates whether the type value is contained with the set
        /// </summary>
        /// <param name="item"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Contains(Keyword item)
        {
            ValidateIndex(item);
            return (collection & (1L << (int)item)) != 0;
        }

         /// <summary>
        /// Copies items from the set to an array
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public void CopyTo(Keyword[] array, int arrayIndex)
        {
            foreach (var v in this)
                array[arrayIndex++] = v;
        }

        /// <summary>
        /// Enumerates items in the set.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<Keyword> GetEnumerator()
        {
            for (var flags = collection;
                flags != 0;
                flags = RemoveLowestBit(flags))
            {
                yield return (Keyword)IndexOfLowestBit(flags);
            }
        }

        /// <summary>
        /// Removes an element from the set
        /// </summary>
        /// <param name="item"></param>
        /// <returns>Returns true if item existed in the set before the operation.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Remove(Keyword item)
        {
            ValidateIndex(item);
            long bit = 1L << (int)item;
            if ((collection & bit) == 0) return false;
            collection -= bit;
            return true;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int BitCount(long y)
        {
            ulong x = (ulong)y;
            x -= (x >> 1) & 0x5555555555555555;
            x = (x & 0x3333333333333333) + ((x >> 2) & 0x3333333333333333);
            x = (x + (x >> 4)) & 0x0f0f0f0f0f0f0f0f;
            return (int)((x * 0x0101010101010101) >> 56);
        }


        public override string ToString() => string.Join(", ", this);

        #region Set Operations
        public bool IsSubsetOf(KeywordSet set) => (collection & ~set.collection) == 0;

        public bool IsSupersetOf(KeywordSet set) => (~collection & set.collection) == 0;

        public bool Overlaps(KeywordSet set) => (collection & set.collection) != 0;

        public bool Equals(KeywordSet set) => collection == set.collection;

        public override bool Equals(object obj) => obj is KeywordSet && Equals((KeywordSet)obj);

        public override int GetHashCode() => collection.GetHashCode();
        #endregion
    }
}

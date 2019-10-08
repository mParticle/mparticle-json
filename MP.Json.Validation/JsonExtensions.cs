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
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

namespace MP.Json
{
    /// <summary>
    /// Extensions to support Json
    /// </summary>
    public static class JsonExtensions
    {
        /// <summary>
        /// Searchs for a property in a JSON object using binary search
        /// </summary>
        /// <typeparam name="V"></typeparam>
        /// <param name="jObject"></param>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [DebuggerStepThrough]
        public static int GetPropertyIndex<V>(this KeyValuePair<string, V>[] jObject, string key, int start = 0)
            => jObject.GetPropertyIndex(key, start, jObject.Length - 1);

        /// <summary>
        /// Searchs for a property in a JSON object using binary search
        /// </summary>
        /// <typeparam name="V"></typeparam>
        /// <param name="jObject"></param>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static int GetPropertyIndex<V>(this KeyValuePair<string, V>[] jObject, string key, int left, int right)
        {
            while (left <= right)
            {
                int mid = (left + right) >> 1;
                int cmp = String.CompareOrdinal(key, jObject[mid].Key);
                if (cmp > 0)
                    left = mid + 1;
                else if (cmp < 0)
                    right = mid - 1;
                else return mid;
            }
            return ~left;
        }

        /// <summary>
        /// Searches for a property in a JSON object
        /// </summary>
        /// <typeparam name="V"></typeparam>
        /// <param name="jObject"></param>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [DebuggerStepThrough]
        public static V GetProperty<V>(this KeyValuePair<string, V>[] jObject, string key, int start = 0)
        {
            int index = GetPropertyIndex(jObject, key, start);
            if (index >= 0) return jObject[index].Value;
            return default;
        }

    }
}

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

namespace MP.Json.Validation
{
    internal class LruCache<K>
    {
        #region Variables
        readonly LinkedList<K> list = new LinkedList<K>();
        readonly Dictionary<K, LinkedListNode<K>> Map = new Dictionary<K, LinkedListNode<K>>();
        readonly int capacity;
        #endregion

        public LruCache(int capacity)
        {
            this.capacity = capacity;
        }

        public K this[K key]
        {
            get
            {
                if (!Map.TryGetValue(key, out var node))
                {
                    Map[key] = list.AddFirst(key);
                    if (Map.Count > capacity)
                    {
                        var last = list.Last;
                        list.Remove(last);
                        Map.Remove(last.Value);
                    }
                    return key;
                }
                list.Remove(node);
                list.AddFirst(node);
                return node.Value;
            }
        }
    }
}

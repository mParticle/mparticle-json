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
using System.Linq;
using System.Text;

namespace MP.Json.Validation
{
    internal class LruCache<K,V> : IEnumerable<KeyValuePair<K,V>>
    {
        #region Variables
        readonly LinkedList<KeyValuePair<K,V>> list 
            = new LinkedList<KeyValuePair<K,V>>();
        readonly Dictionary<K, LinkedListNode<KeyValuePair<K,V>>> map 
            = new Dictionary<K, LinkedListNode<KeyValuePair<K,V>>>();
        readonly int capacity;
        #endregion

        public LruCache(int capacity)
        {
            this.capacity = capacity;
        }

        public V this[K key]
        {
            get
            {
                if (!map.TryGetValue(key, out var node))
                    return default(V);
                list.Remove(node);
                list.AddFirst(node);
                return node.Value.Value;
            }
            set
            {
                if (map.TryGetValue(key, out var node))
                {
                    list.Remove(node);
                    node.Value = new KeyValuePair<K, V>(key, value);
                }
                else
                    map[key] = node = new LinkedListNode<KeyValuePair<K, V>>(
                        new KeyValuePair<K, V>(key, value));

                list.AddFirst(node);
                while (map.Count > capacity)
                {
                    var last = list.Last;
                    list.Remove(last);
                    map.Remove(last.Value.Key);
                }
            }
        }

        public int Count => map.Count;

        public IEnumerator<KeyValuePair<K, V>> GetEnumerator() => list.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}

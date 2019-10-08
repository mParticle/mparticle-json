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
using System.Diagnostics;
using System.Text;

namespace MP.Json
{
    /// <summary>
    /// Convenience class for JSON Object
    /// </summary>
    [DebuggerStepThrough]
    public readonly struct JsonProperty
    {
        #region Constructor
        public JsonProperty(string name, MPJson value)
        {
            Name = name;
            Value = value;
        }
        #endregion

        #region Properties

        /// <summary>
        /// Property name
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// Property value
        /// </summary>
        public readonly MPJson Value;

        #endregion

        #region Methods

        public static implicit operator KeyValuePair<string, object> (JsonProperty prop)
        {
            return new KeyValuePair<string, object>(prop.Name, prop.Value.Value);
        }

        public override string ToString()
        {
            return $"\"{Name}\" : \"{Value}\"";
        }

        #endregion
    }
}

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

namespace MP.Json.Validation
{
    /// <summary>
    /// Extensions to support Json
    /// </summary>
    public static class SchemaExtensions
    {
        /// <summary>
        /// Validate json using schema
        /// </summary>
        /// <param name="json"></param>
        /// <param name="schema"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static bool Validate(this MPJson json, 
            MPSchema schema,
            EventHandler<ValidationArgs> args)
        {
            return schema.Validate(json, args);
        }

    }
}

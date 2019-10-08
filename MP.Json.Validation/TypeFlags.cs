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
    [Flags]
    public enum TypeFlags : long
    {
        None = 0,
        Array = 1L << 0,
        Boolean = 1L << 1,
        Integer = 1L << 2,
        Null = 1L << 3,
        Number = 1L << 4,
        Object = 1L << 5,
        String = 1L << 6,
        All = Array | Boolean | Integer | Null | Number | Object | String,
    }
}

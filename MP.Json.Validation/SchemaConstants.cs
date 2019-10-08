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
    public static class SchemaConstants
    {
        #region Boolean Schema
        /// <summary>
        /// Schema Element matches any object
        /// </summary>
        public static Subschema Everything = new Subschema { Flags = SchemaFlags.TypeAll };

        /// <summary>
        /// Matches none
        /// </summary>
        public static Subschema Nothing = new Subschema { };
        #endregion

        #region Schema Drafts
        public const string StandardPrefix = "http://json-schema.org/draft";
        public const string Schema3 = "http://json-schema.org/draft-03/schema";
        public const string Schema4 = "http://json-schema.org/draft-04/schema";
        public const string Schema5 = "http://json-schema.org/draft-05/schema";
        public const string Schema6 = "http://json-schema.org/draft-06/schema";
        public const string Schema7 = "http://json-schema.org/draft-07/schema";
        public const string Schema201909 = "http://json-schema.org/draft/2019-09/schema";
        #endregion

    }
}

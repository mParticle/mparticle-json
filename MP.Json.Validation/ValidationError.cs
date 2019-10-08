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
    /// <summary>
    /// Newtonsoft.Json.Schema-compatible validation errors.
    /// Unlike validation args, these errors objects are persistent after the
    /// validation callback and are not reused for future errors.
    /// </summary>
    public class ValidationError : EventArgs
    {
        #region Properties

        /// <summary>
        /// Path of JSON where the error occurred
        /// </summary>
        public string Path { get; set; } 

        /// <summary>
        /// The id of the schema, relative to the root schema
        /// </summary>
        public string SchemaId { get; set; }

        /// <summary>
        /// The schema that produced the error.
        /// </summary>
        public Subschema Schema { get; set; }

        /// <summary>
        /// The instance node that produced the error
        /// </summary>
        public MPJson Instance { get; set; }

        /// <summary>
        /// The JSON value where the error occurred
        /// </summary>
        public object Value;

        /// <summary>
        /// Nested errors. This list are errors from children and is typically created
        /// during post-processing.
        /// </summary>
        public IList<ValidationError> ChildErrors;

        /// <summary>
        /// Keyword that produced the error
        /// </summary>        
        public ErrorType ErrorType { get; set; }

        /// <summary>
        /// Gets the base uri of the subschema, when given the root schema
        /// </summary>
        /// <param name="baseUri">uri of root schema</param>
        public string GetBaseUri(string baseUri)
        {
            return System.IO.Path.Combine(baseUri ?? string.Empty, SchemaId ?? string.Empty);
        }

        #endregion
    }
}

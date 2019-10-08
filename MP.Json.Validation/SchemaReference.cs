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
    public class SchemaReference
    {
        #region Constructor
        /// <summary>
        /// Create an unresolved schema reference
        /// </summary>
        /// <param name="s"></param>
        public SchemaReference(string uri)
        {
            this.Uri = uri;
            if (baseUri.StartsWith(SchemaConstants.StandardPrefix))
            {
                if (baseUri == SchemaConstants.Schema4)
                    Version = SchemaVersion.Draft4;
                else if (baseUri == SchemaConstants.Schema6)
                    Version = SchemaVersion.Draft6;
                else if (baseUri == SchemaConstants.Schema7)
                    Version = SchemaVersion.Draft7;
                else if (baseUri == SchemaConstants.Schema201909)
                    Version = SchemaVersion.Draft201909;
                this.Resolved = true;
            }
        }

        /// <summary>
        /// Create a resolved schema reference
        /// </summary>
        /// <param name="s"></param>
        /// <param name="schema"></param>
        public SchemaReference(string s, Subschema schema) : this(s)
        {
            this.Schema = schema;
            this.Resolved = true;
        }

        #endregion

        #region Properties

        private string baseUri;
        private string relative;

        /// <summary>
        /// Uri of schema
        /// </summary>
        public string Uri
        {
            get => baseUri + "#" + relative;
            set => (baseUri, relative) = JsonPointer.Split(value);
        }

        /// <summary>
        /// Base portion of Uri
        /// </summary>
        public string BaseUri
        {
            get => baseUri;
            set => baseUri = value ?? string.Empty;
        }

        /// <summary>
        /// Fragment portion of Uri
        /// </summary>
        public string Fragment
        {
            get => relative;
            set => relative = value ?? string.Empty;
        }

        /// <summary>
        /// Schema that is being referenced
        /// </summary>
        public Subschema Schema;

        /// <summary>
        /// Id value -- Useful for cycle and recursion checking
        /// </summary>
        public int Id;  // unique id within a schema

        /// <summary>
        /// Schema 8 version recursiveReference
        /// </summary>
        public bool Resolved;

        /// <summary>
        /// Version if uri refers to standard schema, or None
        /// </summary>
        public SchemaVersion Version;

        /// <summary>
        /// Schema was previously searched for and not found
        /// </summary>
        public bool NotFound => Resolved && Schema == null;
        #endregion

        #region Method
        /// <summary>
        /// Returns the uri
        /// </summary>
        public override string ToString()
        {
            return Uri;
        }
        #endregion
    }
}

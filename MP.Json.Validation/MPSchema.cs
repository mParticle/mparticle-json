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
    public partial class MPSchema
    {
        #region Construction

        /// <summary>
        /// Creates a schema document from a json object
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>

        public MPSchema(MPJson json, SchemaVersion version = SchemaVersion.All)
        {
            Version = version;
            var map = json.Value as KeyValuePair<string, object>[];
            if (map != null)
            {
                object schemaVersion = map.GetProperty("$schema");
                if (schemaVersion != null)
                {
                    switch (schemaVersion)
                    {
                        case SchemaConstants.Schema4:
                            Version &= SchemaVersion.Draft4;
                            break;
                        case SchemaConstants.Schema6:
                            Version &= SchemaVersion.Draft6;
                            break;
                        case SchemaConstants.Schema7:
                            Version &= SchemaVersion.Draft7;
                            break;
                        case SchemaConstants.Schema201909:
                            Version &= SchemaVersion.Draft201909;
                            break;
                    }
                }

                object baseUriProp = map.GetProperty("$id");
                if (baseUriProp == null
                    && (baseUriProp = map.GetProperty("id")) != null
                    && !LimitVersion(SchemaVersion.Draft4, false))
                {
                    baseUriProp = null;
                }

                string baseUri = JsonPointer.Canonicalize(baseUriProp as string);
                if (string.IsNullOrEmpty(baseUri)
                    || !Uri.TryCreate(baseUri, UriKind.Absolute, out BaseUri))
                    BaseUri = null;
            }

            var builder = new SchemaBuilder(this, json);
            Root = builder.Root;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Returns whether the root is valid
        /// </summary>
        public bool IsValid => Root != null;

        /// <summary>
        /// Root element of schema document
        /// </summary>
        public Subschema Root;

        /// <summary>
        /// Draft version
        /// </summary>
        public SchemaVersion Version { get; set; }

        public Uri BaseUri;

        #endregion

        #region Methods

        public string ConvertReference(string uriFragment)
        {
            if (uriFragment == null)
                return null;

            if (uriFragment != null && BaseUri != null)
            {
                if (Uri.TryCreate(BaseUri, uriFragment, out Uri result) && result.IsAbsoluteUri)
                    uriFragment = result.AbsoluteUri;
            }

            if (uriFragment.IndexOf('#') < 0)
                uriFragment += "#";

            return uriFragment;
        }

        /// <summary>
        /// Validates json. (It's more efficient to use SchemaValidator for multiple validations).
        /// </summary>
        /// <param name="json"></param>
        /// <param name=""></param>
        /// <returns></returns>
        public bool Validate(MPJson json, EventHandler<ValidationArgs> handler = null)
        {
            var schemaValidator = new JsonValidator();
            return schemaValidator.Validate(this, json, handler);
        }

        public List<ValidationError> ValidateWithErrors(MPJson json, int limit = int.MaxValue)
        {
            var schemaValidator = new JsonValidator();

            List<ValidationError> errors = null;

            schemaValidator.Validate(this, json, (v, args) =>
            {
                if (errors == null)
                    errors = new List<ValidationError>();

                if (errors.Count < limit)
                    errors.Add(args.ValidationError);

                if (errors.Count >= limit)
                    args.CancelValidation();
            });

            return errors;
        }

        internal bool LimitVersion(SchemaVersion version, bool includeLaterVersions = true)
        {
            if (includeLaterVersions)
            {
                int x = (int)version;
                x &= -x; // Get last bit
                x = ~(x - 1); // This is the same as x and above
                version = SchemaVersion.All & ((SchemaVersion)x);
            }

            if ((version & Version) == 0) return false;
            Version &= version;
            return true;
        }


        public override string ToString()
        {
            return Root?.ToString() ?? "";
        }

        #endregion

        #region Convenience

        public static implicit operator MPSchema(MPJson json)
        {
            return new MPSchema(json);
        }

        #endregion
    }
}

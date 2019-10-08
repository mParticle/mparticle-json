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
    /// Validation args for schema
    /// </summary>
    public class ValidationArgs : EventArgs
    {
        #region Variables
        private ValidationError _error;
        internal StringBuilder pathBuffer;
        #endregion

        #region Construction
        internal ValidationArgs(JsonValidator validator)
        {
            Validator = validator;
        }
        #endregion

        #region Properties

        /// <summary>
        /// Parent validator
        /// </summary>
        public readonly JsonValidator Validator;

        /// <summary>
        /// Validate Error Type
        /// </summary>
        public ErrorType ErrorType;

        /// <summary>
        /// Schema pointer
        /// </summary>
        public string SchemaPointer => Validator.SchemaPointer
            .GetText(pathBuffer ?? (pathBuffer = new StringBuilder()));

        /// <summary>
        /// Instance pointer
        /// </summary>
        public string InstancePointer => Validator.InstancePointer
            .GetText(pathBuffer ?? (pathBuffer = new StringBuilder()));

        /// <summary>
        /// Actual instance data or some related scalar value
        /// (`contains` returns actual number of matches)
        /// </summary>
        public MPJson Actual;

        /// <summary>
        /// Actual instance data where failure occurred
        /// </summary>
        public MPJson Instance;

        /// <summary>
        /// Expected data for failing keyword
        /// </summary>
        public object Expected;

        /// <summary>
        /// Subschema
        /// </summary>
        public Subschema Schema;

        /// <summary>
        /// Cancels the current validation
        /// </summary>
        public void CancelValidation()
        {
            Validator.CancelValidation();
        }

        /// <summary>
        /// Generate on demand a Newtonsoft-like validation error object 
        /// for compatibility or permanent storage
        /// </summary>
        public ValidationError ValidationError
        {
            get => _error ?? (_error = new ValidationError
            {
                Schema = Schema,
                SchemaId = SchemaPointer,
                Path = InstancePointer,
                Instance = Instance,
                Value = Actual,
                ErrorType = ErrorType,
            });
            internal set => _error = value;
        }
        #endregion
    }
}

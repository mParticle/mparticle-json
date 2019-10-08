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
using System.Globalization;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using static MP.Json.Validation.BitUtils;
using System.Collections.Generic;

namespace MP.Json.Validation
{

    /// <summary>
    /// Reusable non-thread-safe schema validator.
    /// 
    /// This is not thread-safe, so you need one per thread. However, this can be reused for successive
    /// validations in one thread. Just call Init(document, object).
    /// </summary>
    public class JsonValidator
    {
        #region Variables
        const int MaxTimeToLive = 1000 * 1000;

        private EventHandler<ValidationArgs> errorAction;

        private readonly ValidationArgs validationArgs;

        private StringInfo _stringInfo;

        private int suppressionLevel;

        private int timeToLive;

        #endregion

        #region Construction
        public JsonValidator()
        {
            validationArgs = new ValidationArgs(this);
        }
        #endregion

        #region Properties

        public readonly JsonPointer SchemaPointer = new JsonPointer();

        public readonly JsonPointer InstancePointer = new JsonPointer();

        /// <summary>
        /// Strict compliance at the cost of performance 
        /// </summary>

        public bool Strict { get; set; }

        /// <summary>
        /// Enable coercion of string values to numbers and booleans
        /// </summary>
        public bool CoerceStringsToValues { get; set; }

        /// <summary>
        /// Whether validation should abort on first error
        /// </summary>
        public bool AbortOnError { get; set; }

        public SchemaVersion Version { get; set; } = SchemaVersion.All;

        /// <summary>
        /// For correctly reading string lengths
        /// </summary>
        public StringInfo StringInfo
        {
            get
            {
                if (_stringInfo == null)
                    _stringInfo = new StringInfo();
                return _stringInfo;
            }
        }

        /// <summary>
        /// Indicates whether the validation was successful
        /// </summary>
        public bool Success { get; private set; }

        #endregion

        #region Methods

        internal int TurnOffErrors()
        {
            return suppressionLevel++;
        }

        internal void TurnOnErrors()
        {
            suppressionLevel--;
        }

        internal void RestoreSuppressionLevel(int level)
        {
            suppressionLevel = level;
        }

        internal bool IsRecordingErrors
            => suppressionLevel == 0 && errorAction != null;


        internal bool ReportError(ErrorType errorType,
            Subschema schema = null,
            object actual = null,
            object expected = null,
            object instance = null)
        {
            var result = ReportError((Keyword)errorType, schema, actual, expected, instance);
            if (errorType == ErrorType.TimeLimit) CancelValidation();
            return result;
        }

        /// <summary>
        /// Pushes an error onto the errors list with the given parameters
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="key"></param>
        /// <param name="actual"></param>
        /// <param name="expected"></param>
        /// <param name="instancePath"></param>
        /// <param name="schemaPath"></param>
        internal bool ReportError(Keyword keyword,
            Subschema schema,
            object actual = null,
            object expected = null,
            object instance = null)
        {
            // Errors being suppressed don't directly affect success result
            if (suppressionLevel > 0 || errorAction == null)
                return true;

            if (schema != null)
            {
                if (expected == null)
                    expected = schema.GetData(keyword);
            }

            validationArgs.Actual = MPJson.From(actual);
            validationArgs.Instance = MPJson.From(instance ?? actual);
            validationArgs.Expected = expected;
            validationArgs.Schema = schema;
            validationArgs.ValidationError = null;

            if (keyword >= 0)
            {
                validationArgs.ErrorType = (ErrorType)keyword;
                SchemaPointer.Push(keyword);
            }
            else
            {
                validationArgs.ErrorType = (ErrorType)SchemaPointer.Keyword;
            }

            errorAction(this, validationArgs);
            if (keyword >= 0) SchemaPointer.Pop();

            if (AbortOnError)
                errorAction = null;

            return errorAction == null;
        }

        [DebuggerStepThrough]
        internal bool OutOfTime()
        {
            if (timeToLive-- > 0)
                return false;

            ReportError(ErrorType.TimeLimit, null);
            CancelValidation();
            return true;
        }

        public bool Validate(MPSchema schema, MPJson json, EventHandler<ValidationArgs> eventHandler = null)
        {
            return Validate(schema?.Root, json, eventHandler);
        }

        public bool Validate(Subschema root, MPJson json, EventHandler<ValidationArgs> eventHandler = null)
        {
            if (root == null)
                return false;

            object jobject = json.Value;

            SchemaPointer.Clear();
            InstancePointer.Clear();

            suppressionLevel = 0;

            errorAction = eventHandler;
            timeToLive = MaxTimeToLive;
            Success = Validate(root, jobject);
            return Success;
        }

        public void CancelValidation()
        {
            errorAction = null;
        }

        #endregion

        #region Validation

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool Validate(Subschema schema, object instance)
        {
            return !OutOfTime()
                && ValidateType(schema, instance)
                && ValidateGeneric(schema, instance);
        }

        private bool Validate(Subschema schema, object instance,
            Keyword keyword, string prop1, string prop2)
        {
            SchemaPointer.Push(keyword);
            if (prop1 != null) SchemaPointer.Push(prop1);
            if (prop2 != null) InstancePointer.Push(prop2);

            bool result = Validate(schema, instance);

            SchemaPointer.Pop();
            if (prop1 != null) SchemaPointer.Pop();
            if (prop2 != null) InstancePointer.Pop();
            return result;
        }

        private bool Validate(Subschema schema, object instance, Keyword keyword)
        {
            SchemaPointer.Push(keyword);
            bool result = Validate(schema, instance);
            SchemaPointer.Pop();
            return result;
        }

        private bool Validate(Subschema schema, object instance, int index)
        {
            SchemaPointer.Push(index);
            bool result = Validate(schema, instance);
            SchemaPointer.Pop();
            return result;
        }

        private bool Validate(Subschema schema, object instance, int index, int instanceIndex)
        {
            if (index >= 0) SchemaPointer.Push(index);
            InstancePointer.Push(instanceIndex);
            bool result = Validate(schema, instance);
            InstancePointer.Pop();
            if (index >= 0) SchemaPointer.Pop();
            return result;
        }

        private bool ValidateType(Subschema schema, object value)
        {
            var type = MPJson.GetType(value);
            switch (type)
            {
                case JsonType.Object:
                    if ((schema.Flags & SchemaFlags.TypeObject) == 0)
                        break;
                    return ValidateObject(schema, (KeyValuePair<string, object>[])value);

                case JsonType.Array:
                    if ((schema.Flags & SchemaFlags.TypeArray) == 0)
                        break;
                    return ValidateArray(schema, (object[])value);

                case JsonType.Number:
                    double d = Convert.ToDouble(value);
                    if ((schema.Flags & SchemaFlags.TypeNumber) == 0
                        && ((schema.Flags & SchemaFlags.TypeInteger) == 0 || d != Math.Floor(d)))
                        break;
                    return ValidateNumber(schema, d);

                case JsonType.String:
                    if ((schema.Flags & SchemaFlags.TypeString) != 0)
                        return ValidateString(schema, (string)value);

                    if (this.CoerceStringsToValues)
                    {
                        var str = (string)value;
                        if ((schema.Flags & (SchemaFlags.TypeNumber | SchemaFlags.TypeInteger)) != 0
                            && double.TryParse(str, NumberStyles.Float, CultureInfo.InvariantCulture, out d)
                            && (0 != (schema.Flags & SchemaFlags.TypeNumber) || d == Math.Floor(d)))
                            return ValidateNumber(schema, d);

                        if ((schema.Flags & SchemaFlags.TypeBoolean) != 0
                            && string.Equals(str, str.Length == 4 ? JsonParser.TrueKeyword : JsonParser.FalseKeyword, StringComparison.OrdinalIgnoreCase))
                            return true;
                    }

                    break;

                case JsonType.Boolean:
                    if ((schema.Flags & SchemaFlags.TypeBoolean) == 0)
                        break;
                    return true;

                case JsonType.Null:
                    if ((schema.Flags & SchemaFlags.TypeNull) == 0)
                        break;
                    return true;
            }

            if (!IsRecordingErrors)
                return false;

            return (schema.Flags & SchemaFlags.TypeAll) != 0
                ? ReportError(
                    ErrorType.Type,
                    schema,
                    actual: KeywordUtils.GetTypeText(type),
                    expected: schema.GetValidTypes(),
                    instance: value)
                : ReportError(ErrorType.None, actual: value);
        }

        private bool ValidateGeneric(Subschema schema, object instance)
        {
            bool finalResult = true;

            for (SchemaFlags mask = schema.Flags & (SchemaFlags.GenericProperties & SchemaFlags.AllProperties);
                    mask != 0;
                    mask = (SchemaFlags)RemoveLowestBit((long)mask))
            {
                Keyword keyword = (Keyword)IndexOfLowestBit((long)mask);

                bool? result = null;
                Subschema[] schemaArray;
                int index;
                switch (keyword)
                {
                    case Keyword.If:
                        int level = TurnOffErrors();
                        result = Validate(schema.If, instance, Keyword.If);
                        RestoreSuppressionLevel(level);

                        result = result == true
                            ? Validate(schema.Then, instance, Keyword.Then)
                            : Validate(schema.Else, instance, Keyword.Else);
                        break;

                    case Keyword.Not:
                        level = TurnOffErrors();
                        result = !Validate(schema.Not, instance, Keyword.Not);
                        RestoreSuppressionLevel(level);
                        break;

                    case Keyword.AllOf:
                        result = true;
                        schemaArray = schema.AllOf;
                        SchemaPointer.Push(keyword);
                        for (index = 0; index < schemaArray.Length; index++)
                            if (!Validate(schemaArray[index], instance, index))
                            {
                                finalResult = false;
                                if (!IsRecordingErrors) break;
                            }
                        SchemaPointer.Pop();
                        break;

                    case Keyword.AnyOf:
                        result = false;
                        level = TurnOffErrors();
                        schemaArray = schema.AnyOf;
                        SchemaPointer.Push(keyword);
                        for (index = 0; index < schemaArray.Length; index++)
                            if (Validate(schemaArray[index], instance, index))
                            {
                                result = true;
                                break;
                            }
                        SchemaPointer.Pop();
                        RestoreSuppressionLevel(level);
                        break;

                    case Keyword.OneOf:
                        result = false;
                        level = TurnOffErrors();
                        schemaArray = schema.OneOf;
                        SchemaPointer.Push(keyword);
                        for (index = 0; index < schemaArray.Length; index++)
                            if (Validate(schemaArray[index], instance, index))
                            {
                                result = !result;
                                if (result == false)
                                    break;
                            }
                        SchemaPointer.Pop();
                        RestoreSuppressionLevel(level);
                        break;

                    case Keyword.Enum:
                        result = false;
                        // TODO: Provide support for type coercion
                        foreach (object v in (object[])schema.Enum.Value)
                            if (MPJson.Matches(v, instance))
                            {
                                result = true;
                                break;
                            }
                        break;

                    case Keyword.Const:
                        // TODO: Provide support for string to value coercion
                        result = MPJson.Matches(schema.Const.Value, instance);
                        break;

                    case Keyword._Ref:
                        var r = schema.Ref;
                        if (r.Version != 0)
                        {
                            var draft = new MPSchema(MPJson.From(instance), r.Version);
                            result = draft.IsValid;
                            break; // This was checked on creation
                        }

                        var rSchema = r.Schema;
                        result = rSchema != null && ReferenceUsageIsControlled();
                        if (result == true && !Validate(rSchema, instance, Keyword._Ref))
                            finalResult = false;
                        break;

                    case Keyword.Metadata:
                        break;
                }

                if (result == false)
                {
                    finalResult = false;
                    if (ReportError(keyword, schema, instance))
                        return false;
                }
            }

            return finalResult;
        }

        private bool ReferenceUsageIsControlled()
        {
            // The basic idea is that each use of a reference should contribute to
            // some progress in the processing of the instance

            int schemaDepth = SchemaPointer.Length;
            int instanceDepth = InstancePointer.Length;
            int allowedDepth = 2 * instanceDepth + 2;
            return schemaDepth <= allowedDepth;
        }

        private bool ValidateObject(Subschema schema, KeyValuePair<string, object>[] map)
        {
            bool finalResult = true;
            bool propertyCheck = false;

            for (var mask = schema.Flags & SchemaFlags.ObjectProperties;
                    mask != 0;
                    mask = (SchemaFlags)RemoveLowestBit((long)mask))
            {
                Keyword keyword = (Keyword)IndexOfLowestBit((long)mask);
                Subschema subschema;

                bool result = true;
                switch (keyword)
                {
                    case Keyword.DependentSchemas:
                        foreach (var kv in schema.DependentSchemas)
                        {
                            int index = map.GetPropertyIndex(kv.Key);
                            if (index < 0) continue;
                            if (!Validate(kv.Value, map,
                                Keyword.DependentSchemas, kv.Key, null))
                            {
                                finalResult = false;
                                if (!IsRecordingErrors)
                                    return false;
                            }
                        }
                        break;

                    case Keyword.DependentRequired:
                        foreach (var kv in schema.DependentRequired)
                        {
                            int index = map.GetPropertyIndex(kv.Key);
                            if (index >= 0 && !HandleRequired(schema, map, kv.Value,
                                Keyword.DependentRequired, kv.Key))
                            {
                                finalResult = false;
                                if (!IsRecordingErrors)
                                    return false;
                            }
                        }
                        break;

                    case Keyword.MaxProperties:
                        result = map.Length <= schema.MaxProperties;
                        break;

                    case Keyword.MinProperties:
                        result = map.Length >= schema.MinProperties;
                        break;

                    case Keyword.PropertyNames:
                        subschema = schema.PropertyNames;
                        for (int i = 0; i < map.Length; i++)
                        {
                            var kv = map[i];
                            if (!Validate(subschema, kv.Key,
                                Keyword.PropertyNames, null, kv.Key))
                            {
                                finalResult = false;
                                if (!IsRecordingErrors)
                                    return false;
                            }
                        }
                        break;

                    case Keyword.Required:
                        if (!HandleRequired(schema, map, schema.Required,
                            Keyword.Required, null))
                        {
                            finalResult = false;
                            if (!IsRecordingErrors)
                                return false;
                        }
                        break;

                    case Keyword.AdditionalProperties:
                    case Keyword.PatternProperties:
                    case Keyword.Properties:
                        propertyCheck = true;
                        break;
                }

                if (!result)
                {
                    finalResult = false;
                    if (ReportError(keyword, schema, map))
                        return false;
                }
            }

            if (propertyCheck && !ValidateProperties(map,
                    schema.Properties, schema.PatternProperties, schema.AdditionalProperties))
                finalResult = false;

            return finalResult;
        }

        /// <summary>
        /// Fast approach to search all properties
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instMap"></param>
        /// <param name="schemaMap"></param>
        /// <remarks> O(n lg(m/n)) where n is |schema properties| and m is |instance properties| </remarks>
        private void DivideAndConquer<T>(KeyValuePair<string, object>[] instMap, KeyValuePair<string, T>[] schemaMap)
            => DivideAndConquer(instMap, schemaMap, 0, instMap.Length - 1, 0, schemaMap.Length - 1);

        private void DivideAndConquer<T>(KeyValuePair<string, object>[] instMap, KeyValuePair<string, T>[] schemaMap,
            int istart, int iend,
            int sstart, int send)
        {
            while (sstart < send)
            {
                int mid = sstart + send >> 1;
                int partition = instMap.GetPropertyIndex(schemaMap[mid].Key, istart, iend);
                int istart2;
                if (partition >= 0)
                {
                    istart2 = partition + 1;
                }
                else
                {
                    partition = ~partition;
                    istart2 = partition;
                }

                DivideAndConquer(instMap, schemaMap, istart2, iend, mid + 1, send);
                iend = partition - 1;
                send = mid - 1;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="map"></param>
        /// <param name="validator"></param>
        /// <param name="properties"></param>
        /// <param name="patternProperties"></param>
        /// <param name="additionalProperties"></param>
        /// <param name="recordErrors"></param>
        /// <returns></returns>
        private bool ValidateProperties(
            KeyValuePair<string, object>[] map,
            KeyValuePair<string, Subschema>[] properties,
            KeyValuePair<SchemaRegex, Subschema>[] patternProperties,
            Subschema additionalProperties)
        {
            if (properties == null)
                properties = Array.Empty<KeyValuePair<string, Subschema>>();

            bool result = true;
            for (int i = 0, iprop = 0; i < map.Length; i++)
            {
                string prop = map[i].Key;
                object value = map[i].Value;

                // Match property
                int cmp = 1;
                bool found = false;
                for (; iprop < properties.Length; iprop++)
                {
                    cmp = string.CompareOrdinal(properties[iprop].Key, prop);
                    if (cmp >= 0)
                        break;
                }

                if (cmp == 0)
                {
                    found = true;
                    if (!Validate(properties[iprop].Value, value,
                        Keyword.Properties, prop, prop))
                    {
                        result = false;
                        if (!IsRecordingErrors)
                            return false;
                    }
                }

                // Match patternProperty
                if (patternProperties != null)
                {
                    foreach (var pp in patternProperties)
                    {
                        if (pp.Key.Matches(prop) == SchemaRegexSuccess.Success)
                        {
                            found = true;
                            if (!Validate(pp.Value, value,
                                Keyword.PatternProperties, pp.Key.Pattern, prop))
                            {
                                result = false;
                                if (!IsRecordingErrors)
                                    return false;
                            }
                        }
                    }
                }

                // Match additionalProperties
                if (found == false && additionalProperties != null)
                {
                    if (!Validate(additionalProperties, value,
                        Keyword.AdditionalProperties, null, prop))
                    {
                        result = false;
                        if (!IsRecordingErrors)
                            return false;
                    }
                }
            }

            return result;
        }

        private bool HandleRequired(Subschema schema, KeyValuePair<string, object>[] map, string[] required,
            Keyword keyword, string prop)
        {
            bool result = true;
            SchemaPointer.Push(keyword);
            if (prop != null) SchemaPointer.Push(prop);
            for (int index = 0; index < required.Length; index++)
            {
                string s = required[index];
                if (s == null || map.GetPropertyIndex(s) < 0)
                {
                    SchemaPointer.Push(index);
                    ReportError(Keyword.None, schema, actual: prop, expected: s, instance: map);
                    SchemaPointer.Pop();

                    result = false;
                    if (!IsRecordingErrors)
                        break;
                }
            }
            if (prop != null) SchemaPointer.Pop();
            SchemaPointer.Pop();
            return result;
        }

        private bool ValidateArray(Subschema schema, object[] array)
        {
            bool finalResult = true;

            for (var mask = schema.Flags & (SchemaFlags.ArrayProperties & SchemaFlags.AllProperties);
                    mask != 0;
                    mask = (SchemaFlags)RemoveLowestBit((long)mask))
            {
                Keyword keyword = (Keyword)IndexOfLowestBit((long)mask);
                Subschema subschema;
                Subschema[] schemaArray;
                bool result = true;
                bool respond = true;
                switch (keyword)
                {
                    case Keyword.AdditionalItems:
                        // https://json-schema.org/draft/2019-09/json-schema-core.html#rfc.section.8.2.4
                        // AdditionalItems is ignored if Items is not present 
                        subschema = schema.AdditionalItems;
                        schemaArray = schema.Items as Subschema[];
                        respond = false;
                        if (schemaArray != null && schemaArray.Length < array.Length)
                        {
                            SchemaPointer.Push(Keyword.AdditionalItems);
                            for (int i = schemaArray.Length; i < array.Length; i++)
                            {
                                result &= Validate(subschema, array[i], -1, i);
                                if (!result && !IsRecordingErrors)
                                    break;
                            }
                            SchemaPointer.Pop();
                        }
                        break;

                    case Keyword.Contains:
                        subschema = schema.Contains;
                        double minContains = schema.MinContains;
                        double maxContains = schema.MaxContains;
                        result = minContains == 0;
                        int contains = 0;
                        SchemaPointer.Push(Keyword.Contains);
                        TurnOffErrors();
                        for (int i = 0; i < array.Length; i++)
                        {
                            bool elementResult = Validate(subschema, array[i], -1, i);
                            if (elementResult)
                            {
                                contains++;
                                if (contains >= minContains)
                                {
                                    result = true;
                                    if (contains + (array.Length - i - 1) <= maxContains)
                                        break;
                                }
                            }
                        }
                        TurnOnErrors();
                        SchemaPointer.Pop();
                        if (contains > maxContains)
                            result = false;
                        break;

                    case Keyword.Items:
                        object items = schema.Items;
                        SchemaPointer.Push(Keyword.Items);
                        respond = false;
                        if ((subschema = items as Subschema) != null)
                        {
                            for (int i = 0; i < array.Length; i++)
                            {
                                result &= Validate(subschema, array[i], -1, i);
                                if (!result && !IsRecordingErrors)
                                    break;
                            }
                        }
                        else
                        {
                            schemaArray = (Subschema[])items;
                            result = true;
                            for (int i = Math.Min(array.Length, schemaArray.Length) - 1; i >= 0; i--)
                            {
                                result &= Validate(schemaArray[i], array[i], i, i);
                                if (!result && !IsRecordingErrors)
                                    break;
                            }
                        }
                        SchemaPointer.Pop();
                        break;

                    case Keyword.MaxItems:
                        result = array.Length <= schema.MaxItems;
                        break;

                    case Keyword.MinItems:
                        result = array.Length >= schema.MinItems;
                        break;

                    case Keyword.UniqueItems:
                        var uniques = new HashSet<MPJson>();
                        foreach (object v in array)
                            if (!uniques.Add(MPJson.From(v)))
                            {
                                result = false;
                                break;
                            }
                        break;
                }

                if (!result)
                {
                    finalResult = false;
                    if (respond
                        ? ReportError(keyword, schema, array)
                        : !IsRecordingErrors)
                        return false;
                }
            }

            return finalResult;
        }

        private bool ValidateString(Subschema schema, string value)
        {
            bool finalResult = true;

            for (var mask = schema.Flags & SchemaFlags.StringProperties;
                    mask != 0;
                    mask = (SchemaFlags)RemoveLowestBit((long)mask))
            {
                Keyword keyword = (Keyword)IndexOfLowestBit((long)mask);
                bool? result = true;
                switch (keyword)
                {
                    case Keyword.MinLength:
                        StringInfo si;

                        result = value.Length >= schema.MinLength;
                        if (result == true && Strict)
                        {
                            si = StringInfo;
                            si.String = value;
                            result = si.LengthInTextElements >= schema.MinLength;
                        }
                        break;

                    case Keyword.MaxLength:
                        result = value.Length <= schema.MaxLength;
                        if (result == false && Strict)
                        {
                            si = StringInfo;
                            si.String = value;
                            result = si.LengthInTextElements <= schema.MaxLength;
                        }
                        break;

                    case Keyword.Pattern:
                        SchemaRegex pattern = schema.Pattern;
                        var resultType = pattern.Matches(value);
                        if (resultType != SchemaRegexSuccess.Success)
                        {
                            if (resultType == SchemaRegexSuccess.TimedOut)
                            {
                                SchemaPointer.Push(Keyword.Pattern);
                                ReportError(ErrorType.TimeLimit, schema, value);
                                SchemaPointer.Pop();
                                return false;
                            }

                            finalResult = false;
                            if (ReportError(keyword, schema, value, pattern.Pattern))
                                return false;
                        }
                        continue;

                    case Keyword.Format:
                        result = Formats.IsValueOfFormat(value, schema.Format);
                        break;
                }

                if (result == false)
                {
                    finalResult = false;
                    if (ReportError(keyword, schema, value))
                        return false;
                }
            }

            return finalResult;
        }

        private bool ValidateNumber(Subschema schema, double value)
        {
            bool finalResult = true;

            for (var mask = schema.Flags & SchemaFlags.NumberProperties;
                    mask != 0;
                    mask = (SchemaFlags)RemoveLowestBit((long)mask))
            {
                Keyword keyword = (Keyword)IndexOfLowestBit((long)mask);
                bool? result = true;
                switch (keyword)
                {
                    case Keyword.Minimum:
                        result = value >= schema.Minimum;
                        break;

                    case Keyword.Maximum:
                        result = value <= schema.Maximum;
                        break;

                    case Keyword.ExclusiveMaximum:
                        result = value < schema.ExclusiveMaximum;
                        break;

                    case Keyword.ExclusiveMinimum:
                        result = value > schema.ExclusiveMinimum;
                        break;

                    case Keyword.MultipleOf:
                        double m = (double)schema.MultipleOf;
                        double r = value % m;
                        if (r != 0)
                        {
                            r = Math.Abs(r);
                            r = Math.Min(m - r, r);
                            result = r < 1e-14 * m;
                        }
                        break;
                }

                if (result == false)
                {
                    finalResult = false;
                    if (ReportError(keyword, schema, value))
                        return false;
                }
            }

            return finalResult;
        }

        #endregion
    }
}

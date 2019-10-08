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

namespace MP.Json.Validation.Test
{
    public static class Remotes
    {

        public static void AddRemotes()
        {

        }

        public static (string path, string json) FolderInteger()
        {
            return ("folder/folderInteger.json", @"{ 'type': 'integer' }");
        }

        public static (string path, string json) Integer()
        {
            return ("integer.json", @"{ 'type': 'integer' }");
        }

        public static (string path, string json) Name()
        {
            return ("name.json", @"
{
    'definitions': {
        orNull': {
                'anyOf': [
                    {
                    'type': 'null'
                },
                {
                    $ref': '#'
                }
            ]
        }
    },
    'type': 'string'
}");
        }

        public static (string path, string json) NameDefs()
        {
            return ("name-def.json", @"
{
    '$defs': {
        'orNull': {
                'anyOf': [
                    {
                    'type': 'null'
                },
                {
                    '$ref': '#'
                }
            ]
        }
    },
    'type': 'string'
}
");
        }

        public static (string path, string json) Subschemas()
        {
            return ("subSchemas.json", @"
{
    'integer': {
        'type': 'integer'
    },
    'refToInteger': {
        '$ref': '#/integer'
    }
}
");
        }
    }
}

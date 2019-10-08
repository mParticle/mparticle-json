# MP.Json.Validation

## Introduction

MP.Json.Validation is a high-performance JSON Schema validation library for validating JSON documents against a schema. This library was built for our DataPlanning feature to support the real-time validation of a large number of event messages passing through our system each second.

It includes an extremely lightweight JSON document object and an equally lightweight JSON Schema object model. It supports JSON parsing and JSON Schema validation of JSON documents against JSON schemas.

We built this library because other JSON Schema validation libraries did not perform fast enough and/or simply consumed too much memory. This library has been able to perform validation 1 to 2 orders of magnitude faster than other packages such as Newtonsoft.Json.Schema and Manatee.Json. The memory footprint of each schema is also a tenth or more of Newtonsoft.Json.Schema.

MP.Json.Validation also conforms to the latest standards including Drafts 4, 6, 7 and 2019-09 of JsonSchema. It's been validated by industry-standard JSON and JSON Schema Test Suites.

## Documentation

Documentation for the MP.Json.Validation library is maintained in the [wiki](wiki) for the repository. 

[Wiki Home](wiki)


## Installation

This library is available  as a package in Nuget as MP.Json.Validation.

## Benchmarks

### Memory Usage

|                       |     Memory  | Time to Load|
|-----------------------|-------------|-------------|
|Raw Strings            |   449.5 MB  |        0.39s|
|ManateeJSON            |   886.3 MB  |       24.52s|
|Newtonsoft.Json.Schema | 3,710.0 MB  |       51.20s|
|MP.Json.Schema         |   414.1 MB  |        7.70s|
|- without metadata     |    87.8 MB  |        5.05s|

(Memory tests in release mode, 64-bit.)

MP.Json.Schema objects are so efficient that they take less memory than that of the JSON schema stored as strings.

### Performance

BenchmarkDotNet=v0.11.5, OS=Windows 10.0.18362
Intel Core i9-8950HK CPU 2.90GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET Core SDK=3.0.100
  [Host]     : .NET Core 2.2.7 (CoreCLR 4.6.28008.02, CoreFX 4.6.28008.03), 64bit RyuJIT
  Job-LQQHOE : .NET Core 2.2.7 (CoreCLR 4.6.28008.02, CoreFX 4.6.28008.03), 64bit RyuJIT

IterationCount=50

|                     Method | batchSize |         Mean |         Error |        StdDev |       Median | Rank |
|--------------------------- |---------- |-------------:|--------------:|--------------:|-------------:|-----:|
|          NewtonsoftNoCache |         1 |  13,529.2 us |    166.045 us |    327.757 us |  13,479.8 us |   15 |
|        NewtonsoftWithCache |         1 |   3,585.6 us |     40.866 us |     79.706 us |   3,600.6 us |   10 |
|              MPJsonNoCache |         1 |   2,134.9 us |     39.724 us |     79.333 us |   2,095.4 us |    5 |
|            MPJsonWithCache |         1 |     247.1 us |      3.063 us |      6.187 us |     246.7 us |    1 |
| NewtonsoftValidatingReader |         1 |   3,393.3 us |     46.661 us |     93.186 us |   3,392.9 us |    9 |
|                ManateeJson |         1 |  41,278.4 us |    457.939 us |    914.553 us |  41,072.0 us |   20 |
|          NewtonsoftNoCache |         5 |  17,591.9 us |    294.667 us |    595.242 us |  17,490.3 us |   16 |
|        NewtonsoftWithCache |         5 |   6,527.0 us |     92.568 us |    186.992 us |   6,525.2 us |   12 |
|              MPJsonNoCache |         5 |   2,264.2 us |     41.341 us |     83.510 us |   2,235.3 us |    6 |
|            MPJsonWithCache |         5 |     366.1 us |      6.058 us |     12.099 us |     364.1 us |    2 |
| NewtonsoftValidatingReader |         5 |   6,093.7 us |     95.094 us |    189.912 us |   6,036.4 us |   11 |
|                ManateeJson |         5 |  77,671.7 us |    924.153 us |  1,845.633 us |  77,511.0 us |   22 |
|          NewtonsoftNoCache |        10 |  23,588.2 us |    405.951 us |    801.307 us |  23,631.0 us |   17 |
|        NewtonsoftWithCache |        10 |  12,032.1 us |    192.579 us |    371.034 us |  11,928.2 us |   14 |
|              MPJsonNoCache |        10 |   2,447.9 us |     18.537 us |     33.426 us |   2,448.8 us |    7 |
|            MPJsonWithCache |        10 |     563.7 us |      3.625 us |      7.239 us |     565.1 us |    3 |
| NewtonsoftValidatingReader |        10 |  11,547.0 us |    458.674 us |    926.544 us |  11,302.7 us |   13 |
|                ManateeJson |        10 | 142,042.6 us |  4,394.123 us |  8,360.275 us | 141,145.8 us |   23 |
|          NewtonsoftNoCache |        25 |  42,801.2 us |  1,331.089 us |  2,596.186 us |  42,406.8 us |   21 |
|        NewtonsoftWithCache |        25 |  30,304.8 us |    376.073 us |    724.566 us |  30,030.5 us |   19 |
|              MPJsonNoCache |        25 |   3,037.4 us |     11.700 us |     22.261 us |   3,032.7 us |    8 |
|            MPJsonWithCache |        25 |   1,153.8 us |      9.272 us |     16.953 us |   1,155.6 us |    4 |
| NewtonsoftValidatingReader |        25 |  25,147.3 us |    687.074 us |  1,340.085 us |  24,730.8 us |   18 |
|                ManateeJson |        25 | 334,032.8 us | 21,595.859 us | 40,029.309 us | 324,807.0 us |   24 |

In this benchmark, we are seeing a factor of 15-20X improvement in validation times with MP.Json.Validation against Newtonsoft.Json.Schema and hundreds of times more against ManateeJson. 

MP.Json.Validation claims the top eight spots. With a larger batch size of 25 and without caching of schemas, it still outperforms validation by competing products with just a batch size of 1 with caching.

## License

Apache header:

    Copyright 2020 mParticle LLC

    Licensed under the Apache License, Version 2.0 (the "License");
    you may not use this file except in compliance with the License.
    You may obtain a copy of the License at

        https://www.apache.org/licenses/LICENSE-2.0

    Unless required by applicable law or agreed to in writing, software
    distributed under the License is distributed on an "AS IS" BASIS,
    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
    See the License for the specific language governing permissions and
    limitations under the License.

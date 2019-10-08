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

using System.Runtime.CompilerServices;

namespace MP.Json.Validation
{
    internal static class BitUtils
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long RemoveLowestBit(long x) => (x & x - 1);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AtMostOneBit(long x) => (x & x - 1) == 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe int Log2(long value)
        {
            double f = unchecked((ulong)value) + .5; // +.5 -> -1 for zero
            return (((int*)&f)[1] >> 20) - 1023;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe int IndexOfLowestBit(long value) => Log2(value & -value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int BitCount(long y)
        {
            ulong x = (ulong)y;
            x -= (x >> 1) & 0x5555555555555555;
            x = (x & 0x3333333333333333) + ((x >> 2) & 0x3333333333333333);
            x = (x + (x >> 4)) & 0x0f0f0f0f0f0f0f0f;
            return (int)((x * 0x0101010101010101) >> 56);
        }
    }
}

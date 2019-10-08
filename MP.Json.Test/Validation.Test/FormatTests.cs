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
using System.Linq;
using System.Text;
using Xunit;
using static MP.Json.Validation.Formats;

namespace MP.Json.Validation.Test
{
    public class FormatTests
    {
        [Fact]
        public void IsDigitTest()
        {
            Assert.Equal(0, Digit('0'));
            Assert.Equal(9, Digit('9'));
            Assert.Equal(-1, Digit(' '));
            Assert.Equal(-1, Digit('A'));

            Assert.True(IsDigit('0'));
            Assert.True(IsDigit('9'));
            Assert.False(IsDigit((char)('9'+1)));
            Assert.False(IsDigit((char)('0'-1)));
            Assert.Equal(10, Enumerable.Range(0, 128).Count(x => IsDigit((char)x)));
        }

        [Fact] 
        public void IsLetterTest()
        {
            for (int i = 'A'; i <= 'Z'; i++)
                Assert.True(IsLetter((char)i));

            for (int i = 'a'; i <= 'z'; i++)
                Assert.True(IsLetter((char)i));

            Assert.Equal(52, Enumerable.Range(0, 128).Count(x=>IsLetter((char)x)));
        }

        [Fact]
        public void IsDateTest()
        {
            Assert.False(IsDate(""));
            Assert.True(IsDate("2019-12-17"));
            Assert.False(IsDate("2019-12-17T11:11:11"));
            Assert.True(IsDate("2019-12-17T11:11:11",0,10));
            Assert.False(IsDate("11:11:11"));

            Assert.False(IsDate("2019-01-32"));
            Assert.False(IsDate("2019-00-01"));
            Assert.False(IsDate("2019-1-2"));
            Assert.False(IsDate("2019-02-30"));
            Assert.True(IsDate("2019-03-31"));
            Assert.False(IsDate("2019-04-31"));
            Assert.True(IsDate("2019-05-31"));
            Assert.False(IsDate("2019-06-31"));
            Assert.True(IsDate("2019-07-31"));
            Assert.True(IsDate("2019-08-31"));
            Assert.False(IsDate("2019-09-31"));
            Assert.True(IsDate("2019-10-31"));
            Assert.False(IsDate("2019-11-31"));
            Assert.True(IsDate("2019-12-31"));
            Assert.False(IsDate("2019-13-01"));
        }

        [Fact]
        public void IsNumberTest()
        {
            Assert.False(IsNumber("E"));
            Assert.True(IsNumber("-1.234e+05"));
            Assert.True(IsNumber("0.234E-7"));
            Assert.True(IsNumber("12e1"));
            Assert.False(IsNumber("8e"));
            Assert.True(IsNumber("1.0"));
        }

        [Fact]
        public void IsBooleanTest()
        {
            Assert.True(IsBoolean("FaLsE"));
            Assert.True(IsBoolean("True"));
            Assert.True(IsBoolean("False"));
            Assert.True(IsBoolean("true"));
            Assert.True(IsBoolean("false"));
            Assert.True(IsBoolean("TRUE"));
            Assert.True(IsBoolean("FALSE"));
            Assert.False(IsBoolean("True "));
            Assert.False(IsBoolean(" True"));
            Assert.False(IsBoolean("TTTT"));
            Assert.False(IsBoolean("f"));
            Assert.False(IsBoolean(""));
        }

        [Fact]
        public void IsTimeTest()
        {
            Assert.True(IsTime("01:01:01"));
            Assert.False(IsTime("01:01"));
            Assert.True(IsTime("01:00:00"));
            Assert.True(IsTime("00:00:00"));
            Assert.True(IsTime("23:59:60"));
            Assert.False(IsTime("23:59:61"));
            Assert.False(IsTime("23:60:59"));
            Assert.False(IsTime("24:59:59"));
            Assert.True(IsTime("23:59:59.1234"));
            Assert.False(IsTime("23:59:59."));
            Assert.True(IsTime("23:59:59Z"));
            Assert.True(IsTime("23:59:59z"));
            Assert.False(IsTime("23:59:59Z "));
            Assert.False(IsTime("23:59:59 "));
            Assert.True(IsTime("23:59:59-00:00"));
            Assert.True(IsTime("23:59:59-23:59"));
            Assert.True(IsTime("23:59:59+23:59"));
            Assert.True(IsTime("00:00:00+00:00"));
            Assert.False(IsTime("00:00:00+00:00 "));
        }

        [Fact]
        public void IsDateTimeTest()
        {
            Assert.False(IsDateTime(""));
            Assert.False(IsDateTime("2019-12"));
            Assert.True(IsDateTime("2019-12-17"));
            Assert.True(IsDateTime("2019-12-17T11:11:11"));
            Assert.True(IsDateTime("2019-12-17t11:11:11"));
            Assert.True(IsDateTime("2019-12-17T11:11:11-00:00"));
            Assert.True(IsDateTime("2019-12-17T11:11:11+00:00"));
            Assert.True(IsDateTime("2019-12-17T11:11:11Z"));
            
            Assert.False(IsDateTime("2019-12-17 "));
            Assert.False(IsDateTime("2019-12-17T "));
            Assert.False(IsDateTime("2019-12-17T11:11:11 "));
            Assert.False(IsDateTime("2019-12-17T11:11:11Z "));
            Assert.False(IsDateTime("2019-12-17T11:11:11+00:00 "));
            
            Assert.False(IsDateTime("11:11:11"));

            Assert.False(IsDateTime("2019-01-32"));
            Assert.False(IsDateTime("2019-00-01"));
            Assert.False(IsDateTime("2019-1-2"));
            Assert.False(IsDateTime("2019-02-30"));
            Assert.True(IsDateTime("2019-03-31"));
            Assert.False(IsDateTime("2019-04-31"));
            Assert.True(IsDateTime("2019-05-31"));
            Assert.False(IsDateTime("2019-06-31"));
            Assert.True(IsDateTime("2019-07-31"));
            Assert.True(IsDateTime("2019-08-31"));
            Assert.False(IsDateTime("2019-09-31"));
            Assert.True(IsDateTime("2019-10-31"));
            Assert.False(IsDateTime("2019-11-31"));
            Assert.True(IsDateTime("2019-12-31"));
            Assert.False(IsDateTime("2019-13-01"));
        }


    }
}

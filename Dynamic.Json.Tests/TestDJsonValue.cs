using System;
using Xunit;

namespace Dynamic.Json.Tests
{
    public class TestDJsonValue
    {
        [Fact]
        public void TestBool()
        {
            Assert.True((bool)DJson.Parse(@"{ ""value"": true }").value);
            Assert.False((bool)DJson.Parse(@"{ ""value"": false }").value);
            Assert.True((bool)DJson.Parse(@"{ ""value"": ""true"" }").value);
            Assert.False((bool)DJson.Parse(@"{ ""value"": ""false"" }").value);
        }

        [Fact]
        public void TestBoolNull()
        {
            var json1 = DJson.Parse(@"{ ""value"": false }");
            Assert.False((bool?)json1.value);

            var json2 = DJson.Parse(@"{ ""value"": null }");
            Assert.Null((bool?)json2.value);

            var json3 = DJson.Parse(@"{ }");
            Assert.Null((bool?)json3.value);
        }

        [Fact]
        public void TestSByte()
        {
            var json = DJson.Parse(@"{ ""value"": -128 }");
            Assert.Equal(-128, (sbyte)json.value);
        }

        [Fact]
        public void TestSByteNull()
        {
            var json1 = DJson.Parse(@"{ ""value"": -128 }");
            Assert.Equal((sbyte?)-128, (sbyte?)json1.value);

            var json2 = DJson.Parse(@"{ ""value"": null }");
            Assert.Null((sbyte?)json2.value);
        }

        [Fact]
        public void TestByte()
        {
            var json = DJson.Parse(@"{ ""value"": 255 }");
            Assert.Equal(255, (byte)json.value);
        }

        [Fact]
        public void TestInt16()
        {
            var json = DJson.Parse(@"{ ""value"": -32768 }");
            Assert.Equal(-32768, (short)json.value);
        }

        [Fact]
        public void TestUInt16()
        {
            var json = DJson.Parse(@"{ ""value"": 65534 }");
            Assert.Equal(65534, (ushort)json.value);
        }

        [Fact]
        public void TestInt32()
        {
            var json = DJson.Parse(@"{ ""value"": -2147483648 }");
            Assert.Equal(-2147483648, (int)json.value);
        }

        [Fact]
        public void TestUInt32()
        {
            var json = DJson.Parse(@"{ ""value"": 4294967295 }");
            Assert.Equal(4294967295U, (uint)json.value);
        }

        [Fact]
        public void TestInt64()
        {
            var json = DJson.Parse(@"{ ""value"": -9223372036854775808 }");
            Assert.Equal(-9223372036854775808L, (long)json.value);
        }

        [Fact]
        public void TestUInt64()
        {
            var json = DJson.Parse(@"{ ""value"": 18446744073709551615 }");
            Assert.Equal(18446744073709551615UL, (ulong)json.value);
        }

        [Fact]
        public void TestFloat()
        {
            var json = DJson.Parse(@"{ ""value"": 3.14 }");
            Assert.Equal(3.14F, (float)json.value);
        }

        [Fact]
        public void TestDouble()
        {
            var json = DJson.Parse(@"{ ""value"": 3.1415926535 }");
            Assert.Equal(3.1415926535, (double)json.value);
        }

        [Fact]
        public void TestDecimal()
        {
            var json = DJson.Parse(@"{ ""value"": 3.14159265358979323846264338327950288419716939937510582097494459230781640628620899 }");
            Assert.Equal(3.14159265358979323846264338327950288419716939937510582097494459230781640628620899M, (decimal)json.value);
        }

        [Fact]
        public void TestGuid()
        {
            var json = DJson.Parse(@"{ ""value"": ""6F9619FF-8B86-D011-B42D-00CF4FC964FF"" }");
            Assert.Equal(new Guid("6F9619FF-8B86-D011-B42D-00CF4FC964FF"), (Guid)json.value);
        }

        [Fact]
        public void TestDateTime()
        {
            var json = DJson.Parse(@"{ ""value"": ""2020-09-15T13:20:32.207Z"" }");
            Assert.Equal(new DateTime(2020, 9, 15, 13, 20, 32, 207, DateTimeKind.Unspecified), (DateTime)json.value);
        }

        [Fact]
        public void TestDateTimeOffset()
        {
            var json = DJson.Parse(@"{ ""value"": ""2020-09-15T13:20:32.20723Z"" }");
            Assert.Equal(DateTimeOffset.Parse("2020-09-15T13:20:32.20723Z"), (DateTimeOffset)json.value);
        }

        [Fact]
        public void TestString()
        {
            var json = DJson.Parse(@"{ ""value"": ""qwerty123"" }");
            Assert.Equal("qwerty123", (string)json.value);
        }
    }
}

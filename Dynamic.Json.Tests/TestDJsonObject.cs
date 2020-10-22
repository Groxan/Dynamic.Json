using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Dynamic.Json.Tests
{
    public class TestDJsonObject
    {
        [Fact]
        public void TestProp()
        {
            Assert.Equal(0, DJson.Parse(@"{ ""a"": 0, ""b"": 1, ""c"": 2 }").a);
            Assert.Equal(1, DJson.Parse(@"{ ""a"": 0, ""b"": 1, ""c"": 2 }").b);
            Assert.Equal(2, DJson.Parse(@"{ ""a"": { ""b"": 2 } }").a.b);
        }

        [Fact]
        public void TestIndex()
        {
            Assert.Equal(0, DJson.Parse(@"{ ""a"": 0, ""b"": 1, ""c"": 2 }")["a"]);
            Assert.Equal(1, DJson.Parse(@"{ ""a"": 0, ""b"": 1, ""c"": 2 }")["b"]);
            Assert.Equal(2, DJson.Parse(@"{ ""a"": { ""b"": 2 } }")["a"]["b"]);
        }

        [Fact]
        public void TestEnumerator()
        {
            var i = 0;
            var keys = new List<string> { "a", "b", "c" };
            foreach (var kv in DJson.Parse(@"{ ""a"": 0, ""b"": 1, ""c"": 2 }"))
            {
                Assert.Equal(keys[i], kv.Name);
                Assert.Equal(i++, kv.Value);
            }
        }

        [Fact]
        public void TestEnumerable()
        {
            var kvs = (IEnumerable<dynamic>)DJson.Parse(@"{ ""a"": { ""value"": 1 }, ""b"": { ""value"": 2 }, ""c"": { ""value"": 3 } }");

            Assert.Equal(3, kvs.Count());
            Assert.Equal(3, kvs.First(x => x.Name == "c").Value.Value);
            Assert.Equal(2, kvs.First(x => x.Value.Value == 2).Value.Value);
        }

        [Fact]
        public void TestConvert()
        {
            var obj = (TestClass)DJson.Parse(@"{ ""field"": ""qwerty"" }");
            Assert.Equal("qwerty", obj.field);
        }

        [Fact]
        public void TestImplicit()
        {
            var json = DJson.Parse(@"{ ""a"": { ""value"": 1 }, ""b"": { ""value"": 2 } }");
            Assert.Equal(@"{ ""value"": 1 }", (string)json.a);
        }

        class TestClass
        {
            public string field { get; set; }
        }
    }
}

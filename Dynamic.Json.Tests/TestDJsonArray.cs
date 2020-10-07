using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Dynamic.Json.Tests
{
    public class TestDJsonArray
    {
        [Fact]
        public void TestCount()
        {
            Assert.Equal(3, DJson.Parse(@"[ 1, 2, 3 ]").Count);
            Assert.Equal(3, DJson.Parse(@"[ 1, 2, 3 ]").Length);
        }

        [Fact]
        public void TestIndex()
        {
            Assert.Equal(1, DJson.Parse(@"[ 1, 2, 3 ]")[0]);
            Assert.Equal(2, DJson.Parse(@"[ 1, 2, 3 ]")[1]);
            Assert.Equal(3, DJson.Parse(@"[ 1, 2, 3 ]")[2]);
        }

        [Fact]
        public void TestEnumerator()
        {
            int value = 1;
            foreach (var item in DJson.Parse(@"[ 1, 2, 3 ]"))
                Assert.Equal(value++, item);
        }

        [Fact]
        public void TestEnumerable()
        {
            var array = (IEnumerable<dynamic>)DJson.Parse(@"[ { ""value"": 1 }, { ""value"": 2 }, { ""value"": 3 } ]");

            Assert.Equal(3, array.Count());
            Assert.Equal(2, array.First(x => x.Value == 2).Value);
        }

        [Fact]
        public void TestConvert()
        {
            var list = (List<int>)DJson.Parse(@"{ ""value"": [ 1, 2, 3, 4 ] }").Value;
            Assert.Equal(new List<int> { 1, 2, 3, 4 }, list);
        }
    }
}

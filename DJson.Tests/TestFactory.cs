using System.Threading.Tasks;
using Xunit;

namespace DJson.Tests
{
    public class TestFactory
    {
        [Fact]
        public void TestParse()
        {
            var str = @"{ ""value"": 1 }";
            Assert.Equal(1, DJson.Parse(str).value);
        }

        [Fact]
        public void TestRead()
        {
            var json = DJson.Read("sample.json");
            Assert.Equal(1, json.value);
        }

        [Fact]
        public async Task TestReadAsync()
        {
            var json = await DJson.ReadAsync("sample.json");
            Assert.Equal(1, json.value);
        }

        //[Fact]
        //public async Task TestGetAsync()
        //{
        //    var json1 = await DJson.GetAsync("https://");
        //    Assert.Equal();

        //    var json2 = await DJson.GetAsync(new Uri("https://"));
        //    Assert.Equal();
        //}
    }
}

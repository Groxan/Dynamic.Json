using Xunit;

namespace Dynamic.Json.Tests
{
    public class TestNaming
    {
        [Fact]
        public void TestReservedWords()
        {
            var json = DJson.Parse(@"{ ""int"": 123, ""string"": ""qwerty"" }");

            Assert.Equal(123, json.@int);
            Assert.Equal("qwerty", json.@string);
        }

        [Fact]
        public void TestCamelCase()
        {
            var json = DJson.Parse(@"{ ""camel"": true, ""camelCase"": true, ""ipEndpoint"": true, ""userId"": true }");

            Assert.True(json.camel);
            Assert.True(json.Camel);

            Assert.True(json.camelCase);
            Assert.True(json.CamelCase);

            Assert.True(json.ipEndpoint);
            Assert.True(json.IpEndpoint);
            Assert.True(json.IPEndpoint);

            Assert.True(json.userId);
            Assert.True(json.UserId);
            Assert.True(json.UserID);
        }

        [Fact]
        public void TestSnakeCase()
        {
            var json = DJson.Parse(@"{ ""snake"": true, ""snake_case"": true, ""ip_endpoint"": true, ""user_id"": true }");

            Assert.True(json.snake);
            Assert.True(json.Snake);

            Assert.True(json.snake_case);
            Assert.True(json.SnakeCase);

            Assert.True(json.ip_endpoint);
            Assert.True(json.IpEndpoint);
            Assert.True(json.IPEndpoint);

            Assert.True(json.user_id);
            Assert.True(json.UserId);
            Assert.True(json.UserID);
        }
    }
}

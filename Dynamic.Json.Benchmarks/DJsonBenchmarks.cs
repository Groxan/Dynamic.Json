using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using BenchmarkDotNet.Attributes;
using Newtonsoft.Json.Linq;

namespace Dynamic.Json.Benchmark
{
    [MemoryDiagnoser]
    public class DJsonBenchmarks
    {
        public class HeaderObject
        {
            public long level { get; set; }
            public int proto { get; set; }
            public string predecessor { get; set; }
            public DateTime timestamp { get; set; }
            public int validation_pass { get; set; }
            public string operations_hash { get; set; }
            public List<string> fitness { get; set; }
            public string context { get; set; }
            public int priority { get; set; }
            public string proof_of_work_nonce { get; set; }
            public string signature { get; set; }
        }

        string Text;

        [GlobalSetup]
        public void GlobalSetup()
        {
            Text = @"{""header"":{""level"":1140075,""proto"":6,""predecessor"":""BLJY9TmU8w7TCeG1AVYS2KpmDzonQCDkUQb4XM4BZqKr7Nzdvk8"",""timestamp"":""2020-09-22T23:36:12Z"",""validation_pass"":4,""operations_hash"":""LLoZLw5K6RMohc6MgFdb2uwf6JYvUBDEFh5tyfJs1efZqFpcnMdGK"",""fitness"":[""01"",""000000000007656b""],""context"":""CoV8c4nnV48gKZPLdUGZjVAnMrumEUumoXL66mq3uYLoPsXkv1N5"",""priority"":0,""proof_of_work_nonce"":""e69b63f1bae50100"",""signature"":""sigbtXFWfGbKebPT7jEWKq8LMGEQvnoJFYkFhH6qyoLbrvsfPYBZ2MbmU7xU4tC5aLGHSGy5aCdjJhRveBWDvZuad6DpVC9f""}}";
        }

        [Benchmark(Baseline = true)]
        public HeaderObject SystemTextJsonExtract()
        {
            var json = JsonDocument.Parse(Text).RootElement;
            return new HeaderObject
            {
                level = json.GetProperty("header").GetProperty("level").GetInt64(),
                proto = json.GetProperty("header").GetProperty("proto").GetInt32(),
                predecessor = json.GetProperty("header").GetProperty("predecessor").GetString(),
                timestamp = json.GetProperty("header").GetProperty("timestamp").GetDateTime(),
                validation_pass = json.GetProperty("header").GetProperty("validation_pass").GetInt32(),
                operations_hash = json.GetProperty("header").GetProperty("operations_hash").GetString(),
                fitness = new List<string>
                {
                    json.GetProperty("header").GetProperty("fitness").EnumerateArray().First().GetString(),
                    json.GetProperty("header").GetProperty("fitness").EnumerateArray().Skip(1).First().GetString()
                },
                context = json.GetProperty("header").GetProperty("context").GetString(),
                priority = json.GetProperty("header").GetProperty("priority").GetInt32(),
                proof_of_work_nonce = json.GetProperty("header").GetProperty("proof_of_work_nonce").GetString(),
                signature = json.GetProperty("header").GetProperty("signature").GetString()
            };
        }

        [Benchmark]
        public HeaderObject DJsonExtract()
        {
            var json = DJson.Parse(Text);
            return new HeaderObject
            {
                level = json.header.level,
                proto = json.header.proto,
                predecessor = json.header.predecessor,
                timestamp = json.header.timestamp,
                validation_pass = json.header.validation_pass,
                operations_hash = json.header.operations_hash,
                fitness = new List<string> { json.header.fitness[0], json.header.fitness[1] },
                context = json.header.context,
                priority = json.header.priority,
                proof_of_work_nonce = json.header.proof_of_work_nonce,
                signature = json.header.signature
            };
        }

        [Benchmark]
        public HeaderObject NewtonsoftExtract()
        {
            var json = JToken.Parse(Text);
            return new HeaderObject
            {
                level = json["header"]["level"].Value<long>(),
                proto = json["header"]["proto"].Value<int>(),
                predecessor = json["header"]["predecessor"].Value<string>(),
                timestamp = json["header"]["timestamp"].Value<DateTime>(),
                validation_pass = json["header"]["validation_pass"].Value<int>(),
                operations_hash = json["header"]["operations_hash"].Value<string>(),
                fitness = new List<string> { json["header"]["fitness"][0].Value<string>(), json["header"]["fitness"][1].Value<string>() },
                context = json["header"]["context"].Value<string>(),
                priority = json["header"]["priority"].Value<int>(),
                proof_of_work_nonce = json["header"]["proof_of_work_nonce"].Value<string>(),
                signature = json["header"]["signature"].Value<string>()
            };
        }
    }
}

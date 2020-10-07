using BenchmarkDotNet.Running;

namespace Dynamic.Json.Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<DJsonBenchmarks>();
        }
    }

}

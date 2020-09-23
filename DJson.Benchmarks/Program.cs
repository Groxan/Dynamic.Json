using BenchmarkDotNet.Running;

namespace DJson.Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<DJsonBenchmarks>();
        }
    }

}

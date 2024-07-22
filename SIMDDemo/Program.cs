using BenchmarkDotNet.Running;

namespace SIMDDemo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<PerfTest>();
            Console.ReadLine();
        }
    }
}

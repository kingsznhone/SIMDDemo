using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace SIMDDemo
{
    [SimpleJob(RuntimeMoniker.Net80)]
    [MemoryDiagnoser]
    public class PerfTest
    {
        Calculator calculator;

        Random random;
        public double[] DoubleData = new double[1048576];
        public long[] LongData = new long[1048576];
        public float[] FloatData = new float[1048576];
        public int[] IntegerData = new int[1048576];
        public short[] ShortData = new short[1048576];
        public byte[] ByteData = new byte[1048576];

        public Half[] HalfData = new Half[1048576];
        public PerfTest()
        {
            calculator = new Calculator();
            random = new Random();
        }

        [GlobalSetup]
        public void init()
        {
            for (int i = 0; i < IntegerData.Length; i++)
            {
                IntegerData[i] = random.Next(100);
                ShortData[i] =(short) random.Next(ushort.MaxValue/16);
                HalfData[i] =(Half) random.NextSingle();
                FloatData[i] = random.NextSingle();
                DoubleData[i] = random.NextDouble();
                LongData[i] = random.NextInt64(100);
                ByteData[i] = (byte)random.Next(byte.MaxValue/16);
            }
        }

        [Benchmark(Baseline = true)]
        public double Compute()
        {
            return calculator.SumArray(ByteData.AsSpan());
        }
        [Benchmark]
        public double Compute_SIMD()
        {
            return calculator.SumArray_SIMD(ByteData.AsSpan());
        }
    }
}

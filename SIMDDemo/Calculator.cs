using System.Numerics;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;

namespace SIMDDemo
{
    public class Calculator
    {



        public Calculator()
        {

        }

        //sum of integer array
        public T SumArray<T>(Span<T> data) where T : INumber<T>
        {
            T sum = T.Zero;
            for (int i = 0; i < data.Length; i++)
            {
                sum += data[i];
            }
            return sum;
        }

        public T SumArray_SIMD<T>(Span<T> data) where T : INumber<T>
        {
            T sum = T.Zero;
            int vectorSize = Vector256<T>.Count;
            Vector256<T> v_sum = Vector256<T>.Zero;
            ref T ref_data = ref MemoryMarshal.GetReference(data);
            int SIMDLength = (data.Length - vectorSize);
            int Offset = 0;
            for (Offset = 0; Offset <= SIMDLength; Offset += vectorSize)
            {
                var vc = Vector256.LoadUnsafe(ref ref_data, (nuint)Offset);
                v_sum += vc;
            }
            sum = Vector256.Sum(v_sum);
            for (; Offset < data.Length; Offset++)
            {
                sum += data[Offset];
            }
            return sum;
        }

        
    }
}

using SIMDDemo;

namespace Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Test()
        {
            Random random = new Random();
            int[] array = new int[974];

            // 填充数组
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = random.Next(99); // 生成0到100之间的随机数
            }

            var calculator = new Calculator();

            // Act
            int result = calculator.SumArray<int>(array);
            // Assert
            Assert.AreEqual(array.Sum(), result);
            result = calculator.SumArray_SIMD<int>(array);
            Assert.AreEqual(array.Sum(), result);
        }
    }
}
// SIMDBenchmark.cpp : 此文件包含 "main" 函数。程序执行将在此处开始并结束。
//

#include <iostream>
#include <chrono>
#include "immintrin.h"
#include <random>
float Sum(float* data, int length) {
    float sum = 0;
    for (int i = 0; i < length; i++) {
        sum += data[i];
    }
    return sum;
}

float Sum_SIMD(float* data, int length) {
    __m256 v = _mm256_set_ps(0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8);
    __m256 sum = _mm256_setzero_ps();
    for (int i = 0; i < length; i += 8) {
        sum = _mm256_add_ps(sum, _mm256_load_ps(data + i));
    }
    sum = _mm256_hadd_ps(sum, sum);
    sum = _mm256_hadd_ps(sum, sum);
    __m128 vlow = _mm256_castps256_ps128(sum);
    __m128 vhigh = _mm256_extractf128_ps(sum, 1);
    vlow = _mm_add_ps(vlow, vhigh);
    return vlow.m128_f32[0];
}

int main()
{
    const int length = 1024 * 1024 * 1024;

    float* data = new float[length];
    // 在 main 函数中的 alignas(32) float* data = new float[length]; 之前添加以下代码

    std::random_device rd;
    std::mt19937 gen(rd());
    std::uniform_real_distribution<float> dis(0.0, 1.0);

    for (int i = 0; i < length; i++) {
        data[i] = dis(gen);
    }

    auto start = std::chrono::high_resolution_clock::now();
    float sum = Sum(data, length);
    auto end = std::chrono::high_resolution_clock::now();
    std::chrono::duration<double, std::milli> duration = end - start;
    std::cout << "serial in: " << duration.count() << " milliseconds" << std::endl;

    start = std::chrono::high_resolution_clock::now();
    float sum_simd = Sum_SIMD(data, length);
    end = std::chrono::high_resolution_clock::now();
    duration = end - start;
    std::cout << "parallel in: " << duration.count() << " milliseconds" << std::endl;
    free(data);
    if (sum == sum_simd) {
        return 0;
    }
    else
    {
        return 1;
    }
}



// 代码生成时间: 2025-09-17 04:01:07
// HashValueCalculator.cs
// 这是一个使用C#和Blazor框架创建的哈希值计算工具。

using System;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Components;

namespace HashValueCalculatorApp
{
    public class HashValueCalculator
    {
        [Parameter]
        public string Input { get; set; }

        [Parameter]
        public EventCallback<string> OnHashChanged { get; set; }

        public string? HashValue { get; private set; }

        private void CalculateHash()
        {
            if (string.IsNullOrEmpty(Input))
            {
                HashValue = null; // 输入为空时不进行哈希计算
                return;
            }

            try
            {
                using (var sha256 = SHA256.Create())
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(Input);
                    byte[] hashBytes = sha256.ComputeHash(bytes);
                    HashValue = BitConverter.ToString(hashBytes).Replace('-', '').ToLowerInvariant();
                }
            }
            catch (Exception ex)
            {
                // 错误处理：记录异常信息，并设置哈希值为异常信息
                Console.WriteLine($"Error calculating hash: {ex.Message}");
                HashValue = $"Error: {ex.Message}";
            }
            finally
            {
                OnHashChanged.InvokeAsync(HashValue);
            }
        }

        public void OnInputTextChanged()
        {
            CalculateHash();
        }
    }
}

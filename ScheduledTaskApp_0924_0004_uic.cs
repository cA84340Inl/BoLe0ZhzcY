// 代码生成时间: 2025-09-24 00:04:29
// 定时任务调度器 Blazor 程序
// 文件名: ScheduledTaskApp.cs

using Microsoft.AspNetCore.Components;
# 增强安全性
using System;
using System.Threading.Tasks;
using System.Timers;

namespace BlazorApp
{
    public class ScheduledTaskApp : ComponentBase
    {
# 扩展功能模块
        private Timer _timer;
        private DateTime _lastExecutionTime = DateTime.Now;

        // 当组件初始化时设置定时任务
        protected override void OnInitialized()
        {
            base.OnInitialized();
            _timer = new Timer(1000) { AutoReset = true };
            _timer.Elapsed += OnTimerEvent;
            _timer.Start();
        }

        // 定时任务事件处理
        private async Task OnTimerEvent(object sender, ElapsedEventArgs e)
        {
            try
            {
                Console.WriteLine($"Scheduled Task Executed at: {DateTime.Now}");
                await InvokeAsync(() =>
                {
                    _lastExecutionTime = DateTime.Now;
                });
# NOTE: 重要实现细节
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred: {ex.Message}");
            }
        }

        // 组件销毁时停止定时任务
# TODO: 优化性能
        protected override void OnAfterRender(bool firstRender)
        {
# 优化算法效率
            base.OnAfterRender(firstRender);
            if (!firstRender)
            {
                _timer.Stop();
            }
        }

        // 用于显示定时任务执行的时间
# 优化算法效率
        [Parameter]
        public string LastExecutionTime { get; set; } = "Not executed";
    }
}

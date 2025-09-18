// 代码生成时间: 2025-09-18 22:22:00
// 文件压缩解压工具 - FileCompressorBlazorApp.cs
using System;
using System.IO;
using System.IO.Compression;
using Microsoft.AspNetCore.Components.Forms;

namespace FileCompressorBlazorApp
{
    public class FileCompressorService
    {
        // 解压缩文件
        public async Task ExtractFileAsync(string archivePath, string destinationFolder)
        {
            try
            {
                if (!File.Exists(archivePath))
                {
                    throw new FileNotFoundException("Archive file not found.", archivePath);
                }

                await using var fileStream = File.OpenRead(archivePath);
                await using var archive = new ZipArchive(fileStream);

                foreach (var entry in archive.Entries)
                {
                    if (string.IsNullOrEmpty(entry.Name)) continue;

                    var fullPath = Path.GetFullPath(Path.Combine(destinationFolder, entry.FullName));

                    // 确保目录存在
                    Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

                    if (!entry.FullName.EndsWith("/", StringComparison.OrdinalIgnoreCase))
                    {
                        // 如果不是目录，则写入文件
                        using var entryStream = entry.Open();
                        using var fileStream = File.Create(fullPath);
                        await entryStream.CopyToAsync(fileStream);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error extracting files.", ex);
            }
        }
    }

    public class FileCompressorServiceExtensions
    {
        // 文件上传组件辅助方法
        public static async Task<string> SaveFileAsync(this IBrowserFile file, string targetPath)
        {
            try
            {
                var filePath = Path.Combine(targetPath, file.Name);
                await using var fileStream = new FileStream(filePath, FileMode.Create);
                await file.OpenReadStream().CopyToAsync(fileStream);
                return filePath;
            }
            catch (Exception ex)
            {
                throw new Exception("Error saving file.", ex);
            }
        }
    }

    // Blazor组件
    public partial class FileCompressorComponent : ComponentBase
    {
        private InputFile inputArchiveFile { get; set; }
        private string destinationFolder { get; set; } = "./extractedFiles";
        private bool isProcessing { get; set; } = false;

        [Inject]
        private IJSRuntime JS { get; set; }

        private async Task HandleFileSelected(InputFileChangeEventArgs e)
        {
            if (e.FileCount == 0)
                return;

            var file = e.File;
            var archivePath = await file.SaveFileAsync("./uploads");

            isProcessing = true;
            await JS.InvokeVoidAsync("disableButton", "#extractButton");

            try
            {
                await new FileCompressorService().ExtractFileAsync(archivePath, destinationFolder);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                isProcessing = false;
                await JS.InvokeVoidAsync("enableButton", "#extractButton");
            }
        }
    }
}

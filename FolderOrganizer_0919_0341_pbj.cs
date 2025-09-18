// 代码生成时间: 2025-09-19 03:41:28
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace FolderOrganizer
{
    /// <summary>
    /// Class responsible for organizing folders based on specified criteria.
    /// </summary>
    public class FolderOrganizer
    {
        private readonly string _sourceFolder;
        private readonly string _destinationFolder;

        /// <summary>
        /// Initializes a new instance of FolderOrganizer with source and destination folders.
        /// </summary>
        /// <param name="sourceFolder">The source folder path.</param>
        /// <param name="destinationFolder">The destination folder path.</param>
        public FolderOrganizer(string sourceFolder, string destinationFolder)
        {
            _sourceFolder = sourceFolder;
            _destinationFolder = destinationFolder;
        }

        /// <summary>
        /// Organizes the files in the source folder according to the specified criteria.
        /// </summary>
        public void OrganizeFolders()
        {
            try
            {
                // Ensure the source and destination folders exist
                if (!Directory.Exists(_sourceFolder))
                {
                    throw new DirectoryNotFoundException($"Source folder not found: {_sourceFolder}");
                }

                if (!Directory.Exists(_destinationFolder))
                {
                    Directory.CreateDirectory(_destinationFolder);
                }

                // Get all files from the source folder
                var files = Directory.GetFiles(_sourceFolder).Select(Path.GetFileName).ToList();

                // Organize files into categories (e.g., by extension)
                var organizedFiles = files.GroupBy(file => Path.GetExtension(file))
                    .ToDictionary(group => group.Key, group => group.AsEnumerable());

                // Move files to the destination folder, creating subfolders for each category
                foreach (var pair in organizedFiles)
                {
                    var subfolder = Path.Combine(_destinationFolder, pair.Key.TrimStart('.'));
                    Directory.CreateDirectory(subfolder);
                    foreach (var file in pair.Value)
                    {
                        var sourceFilePath = Path.Combine(_sourceFolder, file);
                        var destinationFilePath = Path.Combine(subfolder, file);
                        File.Move(sourceFilePath, destinationFilePath);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var sourceFolder = @"C:\sourceFolder";
            var destinationFolder = @"C:\destinationFolder";
            var organizer = new FolderOrganizer(sourceFolder, destinationFolder);
            organizer.OrganizeFolders();
        }
    }
}
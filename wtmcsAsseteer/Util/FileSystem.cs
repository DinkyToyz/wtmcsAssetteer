using ColossalFramework.IO;
using System;
using System.IO;
using System.Text;

namespace WhatThe.Mods.CitiesSkylines.Asseteer
{
    /// <summary>
    /// File system helper.
    /// </summary>
    internal static class FileSystem
    {
        /// <summary>
        /// Gets the file path.
        /// </summary>
        /// <value>
        /// The file path.
        /// </value>
        public static string FilePath => Path.Combine(DataLocation.localApplicationData, "ModConfig");

        /// <summary>
        /// Cleans the name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>The clean name.</returns>
        public static string CleanName(string name)
        {
            StringBuilder clean = new StringBuilder();

            for (int i = 0; i < name.Length; i++)
            {
                if ((byte)name[i] < 32 || (byte)name[i] == 127 || (byte)name[i] == 255)
                {
                    clean.Append('#');
                }
                else
                {
                    switch (name[i])
                    {
                        case ':':
                            clean.Append(';');
                            break;

                        case '.':
                            clean.Append(',');
                            break;

                        case '/':
                        case '\\':
                            clean.Append('-');
                            break;

                        case '?':
                        case '*':
                            clean.Append('#');
                            break;

                        default:
                            clean.Append(name[i]);
                            break;
                    }
                }
            }

            return clean.ToString();
        }

        /// <summary>
        /// Check if file exists, with file name automatic.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>True if file exists.</returns>
        public static bool Exists(string fileName = null)
        {
            if (String.IsNullOrEmpty(fileName))
            {
                fileName = FilePathName(".tmp");
            }
            else if (fileName[0] == '.')
            {
                fileName = FilePathName(fileName);
            }

            return File.Exists(fileName);
        }

        /// <summary>
        /// Gets the name of the file.
        /// </summary>
        /// <param name="extension">The extension.</param>
        /// <returns>The name of the file.</returns>
        public static string FileName(string extension = "")
        {
            return Library.Name + extension;
        }

        /// <summary>
        /// Gets the complete path.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>The complete path.</returns>
        public static string FilePathName(string fileName = null)
        {
            if (String.IsNullOrEmpty(fileName))
            {
                fileName = FileName(".tmp");
            }
            else if (fileName[0] == '.')
            {
                fileName = FileName(fileName);
            }

            return Path.GetFullPath(Path.Combine(FilePath, fileName));
        }

        /// <summary>
        /// Writes the file.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="fileContent">Content of the file.</param>
        public static void WriteFile(string fileName, string fileContent)
        {
            fileName = FilePathName(fileName);

            string filePath = Path.GetDirectoryName(Path.GetFullPath(fileName));
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            File.WriteAllText(fileName, fileContent, Encoding.UTF8);
        }
    }
}
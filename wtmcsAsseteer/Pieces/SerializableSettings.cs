using System;
using System.IO;
using System.Xml.Serialization;

namespace WhatThe.Mods.CitiesSkylines.Asseteer
{
    /// <summary>
    /// Saveable configuration.
    /// </summary>
    [Serializable]
    public class SerializableSettings
    {
        /// <summary>
        /// The build.
        /// </summary>
        public string Build = Library.Build;

        /// <summary>
        /// Whether to create data report when level is loaded.
        /// </summary>
        public bool CreateDataReportOnLevelLoaded = false;

        /// <summary>
        /// Whether to create HTML report when level is loaded.
        /// </summary>
        public bool CreateHtmlReportOnLevelLoaded = false;

        /// <summary>
        /// The save count.
        /// </summary>
        public int SaveCount = 0;

        /// <summary>
        /// The version.
        /// </summary>
        public int Version = CurrentVersion;

        /// <summary>
        /// The XML root.
        /// </summary>
        [NonSerialized]
        private const string XmlRoot = "AsseteerSettings";

        /// <summary>
        /// The current version.
        /// </summary>
        [NonSerialized]
        private static readonly int CurrentVersion = 0;

        /// <summary>
        /// Loads this instance.
        /// </summary>
        /// <returns></returns>
        public static SerializableSettings Load()
        {
            return LoadSettings(FileSystem.FilePathName(".xml"));
        }

        /// <summary>
        /// Saves this instance.
        /// </summary>
        public void Save()
        {
            this.SaveCount++;

            SaveSettings(FileSystem.FilePathName(".xml"), this);
        }

        /// <summary>
        /// Loads settings from the specified file name.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>
        /// The settings.
        /// </returns>
        /// <exception cref="InvalidDataException">No data</exception>
        private static SerializableSettings LoadSettings(string fileName)
        {
            Log.Debug(typeof(SerializableSettings), "Load", "Begin");

            try
            {
                if (File.Exists(fileName))
                {
                    Log.Info(typeof(SerializableSettings), "Load", fileName);

                    using (FileStream file = File.OpenRead(fileName))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(SerializableSettings), XmlRoot);

                        SerializableSettings settings = serializer.Deserialize(file) as SerializableSettings;

                        if (settings == null)
                        {
                            throw new InvalidDataException("No data");
                        }

                        Log.Debug(typeof(SerializableSettings), "Load", "Loaded");

                        return settings;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(typeof(SerializableSettings), "Load", ex);
            }
            finally
            {
                Log.Debug(typeof(SerializableSettings), "Load", "End");
            }

            return new SerializableSettings();
        }

        /// <summary>
        /// Saves settings to the specified file name.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="settings">The settings.</param>
        private static void SaveSettings(string fileName, SerializableSettings settings)
        {
            Log.Debug(typeof(SerializableSettings), "Save", "Begin");

            try
            {
                string filePath = Path.GetDirectoryName(Path.GetFullPath(fileName));
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }

                if (File.Exists(fileName))
                {
                    try
                    {
                        File.Copy(fileName, fileName + ".bak", true);
                    }
                    catch (Exception ex)
                    {
                        Log.Error(typeof(SerializableSettings), "Save", ex, "Copy to .bak failed");
                    }
                }

                Log.Info(typeof(SerializableSettings), "Save", fileName);

                using (FileStream file = File.Create(fileName))
                {
                    XmlSerializer ser = new XmlSerializer(typeof(SerializableSettings), XmlRoot);
                    ser.Serialize(file, settings);
                    file.Flush();
                    file.Close();
                }

                return;
            }
            catch (Exception ex)
            {
                Log.Error(typeof(SerializableSettings), "Save", ex);

                return;
            }
            finally
            {
                Log.Debug(typeof(SerializableSettings), "Save", "End");
            }
        }
    }
}
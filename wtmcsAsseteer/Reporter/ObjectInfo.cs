using System;
using System.Text.RegularExpressions;

namespace WhatThe.Mods.CitiesSkylines.Asseteer.Reporter
{
    /// <summary>
    /// Hold info about an asset.
    /// </summary>
    internal class ObjectInfo
    {
        /// <summary>
        /// The steam link template.
        /// </summary>
        private const string SteamLinkTemplate = "http://steamcommunity.com/sharedfiles/filedetails/?id={0}";

        /// <summary>
        /// The title cleaning pattern.
        /// </summary>
        private static readonly Regex titleCleaningPattern = new Regex(@"^[A-Z][_A-Z]+?[A-Z]\[(?:\d+\.)?(.*)\](?::\d+)?$", RegexOptions.Compiled);

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectInfo"/> class.
        /// </summary>
        public ObjectInfo()
        {
            this.InitializeNames((string)null);
            this.Initialized = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectInfo"/> class.
        /// </summary>
        /// <param name="objectInfo">The object information.</param>
        public ObjectInfo(ObjectInfo objectInfo)
        {
            this.Initialized = objectInfo.Initialized;
            this.Title = objectInfo.Title;
            this.Category = objectInfo.Category;
            this.RawCategory = objectInfo.RawCategory;
            this.Name = objectInfo.Name;
            this.PrefabName = objectInfo.PrefabName;
            this.SteamId = objectInfo.SteamId;
            this.SourceType = objectInfo.SourceType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectInfo"/> class.
        /// </summary>
        /// <param name="prefab">The prefab.</param>
        public ObjectInfo(PrefabInfo prefab)
        {
            this.Initialized = this.InitializePrefab(prefab);
        }

        /// <summary>
        /// Asset source types.
        /// </summary>
        public enum SourceTypes
        {
            /// <summary>
            /// Unknown source.
            /// </summary>
            Unknown = 0,

            /// <summary>
            /// Built in or from DLC.
            /// </summary>
            BuiltIn,

            /// <summary>
            /// From workshop.
            /// </summary>
            Workshop,

            /// <summary>
            /// Private custom.
            /// </summary>
            Private
        }

        /// <summary>
        /// Gets a value indicating whether this thing is built in.
        /// </summary>
        /// <value>
        ///   <c>true</c> if built in; otherwise, <c>false</c>.
        /// </value>
        public bool BuiltIn => this.SourceType == SourceTypes.BuiltIn;

        /// <summary>
        /// Gets the category.
        /// </summary>
        /// <value>
        /// The category.
        /// </value>
        public string Category { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this thing is custom.
        /// </summary>
        /// <value>
        ///   <c>true</c> if custom; otherwise, <c>false</c>.
        /// </value>
        public bool Custom => this.SourceType == SourceTypes.Workshop || this.SourceType == SourceTypes.Private;

        /// <summary>
        /// Gets the escaped category.
        /// </summary>
        /// <value>
        /// The escaped category.
        /// </value>
        public string EscapedCategory => (this.Category == null) ? "" : this.Category.HtmlEscape();

        /// <summary>
        /// Gets the escaped name.
        /// </summary>
        /// <value>
        /// The escaped name.
        /// </value>
        public string EscapedName => (this.Name == null) ? this.EscapedPrefabName : this.Name.HtmlEscape();

        /// <summary>
        /// Gets the escaped prefab name.
        /// </summary>
        /// <value>
        /// The escaped prefab name.
        /// </value>
        public string EscapedPrefabName => (this.PrefabName == null) ? "" : this.PrefabName.HtmlEscape();

        /// <summary>
        /// Gets the escaped title.
        /// </summary>
        /// <value>
        /// The escaped title.
        /// </value>
        public string EscapedTitle => (this.Title == null) ? this.EscapedName : this.Title.HtmlEscape();

        /// <summary>
        /// Gets a value indicating whether this <see cref="AssetInfo"/> is initialized.
        /// </summary>
        /// <value>
        ///   <c>true</c> if initialized; otherwise, <c>false</c>.
        /// </value>
        public bool Initialized { get; private set; }

        /// <summary>
        /// Gets the name of the prefab.
        /// </summary>
        /// <value>
        /// The name of the prefab.
        /// </value>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the name of the prefab.
        /// </summary>
        /// <value>
        /// The name of the prefab.
        /// </value>
        public string PrefabName { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this thing is private.
        /// </summary>
        /// <value>
        ///   <c>true</c> if private; otherwise, <c>false</c>.
        /// </value>
        public bool Private => this.SourceType == SourceTypes.Private;

        /// <summary>
        /// Gets the raw category.
        /// </summary>
        /// <value>
        /// The raw category.
        /// </value>
        public string RawCategory { get; private set; }

        /// <summary>
        /// Gets the type of the source.
        /// </summary>
        /// <value>
        /// The type of the source.
        /// </value>
        public SourceTypes SourceType { get; private set; }

        /// <summary>
        /// Gets the steam identifier.
        /// </summary>
        /// <value>
        /// The steam identifier.
        /// </value>
        public uint SteamId { get; private set; }

        /// <summary>
        /// Gets the steam link.
        /// </summary>
        /// <value>
        /// The steam link.
        /// </value>
        public string SteamLink => string.Format(SteamLinkTemplate, this.SteamId);

        /// <summary>
        /// Gets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this thing is from workshop.
        /// </summary>
        /// <value>
        ///   <c>true</c> if from workshop; otherwise, <c>false</c>.
        /// </value>
        public bool Workshop => this.SourceType == SourceTypes.Workshop;

        /// <summary>
        /// Gets a value indicating whether name is required.
        /// </summary>
        /// <value>
        ///   <c>true</c> if requiring name; otherwise, <c>false</c>.
        /// </value>
        protected virtual bool RequireName => true;

        /// <summary>
        /// Fills the information.
        /// </summary>
        /// <param name="info">The information.</param>
        public virtual void FillInfo(Log.InfoList info)
        {
            if (this.SteamId > 0)
            {
                info.Add("SteamId", this.SteamId);
            }

            info.Add("PrefabName", this.PrefabName);
            info.Add("Title", this.Title);
            info.Add("Name", this.Name);
            info.Add("Category", this.Category);
            info.Add("SourceType", this.SourceType);
            info.Add("RawCategory", this.RawCategory);
        }

        /// <summary>
        /// Initializes the current instance with values from specified prefab.
        /// </summary>
        /// <param name="prefab">The prefab.</param>
        public void Initialize(PrefabInfo prefab)
        {
            this.Initialized = this.InitializePrefab(prefab);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return this.ToString(null);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <param name="prefix">The prefix.</param>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public virtual string ToString(string prefix)
        {
            Log.InfoList info = new Log.InfoList(prefix);
            this.FillInfo(info);
            return info.ToString();
        }

        /// <summary>
        /// Initializes the current instance.
        /// </summary>
        protected virtual void Clear()
        { }

        /// <summary>
        /// Initializes the references.
        /// </summary>
        /// <param name="prefab">The prefab.</param>
        /// <returns>True on success.</returns>
        protected virtual bool InitializeData(PrefabInfo prefab)
        {
            return true;
        }

        /// <summary>
        /// Initializes the current instance for failed initialization.
        /// </summary>
        protected virtual void InitializeFailed(PrefabInfo prefab)
        {
            if ((UnityEngine.Object)prefab != (UnityEngine.Object)null)
            {
                if (String.IsNullOrEmpty(this.PrefabName))
                {
                    this.PrefabName = "[" + prefab.ToString() + "]";
                }
            }

            if (String.IsNullOrEmpty(this.Title))
            {
                this.Title = this.PrefabName;
            }

            if (String.IsNullOrEmpty(this.Name))
            {
                this.Name = this.PrefabName;
            }

            if (String.IsNullOrEmpty(this.Category))
            {
                this.Category = "Unknown";
            }
        }

        /// <summary>
        /// Initializes the types.
        /// </summary>
        /// <param name="prefab">The prefab.</param>
        /// <returns>True on success.</returns>
        protected virtual bool InitializeTypes(PrefabInfo prefab)
        {
            return true;
        }

        /// <summary>
        /// Initializes the category.
        /// </summary>
        /// <param name="prefab">The prefab.</param>
        private bool InitializeCategory(PrefabInfo prefab)
        {
            string category = null;

            try
            {
                ItemClass.SubService subService = prefab.GetSubService();
                if (subService != ItemClass.SubService.None && subService != ItemClass.SubService.Unused2)
                {
                    category = subService.ToString();
                    if (category.Length < 6 || category.Substring(0, 6).ToLowerInvariant() != "unused")
                    {
                        this.RawCategory = "SubService:" + category;
                        this.Category = InfoHelper.CleanCategory(category);

                        return true;
                    }
                }
            }
            catch
            { }

            try
            {
                ItemClass.Service service = prefab.GetService();
                if (service != ItemClass.Service.None && service != ItemClass.Service.Unused2)
                {
                    category = service.ToString();
                    if (category.Length < 6 || category.Substring(0, 6).ToLowerInvariant() != "unused")
                    {
                        this.RawCategory = "Service:" + category;
                        this.Category = InfoHelper.CleanCategory(category);

                        return true;
                    }
                }
            }
            catch
            { }

            try
            {
                category = prefab.category;
                if (!String.IsNullOrEmpty(category))
                {
                    this.RawCategory = "Category:" + prefab.category;
                    this.Category = InfoHelper.CleanCategory(prefab.category);

                    return true;
                }
            }
            catch
            { }

            this.RawCategory = null;
            this.Category = "Unknown";

            return true;
        }

        /// <summary>
        /// Initializes the current instance names.
        /// </summary>
        /// <param name="objectName">Name of the object.</param>
        /// <returns></returns>
        private bool InitializeNames(string objectName)
        {
            try
            {
                this.Name = null;
                this.PrefabName = null;
                this.SteamId = 0;
                this.SourceType = SourceTypes.Unknown;

                if (objectName == null)
                {
                    return !this.RequireName;
                }

                this.Name = objectName;
                this.PrefabName = objectName;
                int p = objectName.IndexOf('.');

                if (p <= 0 || p >= objectName.Length - 1)
                {
                    this.SourceType = SourceTypes.BuiltIn;

                    return true;
                }

                uint id;
                if (uint.TryParse(objectName.Substring(0, p), out id) && id > 9999999)
                {
                    this.SteamId = id;
                    this.SourceType = SourceTypes.Workshop;
                    this.Name = objectName.Substring(p + 1);
                }
                else
                {
                    this.SourceType = SourceTypes.Private;
                }

                return true;
            }
            catch (Exception ex)
            {
                if (Log.LogALot)
                {
                    Log.Error(this, "InitializeNames", ex);
                }

                return false;
            }
        }

        /// <summary>
        /// Initializes the current instance with values from specified prefab.
        /// </summary>
        /// <param name="prefab">The prefab.</param>
        private bool InitializePrefab(PrefabInfo prefab)
        {
            try
            {
                this.Clear();

                if ((UnityEngine.Object)prefab == (UnityEngine.Object)null)
                {
                    this.InitializeNames((string)null);
                    this.InitializeFailed(prefab);

                    return false;
                }

                bool success = this.InitializeTypes(prefab);

                try
                {
                    if (!string.IsNullOrEmpty(prefab.name))
                    {
                        success = this.InitializeNames(prefab.name) & success;
                    }
                    else if (!String.IsNullOrEmpty(prefab.gameObject.name))
                    {
                        success = this.InitializeNames(prefab.gameObject.name) && success;
                    }
                    else
                    {
                        this.InitializeNames((string)null);
                        success = false;
                    }
                }
                catch
                {
                    this.InitializeNames((string)null);
                    success = false;
                }

                try
                {
                    this.Title = prefab.GetLocalizedTitle();
                    if (this.Title != null)
                    {
                        this.Title = titleCleaningPattern.Replace(this.Title, "$1");
                    }
                }
                catch
                {
                    this.Title = null;
                }

                success = this.InitializeCategory(prefab) && success;

                if (!success)
                {
                    this.InitializeFailed(prefab);

                    return false;
                }

                return this.InitializeData(prefab) && success;
            }
            catch (Exception ex)
            {
                if (Log.LogALot)
                {
                    Log.Error(this, "Initialize", ex);
                }

                return false;
            }
        }
    }
}
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
        /// Initializes a new instance of the <see cref="ObjectInfo"/> class.
        /// </summary>
        public ObjectInfo()
        {
            this.Initialized = false;
            this.Initialize((string)null);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectInfo"/> class.
        /// </summary>
        public ObjectInfo(string name)
        {
            this.Initialized = false;
            this.Initialize(name);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectInfo"/> class.
        /// </summary>
        /// <param name="prefab">The prefab.</param>
        public ObjectInfo(PrefabInfo prefab)
        {
            this.Initialized = false;
            this.Initialize(prefab);
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
        /// Gets a value indicating whether this thing is custom.
        /// </summary>
        /// <value>
        ///   <c>true</c> if custom; otherwise, <c>false</c>.
        /// </value>
        public bool Custom => this.SourceType == SourceTypes.Workshop || this.SourceType == SourceTypes.Private;

        /// <summary>
        /// Gets the escaped name.
        /// </summary>
        /// <value>
        /// The escaped name.
        /// </value>
        public string EscapedName => this.Name.HtmlEscape();

        /// <summary>
        /// Gets the escaped prefab name.
        /// </summary>
        /// <value>
        /// The escaped prefab name.
        /// </value>
        public string EscapedPrefabName => this.PrefabName.HtmlEscape();

        /// <summary>
        /// Gets a value indicating whether this <see cref="AssetInfo"/> is initialized.
        /// </summary>
        /// <value>
        ///   <c>true</c> if initialized; otherwise, <c>false</c>.
        /// </value>
        public bool Initialized { get; protected set; }

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
        /// Gets a value indicating whether this thing is from workshop.
        /// </summary>
        /// <value>
        ///   <c>true</c> if from workshop; otherwise, <c>false</c>.
        /// </value>
        public bool Workshop => this.SourceType == SourceTypes.Workshop;

        /// <summary>
        /// Initializes the current instance with values from specified prefab.
        /// </summary>
        /// <param name="objectName">Name of the object.</param>
        protected virtual void Initialize(string objectName)
        {
            this.Initialized = false;

            this.Name = null;
            this.PrefabName = null;
            this.SteamId = 0;
            this.SourceType = SourceTypes.Unknown;

            if (objectName == null)
            {
                return;
            }

            this.Initialized = true;

            this.PrefabName = objectName;
            this.Name = objectName;
            int p = objectName.IndexOf('.');

            if (p <= 0 || p >= objectName.Length - 1)
            {
                this.SourceType = SourceTypes.BuiltIn;
                return;
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
        }

        /// <summary>
        /// Initializes the current instance with values from specified prefab.
        /// </summary>
        /// <param name="prefab">The prefab.</param>
        protected virtual void Initialize(PrefabInfo prefab)
        {
            if ((UnityEngine.Object)prefab == (UnityEngine.Object)null)
            {
                this.Initialize((string)null);
                return;
            }

            this.Initialize(prefab.name);
            if (!this.Initialized)
            {
                this.PrefabName = "[" + prefab.ToString() + "]";
                this.Name = this.PrefabName;
            }
        }
    }
}
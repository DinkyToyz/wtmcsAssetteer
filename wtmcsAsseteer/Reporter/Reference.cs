namespace WhatThe.Mods.CitiesSkylines.Asseteer.Reporter
{
    /// <summary>
    /// Hold info about referenced object.
    /// </summary>
    internal class Reference : ObjectInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Reference" /> struct.
        /// </summary>
        /// <param name="referenceType">Type of the reference.</param>
        /// <param name="name">The name.</param>
        /// <param name="exists">if set to <c>true</c> [exists].</param>
        public Reference(ReferenceTypes referenceType, string name, bool exists) : base(name)
        {
            this.ReferenceType = referenceType;
            this.Exists = exists;
        }

        /// <summary>
        /// The type of reference.
        /// </summary>
        public enum ReferenceTypes
        {
            /// <summary>
            /// Unknown reference type.
            /// </summary>
            Unknown = 0,

            /// <summary>
            /// A prop.
            /// </summary>
            Prop,

            /// <summary>
            /// A tree.
            /// </summary>
            Tree,

            /// <summary>
            /// A variant version of the asset.
            /// </summary>
            Variation
        }

        /// <summary>
        /// Gets a value indicating whether the referenced object exists.
        /// </summary>
        /// <value>
        ///   <c>true</c> if exists; otherwise, <c>false</c>.
        /// </value>
        public bool Exists { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the referenced object is missing.
        /// </summary>
        /// <value>
        ///   <c>true</c> if missing; otherwise, <c>false</c>.
        /// </value>
        public bool Missing => !this.Exists;

        /// <summary>
        /// Gets the type of the reference.
        /// </summary>
        /// <value>
        /// The type of the reference.
        /// </value>
        public ReferenceTypes ReferenceType { get; private set; }
    }
}
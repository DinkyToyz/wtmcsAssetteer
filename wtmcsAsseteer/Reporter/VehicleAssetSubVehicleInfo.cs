namespace WhatThe.Mods.CitiesSkylines.Asseteer.Reporter
{
    /// <summary>
    /// Holds information about a vehicle asset sub-vehicle.
    /// </summary>
    /// <seealso cref="WhatThe.Mods.CitiesSkylines.Asseteer.Reporter.AssetInfo" />
    internal class VehicleAssetSubVehicleInfo : VehicleAssetInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VehicleAssetInfo"/> class.
        /// </summary>
        /// <param name="prefab">The prefab.</param>
        public VehicleAssetSubVehicleInfo(VehicleInfo prefab) : base(prefab)
        { }

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public override AssetTypes AssetType => AssetTypes.Unknown;

        /// <summary>
        /// Gets a value indicating whether name is required.
        /// </summary>
        /// <value>
        ///   <c>true</c> if requiring name; otherwise, <c>false</c>.
        /// </value>
        protected override bool RequireName => false;
    }
}
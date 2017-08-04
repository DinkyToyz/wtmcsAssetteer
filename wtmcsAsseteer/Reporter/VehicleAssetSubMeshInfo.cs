using UnityEngine;

namespace WhatThe.Mods.CitiesSkylines.Asseteer.Reporter
{
    /// <summary>
    /// Holds information about a vehicle asset sub-mesh.
    /// </summary>
    /// <seealso cref="WhatThe.Mods.CitiesSkylines.Asseteer.Reporter.AssetInfo" />
    internal class VehicleAssetSubMeshInfo : AssetInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VehicleAssetSubMeshInfo"/> class.
        /// </summary>
        /// <param name="prefab">The prefab.</param>
        public VehicleAssetSubMeshInfo(VehicleInfoSub prefab) : base(prefab)
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

        /// <summary>
        /// Gets the lod mesh.
        /// </summary>
        /// <param name="prefab">The prefab.</param>
        /// <returns>
        /// The lod mesh.
        /// </returns>
        protected override Mesh GetLodMesh(PrefabInfo prefab)
        {
            return GetMeshWithFallback(((VehicleInfoSub)prefab).m_lodObject, ((VehicleInfoSub)prefab).m_lodMesh);
        }

        /// <summary>
        /// Gets the lod texture.
        /// </summary>
        /// <param name="prefab">The prefab.</param>
        /// <returns>
        /// The lod texture.
        /// </returns>
        protected override Texture GetLodTexture(PrefabInfo prefab)
        {
            return GetTextureWithFallback(((VehicleInfoSub)prefab).m_lodObject, ((VehicleInfoSub)prefab).m_material);
        }

        /// <summary>
        /// Gets the mesh.
        /// </summary>
        /// <param name="prefab">The prefab.</param>
        /// <returns>
        /// The mesh.
        /// </returns>
        protected override Mesh GetMainMesh(PrefabInfo prefab)
        {
            return GetMeshWithFallback(prefab, ((VehicleInfoSub)prefab).m_mesh);
        }

        /// <summary>
        /// Gets the texture.
        /// </summary>
        /// <param name="prefab">The prefab.</param>
        /// <returns>
        /// The texture.
        /// </returns>
        protected override Texture GetMainTexture(PrefabInfo prefab)
        {
            return GetTextureWithFallback(prefab, ((VehicleInfoSub)prefab).m_material);
        }
    }
}
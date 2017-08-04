using System.Collections.Generic;
using UnityEngine;

namespace WhatThe.Mods.CitiesSkylines.Asseteer.Reporter
{
    /// <summary>
    /// Holds information about a vehicle asset.
    /// </summary>
    /// <seealso cref="WhatThe.Mods.CitiesSkylines.Asseteer.Reporter.AssetInfo" />
    internal class VehicleAssetInfo : AssetInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VehicleAssetInfo"/> class.
        /// </summary>
        public VehicleAssetInfo() : base()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="VehicleAssetInfo"/> class.
        /// </summary>
        /// <param name="prefab">The prefab.</param>
        public VehicleAssetInfo(VehicleInfo prefab) : base(prefab)
        { }

        /// <summary>
        /// Gets the collection.
        /// </summary>
        /// <value>
        /// The collection.
        /// </value>
        public static IEnumerable<AssetInfo> Collection => CollectPrefabs<VehicleInfo, VehicleAssetInfo>();

        /// <summary>
        /// Gets the prefab count.
        /// </summary>
        /// <value>
        /// The prefab count.
        /// </value>
        public static int PrefabCount => PrefabHelper.Count<VehicleInfo>();

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public override AssetTypes AssetType => AssetInfo.AssetTypes.Vehicle;

        /// <summary>
        /// Gets the lod mesh.
        /// </summary>
        /// <param name="prefab">The prefab.</param>
        /// <returns>
        /// The lod mesh.
        /// </returns>
        protected override Mesh GetLodMesh(PrefabInfo prefab)
        {
            return GetMeshWithFallback(((VehicleInfo)prefab).m_lodObject, ((VehicleInfo)prefab).m_lodMesh);
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
            return GetTextureWithFallback(((VehicleInfo)prefab).m_lodObject, ((VehicleInfo)prefab).m_lodMaterial);
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
            return GetMeshWithFallback(prefab, ((VehicleInfo)prefab).m_mesh);
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
            return GetTextureWithFallback(prefab, ((VehicleInfo)prefab).m_material);
        }

        /// <summary>
        /// Initializes the current instance with values from specified prefab.
        /// </summary>
        /// <param name="prefab">The prefab.</param>
        protected override bool InitializeData(PrefabInfo prefab)
        {
            bool success = base.InitializeData(prefab);

            VehicleInfo vehicle = (VehicleInfo)prefab;

            // todo: VehicleInfo.MeshInfo[] m_subMeshes;
            /*
            if (vehicle.m_subMeshes != null)
            {
                for (int i = 0; i < vehicle.m_subMeshes.Length; i++)
                {
                    if (vehicle.m_subMeshes[i] != null &&
                        (UnityEngine.Object)vehicle.m_subMeshes[i].m_subInfo != (UnityEngine.Object)null &&
                        vehicle.m_subMeshes[i].m_subInfo is VehicleInfoSub)
                    {
                        AssetInfo subAsset = new SubMeshInfo((VehicleInfoSub)vehicle.m_subMeshes[i].m_subInfo);
                        if (subAsset.Initialized)
                        {
                            this.Add(subAsset);
                        }
                    }
                }
            }
            */

            // todo: VehicleInfo.VehicleTrailer[] m_trailers;
            /*
            if (vehicle.m_trailers != null)
            {
                for (int i = 0; i < vehicle.m_trailers.Length; i++)
                {
                    if ((UnityEngine.Object)vehicle.m_trailers[i].m_info != (UnityEngine.Object)null)
                    {
                        this.AddReferencedAsset<VehicleInfo>(Reference.ReferenceTypes.SubAssset, vehicle.m_trailers[i].m_info);

                        AssetInfo subAsset = new SubVehicleInfo(vehicle.m_trailers[i].m_info);
                        if (subAsset.Initialized)
                        {
                            this.Add(subAsset);
                        }
                    }
                }
            }
            */

            return success;
        }

        /// <summary>
        /// Holds information about a vehicle asset sub-mesh.
        /// </summary>
        /// <seealso cref="WhatThe.Mods.CitiesSkylines.Asseteer.Reporter.AssetInfo" />
        private class SubMeshInfo : AssetInfo
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="SubMeshInfo"/> class.
            /// </summary>
            /// <param name="prefab">The prefab.</param>
            public SubMeshInfo(VehicleInfoSub prefab) : base(prefab)
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

            protected override Texture GetLodTexture(PrefabInfo prefab)
            {
                return GetTextureWithFallback(((VehicleInfoSub)prefab).m_lodObject, ((VehicleInfoSub)prefab).m_lodMaterial);
            }

            protected override Mesh GetMainMesh(PrefabInfo prefab)
            {
                return GetMeshWithFallback(prefab, ((VehicleInfoSub)prefab).m_mesh);
            }

            protected override Texture GetMainTexture(PrefabInfo prefab)
            {
                return GetTextureWithFallback(prefab, ((VehicleInfoSub)prefab).m_material);
            }
        }

        /// <summary>
        /// Holds information about a vehicle asset sub-vehicle.
        /// </summary>
        /// <seealso cref="WhatThe.Mods.CitiesSkylines.Asseteer.Reporter.AssetInfo" />
        private class SubVehicleInfo : VehicleAssetInfo
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="SubVehicleInfo"/> class.
            /// </summary>
            /// <param name="prefab">The prefab.</param>
            public SubVehicleInfo(VehicleInfo prefab) : base(prefab)
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
}
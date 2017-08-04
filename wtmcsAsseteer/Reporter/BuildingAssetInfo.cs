using System.Collections.Generic;
using UnityEngine;

namespace WhatThe.Mods.CitiesSkylines.Asseteer.Reporter
{
    /// <summary>
    /// Holds information about a building asset.
    /// </summary>
    /// <seealso cref="WhatThe.Mods.CitiesSkylines.Asseteer.Reporter.AssetInfo" />
    internal class BuildingAssetInfo : AssetInfo
    {
        /// <summary>
        /// The type.
        /// </summary>
        private AssetTypes type = AssetTypes.Unknown;

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildingAssetInfo"/> class.
        /// </summary>
        public BuildingAssetInfo() : base()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildingAssetInfo"/> class.
        /// </summary>
        /// <param name="prefab">The prefab.</param>
        public BuildingAssetInfo(BuildingInfo prefab) : base(prefab)
        { }

        /// <summary>
        /// Gets the collection.
        /// </summary>
        /// <value>
        /// The collection.
        /// </value>
        public static IEnumerable<AssetInfo> Collection => CollectPrefabs<BuildingInfo, BuildingAssetInfo>();

        /// <summary>
        /// Gets the prefab count.
        /// </summary>
        /// <value>
        /// The prefab count.
        /// </value>
        public static int PrefabCount => PrefabHelper.Count<BuildingInfo>();

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public override AssetTypes AssetType => this.type;

        /// <summary>
        /// Gets the lod mesh.
        /// </summary>
        /// <param name="prefab">The prefab.</param>
        /// <returns>
        /// The lod mesh.
        /// </returns>
        protected override Mesh GetLodMesh(PrefabInfo prefab)
        {
            return GetMeshWithFallback(((BuildingInfo)prefab).m_lodObject, ((BuildingInfo)prefab).m_lodMesh);
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
            return GetTextureWithFallback(((BuildingInfo)prefab).m_lodObject, ((BuildingInfo)prefab).m_lodMaterial);
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
            try
            {
                if ((UnityEngine.Object)((BuildingInfo)prefab).m_mesh != (UnityEngine.Object)null)
                {
                    return ((BuildingInfo)prefab).m_mesh;
                }

                if ((UnityEngine.Object)((BuildingInfo)prefab).m_overrideMainMesh != (UnityEngine.Object)null)
                {
                    return ((BuildingInfo)prefab).m_overrideMainMesh;
                }

                MeshFilter filter = prefab.GetComponent<MeshFilter>();
                if ((UnityEngine.Object)filter != (UnityEngine.Object)null &&
                    (UnityEngine.Object)filter.sharedMesh != (UnityEngine.Object)null)
                {
                    return filter.sharedMesh;
                }

                return null;
            }
            catch
            {
                return null;
            }
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
            try
            {
                if ((UnityEngine.Object)((BuildingInfo)prefab).m_material != (UnityEngine.Object)null &&
                    (UnityEngine.Object)((BuildingInfo)prefab).m_material.mainTexture != (UnityEngine.Object)null)
                {
                    return ((BuildingInfo)prefab).m_material.mainTexture;
                }

                if ((UnityEngine.Object)((BuildingInfo)prefab).m_overrideMainRenderer != (UnityEngine.Object)null &&
                    (UnityEngine.Object)((BuildingInfo)prefab).m_overrideMainRenderer.sharedMaterial != (UnityEngine.Object)null &&
                    (UnityEngine.Object)((BuildingInfo)prefab).m_overrideMainRenderer.sharedMaterial.mainTexture != (UnityEngine.Object)null)
                {
                    return ((BuildingInfo)prefab).m_overrideMainRenderer.sharedMaterial.mainTexture;
                }

                Renderer renderer = prefab.GetComponent<Renderer>();
                if ((UnityEngine.Object)renderer != (UnityEngine.Object)null &&
                    (UnityEngine.Object)renderer.sharedMaterial != (UnityEngine.Object)null &&
                    (UnityEngine.Object)renderer.sharedMaterial.mainTexture != (UnityEngine.Object)null)
                {
                    return renderer.sharedMaterial.mainTexture;
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Initializes the current instance with values from specified prefab.
        /// </summary>
        /// <param name="prefab">The prefab.</param>
        protected override bool InitializeData(PrefabInfo prefab)
        {
            bool success = base.InitializeData(prefab);

            BuildingInfo building = (BuildingInfo)prefab;

            if (building.m_class != null &&
                building.m_class.m_service != ItemClass.Service.None && building.m_class.m_service <= ItemClass.Service.Office &&
                building.m_placementStyle == ItemClass.Placement.Automatic &&
                building.m_cellWidth > 0 && building.m_cellWidth <= 4 && building.m_cellLength > 0 && building.m_cellLength <= 4 &&
                ItemClass.GetPrivateServiceIndex(building.m_class.m_service) != -1)
            {
                this.type = AssetTypes.Growable;
            }
            else
            {
                this.type = AssetTypes.Building;
            }

            if (building.m_props != null)
            {
                for (int i = 0; i < building.m_props.Length; i++)
                {
                    if (building.m_props[i] != null)
                    {
                        if ((UnityEngine.Object)building.m_props[i].m_prop != (UnityEngine.Object)null)
                        {
                            this.AddReferencedAsset<PropInfo>(Reference.ReferenceTypes.Prop, building.m_props[i].m_prop);
                        }

                        if ((UnityEngine.Object)building.m_props[i].m_tree != (UnityEngine.Object)null)
                        {
                            this.AddReferencedAsset<TreeInfo>(Reference.ReferenceTypes.Tree, building.m_props[i].m_tree);
                        }
                    }
                }
            }

            if (building.m_subMeshes != null)
            {
                for (int i = 0; i < building.m_subMeshes.Length; i++)
                {
                    if (building.m_subMeshes[i] != null &&
                        (UnityEngine.Object)building.m_subMeshes[i].m_subInfo != (UnityEngine.Object)null &&
                        building.m_subMeshes[i].m_subInfo is BuildingInfoSub)
                    {
                        AssetInfo subAsset = new SubMeshInfo((BuildingInfoSub)building.m_subMeshes[i].m_subInfo);
                        if (subAsset.Initialized)
                        {
                            this.Add(subAsset);
                        }
                    }
                }
            }

            if (building.m_subBuildings != null)
            {
                for (int i = 0; i < building.m_subBuildings.Length; i++)
                {
                    if (building.m_subBuildings[i] != null &&
                        (UnityEngine.Object)building.m_subBuildings[i].m_buildingInfo != (UnityEngine.Object)null)
                    {
                        this.AddReferencedAsset<BuildingInfo>(Reference.ReferenceTypes.SubAssset, building.m_subBuildings[i].m_buildingInfo);

                        AssetInfo subAsset = new SubBuildingInfo(building.m_subBuildings[i].m_buildingInfo);
                        if (subAsset.Initialized)
                        {
                            this.Add(subAsset);
                        }
                    }
                }
            }

            return success;
        }

        /// <summary>
        /// Initializes the types.
        /// </summary>
        /// <param name="prefab">The prefab.</param>
        /// <returns>
        /// True on success.
        /// </returns>
        protected override bool InitializeTypes(PrefabInfo prefab)
        {
            bool success = base.InitializeTypes(prefab);

            if (((BuildingInfo)prefab).m_class != null &&
                ((BuildingInfo)prefab).m_class.m_service != ItemClass.Service.None && ((BuildingInfo)prefab).m_class.m_service <= ItemClass.Service.Office &&
                ((BuildingInfo)prefab).m_placementStyle == ItemClass.Placement.Automatic &&
                ((BuildingInfo)prefab).m_cellWidth > 0 && ((BuildingInfo)prefab).m_cellWidth <= 4 && ((BuildingInfo)prefab).m_cellLength > 0 && ((BuildingInfo)prefab).m_cellLength <= 4 &&
                ItemClass.GetPrivateServiceIndex(((BuildingInfo)prefab).m_class.m_service) != -1)
            {
                this.type = AssetTypes.Growable;
            }
            else
            {
                this.type = AssetTypes.Building;
            }

            return success;
        }

        /// <summary>
        /// Holds information about a building asset sub-building.
        /// </summary>
        /// <seealso cref="WhatThe.Mods.CitiesSkylines.Asseteer.Reporter.AssetInfo" />
        private class SubBuildingInfo : BuildingAssetInfo
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="SubBuildingInfo"/> class.
            /// </summary>
            /// <param name="prefab">The prefab.</param>
            public SubBuildingInfo(BuildingInfo prefab) : base(prefab)
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

        /// <summary>
        /// Holds information about a building asset sub-mesh.
        /// </summary>
        /// <seealso cref="WhatThe.Mods.CitiesSkylines.Asseteer.Reporter.AssetInfo" />
        private class SubMeshInfo : AssetInfo
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="SubMeshInfo"/> class.
            /// </summary>
            /// <param name="prefab">The prefab.</param>
            public SubMeshInfo(BuildingInfoSub prefab) : base(prefab)
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
                return GetMeshWithFallback(((BuildingInfoSub)prefab).m_lodObject, ((BuildingInfoSub)prefab).m_lodMesh);
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
                return GetTextureWithFallback(((BuildingInfoSub)prefab).m_lodObject, ((BuildingInfoSub)prefab).m_lodMaterial);
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
                return GetMeshWithFallback(prefab, ((BuildingInfoSub)prefab).m_mesh);
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
                return GetTextureWithFallback(prefab, ((BuildingInfoSub)prefab).m_material);
            }
        }
    }
}
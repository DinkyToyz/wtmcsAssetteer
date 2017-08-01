using UnityEngine;

namespace WhatThe.Mods.CitiesSkylines.Asseteer.Reporter
{
    /// <summary>
    /// Holds info about triangles.
    /// </summary>
    internal struct Triangles
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Triangles"/> struct.
        /// </summary>
        /// <param name="count">The count.</param>
        /// <param name="volume">The volume.</param>
        /// <param name="weight">The weight.</param>
        /// <param name="isLod">if set to <c>true</c> [is lod].</param>
        public Triangles(int count, int volume, int weight, bool isLod)
        {
            this.Initialized = true;

            this.Count = count;
            this.Volume = volume;
            this.Weight = weight;
            this.Lod = isLod;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Triangles"/> struct.
        /// </summary>
        /// <param name="count">The count.</param>
        /// <param name="volume">The volume.</param>
        /// <param name="isLod">if set to <c>true</c> [is lod].</param>
        public Triangles(int count, int volume, bool isLod)
        {
            this.Initialized = true;

            this.Count = count;
            this.Volume = volume;
            this.Weight = (int)(((float)count / (float)volume) * 1000.0f);
            this.Lod = isLod;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Triangles" /> struct.
        /// </summary>
        /// <param name="mesh">The mesh.</param>
        /// <param name="isLod">if set to <c>true</c> this is for lod.</param>
        public Triangles(Mesh mesh, bool isLod)
        {
            this.Lod = isLod;

            if (mesh == null || !mesh.isReadable || mesh.triangles == null)
            {
                this.Initialized = false;

                this.Count = 0;
                this.Volume = 0;
                this.Weight = 0;
            }
            else
            {
                this.Initialized = true;

                float volume = mesh.bounds.size.x * mesh.bounds.size.y * mesh.bounds.size.z;

                this.Count = mesh.triangles.Length / 3;
                this.Volume = (int)volume;

                if (this.Count == 0)
                {
                    this.Weight = 0;
                }
                else
                {
                    if (this.Volume <= 0)
                    {
                        this.Weight = this.Count;
                    }
                    else
                    {
                        this.Weight = (int)(((float)this.Count / volume) * 1000.0f);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>
        /// The count.
        /// </value>
        public int Count { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Triangles"/> is heavy.
        /// </summary>
        /// <value>
        ///   <c>true</c> if heavy; otherwise, <c>false</c>.
        /// </value>
        public bool Heavy => this.Weight > (this.Lod ? 5 : 100);

        /// <summary>
        /// Gets a value indicating whether this <see cref="Triangles"/> is initialized.
        /// </summary>
        /// <value>
        ///   <c>true</c> if initialized; otherwise, <c>false</c>.
        /// </value>
        public bool Initialized { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance is a lod.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is a lod; otherwise, <c>false</c>.
        /// </value>
        public bool Lod { get; private set; }

        /// <summary>
        /// Gets the weight.
        /// </summary>
        /// <value>
        /// The weight.
        /// </value>
        public int Weight { get; private set; }

        /// <summary>
        /// Gets a value indicating whether [very heavy].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [very heavy]; otherwise, <c>false</c>.
        /// </value>
        public bool VeryHeavy => this.Weight > (this.Lod ? 10 : 200);

        /// <summary>
        /// Gets the volume.
        /// </summary>
        /// <value>
        /// The volume.
        /// </value>
        public int Volume { get; private set; }
    }
}
using System;
using System.Reflection;

namespace WhatThe.Mods.CitiesSkylines.Asseteer
{
    /// <summary>
    /// Building extension methods.
    /// </summary>
    public static class BuildingExtensions
    {
        /// <summary>
        /// Gets the high wealth tourist value.
        /// </summary>
        /// <param name="ai">The building ai.</param>
        /// <returns>The high wealth tourist target number.</returns>
        public static int GetHighWealthTourists(this BuildingAI ai)
        {
            if (ai is MonumentAI)
            {
                return ((MonumentAI)ai).m_visitPlaceCount2;
            }
            else if (ai is ParkAI)
            {
                return ((ParkAI)ai).m_visitPlaceCount2;
            }
            else
            {
                return GetIntProperty(ai, "m_visitPlaceCount2", 0);
            }
        }

        /// <summary>
        /// Gets the low wealth tourist value.
        /// </summary>
        /// <param name="ai">The building ai.</param>
        /// <returns>The low wealth tourist target number.</returns>
        public static int GetLowWealthTourists(this BuildingAI ai)
        {
            if (ai is MonumentAI)
            {
                return ((MonumentAI)ai).m_visitPlaceCount0;
            }
            else if (ai is ParkAI)
            {
                return ((ParkAI)ai).m_visitPlaceCount0;
            }
            else
            {
                return GetIntProperty(ai, "m_visitPlaceCount0", 0);
            }
        }

        /// <summary>
        /// Gets the medium wealth tourist value.
        /// </summary>
        /// <param name="ai">The building ai.</param>
        /// <returns>The medium wealth tourist target number.</returns>
        public static int GetMediumWealthTourists(this BuildingAI ai)
        {
            if (ai is MonumentAI)
            {
                return ((MonumentAI)ai).m_visitPlaceCount1;
            }
            else if (ai is ParkAI)
            {
                return ((ParkAI)ai).m_visitPlaceCount1;
            }
            else
            {
                return GetIntProperty(ai, "m_visitPlaceCount1", 0);
            }
        }

        /// <summary>
        /// Sets the high wealth tourist value.
        /// </summary>
        /// <param name="ai">The building ai.</param>
        /// <param name="value">The high wealth tourist target number.</param>
        public static void SetHighWealthTourists(this BuildingAI ai, int value)
        {
            if (ai is MonumentAI)
            {
                ((MonumentAI)ai).m_visitPlaceCount2 = value;
            }
            else if (ai is ParkAI)
            {
                ((ParkAI)ai).m_visitPlaceCount2 = value;
            }
            else
            {
                SetIntProperty(ai, "m_visitPlaceCount2", value);
            }
        }

        /// <summary>
        /// Sets the low wealth tourist value.
        /// </summary>
        /// <param name="ai">The building ai.</param>
        /// <param name="value">The low wealth tourist target number.</param>
        public static void SetLowWealthTourists(this BuildingAI ai, int value)
        {
            if (ai is MonumentAI)
            {
                ((MonumentAI)ai).m_visitPlaceCount0 = value;
            }
            else if (ai is ParkAI)
            {
                ((ParkAI)ai).m_visitPlaceCount0 = value;
            }
            else
            {
                SetIntProperty(ai, "m_visitPlaceCount0", value);
            }
        }

        /// <summary>
        /// Sets the medium wealth tourist value.
        /// </summary>
        /// <param name="ai">The building ai.</param>
        /// <param name="value">The medium wealth tourist target number.</param>
        public static void SetMediumWealthTourists(this BuildingAI ai, int value)
        {
            if (ai is MonumentAI)
            {
                ((MonumentAI)ai).m_visitPlaceCount1 = value;
            }
            else if (ai is ParkAI)
            {
                ((ParkAI)ai).m_visitPlaceCount1 = value;
            }
            else
            {
                SetIntProperty(ai, "m_visitPlaceCount1", value);
            }
        }

        /// <summary>
        /// Gets an integer property.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="name">The property name.</param>
        /// <returns>The property value.</returns>
        private static int GetIntProperty(object obj, string name)
        {
            PropertyInfo propInf = obj.GetType().GetProperty(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
            if (propInf == null)
            {
                propInf = obj.GetType().GetProperty(name, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);

                if (propInf == null)
                {
                    throw new MissingFieldException("Property not found");
                }
            }

            if (!propInf.CanRead)
            {
                throw new MemberAccessException("Property not readable");
            }

            if (!obj.GetType().IsAssignableFrom(propInf.PropertyType))
            {
                throw new InvalidCastException("Property not assignable to int");
            }

            return (int)propInf.GetValue(obj, null);
        }

        /// <summary>
        /// Gets an integer property.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="name">The property name.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The property value, or the default value if the property value cannot be retrieved.</returns>
        private static int GetIntProperty(object obj, string name, int defaultValue)
        {
            try
            {
                return GetIntProperty(obj, name);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Sets an integer property.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="name">The property name.</param>
        /// <param name="value">The property value.</param>
        /// <exception cref="MissingFieldException">Property not found.</exception>
        /// <exception cref="MemberAccessException">Property not writeable.</exception>
        /// <exception cref="InvalidCastException">Property not assignable from int.</exception>
        private static void SetIntProperty(object obj, string name, int value)
        {
            PropertyInfo propInf = obj.GetType().GetProperty(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
            if (propInf == null)
            {
                propInf = obj.GetType().GetProperty(name, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);

                if (propInf == null)
                {
                    throw new MissingFieldException("Property not found");
                }
            }

            if (!propInf.CanWrite)
            {
                throw new MemberAccessException("Property not writeable");
            }

            if (!propInf.PropertyType.IsAssignableFrom(obj.GetType()))
            {
                throw new InvalidCastException("Property not assignable from int");
            }

            propInf.SetValue(obj, value, null);
        }
    }
}
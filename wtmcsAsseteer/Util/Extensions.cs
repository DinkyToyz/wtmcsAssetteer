﻿using System;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace WhatThe.Mods.CitiesSkylines.Asseteer
{
    /// <summary>
    /// Type extension methods.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// The ASCII capitals pattern.
        /// </summary>
        private static readonly Regex ASCIICapitalsPattern = new Regex("[^A-Z]", RegexOptions.Compiled);

        /// <summary>
        /// The new lines pattern
        /// </summary>
        private static readonly Regex NewLinesPattern = new Regex("[\r\n]+", RegexOptions.Compiled);

        /// <summary>
        /// Get only ASCII capitals.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>The ASCII capitals.</returns>
        public static string ASCIICapitals(this string text)
        {
            return ASCIICapitalsPattern.Replace(text, "");
        }

        /// <summary>
        /// Invokes method in base class.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The return object.</returns>
        public static object BaseInvoke(this object instance, string methodName, object[] parameters)
        {
            try
            {
                Type baseType = instance.GetType().BaseType;

                if (baseType == null)
                {
                    return null;
                }

                MethodInfo methodInfo = baseType.GetMethod(methodName);
                if (methodInfo == null)
                {
                    return null;
                }

                return methodInfo.Invoke(instance, parameters);
            }
            catch (Exception ex)
            {
                Log.Error(instance, "BaseInvoke", ex, methodName);
                return null;
            }
        }

        /// <summary>
        /// Cleans the newlines.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>The clean text.</returns>
        public static string CleanNewLines(this string text)
        {
            return NewLinesPattern.Replace(text, "\n");
        }

        /// <summary>
        /// Cleans the newlines.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>The clean text.</returns>
        public static string CleanNewLines(this StringBuilder text)
        {
            return text.ToString().CleanNewLines();
        }

        /// <summary>
        /// Conforms the newlines to the environment.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>The conforming text.</returns>
        public static string ConformNewlines(this string text)
        {
            return NewLinesPattern.Replace(text, Environment.NewLine);
        }

        /// <summary>
        /// Conforms the newlines to the environment.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>The conforming text.</returns>
        public static string ConformNewlines(this StringBuilder text)
        {
            return text.ToString().ConformNewlines();
        }

        /// <summary>
        /// Escapes the specified text fro HTML.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>The escaped text.</returns>
        public static string HtmlEscape(this string text)
        {
            StringBuilder escaped = new StringBuilder(text.Length);

            for (int i = 0; i < text.Length; i++)
            {
                switch (text[i])
                {
                    case '<':
                        escaped.Append("&lt;");
                        break;

                    case '>':
                        escaped.Append("&gt;");
                        break;

                    case '&':
                        escaped.Append("&amp;");
                        break;

                    case '"':
                        escaped.Append("&quot");
                        break;

                    default:
                        escaped.Append(text[i]);
                        break;
                }
            }

            return escaped.ToString();
        }
    }
}
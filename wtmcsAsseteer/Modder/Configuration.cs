using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace WhatThe.Mods.CitiesSkylines.Asseteer.Modder
{
    /// <summary>
    /// Configuration file.
    /// </summary>
    internal class Configuration
    {
        /// <summary>
        /// The asset type.
        /// </summary>
        private AssetTypes assetType;

        /// <summary>
        /// The city name.
        /// </summary>
        private string cityName;

        /// <summary>
        /// The index.
        /// </summary>
        private Dictionary<string, int> index;

        /// <summary>
        /// The items.
        /// </summary>
        private List<Item> items;

        /// <summary>
        /// Initializes a new instance of the <see cref="Configuration" /> class.
        /// </summary>
        /// <param name="itemType">Type of the item.</param>
        /// <param name="assetType">Type of the asset.</param>
        /// <param name="cityName">Name of the city.</param>
        public Configuration(ItemTypes itemType, AssetTypes assetType, string cityName)
        {
            this.ItemType = itemType;
            this.assetType = assetType;
            this.items = new List<Item>();
            this.cityName = cityName;

            if (itemType == ItemTypes.Configuration)
            {
                this.index = new Dictionary<string, int>();
            }
        }

        /// <summary>
        /// Types of assets.
        /// </summary>
        public enum AssetTypes
        {
            /// <summary>
            /// Building assets.
            /// </summary>
            Buildings = 1
        }

        /// <summary>
        /// Parts of items.
        /// </summary>
        public enum ItemParts
        {
            /// <summary>
            /// The identity part.
            /// </summary>
            Identity = 1,

            /// <summary>
            /// The values part.
            /// </summary>
            Values = 2
        }

        /// <summary>
        /// Types of items.
        /// </summary>
        public enum ItemTypes
        {
            /// <summary>
            /// Configuration data.
            /// </summary>
            Configuration = 1,

            /// <summary>
            /// Asset items.
            /// </summary>
            Assets = 2,

            /// <summary>
            /// Object items.
            /// </summary>
            Objects = 3
        }

        /// <summary>
        /// Gets the configuration string.
        /// </summary>
        /// <value>
        /// The configuration string.
        /// </value>
        public string ConfigurationString
        {
            get
            {
                StringBuilder builder = new StringBuilder();
                this.BuildConfigurationString(builder);
                return builder.ToString();
            }
        }

        /// <summary>
        /// Gets the type of items.
        /// </summary>
        public ItemTypes ItemType
        {
            get;
            private set;
        }

        /// <summary>
        /// Adds the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <exception cref="InvalidOperationException">Item exists.</exception>
        public void Add(Item item)
        {
            if (this.index != null)
            {
                if (this.index.ContainsKey(item.IdentityString))
                {
                    throw new InvalidOperationException("Item exists");
                }

                this.index[item.IdentityString] = this.items.Count;
            }

            this.items.Add(item);
        }

        /// <summary>
        /// Builds the configuration string.
        /// </summary>
        /// <param name="builder">The string builder.</param>
        public void BuildConfigurationString(StringBuilder builder)
        {
            bool first = true;

            foreach (Item item in this.items.OrderBy(i => i.IdentityString.ToLowerInvariant()))
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    builder.Append(Environment.NewLine);
                }

                item.BuildConfigurationString(builder);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Item" /> class.
        /// </summary>
        /// <returns>A new item.</returns>
        public Item NewItem()
        {
            return new Item(this.assetType);
        }

        /// <summary>
        /// Saves this instance to a text file.
        /// </summary>
        public void Save()
        {
            string extension = "." + this.assetType.ToString() + "." + this.ItemType.ToString();
            if (this.cityName != null)
            {
                extension = "." + FileSystem.CleanName(this.cityName) + extension;
            }

            string fileName = FileSystem.FilePathName(extension);
            string filePath = Path.GetDirectoryName(Path.GetFullPath(fileName));
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            File.WriteAllText(fileName, this.ConfigurationString);
        }

        /// <summary>
        /// Dictionary of asset configuration parts.
        /// </summary>
        public class ConfigurationDictionary : IDictionary<string, string>
        {
            /// <summary>
            /// Regular expression matching strings needing quotes.
            /// </summary>
            private static readonly Regex QuotesNeeded = new Regex("[^1234567890]");

            /// <summary>
            /// The configuration-string assignment separator string.
            /// </summary>
            private string assigner;

            /// <summary>
            /// The changed value flag.
            /// </summary>
            private bool changedValue;

            /// <summary>
            /// The dictionary.
            /// </summary>
            private Dictionary<string, string> dictionary;

            /// <summary>
            /// The identity string value.
            /// </summary>
            private string identityStringValue;

            /// <summary>
            /// The post-configuration-string separator string.
            /// </summary>
            private string postSeparator;

            /// <summary>
            /// The pre-configuration-string separator string.
            /// </summary>
            private string preSeparator;

            /// <summary>
            /// The configuration-string separator string.
            /// </summary>
            private string separator;

            /// <summary>
            /// The string for ToString() value.
            /// </summary>
            private string stringForToStringValue;

            /// <summary>
            /// The ToString() value.
            /// </summary>
            private string toStringValue;

            /// <summary>
            /// Initializes a new instance of the <see cref="ConfigurationDictionary"/> class.
            /// </summary>
            /// <param name="assetType">Type of the asset.</param>
            /// <param name="itemPart">The item part.</param>
            public ConfigurationDictionary(AssetTypes assetType, ItemParts itemPart)
            {
                this.dictionary = new Dictionary<string, string>();
                this.Changed = true;

                this.AssetType = assetType;
                this.ItemPart = itemPart;

                switch (itemPart)
                {
                    case ItemParts.Identity:
                        this.assigner = "=";
                        this.separator = ", ";
                        this.preSeparator = null;
                        this.postSeparator = Environment.NewLine;
                        break;

                    case ItemParts.Values:
                        this.assigner = "=";
                        this.separator = Environment.NewLine + "\t";
                        this.preSeparator = "\t";
                        this.postSeparator = Environment.NewLine;
                        break;

                    default:
                        this.assigner = "=";
                        this.separator = ", ";
                        this.preSeparator = null;
                        this.postSeparator = null;
                        break;
                }
            }

            /// <summary>
            /// Gets the type of the asset.
            /// </summary>
            /// <value>
            /// The type of the asset.
            /// </value>
            public AssetTypes AssetType
            {
                get;
                private set;
            }

            /// <summary>
            /// Gets or sets a value indicating whether this <see cref="ConfigurationDictionary"/> has been changed.
            /// </summary>
            /// <value>
            ///   <c>true</c> if changed; otherwise, <c>false</c>.
            /// </value>
            public bool Changed
            {
                get
                {
                    return this.changedValue;
                }

                set
                {
                    this.changedValue = value;
                    if (value)
                    {
                        this.toStringValue = null;
                        this.stringForToStringValue = null;
                        this.identityStringValue = null;
                    }
                }
            }

            /// <summary>
            /// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1" />.
            /// </summary>
            public int Count
            {
                get
                {
                    return this.dictionary.Count;
                }
            }

            /// <summary>
            /// Gets the identity string.
            /// </summary>
            /// <value>
            /// The identity string.
            /// </value>
            public string IdentityString
            {
                get
                {
                    if (this.identityStringValue == null)
                    {
                        this.identityStringValue = this.GetString(null, null, "=", ",", null, null);
                    }

                    return this.identityStringValue;
                }
            }

            /// <summary>
            /// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only.
            /// </summary>
            public bool IsReadOnly
            {
                get
                {
                    return false;
                }
            }

            /// <summary>
            /// Gets the item part.
            /// </summary>
            /// <value>
            /// The item part.
            /// </value>
            public ItemParts ItemPart
            {
                get;
                private set;
            }

            /// <summary>
            /// Gets an <see cref="T:System.Collections.Generic.ICollection`1" /> containing the keys of the <see cref="T:System.Collections.Generic.IDictionary`2" />.
            /// </summary>
            public ICollection<string> Keys
            {
                get
                {
                    return this.dictionary.Keys;
                }
            }

            /// <summary>
            /// Gets the string for ToString().
            /// </summary>
            /// <value>
            /// The string for ToString().
            /// </value>
            public string StringForToString
            {
                get
                {
                    if (this.stringForToStringValue == null)
                    {
                        this.stringForToStringValue = this.GetString(null, this.ItemPart.ToString(), "=", ", ", "; ", null);
                    }

                    return this.stringForToStringValue;
                }
            }

            /// <summary>
            /// Gets an <see cref="T:System.Collections.Generic.ICollection`1" /> containing the values in the <see cref="T:System.Collections.Generic.IDictionary`2" />.
            /// </summary>
            public ICollection<string> Values
            {
                get
                {
                    return this.dictionary.Values;
                }
            }

            /// <summary>
            /// Gets or sets the element with the specified key.
            /// </summary>
            /// <param name="key">The key.</param>
            /// <returns>The element with the specified key.</returns>
            public string this[string key]
            {
                get
                {
                    string value;
                    return this.dictionary.TryGetValue(key, out value) ? value : null;
                }

                set
                {
                    this.Changed = true;
                    this.dictionary[key] = value;
                }
            }

            /// <summary>
            /// Adds the specified key.
            /// </summary>
            /// <param name="key">The key.</param>
            /// <param name="value">The value.</param>
            public void Add(string key, string value)
            {
                this.Changed = true;
                this.dictionary.Add(key, value);
            }

            /// <summary>
            /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1" />.
            /// </summary>
            /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
            public void Add(KeyValuePair<string, string> item)
            {
                this.Changed = true;
                this.dictionary.Add(item.Key, item.Value);
            }

            /// <summary>
            /// Builds the configuration string.
            /// </summary>
            /// <param name="builder">The string builder.</param>
            public void BuildConfigurationString(StringBuilder builder)
            {
                this.BuildString(builder, null, null, this.assigner, this.separator, this.preSeparator, this.postSeparator);
            }

            /// <summary>
            /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1" />.
            /// </summary>
            public void Clear()
            {
                this.Changed = true;
                this.dictionary.Clear();
            }

            /// <summary>
            /// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1" /> contains a specific value.
            /// </summary>
            /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
            /// <returns>
            /// True if <paramref name="item" /> is found in the <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, false.
            /// </returns>
            public bool Contains(KeyValuePair<string, string> item)
            {
                return this.dictionary.Contains(item);
            }

            /// <summary>
            /// Determines whether the <see cref="T:System.Collections.Generic.IDictionary`2" /> contains an element with the specified key.
            /// </summary>
            /// <param name="key">The key to locate in the <see cref="T:System.Collections.Generic.IDictionary`2" />.</param>
            /// <returns>
            /// True if the <see cref="T:System.Collections.Generic.IDictionary`2" /> contains an element with the key; otherwise, false.
            /// </returns>
            public bool ContainsKey(string key)
            {
                return this.dictionary.ContainsKey(key);
            }

            /// <summary>
            /// Copies all key-value-pairs to the specified array.
            /// </summary>
            /// <param name="array">The array.</param>
            /// <param name="arrayIndex">Start index of the array.</param>
            public void CopyTo(KeyValuePair<string, string>[] array, int arrayIndex)
            {
                foreach (KeyValuePair<string, string> kvp in this.dictionary)
                {
                    array[arrayIndex] = new KeyValuePair<string, string>(kvp.Key, kvp.Value);
                    arrayIndex++;
                }
            }

            /// <summary>
            /// Returns an enumerator that iterates through the collection.
            /// </summary>
            /// <returns>
            /// A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.
            /// </returns>
            public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
            {
                return this.dictionary.GetEnumerator();
            }

            /// <summary>
            /// Removes the element with the specified key from the <see cref="T:System.Collections.Generic.IDictionary`2" />.
            /// </summary>
            /// <param name="key">The key of the element to remove.</param>
            /// <returns>
            /// True if the element is successfully removed; otherwise, false.  This method also returns false if <paramref name="key" /> was not found in the original <see cref="T:System.Collections.Generic.IDictionary`2" />.
            /// </returns>
            public bool Remove(string key)
            {
                this.Changed = true;
                return this.dictionary.Remove(key);
            }

            /// <summary>
            /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1" />.
            /// </summary>
            /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
            /// <returns>
            /// True if <paramref name="item" /> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, false. This method also returns false if <paramref name="item" /> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1" />.
            /// </returns>
            public bool Remove(KeyValuePair<string, string> item)
            {
                if (this[item.Key] == item.Value)
                {
                    this.Changed = true;
                    return this.dictionary.Remove(item.Key);
                }
                else
                {
                    return false;
                }
            }

            /// <summary>
            /// Sets the value of the specified element.
            /// </summary>
            /// <param name="key">The key.</param>
            /// <param name="value">The value.</param>
            public void Set(string key, object value)
            {
                this[key] = value.ToString();
            }

            /// <summary>
            /// Returns an enumerator that iterates through a collection.
            /// </summary>
            /// <returns>
            /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
            /// </returns>
            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return this.dictionary.GetEnumerator();
            }

            /// <summary>
            /// Returns a <see cref="System.String" /> that represents this instance.
            /// </summary>
            /// <returns>
            /// A <see cref="System.String" /> that represents this instance.
            /// </returns>
            public override string ToString()
            {
                if (this.toStringValue == null)
                {
                    this.toStringValue = this.GetString(this.AssetType.ToString(), this.ItemPart.ToString(), "=", ", ", ": ", null);
                }

                return this.toStringValue;
            }

            /// <summary>
            /// Tries the get value.
            /// </summary>
            /// <param name="key">The key.</param>
            /// <param name="value">The value.</param>
            /// <returns>True if the value was returned.</returns>
            public bool TryGetValue(string key, out string value)
            {
                this.Changed = true;
                return this.dictionary.TryGetValue(key, out value);
            }

            /// <summary>
            /// Builds a string containing the dictionaries content.
            /// </summary>
            /// <param name="builder">The string builder.</param>
            /// <param name="superPrefix">The super-prefix.</param>
            /// <param name="subPrefix">The sub-prefix.</param>
            /// <param name="assigner">The assignment separator string.</param>
            /// <param name="separator">The separator string.</param>
            /// <param name="preSeparator">The pre-string separator string.</param>
            /// <param name="postSeparator">The post-string separator string.</param>
            private void BuildString(StringBuilder builder, string superPrefix, string subPrefix, string assigner, string separator, string preSeparator, string postSeparator)
            {
                bool has = false;

                foreach (KeyValuePair<string, string> item in this.dictionary.OrderBy(i => i.Key.ToLowerInvariant()))
                {
                    if (!string.IsNullOrEmpty(item.Value))
                    {
                        if (!has)
                        {
                            if (preSeparator != null)
                            {
                                builder.Append(preSeparator);
                            }

                            has = true;
                        }
                        else if (separator != null)
                        {
                            builder.Append(separator);
                        }

                        builder.Append(item.Key);
                        if (assigner != null)
                        {
                            builder.Append(assigner);
                        }

                        if (QuotesNeeded.IsMatch(item.Value))
                        {
                            builder.Append('"').Append(item.Value.Replace("\"", "\"\"")).Append('"');
                        }
                        else
                        {
                            builder.Append(item.Value);
                        }
                    }
                }

                if (has && postSeparator != null)
                {
                    builder.Append(postSeparator);
                }

                if (subPrefix != null && (has || superPrefix != null))
                {
                    builder.Insert(0, subPrefix);
                }

                if (superPrefix != null)
                {
                    if (subPrefix != null)
                    {
                        builder.Insert(0, '.');
                    }

                    builder.Insert(0, superPrefix);
                }
            }

            /// <summary>
            /// Gets the string containing the dictionary's content.
            /// </summary>
            /// <param name="superPrefix">The super-prefix.</param>
            /// <param name="subPrefix">The sub-prefix.</param>
            /// <param name="assigner">The assignment separator string.</param>
            /// <param name="separator">The separator string.</param>
            /// <param name="preSeparator">The pre-string separator string.</param>
            /// <param name="postSeparator">The post-string separator string.</param>
            /// <returns>A string string containing the dictionary's content.</returns>
            private string GetString(string superPrefix, string subPrefix, string assigner, string separator, string preSeparator, string postSeparator)
            {
                StringBuilder builder = new StringBuilder();

                this.BuildString(builder, superPrefix, subPrefix, assigner, separator, preSeparator, postSeparator);

                return builder.ToString();
            }
        }

        /// <summary>
        /// Am asset configuration item.
        /// </summary>
        public class Item
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="Item"/> class.
            /// </summary>
            /// <param name="assetType">Type of the asset.</param>
            public Item(AssetTypes assetType)
            {
                this.AssetType = assetType;

                this.Identity = new ConfigurationDictionary(assetType, ItemParts.Identity);
                this.Values = new ConfigurationDictionary(assetType, ItemParts.Values);
            }

            /// <summary>
            /// Gets the type of the asset.
            /// </summary>
            /// <value>
            /// The type of the asset.
            /// </value>
            public AssetTypes AssetType
            {
                get;
                private set;
            }

            /// <summary>
            /// Gets the identity.
            /// </summary>
            /// <value>
            /// The identity.
            /// </value>
            public ConfigurationDictionary Identity
            {
                get;
                private set;
            }

            /// <summary>
            /// Gets the identity string.
            /// </summary>
            /// <value>
            /// The identity string.
            /// </value>
            public string IdentityString
            {
                get
                {
                    return this.Identity.IdentityString;
                }
            }

            /// <summary>
            /// Gets the values.
            /// </summary>
            /// <value>
            /// The values.
            /// </value>
            public ConfigurationDictionary Values
            {
                get;
                private set;
            }

            /// <summary>
            /// Builds the configuration string.
            /// </summary>
            /// <param name="builder">The string builder.</param>
            public void BuildConfigurationString(StringBuilder builder)
            {
                this.Identity.BuildConfigurationString(builder);
                this.Values.BuildConfigurationString(builder);
            }

            /// <summary>
            /// Returns a <see cref="System.String" /> that represents this instance.
            /// </summary>
            /// <returns>
            /// A <see cref="System.String" /> that represents this instance.
            /// </returns>
            public override string ToString()
            {
                return this.AssetType + "; " + this.Identity.StringForToString + this.Values.StringForToString;
            }
        }
    }
}
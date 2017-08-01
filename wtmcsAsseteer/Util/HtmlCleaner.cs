using System.Text;
using System.Text.RegularExpressions;

namespace WhatThe.Mods.CitiesSkylines.Asseteer
{
    /// <summary>
    /// Cleans whitespace from HTML.
    /// </summary>
    internal class HtmlCleaner
    {
        /// <summary>
        /// Gets or sets a value indicating whether to keep line breaks.
        /// </summary>
        /// <value>
        ///   <c>true</c> if keeping line breaks; otherwise, <c>false</c>.
        /// </value>
        public bool KeepNewLines { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HtmlCleaner"/> class.
        /// </summary>
        /// <param name="keepNewLines">if set to <c>true</c> [keep new lines].</param>
        public HtmlCleaner(bool keepNewLines = false)
        {
            this.KeepNewLines = keepNewLines;
        }

        /// <summary>
        /// Finds line breaks that can be collapsed.
        /// </summary>
        private Regex CollapsibleNewLines = new Regex(@"\r{2,}", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.ExplicitCapture);

        /// <summary>
        /// Finds whitespace that can be collapsed.
        /// </summary>
        private Regex CollapsibleSpace = new Regex(@"[^\S\r]{2,}", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.ExplicitCapture);

        /// <summary>
        /// Finds the document header.
        /// </summary>
        private Regex DocHeader = new Regex(@"^\s*(?'tag'<[?!].*?>)\s*", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.ExplicitCapture);

        /// <summary>
        /// Finds whitespace that can be removed.
        /// </summary>
        private Regex RemovableWhiteSpace = new Regex(@"((?<=(?<!</?span(\s[^>]*)?)>)[^\S\r]+|(?<=</?)\s+|\s+(?=/?>)|[^\S\r]+(?=<(?!/?span(\s[^>]*)?>))|(?<=\r)[^\S\r]+|[^\S\r]+(?=\r))", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.ExplicitCapture);

        /// <summary>
        /// Finds blocks that should not be cleaned.
        /// </summary>
        private Regex UncleanBlocks = new Regex(@"(?'prespace'\s*)(?'starttag'<\s*(?'tag'script|style)(\s[^>]*)?>)(?'body'.*?)(?'endtag'</\s*\k'tag'(\s[^>]*)?>)(?'postspace'\s*)", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture);

        /// <summary>
        /// Cleans the specified HTML.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <returns>The clean HTML.</returns>
        public string Clean(string html)
        {
            StringBuilder clean = new StringBuilder();

            html = html.Replace("\r\n", "\r").Replace('\n', '\r');

            if (!this.KeepNewLines)
            {
                html = html.Replace('\r', ' ');
            }

            int pos = 0;

            Match blockMatch = this.UncleanBlocks.Match(html);
            while (blockMatch.Success)
            {
                if (blockMatch.Index > pos)
                {
                    clean.Append(this.CleanPart(html.Substring(pos, blockMatch.Index - pos)));
                }

                clean.Append(this.CleanBlock(blockMatch));

                pos = blockMatch.Index + blockMatch.Length;
                blockMatch = blockMatch.NextMatch();
            }

            if (pos < html.Length)
            {
                clean.Append(this.CleanPart(html.Substring(pos, html.Length - pos)));
            }

            html = clean.ToString().Trim().Replace('\r', '\n');
            html = DocHeader.Replace(html, "${tag}\r");

            return html;
        }

        /// <summary>
        /// Cleans the block.
        /// </summary>
        /// <param name="blockMatch">The block match.</param>
        /// <returns>The clean block</returns>
        private string CleanBlock(Match blockMatch)
        {
            StringBuilder clean = new StringBuilder();

            if (blockMatch.Groups["prespace"].Value.IndexOf('\r') >= 0)
            {
                clean.Append('\r');
            }

            clean.Append(this.CleanPart(blockMatch.Groups["starttag"].Value));
            clean.Append(blockMatch.Groups["body"].Value.Trim());
            clean.Append(this.CleanPart(blockMatch.Groups["endtag"].Value));

            if (blockMatch.Groups["postspace"].Value.IndexOf('\r') >= 0)
            {
                clean.Append('\r');
            }

            return clean.ToString();
        }

        /// <summary>
        /// Cleans the part.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <returns>The clean HTML.</returns>
        private string CleanPart(string html)
        {
            html = this.RemovableWhiteSpace.Replace(html, "");
            html = this.CollapsibleSpace.Replace(html, " ");
            html = this.CollapsibleNewLines.Replace(html, "\r");

            return html.Trim();
        }
    }
}
using System.Text.RegularExpressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test
{
    class Program
    {
        internal class HtmlCleaner
        {
            private Regex HtmlRemovableWhiteSpace = new Regex(@"((?<=>)[^\S\r]+|(?<=</?)\s+|\s+(?=/?>)|[^\S\r]+(?=<)|(?<=\r)[^\S\r]+|[^\S\r]+(?=\r))", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.ExplicitCapture);
            private Regex HtmlCollapsibleSpace = new Regex(@"[^\S\r]{2,}", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.ExplicitCapture);
            private Regex HtmlCollapsibleNewLines = new Regex(@"\r{2,}", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.ExplicitCapture);

            private Regex HtmlUncleanBlocks = new Regex(@"(?'prespace'\s*)(?'starttag'<\s*(?'tag'script|style)(\s[^>]*)?>)(?'body'.*?)(?'endtag'</\s*\k'tag'(\s[^>]*)?>)(?'postspace'\s*)", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture);

            private Regex HtmlDocHeader = new Regex(@"^\s*(?'tag'<\?.*?\?>)\s*", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.ExplicitCapture);

            private string CleanPart(string html)
            {
                html = this.HtmlRemovableWhiteSpace.Replace(html, "");
                html = this.HtmlCollapsibleSpace.Replace(html, " ");
                html = this.HtmlCollapsibleNewLines.Replace(html, "\r");

                return html.Trim();
            }

            //private Regex HtmlBlockCollapsibleNewLines = new Regex(@"(^\s*?\r\s*(?=<)|(?<=>)\s*?\r\s*$)", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.ExplicitCapture);
            //private Regex HtmlBlockRemovableWhiteSpace = new Regex(@"(^[^\S\r](?=<)|(?<=>)[^\S\r]$|   )", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.ExplicitCapture);

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

            public string Clean(string html)
            {
                StringBuilder clean = new StringBuilder();

                html = html.Replace("\r\n", "\r").Replace('\n', '\r');

                int pos = 0;

                Match blockMatch = this.HtmlUncleanBlocks.Match(html);
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
                html = HtmlDocHeader.Replace(html, "${tag}\r");

                return html;
            }
        }

        private static string html = "  \r\r <? vava ?>   <html>\r\n\r\n<head>\r\n<style>\r\n\t...\t\n</style>   \r\n\r \n\n <script type=\"...\">\r\n\t...\r\n</script hiho> <style> </style>  </head>  <body>\r\n\rhi\r\n    ho   ho   ho\r\r\r\r\n\n\n   hoho   \nhihi  </body>  \r  </html>  \r\r\n";

        static void Main(string[] args)
        {
            HtmlCleaner cleaner = new HtmlCleaner();

            Console.WriteLine(cleaner.Clean(html));
        }
    }
}

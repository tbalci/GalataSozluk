using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;
namespace Tuanna.Lib
{
    /// <summary>
    /// Summary description for HtmlText
    /// </summary>
    public class HtmlText
    {
        #region Constants
        private class Patterns
        {
            public const string CommentPattern = @"";
            public const string MultiSpacePattern = @"(\s|$)+";
        }
        private const string SINGLE_SPACE = " ";
        private const string NEW_LINE = "";
        private const string TAB = "    ";
        //private const string NEW_LINE = "[*]";
        //private const string TAB = "[**]";
        #endregion
        #region Fields
        private string fHtml;
        private string[] fClosableTagsToRemoveContent;
        private string[] fSingleTagsToRemoveContent;
        #endregion
        #region Constructors
        public HtmlText()
        {
            this.fHtml = String.Empty;
            this.fClosableTagsToRemoveContent = new string[] {
"script",
"style",
"select",
"layer",
"object"
};

            this.fSingleTagsToRemoveContent = new string[]{
"meta",
"input",
"img",
"link"
};
        }
        #endregion
        #region Methods
        public string Cevir(string html)
        {
            if (html != null)
            {
                //replace   with white space
                this.fHtml = html.Replace(" ", SINGLE_SPACE);
                this.fHtml = html.Replace("&nbsp;", SINGLE_SPACE);

                //remove whitespaces and line breaks
                this.Replace(Patterns.MultiSpacePattern, SINGLE_SPACE);

                //remove comments
                this.Remove(Patterns.CommentPattern);

                foreach (string tag in this.fSingleTagsToRemoveContent)
                {
                    this.Remove(this.GetSingleTagPattern(tag));
                }

                foreach (string tag in this.fClosableTagsToRemoveContent)
                {
                    this.Remove(this.GetBetweenTagsPattern(tag));
                }

                this.Replace(this.GetSingleTagPattern("br"), NEW_LINE);

                this.Remove(this.GetOpeningTagPattern("title"));
                this.Replace(this.GetClosingTagPattern("title"), NEW_LINE);

                this.Replace(this.GetOpeningTagPattern("body"), NEW_LINE);
                this.Remove(this.GetClosingTagPattern("body"));

                this.Remove(this.GetOpeningTagPattern("div"));
                this.Replace(this.GetClosingTagPattern("div"), NEW_LINE);

                this.Replace(this.GetOpeningTagPattern("p"), NEW_LINE);
                this.Remove(this.GetClosingTagPattern("p"));

                this.Replace(this.GetOpeningTagPattern("tr"), NEW_LINE);
                this.Remove(this.GetClosingTagPattern("tr"));

                this.Replace(this.GetOpeningTagPattern("td"), TAB);
                this.Remove(this.GetClosingTagPattern("td"));

                //Remove all remaining html tags
                this.Remove(@"<(\s*)((!?[a-zA-Z]+(.*?))|(/(\s*)[a-zA-Z]+(\s*)))>");
                this.Replace(@"$+\s*", "\r\n");
                return this.fHtml.Trim();
            }
            else
            {
                return "";
            }
        }
        private string GetOpeningTagPattern(string tag)
        {
            return String.Format(@"<(\s*)({0}|{0}(\s.*?))>", tag);
        }
        private string GetClosingTagPattern(string tag)
        {
            return String.Format(@"<(\s*)/(\s*){0}(\s*)>", tag);
        }
        private string GetBetweenTagsPattern(string tag)
        {
            return String.Format("{0}(.*?){1}", this.GetOpeningTagPattern(tag), this.GetClosingTagPattern(tag));
        }
        private string GetSingleTagPattern(string tag)
        {
            return String.Format(@"<(\s*){0}(.*?)/?(\s*)>", tag);
        }
        private void Remove(string pattern)
        {
            this.Replace(pattern, String.Empty);
        }
        private void Replace(string pattern, string replacer)
        {
            this.fHtml = System.Text.RegularExpressions.Regex.Replace(this.fHtml, pattern, replacer, (RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.CultureInvariant));
        }
        #endregion
    }
}
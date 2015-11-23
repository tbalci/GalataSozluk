using System;
using System.Data;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;
using System.Text;

/// <summary>
/// 
/// </summary>
public class TuaGridViewExcelExport
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="gv"></param>
    public void ExceleAktar(DataTable Tablo, string fileName, GridLines Kenarlik)
    {
        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.AddHeader(
            "content-disposition", string.Format("attachment; filename={0}", fileName));
        HttpContext.Current.Response.ContentType = "application/ms-excel";

        GridView gv = new GridView();
        gv.DataSource = Tablo;
        gv.DataBind();
        using(StringWriter sw = new StringWriter())
        {
            using(HtmlTextWriter htw = new HtmlTextWriter(sw))
            {
                //  Create a table to contain the grid
                Table table = new Table();

                //  include the gridline settings
                table.GridLines = Kenarlik;

                //  add the header row to the table
                if(gv.HeaderRow != null)
                {
                    TuaGridViewExcelExport.PrepareControlForExport(gv.HeaderRow);
                    table.Rows.Add(gv.HeaderRow);
                }

                //  add each of the data rows to the table
                foreach(GridViewRow row in gv.Rows)
                {
                    TuaGridViewExcelExport.PrepareControlForExport(row);
                    table.Rows.Add(row);
                }

                //  add the footer row to the table
                if(gv.FooterRow != null)
                {
                    TuaGridViewExcelExport.PrepareControlForExport(gv.FooterRow);
                    table.Rows.Add(gv.FooterRow);
                }

                //  render the table into the htmlwriter
                table.RenderControl(htw);
                //  render the htmlwriter into the response
                HttpContext.Current.Response.Write(RecursiveTranslateAjax(sw.ToString()));
                HttpContext.Current.Response.End();
            }
        }
    }

    public void ExceleAktar(GridView Grid, string fileName, GridLines Kenarlik)
    {
        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.AddHeader(
            "content-disposition", string.Format("attachment; filename={0}", fileName));
        HttpContext.Current.Response.ContentType = "application/ms-excel";
        using(StringWriter sw = new StringWriter())
        {
            using(HtmlTextWriter htw = new HtmlTextWriter(sw))
            {
                //  Create a table to contain the grid
                Table table = new Table();

                //  include the gridline settings
                table.GridLines = Kenarlik;

                //  add the header row to the table
                if(Grid.HeaderRow != null)
                {
                    TuaGridViewExcelExport.PrepareControlForExport(Grid.HeaderRow);
                    table.Rows.Add(Grid.HeaderRow);
                }

                //  add each of the data rows to the table
                foreach(GridViewRow row in Grid.Rows)
                {
                    TuaGridViewExcelExport.PrepareControlForExport(row);
                    table.Rows.Add(row);
                }

                //  add the footer row to the table
                if(Grid.FooterRow != null)
                {
                    TuaGridViewExcelExport.PrepareControlForExport(Grid.FooterRow);
                    table.Rows.Add(Grid.FooterRow);
                }

                //  render the table into the htmlwriter
                table.RenderControl(htw);
                //  render the htmlwriter into the response
                HttpContext.Current.Response.Write(RecursiveTranslateAjax(sw.ToString()));
                HttpContext.Current.Response.End();
            }
        }
    }



    /// <summary>
    /// Replace any of the contained controls with literals
    /// </summary>
    /// <param name="control"></param>
    private static void PrepareControlForExport(Control control)
    {
        for(int i = 0; i < control.Controls.Count; i++)
        {
            Control current = control.Controls[i];
            if(current is LinkButton)
            {
                control.Controls.Remove(current);
                control.Controls.AddAt(i, new LiteralControl((current as LinkButton).Text));
            }
            else if(current is ImageButton)
            {
                control.Controls.Remove(current);
                control.Controls.AddAt(i, new LiteralControl((current as ImageButton).AlternateText));
            }
            else if(current is HyperLink)
            {
                control.Controls.Remove(current);
                control.Controls.AddAt(i, new LiteralControl((current as HyperLink).Text));
            }
            else if(current is DropDownList)
            {
                control.Controls.Remove(current);
                control.Controls.AddAt(i, new LiteralControl((current as DropDownList).SelectedItem.Text));
            }
            else if(current is CheckBox)
            {
                control.Controls.Remove(current);
                control.Controls.AddAt(i, new LiteralControl((current as CheckBox).Checked ? "True" : "False"));
            }

            if(current.HasControls())
            {
                TuaGridViewExcelExport.PrepareControlForExport(current);
            }
        }
    }

    private string RecursiveTranslateAjax(string content)
    {
        // look for the basic Ajax response syntax
        Regex reg = new Regex(@"^(\d+)\|[^\|]*\|[^\|]*\|",
              RegexOptions.Singleline);
        Match m = reg.Match(content);
        // if found, search deeper, by taking 
        // into account the length of the html text
        if(m.Success)
        {
            // custom method to get an integer value
            int length = Int(m.Groups[1]);
            reg = new Regex(@"^(\d+)(\|[^\|]*\|[^\|]*\|)(.{" + length + @"})\|",
                  RegexOptions.Singleline);
            m = reg.Match(content);
            if(m.Success)
            {
                string trans = Translate(m.Groups[3].Value);
                return
                    trans.Length + m.Groups[2].Value +
                    trans + "|" +
                    RecursiveTranslateAjax(content.Substring(m.Length));
            }
        }
        // if not Ajax, just translate everything,
        // it must be a normal PostBack or a string of some sort.
        return Translate(content);
    }

    public int Int(object o)
    {
        if(o == null) return 0;
        if(IsNumericVariable(o)) return (int)CastDouble(o);
        string s = o.ToString();
        if(s == "") return 0;
        Match m = Regex.Match(s, "(-\\d+|\\d+)");
        if(m.Success)
            try
            {
                return Int32.Parse(m.Groups[0].Value);
            }
            catch
            {
            }
        return 0;
    }
    private double CastDouble(object o)
    {
        if(o is byte) return (byte)o;
        if(o is int) return (int)o;
        if(o is long) return (long)o;
        if(o is float) return (float)o;
        if(o is double) return (double)o;
        if(o is decimal) return (double)(decimal)o;
        throw new ArgumentException("Type is not convertable to double: " + o.GetType().FullName);
    }

    bool IsNumericVariable(object o)
    {
        return (o is byte || o is int || o is long || o is float || o is double || o is decimal);
    }

    // this method only fixes the weird characters
    // but you can put here any string change you would like
    // like search and replace some words.
    private string Translate(string content)
    {
        // Html code all chars that are not ASCII, thus getting rid of strange or Unicode characters
        StringBuilder sb = new StringBuilder();
        for(int c = 0; c < content.Length; c++)
        {
            if(content[c] > 127) sb.Append("&#" + ((int)content[c]) + ";");
            else sb.Append(content[c]);
        }
        return sb.ToString();
    }
}

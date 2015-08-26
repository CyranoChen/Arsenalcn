using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Arsenalcn.Common.Utility
{
    public static class ExportUtl
    {
        public static void ExportToExcel(GridView gv, string fileName)
        {
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.ClearHeaders();

            HttpContext.Current.Response.Charset = "utf-8";
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("utf-8");
            //HttpContext.Current.Response.Charset = "GB2312"; //设置了类型为中文防止乱码的出现 
            //HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");//设置输出流为简体中文

            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + fileName);

            //HttpContext.Current.Response.AppendHeader("Content-Type", "text/html; charset=gb2312");
            HttpContext.Current.Response.ContentType = "application/ms-excel";//设置输出文件类型为excel文件。 
            //HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";

            var tw = new StringWriter();
            var hw = new HtmlTextWriter(tw);

            SetStyle(gv);
            gv.RenderControl(hw);

            HttpContext.Current.Response.Write(tw.ToString());
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.Close();
            HttpContext.Current.Response.End();

            //gv.Dispose();
            //tw.Dispose();
            //hw.Dispose();

            gv = null;
            tw = null;
            hw = null;
        }

        public static void SetStyle(GridView gv)
        {
            gv.Font.Name = "Tahoma";
            //gv.BorderStyle = BorderStyle.Solid;
            //gv.HeaderStyle.BackColor = Color.LightCyan;
            //gv.HeaderStyle.ForeColor = Color.Black;
            
            gv.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            gv.HeaderStyle.Wrap = false;
            gv.HeaderStyle.Font.Bold = true;
            gv.HeaderStyle.Font.Size = 10;
            
            gv.RowStyle.Font.Size = 10;
        }
    }
}

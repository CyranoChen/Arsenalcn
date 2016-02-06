using System;
using System.Web.UI;

namespace iArsenal.Web.Control
{
    public partial class CustomPagerInfo : UserControl
    {
        public delegate void PageChangedEventHandler(object sender, DataNavigatorEventArgs e);

        public int PageIndex { get; set; }

        public int PageCount { get; set; }

        public int RowCount { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitComponent();
            }
        }

        public void InitComponent()
        {
            ltrlPagerInfo.Text = $"Rows: {RowCount} | Pages: {PageCount} | Goto: ";
            tbPagerIndex.Text = (PageIndex + 1).ToString();
        }

        protected void tbPagerGoto_Click(object sender, EventArgs e)
        {
            var args = new DataNavigatorEventArgs();

            var i = 0;
            if (!string.IsNullOrEmpty(tbPagerIndex.Text.Trim()) && int.TryParse(tbPagerIndex.Text.Trim(), out i))
                args.PageIndex = i - 1;

            PageChanged(this, args);
        }

        public event PageChangedEventHandler PageChanged;

        public class DataNavigatorEventArgs : EventArgs
        {
            public int PageIndex { get; set; }

            public int PageCount { get; set; }

            public int PageSize { get; set; }
        }
    }
}
using System;

namespace iArsenal.Web.Control
{
    public partial class CustomPagerInfo : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitComponent();
            }
        }

        public void InitComponent()
        {
            ltrlPagerInfo.Text = string.Format("Rows: {0} | Pages: {1} | Goto: ", RowCount.ToString(), PageCount.ToString());
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

        public int PageIndex
        { get; set; }

        public int PageCount
        { get; set; }

        public int RowCount
        { get; set; }

        public event PageChangedEventHandler PageChanged;

        public delegate void PageChangedEventHandler(object sender, DataNavigatorEventArgs e);

        public class DataNavigatorEventArgs : EventArgs
        {
            public DataNavigatorEventArgs() { }

            public int PageIndex
            { get; set; }

            public int PageCount
            { get; set; }

            public int PageSize
            { get; set; }
        }
    }
}
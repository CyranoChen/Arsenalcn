using System.Web.UI;

namespace Arsenalcn.ClubSys.Web.Common
{
    public class CollectionBase : UserControl
    {
        protected int _userid = -1;

        public int ProfileUserID { get; set; } = -1;

        public int CurrentUserID
        {
            get { return _userid; }
            set { _userid = value; }
        }
    }
}
using System.Web.UI;

namespace Arsenalcn.ClubSys.Web.Common
{
    public class CollectionBase : UserControl
    {
        private int _profileUserID = -1;
        public int ProfileUserID
        {
            get
            {
                return _profileUserID;
            }
            set
            {
                _profileUserID = value;
            }
        }

        protected int _userid = -1;
        public int CurrentUserID
        {
            get
            {
                return _userid;
            }
            set
            {
                _userid = value;
            }
        }
    }
}

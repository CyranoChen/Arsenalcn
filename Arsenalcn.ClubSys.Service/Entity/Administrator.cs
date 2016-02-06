using System.Data;

namespace Arsenalcn.ClubSys.Entity
{
    public class Administrator : ClubSysObject
    {
        public Administrator()
        {
        }

        public Administrator(DataRow dr)
            : base(dr)
        {
        }

        #region Members and Properties

        /// <summary>
        ///     User ID
        /// </summary>
        [ClubSysDbColumn("UserID")]
        public int UserID { get; set; }

        /// <summary>
        ///     UserName
        /// </summary>
        [ClubSysDbColumn("UserName")]
        public string UserName { get; set; } = string.Empty;

        #endregion
    }
}
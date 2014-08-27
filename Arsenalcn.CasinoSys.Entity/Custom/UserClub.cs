using System;
using System.Data;

namespace Arsenalcn.CasinoSys.Entity
{
    public class UserClub
    {
        public static DataTable GetAllClubs()
        {
            return DataAccess.UserClub.GetAllClubs();
        }

        public static DataRow GetUserClubHistoryInfo(int userID, DateTime betTime)
        {
            return DataAccess.UserClub.GetUserClubHistoryInfo(userID, betTime);
        }
    }
}

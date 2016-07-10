using System;
using System.Data;

namespace Arsenalcn.CasinoSys.Entity
{
    public static class UserClub
    {
        public static DataTable GetAllClubs()
        {
            return DataAccess.UserClub.GetAllClubs();
        }

        public static DataRow GetUserClubHistoryInfo(int id, DateTime betTime)
        {
            return DataAccess.UserClub.GetUserClubHistoryInfo(id, betTime);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace iArsenal.Service
{
    public interface IMatchTicket
    {
        void Single();
        bool Any();

        List<MatchTicket> All();

        void Create(SqlTransaction trans = null);

        void Update(SqlTransaction trans = null);

        void Delete(SqlTransaction trans = null);

        bool CheckMemberCanPurchase(MemberPeriod mp);

        void MatchTicketCountStatistics();
    }
}

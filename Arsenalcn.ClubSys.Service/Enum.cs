using System;
using System.Data;
using System.Configuration;
using System.Web;

namespace Arsenalcn.ClubSys.Service
{
    public enum Responsibility
    {
        Manager = 1,
        Executor = 10,
        Member = 20
    }

    public enum UserClubStatus
    {
        Member = 2,
        Applied = 1,
        No = 0
    }
}

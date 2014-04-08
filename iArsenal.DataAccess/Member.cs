using System;
using System.Data;
using System.Data.SqlClient;

using Microsoft.ApplicationBlocks.Data;

namespace iArsenal.DataAccess
{
    public class Member
    {
        public static DataRow GetMemberByID(int mID)
        {
            string sql = "SELECT * FROM dbo.iArsenal_Member WHERE ID = @mID";

            DataSet ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql, new SqlParameter("@mID", mID));

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            else
                return ds.Tables[0].Rows[0];
        }

        public static DataRow GetMemberByAcnID(int acnID)
        {
            string sql = "SELECT * FROM dbo.iArsenal_Member WHERE AcnID = @acnID";

            DataSet ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql, new SqlParameter("@acnID", acnID));

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            else
                return ds.Tables[0].Rows[0];
        }

        public static void UpdateMember(int mID, string name, Boolean gender, DateTime? birthday, string career, string nation, string region, string mobile, string telephone, string address, string email, string zipcode, string msn, string qq, string IDCardNo, string passportNo, string passportName, int acnID, string acnName, string ip, string taobaoName, int memberType, string memberCardNo, DateTime joinDate, DateTime lastLoginTime, Boolean isActive, string description, string remark)
        {
            string sql = @"UPDATE dbo.iArsenal_Member SET [Name] = @name, Gender = @gender, Birthday = @birthday, Career = @career, Nation = @nation, Region = @region, Mobile = @mobile, Telephone = @telephone, [Address] = @address, Email = @email, Zipcode = @zipcode, MSN = @msn, QQ = @qq, IDCardNo = @IDCardNo,  
                               PassportNo = @passportNo, PassportName = @passportName, AcnID = @acnID, AcnName = @acnName, IP = @ip, TaobaoName = @taobaoName, MemberType = @memberType, MemberCardNo = @memberCardNo, JoinDate = @joinDate, LastLoginTime = @lastLoginTime, IsActive = @isActive, [Description] = @description, Remark = @remark WHERE ID = @mID";

            SqlParameter[] para = { 
                                      new SqlParameter("@mID", mID),
                                      new SqlParameter("@name", name),
                                      new SqlParameter("@gender", gender),
                                      new SqlParameter("@birthday", !birthday.HasValue ? (object)DBNull.Value : (object)birthday.Value),
                                      new SqlParameter("@career", career),
                                      new SqlParameter("@nation", nation),
                                      new SqlParameter("@region", region),
                                      new SqlParameter("@mobile", mobile),
                                      new SqlParameter("@telephone", telephone),
                                      new SqlParameter("@address", address),
                                      new SqlParameter("@email", email),
                                      new SqlParameter("@zipcode", zipcode),
                                      new SqlParameter("@msn", msn),
                                      new SqlParameter("@qq", qq),
                                      new SqlParameter("@IDCardNo", IDCardNo),
                                      new SqlParameter("@passportNo", passportNo),
                                      new SqlParameter("@PassportName", passportName),
                                      new SqlParameter("@acnID", acnID),
                                      new SqlParameter("@acnName", acnName),
                                      new SqlParameter("@ip", ip),
                                      new SqlParameter("@taobaoName", taobaoName),
                                      new SqlParameter("@memberType", memberType),
                                      new SqlParameter("@memberCardNo", memberCardNo),
                                      new SqlParameter("@joinDate", joinDate),
                                      new SqlParameter("@lastLoginTime", lastLoginTime),
                                      new SqlParameter("@isActive", isActive),
                                      new SqlParameter("@description", description),
                                      new SqlParameter("@remark", remark)
                                  };

            SqlHelper.ExecuteNonQuery(SQLConn.GetConnection(), CommandType.Text, sql, para);
        }

        public static void InsertMember(int mID, string name, Boolean gender, DateTime? birthday, string career, string nation, string region, string mobile, string telephone, string address, string email, string zipcode, string msn, string qq, string IDCardNo, string passportNo, string passportName, int acnID, string acnName, string ip, string taobaoName, int memberType, string memberCardNo, DateTime joinDate, DateTime lastLoginTime, Boolean isActive, string description, string remark)
        {
            string sql = @"INSERT INTO dbo.iArsenal_Member ([Name], Gender, Birthday, Career, Nation, Region, Mobile, Telephone, [Address], Email, Zipcode, MSN, QQ, IDCardNo, PassportNo, PassportName, AcnID, AcnName, IP, TaobaoName, MemberType, MemberCardNo, JoinDate, LastLoginTime, IsActive, [Description], Remark) 
                               VALUES (@name, @gender, @birthday, @career, @nation, @region, @mobile, @telephone, @address, @email, @zipcode, @msn, @qq, @IDCardNo, @passportNo, @passportName, @acnID, @acnName, @ip, @taobaoName, @memberType, @memberCardNo, @joinDate, @lastLoginTime, @isActive, @description, @remark)";

            SqlParameter[] para = { 
                                      new SqlParameter(),
                                      new SqlParameter("@name", name),
                                      new SqlParameter("@gender", gender),
                                      new SqlParameter("@birthday", !birthday.HasValue ? (object)DBNull.Value : (object)birthday.Value),
                                      new SqlParameter("@career", career),
                                      new SqlParameter("@nation", nation),
                                      new SqlParameter("@region", region),
                                      new SqlParameter("@mobile", mobile),
                                      new SqlParameter("@telephone", telephone),
                                      new SqlParameter("@address", address),
                                      new SqlParameter("@email", email),
                                      new SqlParameter("@zipcode", zipcode),
                                      new SqlParameter("@msn", msn),
                                      new SqlParameter("@qq", qq),
                                      new SqlParameter("@IDCardNo", IDCardNo),
                                      new SqlParameter("@passportNo", passportNo),
                                      new SqlParameter("@PassportName", passportName),
                                      new SqlParameter("@acnID", acnID),
                                      new SqlParameter("@acnName", acnName),
                                      new SqlParameter("@ip", ip),
                                      new SqlParameter("@taobaoName", taobaoName),
                                      new SqlParameter("@memberType", memberType),
                                      new SqlParameter("@memberCardNo", memberCardNo),
                                      new SqlParameter("@joinDate", joinDate),
                                      new SqlParameter("@lastLoginTime", lastLoginTime),
                                      new SqlParameter("@isActive", isActive),
                                      new SqlParameter("@description", description),
                                      new SqlParameter("@remark", remark)
                                  };

            SqlHelper.ExecuteNonQuery(SQLConn.GetConnection(), CommandType.Text, sql, para);
        }

        public static void DeleteMember(int mID)
        {
            string sql = "DELETE dbo.iArsenal_Member WHERE ID = @mID";

            SqlParameter[] para = { new SqlParameter("@mID", mID) };

            SqlHelper.ExecuteNonQuery(SQLConn.GetConnection(), CommandType.Text, sql, para);
        }

        public static DataTable GetMembers()
        {
            string sql = @"SELECT ID, Name, Gender, Birthday, Career, Nation, Region, Mobile, Telephone, Address, Email, Zipcode, MSN, QQ, IDCardNo, PassportNo, PassportName, AcnID, AcnName,  
                               IP, TaobaoName, MemberType, MemberCardNo, JoinDate, LastLoginTime, IsActive, Description, Remark FROM iArsenal_Member ORDER BY ID DESC";

            DataSet ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql);

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            else
                return ds.Tables[0];
        }
    }
}

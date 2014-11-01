using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Arsenalcn.ClubSys.Entity
{
    public class UserVideo
    {
        public UserVideo() { }

        private UserVideo(DataRow dr)
        {
            InitUserVideo(dr);
        }

        private void InitUserVideo(DataRow dr)
        {
            if (dr != null)
            {
                UserVideoID = Convert.ToInt32(dr["ID"]);
                UserID = Convert.ToInt32(dr["UserID"]);
                UserName = Convert.ToString(dr["UserName"]);
                VideoGuid = (Guid)dr["VideoGuid"];
                ActiveDate = Convert.ToDateTime(dr["ActiveDate"]);
                UserDesc = dr["UserDesc"].ToString();
                IsPublic = Convert.ToBoolean(dr["IsPublic"]);
            }
            else
                throw new Exception("Unable to init UserVideo.");
        }

        public void Select()
        {
            DataRow dr = Service.UserVideo.GetUserVideoByID(UserVideoID);

            if (dr != null)
                InitUserVideo(dr);
        }

        public void Update(SqlTransaction trans = null)
        {
            Service.UserVideo.UpdateUserVideo(UserVideoID, UserID, UserName, VideoGuid, ActiveDate, UserDesc, IsPublic, trans);
        }

        public void Insert(SqlTransaction trans = null)
        {
            Service.UserVideo.InsertUserVideo(UserVideoID, UserID, UserName, VideoGuid, ActiveDate, UserDesc, IsPublic, trans);
        }

        public void Delete(SqlTransaction trans = null)
        {
            Service.UserVideo.DeleteUserVideo(UserVideoID, trans);
        }

        public static List<UserVideo> GetUserVideos()
        {
            DataTable dt = Service.UserVideo.GetUserVideos();
            List<UserVideo> list = new List<UserVideo>();

            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(new UserVideo(dr));
                }
            }

            return list;
        }

        public static List<UserVideo> GetUserVideosByUserID(int userID)
        {
            return GetUserVideos().FindAll(delegate(UserVideo v) { return v.UserID.Equals(userID); });
        }

        public static List<UserVideo> GetUserVideosByClubID(int clubID)
        {
            DataTable dt = Service.UserVideo.GetUserVideoByClubID(clubID);
            List<UserVideo> list = new List<UserVideo>();

            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(new UserVideo(dr));
                }
            }

            return list;
        }

        #region Members and Properties

        public int UserVideoID
        { get; set; }

        public int UserID
        { get; set; }

        public string UserName
        { get; set; }

        public Guid VideoGuid
        { get; set; }

        public DateTime ActiveDate
        { get; set; }

        public string UserDesc
        { get; set; }

        public bool IsPublic
        { get; set; }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Contracts;

namespace Arsenalcn.Core.Scheduler
{
    [AttrDbTable("Arsenalcn_Schedule", Key = "ScheduleKey", Sort = "IsSystem, ScheduleKey")]
    public class Schedule
    {
        public Schedule() { }

        private Schedule(DataRow dr)
        {
            Contract.Requires(dr != null);

            Init(dr);
        }

        private void Init(DataRow dr)
        {
            if (dr != null)
            {
                ScheduleKey = dr["ScheduleKey"].ToString();
                ScheduleType = dr["ScheduleType"].ToString();
                DailyTime = Convert.ToInt32(dr["DailyTime"]);

                Minutes = Convert.ToInt32(dr["Minutes"]);
                Minutes = Minutes < ScheduleManager.TimerMinutesInterval ? ScheduleManager.TimerMinutesInterval : Minutes;

                LastCompletedTime = Convert.ToDateTime(dr["LastCompletedTime"]);
                IsSystem = Convert.ToBoolean(dr["IsSystem"]);
                IsActive = Convert.ToBoolean(dr["IsActive"]);
                Remark = dr["Remark"].ToString();

                #region Generate ExecuteTimeInfo

                if (DailyTime >= 0)
                {
                    ExecuteTimeInfo = string.Format("定时执行：{0}时{1}分",
                        (DailyTime / 60).ToString(), (DailyTime % 60).ToString());
                }
                else
                {
                    ExecuteTimeInfo = string.Format("轮询执行：{0}分钟", Minutes.ToString());
                }

                #endregion
            }
            else
            { throw new Exception("Unable to init Schedule"); }
        }

        public void Single()
        {
            string sql = string.Format("SELECT * FROM {0} WHERE ScheduleKey = @key",
                Repository.GetTableAttr<Schedule>().Name);

            SqlParameter[] para = { new SqlParameter("@key", ScheduleKey) };

            DataSet ds = DataAccess.ExecuteDataset(sql, para);

            if (ds.Tables[0].Rows.Count > 0) { Init(ds.Tables[0].Rows[0]); }
        }

        public bool Any()
        {
            string sql = string.Format("SELECT * FROM {0} WHERE ScheduleKey = @key",
                Repository.GetTableAttr<Schedule>().Name);

            SqlParameter[] para = { new SqlParameter("@key", ScheduleKey) };

            DataSet ds = DataAccess.ExecuteDataset(sql, para);

            return ds.Tables[0].Rows.Count > 0;
        }

        public static List<Schedule> All()
        {
            var attr = Repository.GetTableAttr<Schedule>();

            var list = new List<Schedule>();

            string sql = string.Format("SELECT * FROM {0} ORDER BY {1}", attr.Name, attr.Sort);

            DataSet ds = DataAccess.ExecuteDataset(sql);

            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    list.Add(new Schedule(dr));
                }
            }

            return list;
        }

        public void Update(SqlTransaction trans = null)
        {
            Contract.Requires(this.Any());

            string sql = string.Format(@"UPDATE {0} SET ScheduleType = @scheduleType, DailyTime = @dailyTime, Minutes = @minutes, 
                             LastCompletedTime = @lastCompletedTime, IsSystem = @isSystem, IsActive = @isActive, Remark = @remark 
                             WHERE ScheduleKey = @key", Repository.GetTableAttr<Schedule>().Name);

            SqlParameter[] para = { 
                                      new SqlParameter("@scheduleType", ScheduleType), 
                                      new SqlParameter("@dailyTime", DailyTime), 
                                      new SqlParameter("@minutes", Minutes), 
                                      new SqlParameter("@lastCompletedTime", LastCompletedTime), 
                                      new SqlParameter("@isSystem", IsSystem), 
                                      new SqlParameter("@isActive", IsActive), 
                                      new SqlParameter("@remark", Remark), 
                                      new SqlParameter("@key", ScheduleKey) };

            DataAccess.ExecuteNonQuery(sql, para, trans);
        }

        //public static class Cache
        //{
        //    static Cache()
        //    {
        //        InitCache();
        //    }

        //    public static void RefreshCache()
        //    {
        //        InitCache();
        //    }

        //    private static void InitCache()
        //    {
        //        ScheduleList = All();
        //    }

        //    public static Schedule Load(string key)
        //    {
        //        return ScheduleList.Find(x => x.ScheduleKey.Equals(key, StringComparison.OrdinalIgnoreCase));
        //    }

        //    public static List<Schedule> ScheduleList;
        //}

        #region Members and Properties

        [AttrDbColumn("ScheduleKey", Key = true)]
        public string ScheduleKey
        { get; set; }

        /// <summary>
        /// The Type of class which implements IScheduler
        /// </summary>
        [AttrDbColumn("ScheduleType")]
        public string ScheduleType
        { get; set; }

        /// <summary>
        /// Absolute time in mintues from midnight. Can be used to assure event is only 
        /// executed once per-day and as close to the specified
        /// time as possible. Example times: 0 = midnight, 27 = 12:27 am, 720 = Noon
        /// </summary>
        [AttrDbColumn("DailyTime")]
        public int DailyTime
        { get; set; }

        /// <summary>
        /// The scheduled event interval time in minutes. If TimeOfDay has a value >= 0, Minutes will be ignored. 
        /// This values should not be less than the Timer interval.
        /// </summary>
        [AttrDbColumn("Minutes")]
        public int Minutes
        { get; set; }

        /// <summary>
        /// Last Date and Time this scheduler was processed/completed.
        /// </summary>
        [AttrDbColumn("LastCompletedTime")]
        public DateTime LastCompletedTime
        { get; set; }

        //public DateTime LastCompletedTime
        //{
        //    get { return LastCompletedTime; }
        //    set
        //    {
        //        dateWasSet = true;
        //        LastCompletedTime = value;
        //    }
        //}

        //internal testing variable
        //bool dateWasSet = false;

        [AttrDbColumn("IsSystem")]
        public bool IsSystem
        { get; set; }

        [AttrDbColumn("IsActive")]
        public bool IsActive
        { get; set; }

        [AttrDbColumn("Remark")]
        public string Remark
        { get; set; }

        public string ExecuteTimeInfo
        { get; set; }

        #endregion

        private ISchedule _ischedule = null;
        /// <summary>
        /// The current implementation of IScheduler
        /// </summary>
        public ISchedule IScheduleInstance
        {
            get
            {
                InitISchedule();
                return _ischedule;
            }
        }

        /// <summary>
        /// Private method for loading an instance of ISchedule
        /// </summary>
        private void InitISchedule()
        {
            if (_ischedule == null)
            {
                if (this.ScheduleType == null)
                {
                    //SchedulerLogs.WriteFailedLog("计划任务没有定义其 type 属性");
                }

                Type type = Type.GetType(this.ScheduleType);

                if (type == null)
                {
                    //SchedulerLogs.WriteFailedLog(string.Format("计划任务 {0} 无法被正确识别", this.ScheduleType));
                }
                else
                {
                    _ischedule = (ISchedule)Activator.CreateInstance(type);

                    if (_ischedule == null)
                    {
                        //SchedulerLogs.WriteFailedLog(string.Format("计划任务 {0} 未能正确加载", this.ScheduleType));
                    }
                }
            }
        }

        public bool ShouldExecute()
        {
            //If we have a TimeOfDay value, use it and ignore the Minutes interval
            if (this.DailyTime > -1)
            {
                //Now
                DateTime dtNow = DateTime.Now;  //now
                //We are looking for the current day @ 12:00 am
                DateTime dtCompare = new DateTime(dtNow.Year, dtNow.Month, dtNow.Day);
                //Check to see if the LastCompleted date is less than the 12:00 am + TimeOfDay minutes
                return LastCompletedTime < dtCompare.AddMinutes(this.DailyTime) && dtCompare.AddMinutes(this.DailyTime) <= DateTime.Now;

            }
            else
            {
                //Is the LastCompleted date + the Minutes interval less than now?
                return LastCompletedTime.AddMinutes(this.Minutes) < DateTime.Now;
            }
        }
    }
}

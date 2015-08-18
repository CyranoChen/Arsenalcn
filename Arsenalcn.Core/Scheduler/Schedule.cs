using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Arsenalcn.Core.Scheduler
{
    [DbSchema("Arsenalcn_Schedule", Key = "ScheduleKey", Sort = "IsSystem, ScheduleKey")]
    public class Schedule
    {
        public Schedule() { }

        private static void CreateMap()
        {
            var map = AutoMapper.Mapper.CreateMap<IDataReader, Schedule>();

            map.ForMember(d => d.ScheduleKey, opt => opt.MapFrom(s => s.GetValue("ScheduleKey").ToString()));

            map.ForMember(d => d.Minutes, opt => opt.ResolveUsing(s =>
            {
                var mins = (int)s.GetValue("Minutes");
                if (mins > 0 & mins < ScheduleManager.TimerMinutesInterval)
                {
                    return ScheduleManager.TimerMinutesInterval;
                }
                else
                {
                    return mins;
                }
            }));

            map.ForMember(d => d.ExecuteTimeInfo, opt => opt.ResolveUsing(s =>
            {
                var dailyTime = (int)s.GetValue("DailyTime");

                if (dailyTime >= 0)
                {
                    return string.Format("Run at {0}:{1}", dailyTime / 60, dailyTime % 60);
                }
                else
                {
                    return string.Format("Run By {0} mins", s.GetValue("Minutes").ToString());
                }
            }));
        }

        //private Schedule(DataRow dr)
        //{
        //    Contract.Requires(dr != null);

        //    Init(dr);
        //}

        //private void Init(DataRow dr)
        //{
        //    if (dr != null)
        //    {
        //        ScheduleKey = dr["ScheduleKey"].ToString();
        //        ScheduleType = dr["ScheduleType"].ToString();
        //        DailyTime = Convert.ToInt32(dr["DailyTime"]);

        //        Minutes = Convert.ToInt32(dr["Minutes"]);

        //        if (Minutes > 0 & Minutes < ScheduleManager.TimerMinutesInterval)
        //        {
        //            Minutes = ScheduleManager.TimerMinutesInterval;
        //        }

        //        LastCompletedTime = Convert.ToDateTime(dr["LastCompletedTime"]);
        //        IsSystem = Convert.ToBoolean(dr["IsSystem"]);
        //        IsActive = Convert.ToBoolean(dr["IsActive"]);
        //        Remark = dr["Remark"].ToString();

        //        #region Generate ExecuteTimeInfo

        //        if (DailyTime >= 0)
        //        {
        //            ExecuteTimeInfo = string.Format("Run at {0}:{1}",
        //                (DailyTime / 60).ToString(), (DailyTime % 60).ToString());
        //        }
        //        else
        //        {
        //            ExecuteTimeInfo = string.Format("Run By {0} mins", Minutes.ToString());
        //        }

        //        #endregion
        //    }
        //    else
        //    { throw new Exception("Unable to init Schedule"); }
        //}

        public static Schedule Single(object key)
        {
            string sql = string.Format("SELECT * FROM {0} WHERE ScheduleKey = @key",
                Repository.GetTableAttr<Schedule>().Name);

            SqlParameter[] para = { new SqlParameter("@key", key) };

            DataSet ds = DataAccess.ExecuteDataset(sql, para);

            var dt = ds.Tables[0];

            if (dt.Rows.Count == 0) { return null; }

            using (var reader = dt.CreateDataReader())
            {
                return AutoMapper.Mapper.Map<IDataReader, IEnumerable<Schedule>>(reader).FirstOrDefault();
            }
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

            var dt = ds.Tables[0];

            if (dt.Rows.Count > 0)
            {
                using (var reader = dt.CreateDataReader())
                {
                    Schedule.CreateMap();

                    list = AutoMapper.Mapper.Map<IDataReader, List<Schedule>>(reader);
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

        #region Members and Properties

        [DbColumn("ScheduleKey", IsKey = true)]
        public string ScheduleKey
        { get; set; }

        /// <summary>
        /// The Type of class which implements IScheduler
        /// </summary>
        [DbColumn("ScheduleType")]
        public string ScheduleType
        { get; set; }

        /// <summary>
        /// Absolute time in mintues from midnight. Can be used to assure event is only 
        /// executed once per-day and as close to the specified
        /// time as possible. Example times: 0 = midnight, 27 = 12:27 am, 720 = Noon
        /// </summary>
        [DbColumn("DailyTime")]
        public int DailyTime
        { get; set; }

        /// <summary>
        /// The scheduled event interval time in minutes. If TimeOfDay has a value >= 0, Minutes will be ignored. 
        /// This values should not be less than the Timer interval.
        /// </summary>
        [DbColumn("Minutes")]
        public int Minutes
        { get; set; }

        /// <summary>
        /// Last Date and Time this scheduler was processed/completed.
        /// </summary>
        [DbColumn("LastCompletedTime")]
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

        [DbColumn("IsSystem")]
        public bool IsSystem
        { get; set; }

        [DbColumn("IsActive")]
        public bool IsActive
        { get; set; }

        [DbColumn("Remark")]
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

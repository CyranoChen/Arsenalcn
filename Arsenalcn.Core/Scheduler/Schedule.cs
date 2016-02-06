using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Contracts;
using System.Linq;
using DataReaderMapper;

namespace Arsenalcn.Core.Scheduler
{
    [DbSchema("Arsenalcn_Schedule", Key = "ScheduleKey", Sort = "IsSystem, ScheduleKey")]
    public class Schedule
    {
        private ISchedule _ischedule;

        /// <summary>
        ///     The current implementation of IScheduler
        /// </summary>
        public ISchedule IScheduleInstance
        {
            get
            {
                InitISchedule();
                return _ischedule;
            }
        }

        private static void CreateMap()
        {
            var map = Mapper.CreateMap<IDataReader, Schedule>();

            map.ForMember(d => d.ScheduleKey, opt => opt.MapFrom(s => s.GetValue("ScheduleKey").ToString()));

            map.ForMember(d => d.Minutes, opt => opt.ResolveUsing(s =>
            {
                var mins = (int) s.GetValue("Minutes");
                if (mins > 0 & mins < ScheduleManager.TimerMinutesInterval)
                {
                    return ScheduleManager.TimerMinutesInterval;
                }
                return mins;
            }));

            map.ForMember(d => d.ExecuteTimeInfo, opt => opt.ResolveUsing(s =>
            {
                var dailyTime = (int) s.GetValue("DailyTime");

                if (dailyTime >= 0)
                {
                    return $"Run at {dailyTime/60}:{dailyTime%60}";
                }
                return $"Run By {s.GetValue("Minutes").ToString()} mins";
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
            var sql = $"SELECT * FROM {Repository.GetTableAttr<Schedule>().Name} WHERE ScheduleKey = @key";

            SqlParameter[] para = {new SqlParameter("@key", key)};

            var ds = DataAccess.ExecuteDataset(sql, para);

            var dt = ds.Tables[0];

            if (dt.Rows.Count == 0)
            {
                return null;
            }

            using (var reader = dt.CreateDataReader())
            {
                return Mapper.Map<IDataReader, IEnumerable<Schedule>>(reader).FirstOrDefault();
            }
        }

        public bool Any()
        {
            var sql = $"SELECT * FROM {Repository.GetTableAttr<Schedule>().Name} WHERE ScheduleKey = @key";

            SqlParameter[] para = {new SqlParameter("@key", ScheduleKey)};

            var ds = DataAccess.ExecuteDataset(sql, para);

            return ds.Tables[0].Rows.Count > 0;
        }

        public static List<Schedule> All()
        {
            var attr = Repository.GetTableAttr<Schedule>();

            var list = new List<Schedule>();

            var sql = $"SELECT * FROM {attr.Name} ORDER BY {attr.Sort}";

            var ds = DataAccess.ExecuteDataset(sql);

            var dt = ds.Tables[0];

            if (dt.Rows.Count > 0)
            {
                using (var reader = dt.CreateDataReader())
                {
                    CreateMap();

                    list = Mapper.Map<IDataReader, List<Schedule>>(reader);
                }
            }

            return list;
        }

        public void Update(SqlTransaction trans = null)
        {
            Contract.Requires(Any());

            var sql =
                $@"UPDATE {Repository.GetTableAttr<Schedule>().Name
                    } SET ScheduleType = @scheduleType, DailyTime = @dailyTime, Minutes = @minutes, 
                             LastCompletedTime = @lastCompletedTime, IsSystem = @isSystem, IsActive = @isActive, Remark = @remark 
                             WHERE ScheduleKey = @key";

            SqlParameter[] para =
            {
                new SqlParameter("@scheduleType", ScheduleType),
                new SqlParameter("@dailyTime", DailyTime),
                new SqlParameter("@minutes", Minutes),
                new SqlParameter("@lastCompletedTime", LastCompletedTime),
                new SqlParameter("@isSystem", IsSystem),
                new SqlParameter("@isActive", IsActive),
                new SqlParameter("@remark", Remark),
                new SqlParameter("@key", ScheduleKey)
            };

            DataAccess.ExecuteNonQuery(sql, para, trans);
        }

        /// <summary>
        ///     Private method for loading an instance of ISchedule
        /// </summary>
        private void InitISchedule()
        {
            if (_ischedule == null)
            {
                if (ScheduleType == null)
                {
                    //SchedulerLogs.WriteFailedLog("计划任务没有定义其 type 属性");
                }

                var type = Type.GetType(ScheduleType);

                if (type == null)
                {
                    //SchedulerLogs.WriteFailedLog(string.Format("计划任务 {0} 无法被正确识别", this.ScheduleType));
                }
                else
                {
                    _ischedule = (ISchedule) Activator.CreateInstance(type);

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
            if (DailyTime > -1)
            {
                //Now
                var dtNow = DateTime.Now; //now
                //We are looking for the current day @ 12:00 am
                var dtCompare = new DateTime(dtNow.Year, dtNow.Month, dtNow.Day);
                //Check to see if the LastCompleted date is less than the 12:00 am + TimeOfDay minutes
                return LastCompletedTime < dtCompare.AddMinutes(DailyTime) &&
                       dtCompare.AddMinutes(DailyTime) <= DateTime.Now;
            }
            //Is the LastCompleted date + the Minutes interval less than now?
            return LastCompletedTime.AddMinutes(Minutes) < DateTime.Now;
        }

        #region Members and Properties

        [DbColumn("ScheduleKey", IsKey = true)]
        public string ScheduleKey { get; set; }

        /// <summary>
        ///     The Type of class which implements IScheduler
        /// </summary>
        [DbColumn("ScheduleType")]
        public string ScheduleType { get; set; }

        /// <summary>
        ///     Absolute time in mintues from midnight. Can be used to assure event is only
        ///     executed once per-day and as close to the specified
        ///     time as possible. Example times: 0 = midnight, 27 = 12:27 am, 720 = Noon
        /// </summary>
        [DbColumn("DailyTime")]
        public int DailyTime { get; set; }

        /// <summary>
        ///     The scheduled event interval time in minutes. If TimeOfDay has a value >= 0, Minutes will be ignored.
        ///     This values should not be less than the Timer interval.
        /// </summary>
        [DbColumn("Minutes")]
        public int Minutes { get; set; }

        /// <summary>
        ///     Last Date and Time this scheduler was processed/completed.
        /// </summary>
        [DbColumn("LastCompletedTime")]
        public DateTime LastCompletedTime { get; set; }

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
        public bool IsSystem { get; set; }

        [DbColumn("IsActive")]
        public bool IsActive { get; set; }

        [DbColumn("Remark")]
        public string Remark { get; set; }

        public string ExecuteTimeInfo { get; set; }

        #endregion
    }
}
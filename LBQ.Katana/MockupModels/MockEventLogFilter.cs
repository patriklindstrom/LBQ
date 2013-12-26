using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LBQ.Katana.Model;

namespace LBQ.Katana.MockupModels
{
    internal class MockEventLogFilter : IEventLogFilter
    {
        public string Title { get; set; }
        public DateTime LastRefreshedTime { get; set; }
        public IQueryable<ILogFilterRow> LogRows { get; set; }
        public DateTime FromDateTime { get; set; }
        public DateTime ToDateTime { get; set; }

        public MockEventLogFilter()
        {
            LastRefreshedTime = DateTime.UtcNow;
            ToDateTime = DateTime.UtcNow;
            FromDateTime = ToDateTime.AddHours(-6);
            Title = "MockEventLogFilter";

            LogRows = new List<LogFilterRow>
            {
                new LogFilterRow
                {
                    Time = DateTime.UtcNow.AddMinutes(-5),
                    LogType = "EventRec",
                    Msg = "TCM BatchId= 12345; TCM good",
                    SearchT = "TCM",
                    Src = "ABC_Server1\\EventRec\\Application"
                },
                new LogFilterRow
                {
                    Time = DateTime.UtcNow.AddMinutes(-6),
                    LogType = "EventRec",
                    Msg = "TCM BatchId= 12346; TCM still good",
                    SearchT = "TCM",
                    Src = "ABC_Server2\\EventRec\\Application"
                },
                new LogFilterRow
                {
                    Time = DateTime.UtcNow.AddMinutes(-7),
                    LogType = "EventRec",
                    Msg = "TCM BatchId= 12347; TCM feels a little bad like Error=6658423",
                    SearchT = "Error",
                    Src = "ABC_Server1\\EventRec\\System"
                },
                new LogFilterRow
                {
                    Time = DateTime.UtcNow.AddMilliseconds(-200),
                    LogType = "EventRec",
                    Msg = "TCM BatchId= 12347; TCM good guitar",
                    SearchT = "TCM",
                    Src = "ABC_Server2\\EventRec\\Application"
                }
            }.AsQueryable();
        }
    }
}
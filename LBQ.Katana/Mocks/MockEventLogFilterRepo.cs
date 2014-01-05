using System;
using LBQ.Katana.Model;

namespace LBQ.Katana.Mocks
{
    class MockEventLogFilterRepo : ILogFilterRepo
    {
        public ILogFilter LogFilter { get; set; }
        public MockEventLogFilterRepo(ILogFilter modelFilter)
        {
            LogFilter = modelFilter;
        }
        public ILogFilter GetData(DateTime fromTime, DateTime toTime)
        {
            return LogFilter;
        }
    }
}
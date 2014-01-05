using System;
using System.Management;

namespace LBQ.Katana.Mocks
{
    class MockEventRecordTimeSpanSearcher : IEventRecordTimeSpanSearcher
    {
        public ManagementObjectCollection GetEventsCollection(DateTime fromTime, DateTime toTime)
        {
            throw new NotImplementedException();
        }
    }
}
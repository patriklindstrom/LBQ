using System;
using System.Management;

namespace LBQ.Katana
{
    public interface IEventRecordTimeSpanSearcher
    {
        ManagementObjectCollection GetEventsCollection(DateTime fromTime, DateTime toTime);
    }
}
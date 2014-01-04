using System.Management;

namespace LBQ.Katana
{
    public interface IEventRecordTimeSpanSearcher
    {
        ManagementObjectCollection EventsCollection { get; set; }
    }
}
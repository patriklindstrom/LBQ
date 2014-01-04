using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace LBQ.Katana
{
    public class EventRecordTimeSpanSearcher : IEventRecordTimeSpanSearcher
    {
        public List<string> ServerList { get; set; }
        public EventRecordTimeSpanSearcher(ISettingsProvider settingsProvider)
        {
            ServerList = settingsProvider.GetListOfServersToQuery();
        }

        public ManagementObjectCollection GetEventsCollection(DateTime fromTime, DateTime toTime)
        {
            ManagementObjectCollection mngmtObjectCollection = null;
            foreach (var serv in ServerList)
            {           
            string strFromTime = ManagementDateTimeConverter.ToDmtfDateTime(fromTime);// DateTime outDateTime   String.Format(Global_Const.DATE_FORMAT_STR, fromTime) + ".000000+000";
            string strToTime = ManagementDateTimeConverter.ToDmtfDateTime(toTime);// String.Format(Global_Const.DATE_FORMAT_STR, toTime) + ".000000+000";
            string wmiQuery =
    String.Format(
        "SELECT * FROM Win32_NTLogEvent WHERE Logfile = '{0}' AND TimeGenerated >= '{1}' AND TimeGenerated <= '{2}' ",
        Global_Const.SOURCE, strFromTime, strToTime);
            ManagementObjectSearcher mos;
            mos = new ManagementObjectSearcher("\\\\" + serv + "\\root\\cimv2", wmiQuery);
           mngmtObjectCollection = mos.Get();

            }
            return mngmtObjectCollection;
        }

    }
}

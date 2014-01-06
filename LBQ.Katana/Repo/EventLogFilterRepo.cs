using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Management;
using LBQ.Katana.Model;

namespace LBQ.Katana.Repo
{
 
    class EventLogFilterRepo:ILogFilterRepo
    {
        public List<string> ListOfServersToQuery { get; set; }
        public List<string> SearchTermsList { get; set; }
        public ICacheLayer EventLogCache { get; set; }
        public IEventRecordTimeSpanSearcher EventRTimeSearcher { get; set; }
        public EventLogFilterRepo(ISettingsProvider settingsProvider, IEventRecordTimeSpanSearcher eventRecordTimeSpanSearcher, ICacheLayer sweetCacheLayer)
        {
            EventLogCache = sweetCacheLayer;
            ListOfServersToQuery = settingsProvider.GetListOfServersToQuery();
            SearchTermsList = settingsProvider.GetListOfSearchTerms();
            EventRTimeSearcher = eventRecordTimeSpanSearcher;
        }
        public ILogFilter GetData(DateTime fTime, DateTime tTime )
        {
            EventLogFilter eventLogFilter;
           var cacheKey = fTime.ToUniversalTime() + "-" + tTime.ToUniversalTime();
            if (EventLogCache.Exists(cacheKey))
            {
                eventLogFilter = EventLogCache.Get<EventLogFilter>(cacheKey);
            }
            else
            {
                eventLogFilter = GetEventLogFilter(fTime, tTime);
                EventLogCache.Add(GetEventLogFilter(fTime, tTime), cacheKey);
            }
            return eventLogFilter;
        }

        public EventLogFilter GetEventLogFilter(DateTime fTime, DateTime tTime)
        {
            var eventRecordList = GetEventRecordList(EventRTimeSearcher.GetEventsCollection(fromTime: fTime, toTime: tTime),
                SearchTermsList);
            var eventLogFilter = new EventLogFilter
            {
                FromDateTime = fTime,
                ToDateTime = tTime,
                LastRefreshedTime = DateTime.Now,
                LogRows = eventRecordList.AsQueryable(),
                Title = Global_Const.EVENTLOGTITLE
            };
            return eventLogFilter;
        }

        public List<LogFilterRow> GetEventRecordList(ManagementObjectCollection mossos, List<string> searchTermList)
        {
            var eventRecordList = new List<LogFilterRow>();
            int sTCount = searchTermList.Count;
            foreach (var serv in ListOfServersToQuery)
            {
                var insertionStrings = String.Empty;
                foreach (var mo in mossos)
                {
                    var eventRec = new LogFilterRow();
                    string timeWrittenStr = (mo["TimeWritten"] != null) ? mo["TimeWritten"].ToString() : String.Empty;
                    eventRec.Time = ManagementDateTimeConverter.ToDateTime(timeWrittenStr);
                    // DateTime.ParseExact(timeWrittenStr, "yyyyMMddHHmmss.ffffff-zzz", provider);            
                    eventRec.LogType = Global_Const.EVENTLOGTYPE;
                    eventRec.Src = serv + "\\EventLog\\" + Global_Const.SOURCE;
                    string sourceName = (mo["SourceName"] != null) ? mo["SourceName"].ToString() : String.Empty;
                    string message = (mo["Message"] != null) ? mo["Message"].ToString() : String.Empty;
                    if (mo["InsertionStrings"] != null)
                    {
                        var strList = (String[]) mo["InsertionStrings"];
                        insertionStrings = strList.Aggregate(insertionStrings,
                            (current, insString) => current + (" " + insString));
                    }
                    else
                    {
                        insertionStrings = String.Empty;
                    }
                    // var eventRec = new EventRecord((ManagementObject)mo);
                    //Filter out all data that contains records that we are interested in.
                    for (int index = 0; index < sTCount; index++)
                    {
                        var searchTerm = searchTermList[index];
                        if (sourceName.Contains(searchTerm) || message.Contains(searchTerm) ||
                            insertionStrings.Contains(searchTerm))
                        {
                            eventRec.SearchT = "Word_" + index.ToString(CultureInfo.InvariantCulture);
                            eventRec.Msg = message + " -- " + insertionStrings;
                            eventRecordList.Add(eventRec);
                            insertionStrings = String.Empty;
                            message = String.Empty;
                            break;
                        }
                        insertionStrings = String.Empty;
                        message = String.Empty;
                    }
                }
            }
            return eventRecordList;
        }
    }
}

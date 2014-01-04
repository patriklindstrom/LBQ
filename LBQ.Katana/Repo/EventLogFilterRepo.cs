using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Management;
using LBQ.Katana.MockupModels;
using LBQ.Katana.Model;

namespace LBQ.Katana
{
 
    class EventLogFilterRepo:ILogFilterRepo
    {
        private List<string> _listOfServersToQuery;
        private List<string> _searchTermsList;

        public List<string> ListOfServersToQuery
        {
            get { return _listOfServersToQuery; }
            set { _listOfServersToQuery = value; }
        }

        public List<string> SearchTermsList
        {
            get { return _searchTermsList; }
            set { _searchTermsList = value; }
        }

        public EventRecordTimeSpanSearcher EventRTimeSearcher { get; set; }
        //public EventLogFilterRepo(List<string> listOfServersToQuersy,List<string> listOfSearchTerms,EventRecordTimeSpanSearcher eventRecordTimeSpanSearcher)
        public EventLogFilterRepo(ISettingsProvider settingsProvider)
        {
            ListOfServersToQuery = listOfServersToQuersy;
            SearchTermsList = listOfSearchTerms;
            EventRTimeSearcher = eventRecordTimeSpanSearcher;
        }
        public ILogFilter GetData(DateTime fromTime, DateTime toTime)
        {
         //What gets Returned should be made with dependency injection
          EventRecordTimeSpanSearcher eventRecordTimeSpanSearcher = new EventRecordTimeSpanSearcher(fromTime,toTime);
            var mossos = eventRecordTimeSpanSearcher.EventsCollection;
            if (ListOfServersToQuery == null)
            {
                throw new NullReferenceException(
                    "There is no configuration object and therefore no list of servers. Config object  is null. It is the Func IOC that should have set this in the code. Something trivial is missing.");
            }
            if (SearchTermsList == null)
            {
                throw new NullReferenceException("The list of servers from Config is null. That is bad.");
            }

            var searchTermList = SearchTermsList;
            int sTCount = searchTermList.Count;
            bool multiSearch = (searchTermList.Count() > 1);
            List<LogFilterRow> eventRecordList = new List<LogFilterRow>();
            
            foreach (var serv in ListOfServersToQuery)
            {

                string timeWrittenStr;
                string sourceName;
                string message;
                string insertionStrings = String.Empty;
                foreach (var mo in mossos)
                {
                    var eventRec = new LogFilterRow();
                    timeWrittenStr = (mo["TimeWritten"] != null) ? mo["TimeWritten"].ToString() : String.Empty;
                    eventRec.Time = ManagementDateTimeConverter.ToDateTime(timeWrittenStr); // DateTime.ParseExact(timeWrittenStr, "yyyyMMddHHmmss.ffffff-zzz", provider);            
                    eventRec.LogType = Global_Const.EVENTLOGTYPE;
                    eventRec.Src = serv+"\\EventLog\\" + Global_Const.SOURCE;
                    sourceName = (mo["SourceName"] != null) ? mo["SourceName"].ToString() : String.Empty;
                   message = (mo["Message"] != null) ? mo["Message"].ToString() : String.Empty;
                    if (mo["InsertionStrings"] != null)
                    {
                        var strList = (String[])mo["InsertionStrings"];
                        insertionStrings = strList.Aggregate(insertionStrings, (current, insString) => current + (" " + insString));
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
                            eventRecordList.Add(eventRec);
                            break;
                        }
                    }
                }
            }
            EventLogFilter eventLogFilter = new EventLogFilter();
            eventLogFilter.FromDateTime = fromTime;
            eventLogFilter.ToDateTime = toTime;
            eventLogFilter.LastRefreshedTime = DateTime.Now;
            eventLogFilter.LogRows = eventRecordList.AsQueryable();
            eventLogFilter.Title = Global_Const.EVENTLOGTITLE;
           // return new MockEventLogFilter();
            return eventLogFilter;
        }

   
  
    }
}

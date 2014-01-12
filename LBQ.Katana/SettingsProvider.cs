using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;

namespace LBQ.Katana
{
    internal class SettingsProvider : ISettingsProvider
    {
        #region Private backing fields for properties

        private static List<string> _serversToQuery;
        private static List<string> _filterTerm;
        private static string _sql1Statement;
        private List<string> _sql1ServersToQuery;
        private static int _cacheExpireInMin;

        #endregion

        #region The constructor for the SettingProvider that sets all backing fields at startup

        public SettingsProvider()
        {
            _serversToQuery = GetConfigList("ListOfServers");
            _filterTerm = GetConfigList("FilterTermList");
            _cacheExpireInMin = GetConfigInt("CacheExpireInMin"); //60 * 24 * 7 SQLStatement1
            _sql1Statement = GetConfigStr("SQLStatement1");
            _sql1ServersToQuery = GetConfigList("Sql1ServToQuery");

        }

        #endregion

        #region Private Helper Methods to read webconfig

        private int GetConfigInt(string key)
        {
            return int.Parse(ConfigurationManager.AppSettings[key]);
        }

        private string GetConfigStr(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        private List<string> GetConfigList(string key)
        {
            return ConfigurationManager.AppSettings[key].Split(',').ToList();
        }

        #endregion

        #region Properties for configs with backing fields

        public List<string> ServersToQuery
        {
            get { return _serversToQuery; }
        }

        public List<string> FilterTerm
        {
            get { return _filterTerm; }
        }

        public String Sql1Statement
        {
            get { return _sql1Statement; }
        }

        public List<string> SQL1ServersToQuery
        {
            get { return _sql1ServersToQuery; }
        }

        public int CacheExpireInMin
        {
            get { return _cacheExpireInMin; }
        }

        #endregion
    }

}
   

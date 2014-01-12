using System;
using System.Collections;
using System.Collections.Generic;

namespace LBQ.Katana
{
    public interface ISettingsProvider
    {
        List<string> ServersToQuery { get; }
        List<string> FilterTerm { get; }
        String Sql1Statement { get; }
        List<string> SQL1ServersToQuery { get; }
        int CacheExpireInMin { get; }
    }
}
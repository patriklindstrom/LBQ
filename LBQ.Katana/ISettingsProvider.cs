using System.Collections.Generic;

namespace LBQ.Katana
{
    public interface ISettingsProvider
    {
        List<string> GetListOfServersToQuery();
        List<string> GetListOfSearchTerms();
        int GetCacheExpireInMin();


    }
}
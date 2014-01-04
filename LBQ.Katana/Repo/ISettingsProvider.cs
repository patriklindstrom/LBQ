using System.Collections.Generic;

namespace LBQ.Katana
{
    public interface ISettingsProvider
    {
        List<string> GetListOfServersToQuery();
        List<string> GetlistOfSearchTerms();        
    }

    class SettingsProvider : ISettingsProvider
    {
        public List<string> GetListOfServersToQuery()
        {
           
            return new List<string> { "Herkules" }; ;
        }

        public List<string> GetlistOfSearchTerms()
        {
            return new List<string> { "TCM" }; ;
        }


    }
}
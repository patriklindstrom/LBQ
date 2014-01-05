using System.Collections.Generic;

namespace LBQ.Katana
{
    class SettingsProvider : ISettingsProvider
    {
        public List<string> GetListOfServersToQuery()
        {
           
            return new List<string> { "Herkules" }; ;
        }

        public List<string> GetlistOfSearchTerms()
        {
            return new List<string> { "Fallout" }; ;
        }


    }
}
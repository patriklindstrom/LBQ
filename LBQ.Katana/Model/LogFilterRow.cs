using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LBQ.Katana.Model
{
    class LogFilterRow:ILogFilterRow
    {
        
        public DateTime Time { get; set; }

        //public string TimeAsStr
        //{
        //    get { return Time.ToString(CultureInfo.InvariantCulture); }         
        //}

        public string LogType { get; set; }
        public string Src { get; set; }
        public string Msg { get; set; }
        public string SearchT { get; set; }
    }
}

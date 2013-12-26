using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
namespace LBQ.Katana.Model
{
    class TextLogFilter:ILogFilter
    {
        public string Title { get; set; }
        public DateTime LastRefreshedTime { get; set; }
        public IQueryable<ILogFilterRow> LogRows { get; set; }
        public DateTime FromDateTime { get; set; }
        public DateTime ToDateTime { get; set; }
    }
}

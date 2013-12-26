using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LBQ.Katana.Model
{
    public interface ILogFilter
    {
        string Title { get; set; }
        DateTime LastRefreshedTime { get; set; }
        IQueryable<ILogFilterRow> LogRows { get; set; }
        DateTime FromDateTime { get; set; }
        DateTime ToDateTime { get; set; }
    }
}

using System;
using System.Linq;

namespace LBQ.Katana.Model
{
    internal interface IEventLogFilter : ILogFilter
    {
        string Title { get; set; }
        DateTime LastRefreshedTime { get; set; }
        IQueryable<ILogFilterRow> LogRows { get; set; }
        DateTime FromDateTime { get; set; }
        DateTime ToDateTime { get; set; }
    }
}
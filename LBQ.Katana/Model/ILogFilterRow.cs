using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace LBQ.Katana.Model
{
    public interface ILogFilterRow
    {
        DateTime Time { get; set; }
        string LogType { get; set; }
        string Src { get; set; }
        string Msg { get; set; }
       // string SearchT { get; set; }
    }
}

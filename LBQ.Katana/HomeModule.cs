using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy;

namespace LBQ.Katana
{
    internal class HomeModule : NancyModule
    {
        public HomeModule()
        {
            Get["/"] = _ =>
            {
                var model = new {title = "We've Got Issues..."};
                return View["home", model];
            };
        }
    }
 
}

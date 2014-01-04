using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LBQ.Katana.MockupModels;
using LBQ.Katana.Model;
using Microsoft.Owin;
using Nancy;

namespace LBQ.Katana
{
    public class HomeModule : NancyModule
    {
        public HomeModule(ILogFilter model)
        {
            Get["/"] = _ =>
            {
                var owinEnvironment = (IDictionary<string, object>)this.Context.Items["OWIN_REQUEST_ENVIRONMENT"];
                var owinCtx = new OwinContext(owinEnvironment);
                model.Title =  "We have Issues Again with IOC socks...";
                return View["index", model];
            };
            Get["/EventLogFilter"] = _ =>
            {
                ILogFilterRepo eventLogFilterRepo = new EventLogFilterRepo();
                model = eventLogFilterRepo.GetData();
                model.Title ="EventLog Filter";
                return View["EventLogFilter", model];
            };
        }
    }
 
}

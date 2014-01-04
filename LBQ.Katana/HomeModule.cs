using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
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
        public ILogFilterRepo EventLogFilterRepo { get; set; }

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
                ILogFilterRepo eventLogFilterRepo = EventLogFilterRepo;
                DateTime fTime = DateTime.Now.AddHours(-6);
                DateTime tTime = DateTime.Now;
                model = eventLogFilterRepo.GetData(fromTime:fTime,toTime:tTime);
                model.Title ="EventLog Filter";
                return View["EventLogFilter", model];
            };
        }
    }
 
}

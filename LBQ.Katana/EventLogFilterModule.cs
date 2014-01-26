using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LBQ.Katana.Model;
using Microsoft.Owin;
using Nancy;
namespace LBQ.Katana
{

    public class EventLogFilterModule : NancyModule
    {
        public EventLogFilterModule(ILogFilter model)
            : base("/EventLogFilter")
        {
            var foo = model.GetType();
            Get["/fum/"] = _ =>
            {
                var owinEnvironment = (IDictionary<string, object>)this.Context.Items["OWIN_REQUEST_ENVIRONMENT"];
                var owinCtx = new OwinContext(owinEnvironment);
                model.Title = "Aggregate for filter of EventLogs";
                return View["EventLogFilter", model];
            };
        }
    }
}

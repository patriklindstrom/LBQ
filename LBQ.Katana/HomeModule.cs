using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin;
using Nancy;

namespace LBQ.Katana
{
    public class HomeModule : NancyModule
    {
        public HomeModule()
        {
            Get["/"] = _ =>
            {
                var owinEnvironment = (IDictionary<string, object>)this.Context.Items["OWIN_REQUEST_ENVIRONMENT"];
                var owinCtx = new OwinContext(owinEnvironment);

                var model = new {title = "We've Got Issues..."};
                return View["index", model];
            };
        }
    }
 
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using LBQ.Katana.Model;
using Microsoft.Owin;
using Nancy;
using Nancy.TinyIoc;

namespace LBQ.Katana
{
    public class HomeModule : NancyModule
    {
        public ILogFilter Model { get; set; }

        public HomeModule( ILogFilterRepo eventLogFilterRepo)
        {
            Get["/"] = _ =>
            {
                var owinEnvironment = (IDictionary<string, object>)this.Context.Items["OWIN_REQUEST_ENVIRONMENT"];
                var owinCtx = new OwinContext(owinEnvironment);
                //Model.Title =  "We have Issues Again with IOC socks...";
                return View["index", Model];
            };
            Get["/EventLogFilter"] = _ =>
            {
                DateTime tTime = DateTime.Parse("2014-01-06 21:45:00");
                DateTime fTime = tTime.AddHours(-48);
                
                 Model =  eventLogFilterRepo.GetData(fromTime: fTime, toTime: tTime) ;
                return View["EventLogFilter", Model];
            };
        }
    }
 
}

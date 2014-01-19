using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using LBQ.Katana.Model;
using Microsoft.Owin;
using Microsoft.Web.Editor.Diagnostics;
using Nancy;
using Nancy.TinyIoc;

namespace LBQ.Katana
{
    public class HomeModule : NancyModule
    {
        public ILogFilter Model { get; set; }
        // https://github.com/NancyFx/Nancy/wiki/Defining-routes
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
                DateTime tTime = DateTime.Now;
                DateTime fTime = tTime.AddHours(-6);               
                 Model =  eventLogFilterRepo.GetData(fromTime: fTime, toTime: tTime) ;
                return View["EventLogFilter", Model];
            };


            Get["/EventLogFilter/lasthours/{value:int}"] = _ =>
            {
                DateTime tTime = DateTime.Now;//DateTime.Parse("2014-01-06 21:45:00");
                DateTime tCDateTime = new DateTime(tTime.Year, tTime.Month, tTime.Day, tTime.Hour, tTime.Minute - tTime.Minute % 5, 0);
                int lasthours = -1*_.value;
                DateTime fTime = tCDateTime.AddHours(lasthours);
                Model = eventLogFilterRepo.GetData(fromTime: fTime, toTime: tCDateTime);
                Model.LastRefreshedTime = tCDateTime;
                return View["EventLogFilter", Model];
            };
            Get["/EventLogFilter/between/{fromdate:datetime}/{todate:datetime}"] = _ =>
            {
                DateTime tTime = _.todate;
                DateTime fTime = _.fromdate;
                Model = eventLogFilterRepo.GetData(fromTime: fTime, toTime: tTime);
                return View["EventLogFilter", Model];
            };
            Get["/api/datatables/EventLogFilter"] = _ =>
            {
                DateTime tTime = DateTime.Now;
                DateTime fTime = tTime.AddHours(-6);
                Model = eventLogFilterRepo.GetData(fromTime: fTime, toTime: tTime);
                DataTableAjax aaTableAjax = new DataTableAjax();
                int szOfA = Model.LogRows.Count();
                int i = 0;
                string [,] aaData = new string[szOfA,4];
                foreach (var lRow in Model.LogRows)
                {
                    aaData[i, 0] = lRow.Time.ToString();
                    aaData[i, 1] = lRow.LogType;
                    aaData[i, 2] = lRow.Src;
                    aaData[i, 3] = lRow.Msg;
                        i++;
                    //{ Msg = lRow.Msg, LogType = lRow.LogType, Time = lRow.Time };                   
                }
                aaTableAjax.aaData = aaData;
                return aaTableAjax;
            };
        }
    }
 
}

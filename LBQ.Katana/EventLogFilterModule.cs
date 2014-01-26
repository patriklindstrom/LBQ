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
        private ILogFilter Model;
        public EventLogFilterModule(ILogFilterRepo eventLogFilterRepo)
            : base("/EventLogFilter")
        {
 
            #region Browser routing
            Get["/"] = _ =>
            {
                var owinEnvironment = (IDictionary<string, object>)this.Context.Items["OWIN_REQUEST_ENVIRONMENT"];
                var owinCtx = new OwinContext(owinEnvironment);
                //Model.Title =  "We have Issues Again with IOC socks...";
                return View["views/Home/index", Model];
            };
            Get["/"] = _ =>
            {
                DateTime tTime = DateTime.Now;
                DateTime fTime = tTime.AddHours(Global_Const.LASTHOURS_DEFAULT);
                Model = eventLogFilterRepo.GetData(fromTime: fTime, toTime: tTime);
                return View["EventLogFilter", Model];
            };


            Get["/lasthours/{value:int}"] = _ =>
            {
                DateTime tTime = DateTime.Now; //DateTime.Parse("2014-01-06 21:45:00");
                DateTime tCDateTime = new DateTime(tTime.Year, tTime.Month, tTime.Day, tTime.Hour,
                    tTime.Minute - tTime.Minute % 5, 0);
                int lasthours = -1 * _.value;
                DateTime fTime = tCDateTime.AddHours(lasthours);
                Model = eventLogFilterRepo.GetData(fromTime: fTime, toTime: tCDateTime);
                Model.LastRefreshedTime = tCDateTime;
                return View["EventLogFilter", Model];
            };
            Get["/between/{fromdate:datetime}/{todate:datetime}"] = _ =>
            {
                DateTime tTime = _.todate;
                DateTime fTime = _.fromdate;
                Model = eventLogFilterRepo.GetData(fromTime: fTime, toTime: tTime);
                return View["EventLogFilter", Model];
            };

            #endregion

            #region Rest api calls

            // the datatables.net jQuery grid wants the data as a two dimensional json array called aaData
            // to get best performance for lazy rendering
            // see http://datatables.net/release-datatables/examples/ajax/defer_render.html
            // So we need to convert the LogFilterRow to a jagged string[][] array to get right format 
            // see: http://msdn.microsoft.com/en-us/library/aa288453(v=vs.71).aspx
            Get["/api/datatables/"] = _ =>
            {
                DateTime tTime = DateTime.Now;
                DateTime fTime = tTime.AddHours(Global_Const.LASTHOURS_DEFAULT);
                Model = eventLogFilterRepo.GetData(fromTime: fTime, toTime: tTime);
                var aaTableAjax = new HomeModule.DataTableAjax();
                int i = 0;
                string[][] aaData = new string[Model.LogRows.Count()][];
                foreach (var lRow in Model.LogRows)
                {
                    var aDataTblRow = new string[4];
                    aDataTblRow[0] = lRow.TimeAsStr;
                    aDataTblRow[1] = lRow.LogType;
                    aDataTblRow[2] = lRow.Src;
                    aDataTblRow[3] = lRow.Msg;
                    aaData[i] = aDataTblRow;
                    i++;
                }
                aaTableAjax.aaData = aaData;
                return aaTableAjax;
            };
            #endregion
        }
    }
}

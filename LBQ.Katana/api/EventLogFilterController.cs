using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using LBQ.Katana.Model;
using LBQ.Katana.Repo;

namespace LBQ.Katana.api
{
    class EventLogFilterController : ApiController
    {
        private ILogFilterRepo _eventFilterRepo;

        public EventLogFilterController(ILogFilterRepo eRepo)
        {
            _eventFilterRepo = eRepo;
        }
          [Route("api/EventLogFilter/lasthours/")]
        public IEnumerable<aaData> Get(int hours)
          {
              DateTime tTime = DateTime.Now;//DateTime.Parse("2014-01-06 21:45:00");
              DateTime tCDateTime = new DateTime(tTime.Year, tTime.Month, tTime.Day, tTime.Hour, tTime.Minute - tTime.Minute % 5, 0);
              int lasthours = -1 * hours;
              DateTime fTime = tCDateTime.AddHours(lasthours);
              var foo = _eventFilterRepo.GetData(fromTime: fTime, toTime: tCDateTime);
              return null;
          }
    }
}

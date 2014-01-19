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

        public EventLogFilterController()
        {
            _eventFilterRepo = null;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Demo.Bugs.Web.Model;

namespace LBQ.Katana
{
    public class BugsController : ApiController
    {
        IBugsRepository _bugsRepository = new BugsRepository();
        public IEnumerable<Bug> Get()
        {
            return _bugsRepository.GetBugs();
        }
        [Route("api/bugs/backlog")]
        public Bug MoveToBacklog([FromBody] int id)
        {
            var bug = _bugsRepository.GetBugs().First(b => b.id == id);
            bug.state = "backlog";
            return bug;
        }
        [Route("api/bugs/working")]
        public Bug MoveToWorking([FromBody] int id)
        {
            var bug = _bugsRepository.GetBugs().First(b => b.id == id);
            bug.state = "working";
            return bug;
        }
        [Route("api/bugs/done")]
        public Bug MoveToDone([FromBody] int id)
        {
            var bug = _bugsRepository.GetBugs().First(b => b.id == id);
            bug.state = "done";
            return bug;
        }
    }
}

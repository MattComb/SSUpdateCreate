
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServiceStack;
using ServiceStack.DataAnnotations;

namespace SS.ServiceModel
{

    [Route("/savestatus", "POST")]
    public class SaveStatus
    {
    }

    public class Status
    {

        public Guid DeviceID { get; set; }

        [IgnoreOnInsert, IgnoreOnUpdate]
        public Guid SiteID { get; set; }

        [IgnoreOnInsert, IgnoreOnUpdate]
        public Guid GroupID { get; set; }

 
    }
}

using System.Linq;
using ServiceStack.OrmLite;
using ServiceStack;
using System;
using SS.ServiceModel;

namespace SS.ServiceInterface
{
    public partial class SS
    {

        public void Any(SaveStatus request)
        {
            using (var db = DbFactory.Open())
            {
                CheckTableExistsAndInitialize<Status>(db);

                var status = new Status()
                {
                    DeviceID = Guid.NewGuid(),
                    GroupID = Guid.NewGuid(),
                    SiteID = Guid.NewGuid(),

                };

                /* Steps to reproduce error 
                 
                PreReq. Set the database connection string at top of SS\Program file.
                1. Establish the Table (using the CheckTableExists above)
                2. Manually remove the SiteID and GroupID columns from the database as they should be 
                populated using a custom query.
                3. Table at this point has one key field DeviceID.
                4. Debug the following steps. Insert and Update work with [IgnoreUpdate] and [IgnoreCreate] attributes
                5. Save fails. (This is the bug).

                99. Please note, I cannot use [Ignore] attribute as field is not populated using custom query - it is ignored!
                 
                */
                db.Insert<Status>(status);

                db.Update<Status>(status);

                db.Save<Status>(status);
            }
        }


    }
}

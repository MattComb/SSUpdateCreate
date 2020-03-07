using System;
using ServiceStack;
using ServiceStack.Data;
using ServiceStack.OrmLite;

namespace SS.ServiceInterface
{
    public partial class SS : Service
    {
        public IDbConnectionFactory DbFactory
        {
            get
            {
                return ServiceStackHost.Instance.Container.Resolve<IDbConnectionFactory>();
            }
        }

        private static void CheckTableExistsAndInitialize<T>(System.Data.IDbConnection db)
        {
            if (db.CreateTableIfNotExists<T>())
            {
               

            }
        }
    }
}
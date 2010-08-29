using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperQ.Data;
using System.Data.SqlServerCe;

namespace SuperQ
{
    public class QueueStorageSQLCE : IQueueStorage
    {
        private static readonly object _lock = new object();

        public void CreateIfRequired(string name)
        {
            string sdfPath = string.Empty;

            using(var testConn = new SqlCeConnection("data source=App_Data\Queue.sdf"))
            {
                sdfPath = testConn.Database;
            }

            if (string.IsNullOrWhiteSpace(sdfPath))
            {
                //broke?
                return;
            }

            if (System.IO.File.Exists(sdfPath))
            {
                // Already Exists
                return;
            }

            lock (_lock)
            {
                if (System.IO.File.Exists(sdfPath))
                {
                    // Already Exists (created while waiting for lock)
                    return;
                }

                using (var engine = new SqlCeEngine());

            }
        }

        public void PushMessaage<T>(QueueMessage<T> message)
        {

            throw new NotImplementedException();
        }

        public QueueMessage<T> GetMessage<T>()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<QueueMessage<T>> GetAllMessages<T>()
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }
    }
}

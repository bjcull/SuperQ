using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperQ.Data;
using System.Data.SqlServerCe;

namespace SuperQ.Data
{
    public class QueueStorageSQLCE : IQueueStorage
    {
        private static readonly object _lock = new object();

        public void CreateIfRequired(string name)
        {
            string connection = string.Format(@"data source=|DataDirectory|\{0}.sdf", name);
            string sdfPath = string.Empty;

            using (var testConn = new SqlCeConnection(connection))
            {
                sdfPath = testConn.Database;
            }

            //thread safety first
            if (!System.IO.File.Exists(sdfPath))
            {
                lock (_lock)
                {
                    if (!System.IO.File.Exists(sdfPath))
                    {
                        //create le db
                        using (var engine = new SqlCeEngine(connection))
                        {
                            engine.CreateDatabase();
                        }

                        //script table
                        string createScript = @"";
                    }
                }
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

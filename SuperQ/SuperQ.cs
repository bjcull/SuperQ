using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperQ.Data;

namespace SuperQ
{
    public class SuperQ
    {
        private IQueueStorage _storage;

        private SuperQ(string name)
        {
            // Instantiate new SQL Compact storage DB
            _storage = new QueueStorageSQLCE();
            _storage.CreateIfRequired(name);
        }

        public void PushMessage<T>(QueueMessage<T> message)
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

        public static SuperQ GetQueue(string name)
        {
            return new SuperQ(name);
        }
    }
}

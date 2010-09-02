﻿using System;
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
            _storage.PushMessage<T>(message);
        }

        public QueueMessage<T> GetMessage<T>()
        {
            return _storage.GetMessage<T>();
        }

        public IEnumerable<QueueMessage<T>> GetAllMessages<T>()
        {
            throw new NotImplementedException();
        }

        public static SuperQ GetQueue(string name)
        {
            return new SuperQ(name);
        }

        public void Delete()
        {
            _storage.Delete();
        }
    }
}

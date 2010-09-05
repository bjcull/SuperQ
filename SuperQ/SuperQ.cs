using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperQ.Data;
using System.Threading;

namespace SuperQ
{
    public delegate void OnMessageReceived<T>(QueueMessage<T> message);

    public class MessageEventArgs<T> : EventArgs
    {
        public QueueMessage<T> Message { get; set; }
    }

    public class SuperQ
    {
        private IQueueStorage _storage;
        private bool _receiving;

        public Thread StartReceiving<T>(OnMessageReceived<T> action)
        {
            _receiving = true;
            Thread thread = new Thread(() => this.Poller<T>(action));
            thread.Start();

            return thread;
        }

        public void StopReceiving()
        {
            _receiving = false;
        }

        private void Poller<T>(OnMessageReceived<T> action)
        {
            while (_receiving)
            {
                var message = GetMessage<T>();
                if (message != null)
                {
                    action(message);
                }
                else
                {
                    Thread.Sleep(10000);
                }
            }
        }

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

        public void DeleteMessage<T>(QueueMessage<T> message)
        {
            _storage.DeleteMessage<T>(message);
        }

        public IEnumerable<QueueMessage<T>> GetAllMessages<T>()
        {
            throw new NotImplementedException();
        }

        public static SuperQ GetQueue(string name)
        {
            return new SuperQ(name);
        }

        public void Clear()
        {
            _storage.Clear();
        }

        public void Delete()
        {
            _storage.Delete();
        }

    }
}

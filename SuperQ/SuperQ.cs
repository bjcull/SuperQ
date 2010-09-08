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

    public class SuperQ<T>
    {
        private IQueueStorage _storage;
        private bool _receiving;

        public Thread StartReceiving(OnMessageReceived<T> action)
        {
            _receiving = true;
            Thread thread = new Thread(() => this.Poller(action));
            thread.Start();

            return thread;
        }

        public void StopReceiving()
        {
            _receiving = false;
        }

        private void Poller(OnMessageReceived<T> action)
        {
            while (_receiving)
            {
                var message = GetMessage();
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

        public void PushMessage(QueueMessage<T> message)
        {
            _storage.PushMessage<T>(message);
        }
        public void PushMessage(T payload)
        {
            _storage.PushMessage<T>(new QueueMessage<T>(payload));
        }

        public QueueMessage<T> GetMessage()
        {
            return _storage.GetMessage<T>();
        }

        public T GetPayload()
        {
            var message = _storage.GetMessage<T>();
            return message == null ? default(T) : message.Payload;
        }

        public void DeleteMessage(QueueMessage<T> message)
        {
            _storage.DeleteMessage<T>(message);
        }

        public IEnumerable<QueueMessage<T>> GetAllMessages()
        {
            throw new NotImplementedException();
        }

        public static SuperQ<T> GetQueue(string name)
        {
            return new SuperQ<T>(name);
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

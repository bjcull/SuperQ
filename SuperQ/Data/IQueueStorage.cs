using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperQ.Data
{
    public interface IQueueStorage
    {
        void CreateIfRequired(string name);

        void PushMessage<T>(QueueMessage<T> message);

        QueueMessage<T> GetMessage<T>();

        IEnumerable<QueueMessage<T>> GetAllMessages<T>();

        void DeleteMessage<T>(QueueMessage<T> message);

        void Clear();

        void Delete();
    }
}

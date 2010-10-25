using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperQ
{
    public class QueueMessage<T>
    {
        public T Payload { get; set; }

        public Guid QueueID { get; set; }
        public DateTime Added { get; set; }
        public DateTime? Retrieved { get; set; }
        public DateTime? Schedule { get; set; }
        public decimal? Interval { get; set; }

        public QueueMessage()
        {
        }

        public QueueMessage(T payload)
        {
            Payload = payload;
        }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperQ;

namespace TestPoller
{
    class Program
    {
        private static SuperQ.SuperQ<string> queue;
        
        static void Main(string[] args)
        {
            queue = SuperQ.SuperQ<string>.GetQueue("MyQueue3");
            queue.StartReceiving(MessageReceived);

            bool running = true;
            while (running)
            {
                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Enter)
                    running = false;
                else if (key.Key == ConsoleKey.P)
                {
                    Console.WriteLine("Message Pushed");
                    queue.PushMessage(new QueueMessage<string>("P was pressed in message form"));
                }
                else if (key.Key == ConsoleKey.G)
                {
                    var message = queue.GetMessage();
                    if (message != null)
                        Console.WriteLine("Message: " + message.Payload);
                    else
                        Console.WriteLine("No Message Received...");
                }
                else if (key.Key == ConsoleKey.L)
                {
                    Console.WriteLine("Payload Pushed");
                    queue.PushMessage("P was pressed in Payload form");
                }
                else if (key.Key == ConsoleKey.V)
                {
                    var payload = queue.GetPayload();
                    if (payload != null)
                        Console.WriteLine("Payload: " + payload);
                    else
                        Console.WriteLine("No Payload Received...");
                }
                else if (key.Key == ConsoleKey.A)
                {
                    queue.PushMessage(new QueueMessage<string>()
                    {
                        Payload = "Testing recurrence",
                        Schedule = new DateTime(2010, 10, 25, 19, 37, 0),
                        Interval = 120
                    });
                    Console.WriteLine("Recurring Message Added");
                }
                
            }

            queue.StopReceiving();
        }

        static void MessageReceived(QueueMessage<string> message)
        {
            Console.WriteLine("Message Received: " + message.Payload + " - Schedule: " + message.Schedule + " - Interval: " + message.Interval);

            if (message.Interval != null)
            {
                queue.PushMessage(new QueueMessage<string>()
                {
                    Payload = message.Payload,
                    Schedule = message.Schedule.Value.AddSeconds(Convert.ToDouble(message.Interval)),
                    Interval = message.Interval
                });
            }
            queue.DeleteMessage(message);
        }
    }
}

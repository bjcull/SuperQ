using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperQ;

namespace TestPoller
{
    class Program
    {
        private static SuperQ.SuperQ queue;
        
        static void Main(string[] args)
        {
            queue = SuperQ.SuperQ.GetQueue("MyQueue");
            queue.StartReceving<string>(MessageReceived);

            bool running = true;
            while (running)
            {
                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Enter)
                    running = false;
                else if (key.Key == ConsoleKey.P)
                {
                    Console.WriteLine("Message Pushed");
                    queue.PushMessage<string>(new QueueMessage<string>("P was pressed in message form"));
                }
                else if (key.Key == ConsoleKey.G)
                {
                    var message = queue.GetMessage<string>();
                    if (message != null)
                        Console.WriteLine("Payload: " + message.Payload);
                    else
                        Console.WriteLine("No Message Received...");
                }
                
            }

            queue.StopReceiving();
        }

        static void MessageReceived(QueueMessage<string> message)
        {
            Console.WriteLine("Message Received: " + message.Payload);
            queue.DeleteMessage(message);
        }
    }
}

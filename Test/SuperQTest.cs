﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class SuperQTest
    {
        public SuperQTest()
        {
        }

        [TestMethod]
        public void GetQueue()
        {
            var testQueue = SuperQ.SuperQ.GetQueue("Queue");

            Assert.AreNotEqual(null, testQueue);
        }

        [TestMethod]
        public void PushMessage()
        {
            var message = new SuperQ.QueueMessage<TestClass>()
            {
                Payload = new TestClass
                {
                    word = "anthony",
                    number = 7
                }
            };

            var testQueue = SuperQ.SuperQ.GetQueue("Queue");
            testQueue.PushMessage<TestClass>(message);
        }


        [TestMethod]
        public void GetMessage()
        {
            var testQueue = SuperQ.SuperQ.GetQueue("Queue");
            var message = testQueue.GetMessage<TestClass>();

            Assert.AreEqual("anthony", message.Payload.word);
            Assert.AreEqual(7, message.Payload.number);
        }

        [TestMethod]
        public void DeleteMessage()
        {
            var testQueue = SuperQ.SuperQ.GetQueue("Queue");
            var message = testQueue.GetMessage<TestClass>();

            testQueue.DeleteMessage(message);

            //TODO: Assert message is deleted
        }

        [TestMethod]
        public void ClearQueue()
        {
            var testQueue = SuperQ.SuperQ.GetQueue("Queue");
            testQueue.Clear();
        }

        [TestMethod]
        public void DeleteQueue()
        {
            var testQueue = SuperQ.SuperQ.GetQueue("Queue");
            testQueue.Delete();

            //TODO: Assert database is deleted
        }
    
    }

    public class TestClass
    {
        public string word { get; set; }
        public int number { get; set; }
    }
}

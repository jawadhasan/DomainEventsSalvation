using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace ExampleCode.EventAggregatorDemo
{
    /// <summary>
    /// Marker Interface: Not needed but helpfu in finding all the
    /// events that could be sent through the EventAggregator
    /// </summary>
    public interface IApplicationEvent
    {
    }

    /// <summary>
    /// This one will be implemented as Singleton(Desing Choice)
    /// </summary>
    public interface IEventAggregator
    {
        /// <summary>
        /// You can tell the EventAggregator to Publish a Message for you.
        /// </summary>
        void Publish<T>(T message) where T : IApplicationEvent;

        /// <summary>
        /// You can tell the EventAggregator that your interested in subscribing
        /// to a message and give it an Action you want to call you back on
        /// when that message is published.
        /// </summary>
        void Subscribe<T>(Action<T> action) where T : IApplicationEvent;

        /// <summary>
        /// The EventAggregator also allows you to unsubscribe
        /// </summary>
        void Unsubscribe<T>(Action<T> action) where T : IApplicationEvent;
    }

    
    public class EventAggregator : IEventAggregator, IDisposable
    {
        private static readonly IEventAggregator instance = new EventAggregator();
        public static IEventAggregator Instance => instance;

        private readonly ConcurrentDictionary<Type, List<object>> Subscriptions = 
            new ConcurrentDictionary<Type, List<object>>();
        
        public void Publish<T>(T message) where T : IApplicationEvent
        {
            List<object> subscribers;
            if (Subscriptions.TryGetValue(typeof(T), out subscribers))
            {
                //ToArray creates a copy incase someone unsubscribe in their own handler
                foreach (var subscriber in subscribers.ToArray())
                {
                    ((Action<T>)subscriber) (message);
                }
            }
        }
        public void Subscribe<T>(Action<T> action) where T : IApplicationEvent
        {
            var subscribers = Subscriptions.GetOrAdd(typeof(T), t => new List<object>());
            lock (subscribers)
            {
                subscribers.Add(action);
            }
        }
        public void Unsubscribe<T>(Action<T> action) where T : IApplicationEvent
        {
            List<object> subscribers;
            if (Subscriptions.TryGetValue(typeof(T), out subscribers))
            {
                lock (subscribers)
                {
                    subscribers.Remove(action);
                }
            }
        }
        public void Dispose()
        {
            Subscriptions.Clear();
        }
    }


    //Lets add messages/events that we are going to be publishing
    //Good pratice: always add information, that your subscribers
    //will need to handle it properly
    public class OrderCreated : IApplicationEvent
    {
        public string OrderNumber { get; private set; }

        public OrderCreated(string orderNumber)
        {
            OrderNumber = orderNumber;
        }
    }
    public class OrderRejected : IApplicationEvent
    {
        public string OrderNumber { get; private set; }

        public OrderRejected(string orderNumber)
        {
            OrderNumber = orderNumber;
        }
    }
    public class OrderApproved : IApplicationEvent
    {
        public string OrderNumber { get; private set; }

        public OrderApproved(string orderNumber)
        {
            OrderNumber = orderNumber;
        }
    }


    //Publisher
    public class OrderProducer
    {
        private List<string> _orderNumbers = new List<string>
        {
            "OrderNo1",
            "OrderNo2",
            "OrderNo3",
            "OrderNoForRejection"
        };
        public void Produce()
        {
            foreach (var orderNumber in _orderNumbers)
            {
                Thread.Sleep(1000);
                EventAggregator.Instance.Publish(new OrderCreated(orderNumber));
            }
        }
    }


    //Consumer-1
    public class Logger
    {
        public Logger()
        {
            EventAggregator.Instance.Subscribe<OrderCreated>(e => Log(e.OrderNumber));
            EventAggregator.Instance.Subscribe<OrderRejected>(e => LogRejected(e.OrderNumber));
        }
        public void Log(string orderNumber)
        {
            Console.WriteLine($"Order {orderNumber} is logged.");
        }
        public void LogRejected(string orderNumber)
        {
            Console.WriteLine($"RejectedOrder {orderNumber} is logged.");
        }
    }



    //Consumer-2
    public class OrderProcessor
    {
        public OrderProcessor()
        {
            EventAggregator.Instance.Subscribe<OrderCreated>(e => ProcessOrder(e.OrderNumber));
        }
        public void ProcessOrder(string orderNumber)
        {
            Console.WriteLine($"Processing order {orderNumber}");

            if (orderNumber == "OrderNoForRejection")
            {
                EventAggregator.Instance.Publish(new OrderRejected(orderNumber)); //Raise a rejection Event
            }
            else
            {
                Console.WriteLine($"Processing of order {orderNumber} was successful. Email to client");
            }
        }
    }




    public class EventAggregatorDemo
    {
        public void Run()
        {
            var publisher = new OrderProducer();
            var consumer1 = new Logger();
            var consumer2 = new OrderProcessor();
            publisher.Produce();
        }
    }

}

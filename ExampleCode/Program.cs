using System;
using System.Collections.Generic;
using System.Threading;
using ExampleCode.DelegatesAndEvents;
using ExampleCode.DelegatesAndEvents.EventsCode;
using ExampleCode.DelegatesAndEvents.ObserverImplementation;
using Worker = ExampleCode.DelegatesAndEvents.DelegateCode.Worker;

namespace ExampleCode
{
    class Program
    {
        static void Main(string[] args)
        {
            #region SampleCode
            //DomainEventsDemo();
            //DelegateDemo();
            //EventsDemo();
            //ReportWorkerDemo();
            //OrderManagerDemo();

            // DemoObserver1();
            // DemoObserver2();

            //DemoObserverViaEvents();
            #endregion



            Console.ReadKey();
            Console.WriteLine("Press enter to close...");
        }

        private static void DemoObserver2()
        {
            var doerSubject2 = new Doer2();
            doerSubject2.Attach(new Logger2());
            doerSubject2.DoSomething($"{DateTime.UtcNow}");
        }
        private static void DemoObserver1()
        {
            var doerSubect = new Doer();
            doerSubect.Attach(new UserInterface());
            doerSubect.Attach(new Logger());
            doerSubect.DoSomething($"{DateTime.UtcNow}");
        }


        private static void DemoObserverViaEvents()
        {
            var st = new StockTicker();

            // Create New observers to listen to the stock ticker
            GoogleMonitor gf = new GoogleMonitor(st);
            MicrosoftMinitor mf = new MicrosoftMinitor(st);

            // Load the Sample Stock Data
            foreach (var s in SampleData.getNext())
                st.Stock = s;
        }




        private static void OrderManagerDemo()
        {
            Console.WriteLine("Setting up OrderManager");
            var orderManager = new OrderManager();
            orderManager.OrderRejected += OrderManager_OrderRejected;
            orderManager.OrderCreated +=
                (s, e) => Console.WriteLine($"Order {e.Id} was created for customer: {e.CustomerName}");
            orderManager.OrderApproved += (s, e) => Console.WriteLine($"order {e.Id} was approved. Email this");


            Thread.Sleep(1000);

            orderManager.AddOrder(new Order(1, "Customer1"));
            Thread.Sleep(1000);

            orderManager.AddOrder(new Order(2, "Customer2"));
            Thread.Sleep(1000);

            orderManager.AddOrder(new Order(3, "Customer3"));
            Thread.Sleep(1000);

            orderManager.AddOrder(new Order(4, "Customer4"));
            Thread.Sleep(1000);

            orderManager.AddOrder(new Order(4, "Customer4"));
            Thread.Sleep(1000);

            orderManager.ProcessOrders();
        }
        private static void OrderManager_OrderRejected(object sender, OrderEventArgs e)
        {
            Console.WriteLine($"Order {e.Id} for customer {e.CustomerName} was rejected.");
        }

        private static void ReportWorker_WorkPerformed(object sender, WorkPerformedEventArgs e)
        {
            Console.WriteLine($"{e.WorkType}: {e.Hours}");
        }
        private static void ReportWorkerDemo()
        {
            var reportWorker = new ReportWorker();
            //various ways are commented below
            //reportWorker.WorkPerformed += new EventHandler<WorkPerformedEventArgs>(ReportWorker_WorkPerformed);

            //reportWorker.WorkPerformed += delegate(object sender, WorkPerformedEventArgs e) {
            //    Console.WriteLine($"{e.WorkType}: {e.Hours}");
            //};

            reportWorker.WorkPerformed += (s, e) => Console.WriteLine($"{e.WorkType}: {e.Hours}");
            reportWorker.GenerateReports();
        }


        private static void EventsDemo()
        {
            var worker = new DelegatesAndEvents.EventsCode.Worker();
            //See the result.
        }
        private static void DelegateDemo()
        {
            var worker = new Worker();
            ////See the result
        }
        private static void DomainEventsDemo()
        {
            Customer preffered = null;

            var customer = new Customer("Customer-1");
            DomainEvents.Register<CustomerBecamePreferred>(p => preffered = p.Customer);
            customer.DoSomething();

            Console.WriteLine($"after the callback the preffered variable has value {preffered.Name} ");
        }
    }

    //DomainEvent
    public interface IDomainEvent { }
    public class CustomerBecamePreferred : IDomainEvent
    {
        public Customer Customer { get; set; }
    }
    
    //Entity
    public class Customer
    {
        public string Name { get; private set; }

        public Customer(string name)
        {
            Name = name;
        }
        public void DoSomething()
        {
            Console.WriteLine("DoSomethingMethod");
            DomainEvents.Raise(new CustomerBecamePreferred
                { Customer = this}
            );
        }
    }

    //Handler
    public class CustomerBecamePreferredHandler : 
        IHandles<CustomerBecamePreferred>
    {
        public void Handle(CustomerBecamePreferred args)
        {
            //send email to args.Customer
            Console.WriteLine("CustomerBecamePreferredHandler.HandleMethod()");
            Console.WriteLine($"Sending Email to {args.Customer}");
        }
    }
    public interface IHandles<T> where T : IDomainEvent
    {
        void Handle(T args);
    }
    
    //static DomainEvents class
    public static class DomainEvents
    {
        [ThreadStatic] // so that each thread has its own callbacks
        private static List<Delegate> actions;

        //Notice that while this class *can* use a container, the container isn’t needed for unit tests which use the Register method.
        //public static IContainer Container { get; set; } //as before

        //Registers a callback for the given domain event
        public static void Register<T>(Action<T> callback) where T : IDomainEvent
        {
            if(actions == null)
                actions = new List<Delegate>();

            actions.Add(callback);
        }


        //Clear callbacks passed to Register on the current thread
        public static void ClearCallbacks()
        {
            actions = null;
        }


        //Raises the given domain event
        public static void Raise<T>(T args) where T : IDomainEvent
        {
            //Uncomment, once container is available. should be uncommented.

            //if (Container != null)
            //{
            //    foreach (var VARIABLE in Container.ResolveAll<IHandles<T>>())
            //    {
            //        handler.Handle(args);
            //    }
            //}

            if (actions != null)
            {
                foreach (var action in actions)
                {
                    if (action is Action<T>)
                    {
                        Console.WriteLine("Raise method and calling the action");
                        ((Action<T>)action)(args);
                    }
                }
            }
        }

    }


    /* When used Server side, please make sure that you add a call to ClearCallbacks in your infrastructure's end of message processing section.
    In nServiceBus this is done with a message module like the one below:
    The main reason for this cleanup is that someone just might want to use the Register API in their original service layer code
    rather than writing a separate domain event handler. */

    //public class DomainEventsCleaner : IMessageModule
    //{
    //    public void HandleBeginMessage()
    //    {
    //    }

    //    public void HandleEndMessage()
    //    {
    //        DomainEvents.ClearCallbacks();
    //    }
    //}

}

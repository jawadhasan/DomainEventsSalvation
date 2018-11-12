using System;
using System.Collections.Generic;
using System.Text;

namespace ExampleCode.DomainEventDemoCode
{


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
            DomainEvents.Raise(new CustomerBecamePreferred{ Customer = this });
        }
    }



    //Handler
    public interface IHandles<T> where T : IDomainEvent
    {
        void Handle(T args);
    }
    public class CustomerBecamePreferredHandler :IHandles<CustomerBecamePreferred>
    {
        public void Handle(CustomerBecamePreferred args)
        {
            //send email to args.Customer
            Console.WriteLine("CustomerBecamePreferredHandler.HandleMethod()");
            Console.WriteLine($"Sending Email to {args.Customer}");
        }
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
            if (actions == null)
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



    //Demo
    public class DomainEventsDemo
    {

        public void Run()
        {
            Customer preffered = null;

            var customer = new Customer("Customer-1");

            //Register for Event
            DomainEvents.Register<CustomerBecamePreferred>(p => preffered = p.Customer);

            customer.DoSomething();

            Console.WriteLine($"after the callback the preffered variable has value {preffered.Name} ");

        }
    }
}

using System;
using System.Collections.Generic;

namespace ExampleCode.DelegatesAndEvents.ObserverImplementation
{
    public interface IObserver
    {
        void Update();
    }
    public interface ISubject
    {
        void Attach(IObserver observer);
        void Detach(IObserver observer);
        void Notify();
    }

    //Subject
    public class Doer : ISubject
    {
        private IList<IObserver> _observers = new List<IObserver>();


        public void Attach(IObserver observer)
        {
            _observers.Add(observer);
        }

        public void Detach(IObserver observer)
        {
            _observers.Remove(observer);
        }

        public void Notify()
        {
            foreach (var observer in _observers)
            {
                observer.Update();
            }
        }

        public void DoSomething(string data)
        {
            //DoSomething with data
            Console.WriteLine($"do something with {data}");

            //Notify observers
            Notify();
        }
    }

    //Observers
    public class Logger: IObserver
    {
        public void Update()
        {
            Console.WriteLine($"{nameof(Logger)} is notified.");
            Console.WriteLine("Logging");
        }
    }
    public class UserInterface : IObserver
    {
        public void Update()
        {
            Console.WriteLine($"{nameof(UserInterface)} is notified.");
            Console.WriteLine("UserInterface: Hi User....");
        }
    }


    /*
     * Another variation: Passing a reference of subject to observer
     */

    public interface ISubject2
    {
        string Data { get; }
        void Attach(IObserver2 observer);
        void Detach(IObserver2 observer);
        void Notify();
    }
    
    public interface IObserver2
    {
        void Update(ISubject2 sender);
    }


    public class Doer2 : ISubject2
    {
        private IList<IObserver2> _observers = new List<IObserver2>();

        public string Data { get; private set; }

        public void Attach(IObserver2 observer)
        {
            _observers.Add(observer);
        }

        public void Detach(IObserver2 observer)
        {
            _observers.Remove(observer);
        }

        public void Notify()
        {
            foreach (var observer in _observers)
            {
                observer.Update(this);
            }
        }

        public void DoSomething(string data)
        {
            Data = data;
            Notify();
        }
    }


    public class Logger2: IObserver2
    {
        public void Update(ISubject2 sender)
        {
            Console.WriteLine($"Logger2 {sender.Data}");
        }
    }
}

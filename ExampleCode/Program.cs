using System;
using ExampleCode.DelegatesAndEvents.DelegateCode;
using ExampleCode.DelegatesAndEvents.EventsCode;
using ExampleCode.DelegatesAndEvents.ObserverImplementation;
using ExampleCode.DomainEventDemoCode;

namespace ExampleCode
{
    class Program
    {
        static void Main(string[] args)
        {
            #region SampleCode


            ////****************Delegate Demo*********************************
            //var mediaPlayerDemo = new MediaPlayerDemo();
            //mediaPlayerDemo.Run();


            //****************.NET Event with Delegate*********************************
            //var eventWithDelegateDemo = new EventWithDelegateDemo();
            //eventWithDelegateDemo.Run();

            //var clockDemo = new ClockDemo();
            //clockDemo.Run();

            ////****************.NET EventHandler Demo*********************************
            //var orderManagerDemo = new OrderManagerDemo();
            //orderManagerDemo.Run();


            ////****************Classic Observer*********************************
            //ClassicObserverDemo.RunDemo1();
            //ClassicObserverDemo.RunDemo2();


            ////****************Observer Via Events*********************************
            //var obserViaEventDemo = new ObserViaEventDemo();
            //obserViaEventDemo.Run();


            ////******************Event Aggregator**********************************
            //var eventAggregatorDemo = new EventAggregatorDemo.EventAggregatorDemo();
            //eventAggregatorDemo.Run();
            #endregion


            //var domainEventsDemo = new DomainEventsDemo();
            //domainEventsDemo.Run();


            Console.ReadKey();
            Console.WriteLine("Press enter to close...");
        }

    }
}

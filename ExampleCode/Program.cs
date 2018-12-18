using System;
using ExampleCode.DataStructure.Nodes;
using ExampleCode.DelegatesAndEvents.DelegateCode;
using ExampleCode.DelegatesAndEvents.EventsCode;
using ExampleCode.DelegatesAndEvents.ObserverImplementation;
using ExampleCode.DomainEventDemoCode;
using ExampleCode.Facade;


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

            ////******************Domain Events Demo**********************************
            //var domainEventsDemo = new DomainEventsDemo();
            //domainEventsDemo.Run();


            ////******************PipeLine Demo**********************************
            //var pipeLineDemo = new PipeLineDemo.PipeLineDemo();
            //pipeLineDemo.Run();


            //var facadeDemo = new FacadeDemo();
            //facadeDemo.Run();


            //var genericsDemo = new GenericsDemo.GenericsDemo();
            //genericsDemo.Run();

            var nodesDemo = new NodesDemo();
            nodesDemo.Run();

            Console.ReadKey();
            Console.WriteLine("Press enter to close...");
        }
    }
}

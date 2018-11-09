using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ExampleCode.DelegatesAndEvents.EventsCode
{
    //You can put this into a separate class, if you want
    public delegate void WorkPerformedHandler(int hours, WorkType workType);


    public class ReportWorker
    {
        //Built-in .NET generic delegate
        public EventHandler<WorkPerformedEventArgs> WorkPerformed;

        public void GenerateReports()
        {
            Console.WriteLine("Generating Reports......");
            //...........Generate Reports here
            Thread.Sleep(1000);

            //Raising Events
            if (WorkPerformed != null)
            {
                WorkPerformed(this, new WorkPerformedEventArgs
                {
                    Hours = 5,
                    WorkType = WorkType.GenerateReports
                });
            }
        }
    }


    public class FactoryWorker //aka worker
    {
        //event definition (naming:stripped-off handler of delegate)
        public event WorkPerformedHandler WorkPerformed;

        //Built-in .NET generic delegate
        public EventHandler<WorkPerformedEventArgs> WorkPerformed2;

        //Built-in .NET delegate, also here not passing data, just signaling that work is done.
        public event EventHandler WorkCompleted;

        public virtual void DoWork(int hours, WorkType workType)
        {
            
            //.....Do Work Here

            //Notify the consumer that workType has performed.
            //Good practice: instead of calling event here we are calling it via another method
            OnWorkPerformed(hours, workType);

            //WorkPerformed2(this, new WorkPerformedEventArgs
            //{
            //    WorkType = WorkType.GenerateReports,
            //    Hours = 10
            //});
        }

        //Sub-Classes can override. Make private if needed.
        protected virtual void OnWorkPerformed(int hours, WorkType workType)
        {
            //Raising the event
            if (WorkPerformed != null)
            {
                WorkPerformed(8, WorkType.EmailReports);
            }


            //another way, here we are accessing the underlying delegate and calling it like a method.
            var del = WorkPerformed as WorkPerformedHandler;
            if (del != null)
            {
                del(hours, workType); //Raising event
            }
        }
    }


    public class WorkPerformedEventArgs : EventArgs
    {
        public int Hours { get; set; }
        public WorkType WorkType { get; set; }
    }
    public enum WorkType
    {
        GenerateReports,
        PrintReports,
        EmailReports
    }
}

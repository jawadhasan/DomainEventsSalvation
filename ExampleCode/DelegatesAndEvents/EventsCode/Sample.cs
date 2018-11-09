using System;
using System.Threading;

namespace ExampleCode.DelegatesAndEvents.EventsCode
{
    public class Clock
    {
        private int _hour;
        private int _minute;
        private int _second;

        public delegate void SecondChangedHandler(object clock, TimeInfoEventArgs timeInfo);
        public event SecondChangedHandler SecondChanged;

        public void Run()
        {
            while (true)
            {
                Thread.Sleep(100);
                var now = DateTime.Now;

                if (now.Second != _second)
                {
                    var timeInfoArgs = new TimeInfoEventArgs(now.Hour,now.Minute,now.Second);
                    SecondChanged?.Invoke(this,timeInfoArgs);
                }
            }
        }
    }


    //Various clients of Clock
    public class DigitalClock
    {
        public void Subscribe(Clock theClock)
        {
            theClock.SecondChanged += NewTime;
        }

        private void NewTime(object clock, TimeInfoEventArgs timeInfo)
        {
            Console.WriteLine($"Digital display: {timeInfo.Hour.ToString()} : {timeInfo.Minute.ToString()}");
        }
    }
    public class Log
    {
        public void Subscribe(Clock theClock)
        {
            theClock.SecondChanged += LogTime;
        }

        private void LogTime(object clock, TimeInfoEventArgs timeInfo)
        {
            Console.WriteLine($"Logging {timeInfo.Hour.ToString()} : {timeInfo.Minute.ToString()}");
        }
    }
    public class ClientOfClock
    {
        public void Subscribe(Clock theClock)
        {
            theClock.SecondChanged += NewTime;
        }
        public void NewTime(object clock, TimeInfoEventArgs timeInfo)
        {
            Console.WriteLine($"Current time: {timeInfo.Hour.ToString()} " +
                              $": {timeInfo.Minute.ToString()}");
        }
    }
    public class TimeInfoEventArgs
    {
        public int Hour { get; }
        public int Minute { get; }
        public int Second { get; }

        public TimeInfoEventArgs(int hour, int minute, int second)
        {
            Hour = hour;
            Minute = minute;
            Second = second;
        }
    }



    public class Worker
    {
        public Worker()
        {
            Work();
        }

        public void Work()
        {
            var clock = new Clock();

            //Various clients of Clock
            var clientOfClock = new ClientOfClock();
            clientOfClock.Subscribe(clock);

            var log = new Log();
            log.Subscribe(clock);

            var digitalClock = new DigitalClock();
            digitalClock.Subscribe(clock);

            //Run
            clock.Run();
        }
    }
}

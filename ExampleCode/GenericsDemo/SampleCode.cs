using System;
using System.Collections;
using System.Collections.Generic;

namespace ExampleCode.GenericsDemo
{

    // A buffer interface
    public interface IBuffer<T> : IEnumerable<T>
    {
        bool IsEmpty { get; }
        void Write(T value);
        T Read();
    }

    public class Buffer<T> : IBuffer<T>
    {
        protected Queue<T> _queue = new Queue<T>();

        public bool IsEmpty => _queue.Count == 0;

        public virtual void Write(T value)
        {
            Console.WriteLine($"Writing value.... {value}");
            _queue.Enqueue(value);
        }
        public virtual T Read()
        {
            return _queue.Dequeue();
        }

        public IEnumerator<T> GetEnumerator()
        {
            //one wa to do that is simply forward this call as
            //==> return _queue.GetEnumerator();

            //But if I want to inspect it or check it I can use the 
            //following approach

            foreach (var item in _queue)
            {
                //inspect, process here
                yield return item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }


    public class CircularBuffer<T> : Buffer<T>
    {
        private int _capacity;

        public CircularBuffer(int capacity = 10)
        {
            _capacity = capacity;
        }

        //specific to this class only
        public bool IsFull => _queue.Count == _capacity;

        //we are reusing and adding more to existing method on Buffer<T>
        public override void Write(T value)
        {
            base.Write(value);

            //post processing; 
            if (_queue.Count > _capacity)
            {
                Console.WriteLine($"Dequeing value inside Write method.... {value}");
                //will dequeue oldest item in the list.
                _queue.Dequeue(); 
            }
        }
    }

    public class GenericsDemo
    {
        public void Run()
        {
            var buffer = new CircularBuffer<double>(capacity:3);
            Console.WriteLine($"Buffer Empty? :  {buffer.IsEmpty}");
            Console.WriteLine($"Buffer full? :  {buffer.IsFull}");

            Console.WriteLine("Writing some values to buffer.");
            buffer.Write(10);
            buffer.Write(20);
            buffer.Write(30);
            buffer.Write(40);
            buffer.Write(50);

            Console.WriteLine();
            Console.WriteLine("Reading values from buffer");

            while (!buffer.IsEmpty)
            {
                Console.WriteLine($"value from buffer {buffer.Read()}");
            }
        }
    }
}

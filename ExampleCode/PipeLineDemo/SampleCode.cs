using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ExampleCode.PipeLineDemo
{
    public interface IOperation<T>
    {
        IEnumerable<T> Execute(IEnumerable<T> input);
    }

    public interface IPipeline<T>
    {
        IPipeline<T> Register(IOperation<T> operation);
        void Execute();
    }



    //Stage-1
    //Class doesnt care about its input. it is the source.
    public class GetAllProcesses : IOperation<Process>
    {
        public IEnumerable<Process> Execute(IEnumerable<Process> input)
        {
            return Process.GetProcesses();
        }
    }

    //Stage-2
    //Iterate over the input and use "yield return" keyword to stream the result to the next step.
    //It is important to note that 2nd stage uses the [if] to control what get pass downstream.
    public class LimitByWorkingSetSize : IOperation<Process> {
        public IEnumerable<Process> Execute(IEnumerable<Process> input)
        {
            var maxSizeBytes = 50 * 1024 * 1024;

            foreach (var process in input)
            {
                if (process.WorkingSet64 > maxSizeBytes)
                    yield return process;
            }
        }
    }

    //Stage-3
    //Since this step is the final one, we use the "Yield break" keyword
    //to make it compile without returning anything.
    //we could return NULL, but that would be rude.
    public class PrintProcessName : IOperation<Process>
    {
        public IEnumerable<Process> Execute(IEnumerable<Process> input)
        {
            foreach (var process in input)
            {
                Console.WriteLine(process.ProcessName);
            }
            yield break;
        }
    }
    
    public class PipeLine<T> : IPipeline<T>
    {
        private readonly List<IOperation<T>> operations = new List<IOperation<T>>();
        public IPipeline<T> Register(IOperation<T> operation)
        {
            operations.Add(operation);
            return this;
        }

        public void Execute()
        {
            IEnumerable<T> current = new List<T>();
            foreach (var operation in operations)
            {
                current = operation.Execute(current);
            }

            var enumertor = current.GetEnumerator();
            while (enumertor.MoveNext()) { }
        }
    }

    
    //Now we only have to bring them togather.
    //Executing this pipeline will execute all three steps, in a streaming fashion.
    //What are we getting from this?
    //  => Composability and streaming. When we exeute the pipeline,
    // we are not executing each step in turn, we are executing them all togather
    // (Batch processing, ETL, workflows etc)
    public class TrivialProcessPipeLine : PipeLine<Process>
    {
        public TrivialProcessPipeLine()
        {
            Register(new GetAllProcesses());
            Register(new LimitByWorkingSetSize());
            Register(new PrintProcessName());
        }
    }




    public class PipeLineDemo
    {
        public void Run()
        {
            var pipeLine = new TrivialProcessPipeLine();
            pipeLine.Execute();
        }
    }

}

using System;

namespace ExampleCode.DataStructure.Nodes
{
    public class Node
    {
        public int Value { get; set; }
        public Node Next { get; set; }
    }



    public class NodesDemo
    {
        public void Run()
        {
            var first = new Node {Value = 3};
            var middle = new Node {Value = 5};

            first.Next = middle; //linking

            PrintList(first);
        }
        private static void PrintList(Node node)
        {
            while (node != null)
            {
                Console.WriteLine(node.Value);
                node = node.Next;
            }
        }
    }
}

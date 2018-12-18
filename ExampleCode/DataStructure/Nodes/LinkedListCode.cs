using System.Collections;
using System.Collections.Generic;

namespace ExampleCode.DataStructure.Nodes
{
    public class LinkedListNode<T>
    {
        public T Value { get; set; }
        public LinkedListNode<T> Next { get; set; }

        public LinkedListNode(T value)
        {
            Value = value;
        }
    }

    public class LinkedList<T> : IEnumerable<T>
    {
        public LinkedListNode<T> Head { get; private set; }
        public LinkedListNode<T> Tail { get; private set; }

        //Add the specified node to the start of the link list
        public void AddFirst(LinkedListNode<T> node)
        {
            //Save off the head node so we don't lose it.
            var temp = Head;

            //Point head to the new node
            Head = node;

            //Insert the rest of the list behind the head
            Head.Next = temp;

            Count++;
            if (Count == 1)
            {
                // if the list was empty then Head and Tail should
                //both point to the new node.
            }
        }


        //Add the node to the end of the list
        public void AddLast(LinkedListNode<T> node)
        {
            if (Count == 0)
            {
                Head = node;
            }
            else
            {
                Tail.Next = node;
            }

            Tail = node;
            Count++;
        }


        //Removes the last node from the list.
        public void RemoveLast()
        {
            if (Count != 0)
            {
                if (Count == 1)
                {
                    Head = null;
                    Tail = null;
                }
                else
                {
                    // Before: Head --> 3 --> 5 --> 7
                    //         Tail = 7
                    // After:  Head --> 3 --> 5 --> null
                    //         Tail = 5

                    var current = Head;
                    while (current.Next != Tail)
                    {
                        current = current.Next;
                    }

                    current.Next = null;
                    Tail = current;
                }

                Count--;
            }
        }


        //Remove the first node from the list.
        public void RemoveFirst()
        {
            if (Count != 0)
            {
                // Before: Head -> 3 -> 5
                // After:  Head ------> 5

                // Head -> 5 -> null
                // Head ------> null

                Head = Head.Next;
                Count--;

                if (Count == 0)
                {
                    Tail = null;
                }
            }
        }






        //Collection




        //The number of items currently in the list.
        public new int Count
        {
            get;
            private set;
        }





        //Removes all the nodes from the list
        public new void Clear()
        {
            Head = null;
            Tail = null;
            Count = 0;
        }

        public IEnumerator<T> GetEnumerator()
        {
            var current = Head;
            while (current != null)
            {
                yield return current.Value;
                current = current.Next;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}

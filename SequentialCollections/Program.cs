using System;
using System.Collections;

namespace SequentialCollections
{
    class Program
    {
        static void Main(string[] args)
        {
            Queue q = new Queue();
            q.Enqueue("First");
            q.Enqueue("Second");
            q.Enqueue("Third");
            q.Enqueue("Fourth");

            while(q.Count >0)
            {
                object o = q.Dequeue();
                Console.WriteLine(o);
            }

            Stack s = new Stack();
            s.Push("First");
            s.Push("Second");
            s.Push("Third");
            s.Push("Fourth");

            while (s.Count > 0)
            {
                object obj = s.Pop();
                Console.WriteLine(obj);
            }


        }
    }
}

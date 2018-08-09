using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace MutexApp
{
    class Program
    {
        public static Mutex m = new Mutex();
        public static string[] arr = new string[100];
        public static Random r = new Random();
        public static int pCount = 0;
        public static int cCount = 0;

        static void Main(string[] args)
        {
            while (true)
            {
                if (Console.KeyAvailable)
                {
                    break;
                }
                Thread producer = new Thread(() => Call(Producer));
                Thread consumer = new Thread(() => Call(Consumer));

                producer.Start();
                consumer.Start();
                Thread.Sleep(100);
            }
        }

        static void Producer()
        {
            Console.WriteLine(" Thread " + Thread.CurrentThread.ManagedThreadId + " is working.");

            int length = r.Next(1, arr.Length);

            for (int i = 0; i < length; i++)
            {
                if (arr[pCount] == null)
                {
                    arr[pCount] = "producer No. " + r.Next(1, 10);
                    pCount++;
                }
                if (pCount == arr.Length)
                    pCount = 0;
            }
            Console.WriteLine(" Thread " + Thread.CurrentThread.ManagedThreadId + " has finished and releasing.");
        }

        static void Consumer()
        {
            Console.WriteLine(" Thread " + Thread.CurrentThread.ManagedThreadId + " is working.");

            int length = r.Next(1, arr.Length);

            for (int i = 0; i < length; i++)
            {
                if (arr[cCount] != null)
                {
                    Console.WriteLine("Thread " + Thread.CurrentThread.ManagedThreadId + " Consumer is Reading " + arr[cCount]);
                    arr[cCount] = null;
                    cCount++;
                }
                if (cCount == arr.Length)
                    cCount = 0;
            }
            Console.WriteLine(" Thread " + Thread.CurrentThread.ManagedThreadId + " has finished and releasing.");
        }

        static void Call(Action a)
        {
            m.WaitOne();
            a();
            m.ReleaseMutex();
        }
    }
}

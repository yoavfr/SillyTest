using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GarbageCollection
{
    class Program
    {
        static void Main(string[] args)
        {
            new Program().Go();
        }

        Stopwatch m_stopWatch;
        public void Go()
        {
            GC.RegisterForFullGCNotification(1, 1);

            Thread t = new Thread(new ThreadStart(WaitForFullGcProc));
            t.Start();

            int i=0;
            while (true)
            {
                string s = "a";
                s+="b";
/*                if (i++ %100000 == 0)
                {
                    Console.WriteLine("Working...");
                }*/
            }
        }

        private void WaitForFullGcProc()
        {
            while (true)
            {
                GCNotificationStatus s = GC.WaitForFullGCApproach();
                if (s==GCNotificationStatus.Succeeded)
                {
                    Console.WriteLine("GC started?");
                    m_stopWatch = Stopwatch.StartNew();
                    s = GC.WaitForFullGCComplete();
                    if (s == GCNotificationStatus.Succeeded)
                    {
                        Console.WriteLine("GC completed {0}",m_stopWatch.Elapsed);
                    }
                }
                Thread.Sleep(500);

            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SillyTest
{
    //[EventSource(Name = "Yoav Event"/*, Guid = "{FB94739C-8B0A-45F0-A7D6-9CF687DE4C8D}"*/)]
    [EventSource(Guid = "FB94739C-8B0A-45F0-A7D6-9CF687DE4C8D")]
    public class Logger : EventSource
    {
        private static Logger s_logger = new Logger();
        public static Logger Instance
        {
            get
            {
                return s_logger;
            }
        }

        public void Test(string message)
        {
            WriteEvent(1, message);
        }

    }
}

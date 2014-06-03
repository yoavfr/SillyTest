using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SillyTest
{
    [EventSource(Guid = "896CB5AA-F3D4-42B9-828B-618F45D42E7C")]
    public class MyEventSource : EventSource
    {
        private static MyEventSource s_Instance = new MyEventSource();
        public static MyEventSource Instance
        { 
            get
            {
                return s_Instance;
            }
        }

        private MyEventSource()
        {
        }

        [Event(1, Level = EventLevel.Informational, Message = "{auditLog}")]
        public void AuditLog(AuditItem auditItem)
        {
            WriteEvent(1, auditItem.Statement, auditItem.SubscriptionId, auditItem.ResourceId);
        }

    }
}

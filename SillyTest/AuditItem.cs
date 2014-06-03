using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SillyTest
{
    public class AuditItem : TableEntity
    {
        public AuditItem(string subscriptionId, string resourceId, DateTime eventTime, string statement)
        {
            SubscriptionId = subscriptionId;
            ResourceId = resourceId;
            EventTime = eventTime;
            Statement = statement;

            PartitionKey = String.Format("{0}_{1}",SubscriptionId , ResourceId);
            RowKey = String.Format("{0:10}_{1}", EventTime.Ticks, Guid.NewGuid());
        }

        public AuditItem()
        {

        }

        public string SubscriptionId { get; set; }
        public string ResourceId { get; set; }
        public DateTime EventTime { get; set; }
        public string Statement { get; set; }
    }
}

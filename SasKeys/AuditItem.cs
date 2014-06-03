using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SasKeys
{
    public class AuditItem : TableEntity
    {
        public AuditItem(string server, string table, DateTime eventTime, string statement)
        {
            Server = server;
            Table = table;
            EventTime = eventTime;
            Statement = statement;

            PartitionKey = String.Format("{0}", Server);
            RowKey = String.Format("{0:10}_{1}", EventTime.Ticks, Guid.NewGuid());
        }

        public AuditItem()
        {

        }

        public string Server { get; set; }
        public string Table { get; set; }
        public DateTime EventTime { get; set; }
        public string Statement { get; set; }
    }
}

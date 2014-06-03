using System;
using System.Linq;

namespace SillyTest
{
    using DataSec.Proxy.Core;
    using DataSec.Proxy.Core.UnitTests;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Table;
    using SasKeys;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Diagnostics;
    using System.Globalization;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    internal class Program
    {

        private static void Main(string[] args)
        {
            string c_SecuredConnectionStringFormatAdoNet = @"Server=tcp:{0}-{1}.database.secure.windows.net,{2};Database={3};User ID={{0}}@{4};Password={{your_password_here}};Trusted_Connection=False;Encrypt=True;Connection Timeout=30;";
            string geo = "neu";
            string env = "prod";
            int port = 1433;
            string adoNetString = string.Format(c_SecuredConnectionStringFormatAdoNet, geo, env, port, "databaseName", "serverName");
            Console.WriteLine(adoNetString);
            //new FlagsClass().Run();
            //new Rest().Run();
            //new Enums().Run();
            //Console.WriteLine(new DatabaseSecurityPolicyContract().ToJson()); 
            //MyEventSource.Instance.AuditLog(new AuditItem("a", "b", DateTime.UtcNow, "c"));
            /*CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["SillyTest.Properties.Settings.StorageConnectionString"].ConnectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            CloudTable cloudTable = tableClient.GetTableReference("AuditEvents");
            cloudTable.CreateIfNotExists();

            TableBatchOperation tableOperation = new TableBatchOperation();

            string resourceId = "6d561db9-2a5c-4729-ab98-f8f9b58598e7";
            for (int i = 0; i < 100; i++)
            {
                AuditItem auditItem = new AuditItem("ff554330-6901-4266-8410-f25d0f110792", resourceId, DateTime.UtcNow, "SELECT * FROM X");
                tableOperation.Insert(auditItem);
            }
            cloudTable.ExecuteBatch(tableOperation);

            TableQuery<AuditItem> query = new TableQuery<AuditItem>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "ff554330-6901-4266-8410-f25d0f110792_6d561db9-2a5c-4729-ab98-f8f9b58598e7"));
            foreach (AuditItem auditItem in cloudTable.ExecuteQuery(query))
            {
                Console.WriteLine("{0} {1}", auditItem.EventTime, auditItem.ETag);
            }

            //EventLog.CreateEventSource("YoavEventSource", "LogName");
            Logger.Instance.Test("A message2");

            Console.WriteLine(string.Join("", "a", "b"));
            */
        }


        /*public CloudTable GetTable(string tableName)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["SillyTest.Properties.Settings.StorageConnectionString"].ConnectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            CloudTable cloudTable = tableClient.GetTableReference(tableName);
            return cloudTable;
        }*/

    }
 
}

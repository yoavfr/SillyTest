using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SasKeys
{
    public class GenerateSasKey
    {
        /// <summary>
        /// Creates a Windows Azure SAS key for adding entries to a table. 
        /// Adds a shared policy on the table so that access can later be revoked. If an existing policy with the specified name already
        /// exists, it is used or altered to match the required level of access (SharedAccessTablePermissions.Add).
        /// </summary>
        /// <param name="storageConnectionString">Connection string to storage account where the table resides</param>
        /// <param name="tableName">The name of the table for which access is granted. If the table does not exist, it is created.</param>
        /// <param name="sharedPolicyName">The name that is given to the shared policy through which the SAS key is generated</param>
        /// <param name="expireAfter">How long before the key should expire</param>
        /// <returns></returns>
        public Uri CreateAddOnlySasKey(string storageConnectionString, string tableName, string sharedPolicyName, TimeSpan expireAfter)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(storageConnectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference(tableName);
            
            // Create the table if doesn't exist
            table.CreateIfNotExists();

            // Create a target Shared Policy with desired expiration time and permissions - can later be used to revoke the sas key
            SharedAccessTablePolicy targetSharedPolicy = new SharedAccessTablePolicy()
            {
                SharedAccessExpiryTime = DateTime.UtcNow.Add(expireAfter),
                Permissions = SharedAccessTablePermissions.Add
            };

            // Get current table permissions
            TablePermissions tablePermissions = table.GetPermissions();

            // If the table already has a policy with this name
            if (tablePermissions.SharedAccessPolicies.ContainsKey(sharedPolicyName))
            {
                // update it's values to match the target values
                Console.WriteLine("Updating existing shared policy");
                SharedAccessTablePolicy existingSharedPolicy = tablePermissions.SharedAccessPolicies[sharedPolicyName];
                existingSharedPolicy.SharedAccessExpiryTime = targetSharedPolicy.SharedAccessExpiryTime;
                existingSharedPolicy.Permissions = targetSharedPolicy.Permissions;
                table.SetPermissions(tablePermissions);
            }
            else
            {
                // No such policy found - create a new one
                Console.WriteLine("Adding new shared access policy to table");
                tablePermissions.SharedAccessPolicies.Add(sharedPolicyName, targetSharedPolicy);
                table.SetPermissions(tablePermissions);
            }

            // Generate the SAS key
            string sasTableToken = table.GetSharedAccessSignature(
                null,
                sharedPolicyName,
                null,
                null,
                null,
                null);

            Uri sasUri = new Uri(table.Uri + sasTableToken);
            Console.WriteLine("Sas Uri: {0}",sasUri);
            return sasUri;
        }

        public void TestReadWriteWithSas (Uri sasUri)
        {
            TableBatchOperation tableOperation = new TableBatchOperation();
            for (int i = 0; i < 100; i++)
            {
                AuditItem auditItem = new AuditItem("serverName", "tableName"+i, DateTime.UtcNow, "SELECT * FROM X"+i);
                tableOperation.Insert(auditItem);
            }
            CloudTable cloudTable = new CloudTable(sasUri);
            cloudTable.ExecuteBatch(tableOperation);
            Console.WriteLine("Wrote 100 entries to {0}", sasUri);

            // trying to read should fail
            TableQuery<AuditItem> query = new TableQuery<AuditItem>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "serverName"));
            foreach (AuditItem auditItem in cloudTable.ExecuteQuery(query))
            {
                Console.WriteLine("{0} {1}", auditItem.EventTime, auditItem.ETag);
            }
        }

        public static void Main(string[] args)
        {
            GenerateSasKey test = new GenerateSasKey();
            Uri sasUri = test.CreateAddOnlySasKey(
                "DefaultEndpointsProtocol=https;AccountName=bugtrackeraudit;AccountKey=3VjaEEtopPfOkGT65l5UnO4C3jtjfdBPnD4BEFK4xIMbR/4UbbFctAsuQTpgmzQ9ASRtXF/fPgstPwhOePymqA==",
                "tableX",
                "Db5SasPolicy",
                TimeSpan.FromDays(365));
            //sasUri = new Uri("https://bugtrackeraudit.table.core.windows.net/tableX?sv=2012-02-12&tn=tableX&si=Db5SasPolicy&sig=khZqOu4qg4OjU7nInah0t2lq%2F1TO8h8Qv33OZAu%2BP9o%3D");
            test.TestReadWriteWithSas(sasUri);
        }
    }
}

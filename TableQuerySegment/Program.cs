using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.RetryPolicies;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TableQuerySegment
{
    class Program
    {
        static void Main(string[] args)
        {
            //new Program().RunAsync();
        }

        /*public async Task RunAsync<T>()
        {
            string query = null;
            string connectionString = @"DefaultEndpointsProtocol=https;AccountName=dataseccomponenttests;AccountKey=GOhgbhLrktfenh5mQ7i3JUkfoDq/UpS3+WhN+BZIhhyJ0PH75uLXh4faPCLx9qb5BISNqVhf9OEpKVgX7phwbg==";
            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(connectionString);
            var tableStorage = cloudStorageAccount.CreateCloudTableClient();
            CloudTable table = tableStorage.GetTableReference("MyTableName");
            TableQuerySegment<T> querySegment = null;

            TableRequestOptions requestOptions = new TableRequestOptions()
            {
                RetryPolicy = new LinearRetry(TimeSpan.FromSeconds(2), 3),
                MaximumExecutionTime = TimeSpan.FromSeconds(5),
                ServerTimeout = TimeSpan.FromSeconds(5)
            };

            OperationContext operationContext = new OperationContext();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            CancellationToken token = cancellationTokenSource.Token;
            operationContext.ResponseReceived += (sender, eventArgs) =>
            {
                if (((OperationContext)sender).LastResult.HttpStatusCode != (int)HttpStatusCode.OK)
                {
                    cancellationTokenSource.Cancel();
                }
            };

            while (querySegment == null || querySegment.ContinuationToken != null)
            {
                TableContinuationToken nextSeg = querySegment != null ? querySegment.ContinuationToken : null;
                // bug? this never returns on http: 404 (table doesn't exist)
                //Task<TableQuerySegment<T>> task = table.ExecuteQuerySegmentedAsync(
                querySegment = await table.ExecuteQuerySegmentedAsync(
                    query,
                    nextSeg,
                    requestOptions,
                    operationContext,
                    token);
                //task.Wait();
                //querySegment = task.Result;
            }

        }*/
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
 using System;
    using Azure.Storage.Queues;
    using Microsoft.Azure.WebJobs;

namespace QueueStorageFunction


    {
        public class QueueStorageFunction
        {
            // Define a function named "QueueMessage" to handle incoming queue messages
            [FunctionName("QueueMessage")]
            public void Run(
                // This function is triggered when a new message is added to the specified queue
                [QueueTrigger("your-queue-name", Connection = "AzureWebJobsStorage")] string myQueueItem,
                ILogger log,
                // ICollector is used to collect and write messages to a different queue (sample-queue)
                [Queue("sample-queue", Connection = "QueueStorageConnectionString")] ICollector<string> processedQueue)
            {
                try
                {
                    // Log the incoming queue message that is being processed
                    log.LogInformation($"Processing queue message: {myQueueItem}");

                    // Prepare a new message for writing to the second queue
                    string processedMessage = $"Processed message: {myQueueItem}";

                    // Log the message that will be written to the second queue
                    log.LogInformation($"Writing message to processed queue: {processedMessage}");

                    // Add the processed message to the second queue (sample-queue)
                    processedQueue.Add(processedMessage);
                }
                catch (Exception ex)
                {
                    // Log any errors that occur during message processing
                    log.LogError($"Error processing queue message: {ex.Message}");
                }
            }
        }
    }

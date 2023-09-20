using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using LetsGetChecked.Resequencer.API.Factories;
using LetsGetChecked.Resequencer.API.Interfaces;

namespace LetsGetChecked.Resequencer.API.HostedServices
{
    public class ResequencerHostedService : IHostedService
    {
        private readonly ILogger<ResequencerHostedService> _logger;
        private readonly IShardProcessorService _shardProcessorService;

        public ResequencerHostedService(
            ILogger<ResequencerHostedService> logger,
            IShardProcessorService shardProcessorService)
        {
            _logger = logger;
            _shardProcessorService = shardProcessorService;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Start hosted service.");

            using (var client = AWSClientFactory.CreateDynamoDBClient())
            {
                var tableDescription = await client.DescribeTableAsync("physicians_requests");

                var streamArn = tableDescription.Table.LatestStreamArn;

                _logger.LogInformation($"Connected to DynamoDB client.");

                using (var streamsClient = AWSClientFactory.CreateDynamoDBStreamClient())
                {
                    var stream = await streamsClient.DescribeStreamAsync(streamArn);
                    var shards = stream.StreamDescription.Shards;

                    await _shardProcessorService.Process(shards, streamArn, streamsClient);
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stop hosted service.");
            return Task.CompletedTask;
        }
    }
}

using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using LetsGetChecked.Resequencer.API.Interfaces;

namespace LetsGetChecked.Resequencer.API.Services
{
    public class ShardProcessorService : IShardProcessorService
    {
        private readonly ILogger<ShardProcessorService> _logger;
        private readonly IResequencerService _resequencerService;

        public ShardProcessorService(
            ILogger<ShardProcessorService> logger,
            IResequencerService resequencerService)
        {
            _logger = logger;
            _resequencerService = resequencerService;
        }

        public async Task Process(List<Shard> shards, string streamArn, IAmazonDynamoDBStreams streamsClient)
        {
            _logger.LogInformation($"Connected to DynamoDB Stream client.");

            foreach (var shard in shards)
            {
                var shardId = shard.ShardId;

                _logger.LogInformation($"Searching in shard {shardId}.");

                var shardInterator = new GetShardIteratorRequest()
                {
                    ShardId = shardId,
                    StreamArn = streamArn,
                    ShardIteratorType = ShardIteratorType.TRIM_HORIZON
                    //SequenceNumber = "49644481681152135247613753773917925151980257086390927362"
                };

                var getShardIteratorResult = await streamsClient.GetShardIteratorAsync(shardInterator);

                var currentShardIter = getShardIteratorResult.ShardIterator;

                //Just to close the stream processing before the shard change to sealed, this process might take 4 hours.
                int maxRecords = 100;
                int processedRecordCount = 0;
                while (currentShardIter != null && processedRecordCount < maxRecords)
                {
                    _logger.LogInformation($"Shard IteratorId in shard {shardId}.");

                    var recordsResult = await streamsClient.GetRecordsAsync(currentShardIter);

                    _resequencerService.SortRecords(recordsResult);

                    currentShardIter = recordsResult.NextShardIterator;
                    processedRecordCount += recordsResult.Records.Count;
                }
            }
        }
    }
}

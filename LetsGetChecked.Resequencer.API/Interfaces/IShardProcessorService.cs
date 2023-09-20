using Amazon.DynamoDBv2.Model;
using Amazon.DynamoDBv2;

namespace LetsGetChecked.Resequencer.API.Interfaces
{
    public interface IShardProcessorService
    {
        Task Process(List<Shard> shards, string streamArn, IAmazonDynamoDBStreams streamsClient);
    }
}

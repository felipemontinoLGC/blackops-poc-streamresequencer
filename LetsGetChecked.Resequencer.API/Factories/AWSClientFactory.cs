using Amazon;
using Amazon.DynamoDBv2;
using Amazon.Runtime;

namespace LetsGetChecked.Resequencer.API.Factories
{
    public static class AWSClientFactory
    {
        public static IAmazonDynamoDB CreateDynamoDBClient()
        {
            var clientConfig = new AmazonDynamoDBConfig()
            {
                RegionEndpoint = RegionEndpoint.EUWest1,
                //ServiceURL = "http://localhost:4566",
                AuthenticationRegion = "eu-west-1"
            };

            return new AmazonDynamoDBClient(new BasicAWSCredentials("test", "test"), clientConfig);
        }

        public static IAmazonDynamoDBStreams CreateDynamoDBStreamClient()
        {
            var streamClientConfig = new AmazonDynamoDBStreamsConfig
            {
                RegionEndpoint = RegionEndpoint.EUWest1,
                //ServiceURL = "http://localhost:4566",
                AuthenticationRegion = "eu-west-1"
            };

            return new AmazonDynamoDBStreamsClient(new BasicAWSCredentials("test", "test"), streamClientConfig);
        }
    }
}

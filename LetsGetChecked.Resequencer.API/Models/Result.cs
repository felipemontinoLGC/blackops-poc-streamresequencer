using Amazon.DynamoDBv2.Model;

namespace LetsGetChecked.Resequencer.API.Models
{
    public class Result
    {
        public string Id { get; set; }

        public string Version { get; set; }

        public long EventSize { get; set; }

        public DateTime CreationDate { get; set; }

        public string SequenceNumber { get; set; }

        public Result(string id, string version, long eventSize, DateTime creationDate, string sequenceNumber)
        {
            Id = id;
            Version = version;
            EventSize = eventSize;
            CreationDate = creationDate;
            SequenceNumber = sequenceNumber;
        }

        public static Result Deserialize(Record record)
        {
            var id = record.Dynamodb.Keys["id"].S.ToString();
            var version = record.Dynamodb.Keys["version"].S.ToString();
            var size = record.Dynamodb.SizeBytes;
            var creationDateTime = record.Dynamodb.ApproximateCreationDateTime;
            var sequenceNumber = record.Dynamodb.SequenceNumber;

            return new Result(id, version, size, creationDateTime, sequenceNumber);
        }
    }
}

using Amazon.DynamoDBv2.Model;
using LetsGetChecked.Resequencer.API.Interfaces;
using LetsGetChecked.Resequencer.API.Models;

namespace LetsGetChecked.Resequencer.API.Services
{
    public class ResequencerService : IResequencerService
    {
        private readonly ILogger<ResequencerService> _logger;

        public ResequencerService(ILogger<ResequencerService> logger)
        {
            _logger = logger;
        }

        public void SortRecords(GetRecordsResponse getRecordsResponse)
        {
            List<Record> records = getRecordsResponse.Records;
            PrintResults(records, "original");

            var recordsGroupById = records.GroupBy(r => r.Dynamodb.Keys["id"].S, r => r);

            var sortedOrderedRecords = recordsGroupById.Select(r => r.OrderBy(s => Convert.ToInt32(s.Dynamodb.Keys["version"].S))).ToList();

            var sortedRecords = sortedOrderedRecords.SelectMany(s => s).ToList();

            PrintResults(sortedRecords, "sorted");
        }

        private void PrintResults(List<Record> records, string title)
        {
            foreach (var record in records)
            {
                var result = Result.Deserialize(record);
                _logger.LogInformation($"Processed Record - {title} - {Newtonsoft.Json.JsonConvert.SerializeObject(result)}");
            }
        }
    }
}

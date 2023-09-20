using Amazon.DynamoDBv2.Model;

namespace LetsGetChecked.Resequencer.API.Interfaces
{
    public interface IResequencerService
    {
        void SortRecords(GetRecordsResponse getRecordsResponse);
    }
}

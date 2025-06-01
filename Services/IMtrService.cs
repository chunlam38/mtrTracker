using MtrTracker.Data;

namespace MtrTracker.Services;

public interface IMtrService
{
   Task<StationInformation?> GetTrainSchedules(string line, string station);
}
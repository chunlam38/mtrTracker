using System.Net.Http;
using System.Threading.Tasks;
using MtrTracker.Data;
using MtrTracker.Data.MtrApiResponse;
using MtrTracker.Services;
using Newtonsoft.Json;

public class MtrService : IMtrService
{
    private readonly HttpClient _httpClient;

    public MtrService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<StationInformation> GetTrainSchedules(string line, string station)
    {
        var response = await _httpClient.GetAsync($"https://rt.data.gov.hk/v1/transport/mtr/getSchedule.php?line={line}&sta={station}");
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadAsStringAsync();
            Console.WriteLine("API Response:");
            Console.WriteLine(result);  // This will show the raw JSON response
            var apiResponse = JsonConvert.DeserializeObject<MtrApiResponse>(result);
            Console.WriteLine(line, station);

            return MapApiResponseToStationInfo(apiResponse, line, station);
        }
        return null;
    }
    public StationInformation MapApiResponseToStationInfo(MtrApiResponse apiResponse, string line, string station)
    {
        // Construct the station key (e.g., "TKL-TKO")
        var stationKey = $"{line}-{station}";

        // Use the station key to fetch the relevant station data
        if (!apiResponse.Data.TryGetValue(stationKey, out var stationData))
        {
            Console.WriteLine($"Station data for {stationKey} not found.");
            return null;
        }

        var stationNameMappings = StationMappings.CodeToStationName;
        
        // Map to StationInformation
        var stationInfo = new StationInformation
        {
            // Only process upstream trains if stationData.Up is not null or empty
            UpstreamTrains = stationData.Up != null && stationData.Up.Any()
                ? stationData.Up.Select(train => new TrainInformation
                {
                    Destination = stationNameMappings[train.Dest],
                    // Check if the time is not null or empty before parsing
                    ArrivalTime = DateTime.TryParse(train.Time, out var arrivalTime) ? arrivalTime : DateTime.MinValue,
                    TimeUpdated = DateTime.TryParse(apiResponse.SysTime, out var sysTime) ? sysTime : DateTime.MinValue
                }).ToList() : new List<TrainInformation>(),
            // Only process downstream trains if stationData.Down is not null or empty
            DownstreamTrains = stationData.Down != null && stationData.Down.Any()
                ? stationData.Down.Select(train => new TrainInformation
                {
                    Destination = stationNameMappings[train.Dest],
                    // Check if the time is not null or empty before parsing
                    ArrivalTime = DateTime.TryParse(train.Time, out var arrivalTime) ? arrivalTime : DateTime.MinValue,
                    TimeUpdated = DateTime.TryParse(apiResponse.SysTime, out var sysTime) ? sysTime : DateTime.MinValue
                }).ToList() : new List<TrainInformation>(),
            TimeUpdated = DateTime.TryParse(apiResponse.SysTime, out var apiSysTime) ? apiSysTime : DateTime.Now
        };

        return stationInfo;
    }

}
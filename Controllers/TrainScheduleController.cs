using Microsoft.AspNetCore.Mvc;
using MtrTracker.Data;
using System.Web;  // Import System.Web for URL decoding

[Route("api/getTrainSchedule")]
[ApiController]
public class TrainSchedulesController : ControllerBase
{
    private readonly MtrService _mtrService;

    public TrainSchedulesController(MtrService mtrService)
    {
        _mtrService = mtrService;
    }

    [HttpGet("{stationName}")]
    public async Task<IActionResult> GetTrainSchedules(string stationName)
    {
        // Decode the URL-encoded station name
        string decodedStationName = HttpUtility.UrlDecode(stationName);

        // Check if the station name exists in the mappings
        if (!StationMappings.StationNameToCode.ContainsKey(decodedStationName))
        {
            return NotFound($"Station '{decodedStationName}' not found.");
        }

        // Get the station code and lines
        var stationCode = StationMappings.StationNameToCode[decodedStationName];
        var lines = StationMappings.StationNameToLines[decodedStationName];

        // Create a dictionary to store schedules for each line
        var allSchedules = new Dictionary<string, StationInformation>();

        // Fetch schedules for each line
        foreach (var line in lines)
        {
            try
            {
                var schedule = await _mtrService.GetTrainSchedules(line, stationCode);
                if (schedule != null)
                {
                    // Use the line name from the Lines dictionary
                    var lineName = StationMappings.Lines[line].name;
                    allSchedules[lineName] = schedule;
                }
            }
            catch (Exception ex)
            {
                // Log the error but continue with other lines
                Console.WriteLine($"Error fetching schedule for line {line}: {ex.Message}");
            }
        }

        if (allSchedules.Count == 0)
        {
            return NotFound($"No train schedules found for station '{decodedStationName}'.");
        }

        return Ok(allSchedules);
    }
}
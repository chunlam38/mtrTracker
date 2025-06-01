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
        // Decode the URL-encoded station name (this handles spaces like '%20')
        string decodedStationName = HttpUtility.UrlDecode(stationName);

        // Check if the station name exists in the mappings
        if (!StationMappings.StationNameToCode.ContainsKey(decodedStationName))
        {
            return NotFound("Station name not found.");
        }

        var stationCode = StationMappings.StationNameToCode[decodedStationName];
        var lines = StationMappings.StationNameToLines[decodedStationName];

        // Assuming we just want the first line for simplicity
        var line = lines.FirstOrDefault();

        var trainSchedules = await _mtrService.GetTrainSchedules(line, stationCode);
        
        if (trainSchedules == null)
        {
            return NotFound("No train schedules found for this station.");
        }
        
        return Ok(trainSchedules);
    }
}
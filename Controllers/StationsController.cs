using Microsoft.AspNetCore.Mvc;
using MtrTracker.Data;
using System.Linq;

namespace MtrTracker.Controllers
{
    [Route("api/stations")]
    [ApiController]
    public class StationsController : ControllerBase
    {
        // This endpoint returns a list of station names and their codes
        [HttpGet]
        public IActionResult GetStations()
        {
            // Return the dictionary containing station name and code
            return Ok(StationMappings.StationNameToCode);
        }

        // This endpoint returns stations for a specific line
        [HttpGet("byline/{line}")]
        public IActionResult GetStationsByLine(string line)
        {
            var stations = StationMappings.StationNameToLines
                .Where(x => x.Value.Contains(line))
                .ToDictionary(x => x.Key, x => StationMappings.StationNameToCode[x.Key]);

            if (stations.Count == 0)
            {
                return NotFound($"No stations found for line {line}");
            }

            return Ok(stations);
        }
    }
}
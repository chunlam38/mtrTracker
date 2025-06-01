namespace MtrTracker.Data;

public class StationInformation
{
    public IEnumerable<TrainInformation>? UpstreamTrains { get; set; }
    public IEnumerable<TrainInformation>? DownstreamTrains { get; set; }
    public DateTime TimeUpdated { get; set; }
}
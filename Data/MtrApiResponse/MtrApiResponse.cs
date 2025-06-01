using System.Text.Json.Serialization;

namespace MtrTracker.Data.MtrApiResponse
{
    public class MtrApiResponse
    {
        [JsonPropertyName("sys_time")]
        public string SysTime { get; set; }

        [JsonPropertyName("curr_time")]
        public string CurrTime { get; set; }

        [JsonPropertyName("data")]
        public Dictionary<string, MtrStationData> Data { get; set; }

        [JsonPropertyName("isdelay")]
        public string IsDelay { get; set; }

        [JsonPropertyName("status")]
        public int Status { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }
    }

    public class MtrStationData
    {
        [JsonPropertyName("curr_time")]
        public string CurrTime { get; set; }

        [JsonPropertyName("sys_time")]
        public string SysTime { get; set; }

        [JsonPropertyName("UP")]
        public List<MtrTrain> Up { get; set; }

        [JsonPropertyName("DOWN")]
        public List<MtrTrain> Down { get; set; }
    }

    public class MtrTrain
    {
        [JsonPropertyName("seq")]
        public string Seq { get; set; }

        [JsonPropertyName("dest")]
        public string Dest { get; set; }

        [JsonPropertyName("plat")]
        public string Plat { get; set; }

        [JsonPropertyName("time")]
        public string Time { get; set; }

        [JsonPropertyName("ttnt")]
        public string Ttnt { get; set; }

        [JsonPropertyName("valid")]
        public string Valid { get; set; }

        [JsonPropertyName("source")]
        public string Source { get; set; }
    }
}
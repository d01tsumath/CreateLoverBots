using System.Text.Json.Serialization;

namespace BotGetsEvent.Models
{
    public class Events
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("channel")]
        public string Channel { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("user")]
        public string User { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("text")]
        public string Text { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("ts")]
        public string TimeStamp { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("event_ts")]
        public string EventTimeStamp { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("channel_type")]
        public string ChannelType { get; set; }
    }
}

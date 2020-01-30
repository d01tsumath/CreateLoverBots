using System.Text.Json.Serialization;

namespace BotGetsEvent.Model
{
    /// <summary>
    /// Events API リクエストで受け取る Json のモデルです。
    /// </summary>
    public class EventRequestModel
    {
        /// <summary>
        /// トークン
        /// </summary>
        [JsonPropertyName("token")]
        public string Token { get; set; }

        /// <summary>
        /// チャレンジ
        /// </summary>
        [JsonPropertyName("challenge")]
        public string Challenge { get; set; }

        /// <summary>
        /// タイプ
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; }
    }
}

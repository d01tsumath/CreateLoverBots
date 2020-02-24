using System.Text.Json.Serialization;

namespace SlackCallbackEvent.Models
{
    /// <summary>
    /// Events API リクエストで受け取る Json のモデルです。
    /// </summary>
    public class EventRequestModel
    {
        #region 共通
        /// <summary>
        /// トークン
        /// </summary>
        [JsonPropertyName("token")]
        public string Token { get; set; }

        /// <summary>
        /// タイプ
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; }

        #endregion

        #region 登録時のみ

        /// <summary>
        /// チャレンジ
        /// </summary>
        [JsonPropertyName("challenge")]
        public string Challenge { get; set; }

        #endregion

        #region 通常のイベント時

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("team_id")]
        public string TeamId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("api_app_id")]
        public string ApiAppId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("event")]
        public Events Event { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("authed_teams")]
        public string[] AuthedTeams { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("event_id")]
        public string EventId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("event_time")]
        public int EventTime { get; set; }

        #endregion
    }
}

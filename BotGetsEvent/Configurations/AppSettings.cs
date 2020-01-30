namespace BotGetsEvent.Configurations
{
    /// <summary>
    /// アプリケーション設定を表します。
    /// </summary>
    public class AppSettings
    {
        /// <summary>
        /// Slack の構成情報を取得します。
        /// </summary>
        public SlackConfiguration SlackConfiguration { get; private set; }
    }
}

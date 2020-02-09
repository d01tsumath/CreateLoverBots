namespace SlackCallbackEvent.Configurations
{
    /// <summary>
    /// Slack の構成情報を表します。
    /// </summary>
    public class SlackConfiguration
    {
        public string HarukaWebhookEndpoint { get; private set; }

        public string MakotoWebhookEndpoint { get; private set; }

        public string RinWebhookEndpoint { get; private set; }

        public string NagisaWebhookEndpoint { get; private set; }

        public string ReiWebhookEndpoint { get; private set; }
    }
}

using SlackCallbackEvent.Configurations;
using SlackCallbackEvent.Models;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SlackCallbackEvent.Domain.Services
{
    /// <summary>
    /// Slack Bot の処理について提供します。
    /// </summary>
    public class BotService
    {

        #region プロパティ

        private HttpClient HttpClient { get; set; }

        private SlackConfiguration SlackConfiguration { get; set; }

        #endregion

        #region コンストラクタ

        public BotService(HttpClient httpClient, SlackConfiguration configuration)
        {
            if (httpClient == null) throw new ArgumentNullException(nameof(httpClient));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            HttpClient = httpClient;
            SlackConfiguration = configuration;
        }

        #endregion

        /// <summary>
        /// ペイロードを処理します。
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        public async Task ProcessAsync(EventRequestModel model)
        {
            if (model.Type.Equals("event_callback"))
            {
                // "event_callback" のときのみ
                if (model.Event.Text.Contains("はるちゃん"))
                {
                    await this.SpeakMessageAsync();
                }
            }
            return;
        }

        /// <summary>
        /// Bot に発言させます。
        /// </summary>
        /// <returns></returns>
        public async Task SpeakMessageAsync()
        {
            var slack_text = "本気のときはいつもフリーだ";
            var uri = new Uri(SlackConfiguration.WebhookEndpoint);

            var data = JsonSerializer.Serialize(new
            {
                text = slack_text,
            });

            var content = new StringContent(data, Encoding.UTF8, @"application/json");

            try
            {
                var response = await HttpClient.PostAsync(uri, content);
            }
            catch(HttpRequestException e)
            {
                throw new HttpRequestException(e.Message);
            }

        }
    }
}

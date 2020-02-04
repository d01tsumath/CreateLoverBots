using BotGetsEvent.Configurations;
using BotGetsEvent.Models;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BotGetsEvent.Domain.Services
{
    /// <summary>
    /// Slack Bot の処理について提供します。
    /// </summary>
    public class BotService
    {

        #region プロパティ

        private SlackConfiguration SlackConfiguration { get; set; }

        #endregion

        #region コンストラクタ

        public BotService(SlackConfiguration configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            SlackConfiguration = configuration;
        }

        #endregion

        /// <summary>
        /// ペイロードを処理します。
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        public async Task ProcessAsync(string payload)
        {
            var json = JsonSerializer.Deserialize<EventRequestModel>(payload);
            if (json.Type.Equals("event_callback"))
            {
                // "event_callback" のときのみ
                await this.SpeakMessageAsync();
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

            var client = new HttpClient();
            try
            {
                var response = await client.PostAsync(uri, content);
            }
            catch(HttpRequestException e)
            {
                throw new HttpRequestException(e.Message);
            }

        }
    }
}

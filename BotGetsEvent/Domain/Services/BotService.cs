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
        private AppSettings AppSettings { get; set; }

        #region コンストラクタ

        public BotService(AppSettings appSettings)
        {
            if (appSettings == null)
                throw new ArgumentNullException(nameof(appSettings));

            AppSettings = appSettings;
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

        public async Task SpeakMessageAsync()
        {
            var slack_text = "本気のときはいつもフリーだ";
            var uri = new Uri(AppSettings.SlackConfiguration.WebhookEndpoint);

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

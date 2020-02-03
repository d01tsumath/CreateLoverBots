using BotGetsEvent.Configurations;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BotGetsEvent.Domain.Services
{
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

        public async Task SpeakMessageAsync(ILogger logger)
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
                logger.LogError(e.Message);
            }

        }
    }
}

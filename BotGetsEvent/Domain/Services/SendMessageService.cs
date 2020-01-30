using BotGetsEvent.Configurations;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BotGetsEvent.Controller
{
    public class SendMessageService
    {
        private AppSettings AppSettings { get; set; }

        public SendMessageService(AppSettings appSettings)
        {
            if (appSettings == null)
                throw new ArgumentNullException(nameof(appSettings));

            AppSettings = appSettings;
        }

        public async Task<IActionResult> SendMessageAsync()
        {
            var slack_text = "本気のときはいつもフリーだ";
            var uri = new Uri(AppSettings.SlackConfiguration.WebhookEndpoint);

            var data = JsonSerializer.Serialize(new
            {
                text = slack_text,
            });

            var content = new StringContent(data, Encoding.UTF8, @"application/json");

            var client = new HttpClient();
            var response = await client.PostAsync(uri, content);
            if (response.IsSuccessStatusCode)
                return new AcceptedResult();

            return new BadRequestResult();
        }
    }
}

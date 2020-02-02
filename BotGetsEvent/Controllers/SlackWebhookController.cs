using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.IO;
using BotGetsEvent.Models;
using BotGetsEvent.Domain.Services;

namespace BotGetsEvent.Controllers
{
    /// <summary>
    /// Slack の Events API リクエストを受け取ります。
    /// </summary>
    public class SlackWebhookController
    {
        private SendMessageService _sendMessageService;

        public SlackWebhookController(SendMessageService sendMessageService)
        {
            _sendMessageService = sendMessageService;
        }

        [FunctionName("Api_ReturnMessage")]
        public async ValueTask<IActionResult> ReturnMessageAsync(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest request,
            ILogger log)
        {
            var requestBody = await new StreamReader(request.Body).ReadToEndAsync();
            var json = JsonSerializer.Deserialize<EventRequestModel>(requestBody);
            
            // 中身確認したい
            log.LogInformation($"token:{json.Token}, challenge:{json.Challenge}, type:{json.Type}");

            // Bot からメッセージをしゃべらせる
            var result = await _sendMessageService.SendMessageAsync();

            // そのまま challenge の値を返す
            return new ObjectResult(JsonSerializer.Serialize(new { challenge = json.Challenge }));
        }

    }
}

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
using Microsoft.Azure.Storage.Queue;
using System;

namespace BotGetsEvent.Controllers
{
    /// <summary>
    /// Slack の Events API リクエストを受け取ります。
    /// </summary>
    public class SlackWebhookController
    {
        #region 定数
        /// <summary>
        /// Queue 名を表します。
        /// </summary>
        private const string QueueName = "slack-message";
        #endregion

        private StorageQueueService _queueService;

        private BotService _botService;

        public SlackWebhookController(StorageQueueService queueService, BotService botService)
        {
            _queueService = queueService;
            _botService = botService;
        }

        [FunctionName("Http_ReturnMessage")]
        public async ValueTask<IActionResult> ReturnMessageAsync(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest request,
            ILogger log)
        {
            try
            {
                var requestBody = await new StreamReader(request.Body).ReadToEndAsync();
                var json = JsonSerializer.Deserialize<EventRequestModel>(requestBody);

                // 中身確認したい
                log.LogInformation($"token:{json.Token}, challenge:{json.Challenge}, type:{json.Type}");
                
                if (json.Type.Equals("url_verification"))
                {
                    // "url_verification" ならそのまま challenge の値を返す
                    return new ObjectResult(JsonSerializer.Serialize(new { challenge = json.Challenge }));
                }

                // "url_verification" 以外なら Queue へ投げる
                await _queueService.AddAsync(QueueName, requestBody).ConfigureAwait(false);
                return new OkObjectResult("");
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Event Callback の処理に失敗しました。");
                return new ObjectResult(ex);
            }

        }

        /// <summary>
        /// Slack の Event API のデータを処理します。
        /// </summary>
        /// <returns></returns>
        [FunctionName("Queue_ProcessMessage")]
        public async Task ProcessPayload(
            [QueueTrigger(QueueName)]CloudQueueMessage message)
        {
            
        }

    }
}

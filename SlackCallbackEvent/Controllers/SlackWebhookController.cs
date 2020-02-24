using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.IO;
using SlackCallbackEvent.Models;
using SlackCallbackEvent.Domain.Services;
using Microsoft.Azure.Storage.Queue;
using System;

namespace SlackCallbackEvent.Controllers
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

        #region プロパティ

        private StorageQueueService QueueService { get; }

        private BotService BotService { get; }

        private ILogger<SlackWebhookController> Logger { get; }

        #endregion

        #region コンストラクタ

        public SlackWebhookController(StorageQueueService queueService, BotService botService, ILogger<SlackWebhookController> logger)
        {
            QueueService = queueService;
            BotService = botService;
            Logger = logger;
        }

        #endregion

        /// <summary>
        /// Slack Event API からの Webhook を処理します。
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [FunctionName("Http_ReturnMessage")]
        public async ValueTask<IActionResult> ReturnMessageAsync(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest request)
        {
            try
            {
                var requestBody = await new StreamReader(request.Body).ReadToEndAsync();
                var json = JsonSerializer.Deserialize<EventRequestModel>(requestBody);

                // 中身確認したい
                Logger.LogInformation($"token:{json.Token}, challenge:{json.Challenge}, type:{json.Type}");
                
                if (json.Type.Equals("url_verification"))
                {
                    // "url_verification" ならそのまま challenge の値を返す
                    return new ObjectResult(JsonSerializer.Serialize(new { challenge = json.Challenge }));
                }

                // "url_verification" 以外は何が入っているのか見たい
                Logger.LogInformation($"Type:{json.Type}, User:{json.Event.User}");

                // "url_verification" 以外なら Queue へ投げる
                await QueueService.AddAsync(QueueName, requestBody).ConfigureAwait(false);
                return new OkObjectResult("");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Event Callback の処理に失敗しました。");
                return new ObjectResult(ex);
            }

        }

        /// <summary>
        /// Slack の Event API のデータを処理します。
        /// </summary>
        /// <returns></returns>
        [FunctionName("Queue_ProcessMessage")]
        public async Task ProcessPayload(
            [QueueTrigger(QueueName)]EventRequestModel eventModel)
        {
            try
            {
                this.Logger.LogInformation($"データ処理 : Start | MessageId : {eventModel.ApiAppId}");
                await this.BotService.ProcessAsync(eventModel).ConfigureAwait(false);
            }
            finally
            {
                this.Logger.LogInformation($"データ処理 : End | MessageId : {eventModel.ApiAppId}");
            }
        }

    }
}

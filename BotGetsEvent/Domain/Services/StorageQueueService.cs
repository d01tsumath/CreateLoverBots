using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Queue;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BotGetsEvent.Domain.Services
{
    public class StorageQueueService
    {
        #region プロパティ
        /// <summary>
        /// キュー操作用のクライアントを取得します。
        /// </summary>
        private CloudQueueClient Client { get; }
        #endregion

        #region コンストラクタ
        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="config"></param>
        public StorageQueueService(IConfigurationRoot config)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));

            var connectionString = config["AzureWebJobsStorage"];
            var account = CloudStorageAccount.Parse(connectionString);
            this.Client = account.CreateCloudQueueClient();
        }
        #endregion

        /// <summary>
        /// 指定されたキューにデータを投入します。
        /// </summary>
        /// <param name="queueName"></param>
        /// <param name="message"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async ValueTask AddAsync(string queueName, CloudQueueMessage message, CancellationToken cancellationToken = default)
        {
            var queue = this.Client.GetQueueReference(queueName);
            await queue.CreateIfNotExistsAsync(cancellationToken).ConfigureAwait(false);
            await queue.AddMessageAsync(message, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// 指定されたキューにデータを投入します。
        /// </summary>
        /// <param name="queueName"></param>
        /// <param name="content"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public ValueTask AddAsync(string queueName, string content, CancellationToken cancellationToken = default)
        {
            var message = new CloudQueueMessage(content);
            return this.AddAsync(queueName, message, cancellationToken);
        }
    }
}

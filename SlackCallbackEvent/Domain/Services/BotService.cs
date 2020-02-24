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
        #region 定数

        private const string Haruka_Nickname1 = "ハル";
        private const string Haruka_Nickname2 = "はるちゃん";
        private const string Haruka_Nickname3 = "遙先輩";

        private const string Makoto_Nickname1 = "真琴";
        private const string Makoto_Nickname2 = "まこちゃん";
        private const string Makoto_Nickname3 = "真琴先輩";

        private const string Rin_Nickname1 = "凛";
        private const string Rin_Nickname2 = "りんちゃん";
        private const string Rin_Nickname3 = "凛さん";

        private const string Nagisa_Nickname1 = "渚";
        private const string Nagisa_Nickname2 = "渚くん";

        private const string Rei_Nickname1 = "怜";
        private const string Rei_Nickname2 = "れいちゃん";

        #endregion

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
                // "event_callback" のとき
                if (model.Event.Text.Contains(Haruka_Nickname1) ||
                    model.Event.Text.Contains(Haruka_Nickname2)||
                    model.Event.Text.Contains(Haruka_Nickname3))
                {
                    // 遙の名前が呼ばれたとき
                    await HarukaSpeakAsync(model.Event.User);
                }

                if (model.Event.Text.Contains(Makoto_Nickname1) ||
                    model.Event.Text.Contains(Makoto_Nickname2) ||
                    model.Event.Text.Contains(Makoto_Nickname3))
                {
                    // 真琴の名前が呼ばれたとき
                    await MakotoSpeakAsync(model.Event.User);
                }


                if (model.Event.Text.Contains(Rin_Nickname1) ||
                    model.Event.Text.Contains(Rin_Nickname2) ||
                    model.Event.Text.Contains(Rin_Nickname3))
                {
                    // 凛の名前が呼ばれたとき
                    await RinSpeakAsync(model.Event.User);
                }


                if (model.Event.Text.Contains(Nagisa_Nickname1) ||
                    model.Event.Text.Contains(Nagisa_Nickname2))
                {
                    // 渚の名前が呼ばれたとき
                    await NagisaSpeakAsync(model.Event.User);
                }


                if (model.Event.Text.Contains(Rei_Nickname1) ||
                    model.Event.Text.Contains(Rei_Nickname2))
                {
                    // 怜の名前が呼ばれたとき
                    await ReiSpeakAsync(model.Event.User);
                }

                if (model.Event.Text.Contains("夕飯") ||
                    model.Event.Text.Contains("夜ごはん"))
                {
                    // 今日の夕飯を聞かれたとき
                    await HarukaSpeakDinnerAsync(model.Event.User);
                    await NagisaSpeakDinnerAsync();
                }
                if (model.Event.Text.Contains("鯖カレー"))
                {
                    // 「鯖」ワードが出たとき
                    await MakotoSpeakDinnerAsync();
                    await RinSpeakDinnerAsync();
                }
                if(model.Event.Text.Contains("イワトびっくりパン") && model.Event.Text.Contains("イチゴ味"))
                {
                    await ReiSpeakDinnerAsync();
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
            var uri = new Uri(SlackConfiguration.HarukaWebhookEndpoint);

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

        #region Haruka speaks

        public async Task HarukaSpeakAsync(string userId)
        {
            var slackText = $"<@{userId}> どうした";
            var uri = new Uri(SlackConfiguration.HarukaWebhookEndpoint);

            var data = JsonSerializer.Serialize(new
            {
                text = slackText,
            });

            var content = new StringContent(data, Encoding.UTF8, @"application/json");

            try
            {
                var response = await HttpClient.PostAsync(uri, content);
            }
            catch (HttpRequestException e)
            {
                throw new HttpRequestException(e.Message);
            }
        }

        public async Task HarukaSpeakDinnerAsync(string userId)
        {
            var slackText = $"<@{userId}> 今日は鯖カレーだ";
            var uri = new Uri(SlackConfiguration.HarukaWebhookEndpoint);

            var data = JsonSerializer.Serialize(new
            {
                text = slackText,
            });

            var content = new StringContent(data, Encoding.UTF8, @"application/json");

            try
            {
                var response = await HttpClient.PostAsync(uri, content);
            }
            catch (HttpRequestException e)
            {
                throw new HttpRequestException(e.Message);
            }
        }

        #endregion

        #region Makoto speaks

        public async Task MakotoSpeakAsync(string userId)
        {
            var slackText = $"<@{userId}> 呼んだ？";
            var uri = new Uri(SlackConfiguration.MakotoWebhookEndpoint);

            var data = JsonSerializer.Serialize(new
            {
                text = slackText,
            });

            var content = new StringContent(data, Encoding.UTF8, @"application/json");

            try
            {
                var response = await HttpClient.PostAsync(uri, content);
            }
            catch (HttpRequestException e)
            {
                throw new HttpRequestException(e.Message);
            }
        }

        public async Task MakotoSpeakDinnerAsync()
        {
            var slackText = "えっ、また鯖？今週毎日鯖だよね？";
            var uri = new Uri(SlackConfiguration.MakotoWebhookEndpoint);

            var data = JsonSerializer.Serialize(new
            {
                text = slackText,
            });

            var content = new StringContent(data, Encoding.UTF8, @"application/json");

            try
            {
                var response = await HttpClient.PostAsync(uri, content);
            }
            catch (HttpRequestException e)
            {
                throw new HttpRequestException(e.Message);
            }
        }

        #endregion

        #region Rin speaks

        public async Task RinSpeakAsync(string userId)
        {
            var slackText = $"<@{userId}> おう、どうした";
            var uri = new Uri(SlackConfiguration.RinWebhookEndpoint);

            var data = JsonSerializer.Serialize(new
            {
                text = slackText,
            });

            var content = new StringContent(data, Encoding.UTF8, @"application/json");

            try
            {
                var response = await HttpClient.PostAsync(uri, content);
            }
            catch (HttpRequestException e)
            {
                throw new HttpRequestException(e.Message);
            }
        }

        public async Task RinSpeakDinnerAsync()
        {
            var slackText = "ったく、お前はブレねーなぁ";
            var uri = new Uri(SlackConfiguration.RinWebhookEndpoint);

            var data = JsonSerializer.Serialize(new
            {
                text = slackText,
            });

            var content = new StringContent(data, Encoding.UTF8, @"application/json");

            try
            {
                var response = await HttpClient.PostAsync(uri, content);
            }
            catch (HttpRequestException e)
            {
                throw new HttpRequestException(e.Message);
            }
        }

        #endregion

        #region Nagisa speaks

        public async Task NagisaSpeakAsync(string userId)
        {
            var slackText = $"<@{userId}> なになにー？";
            var uri = new Uri(SlackConfiguration.NagisaWebhookEndpoint);

            var data = JsonSerializer.Serialize(new
            {
                text = slackText,
            });

            var content = new StringContent(data, Encoding.UTF8, @"application/json");

            try
            {
                var response = await HttpClient.PostAsync(uri, content);
            }
            catch (HttpRequestException e)
            {
                throw new HttpRequestException(e.Message);
            }
        }

        public async Task NagisaSpeakDinnerAsync()
        {
            var slackText = "僕はイワトびっくりパンが食べたいな～！イチゴ味！";
            var uri = new Uri(SlackConfiguration.NagisaWebhookEndpoint);

            var data = JsonSerializer.Serialize(new
            {
                text = slackText,
            });

            var content = new StringContent(data, Encoding.UTF8, @"application/json");

            try
            {
                var response = await HttpClient.PostAsync(uri, content);
            }
            catch (HttpRequestException e)
            {
                throw new HttpRequestException(e.Message);
            }
        }

        #endregion

        #region Rei speaks

        public async Task ReiSpeakAsync(string userId)
        {
            var slackText = $"<@{userId}> はい、なんでしょう？";
            var uri = new Uri(SlackConfiguration.ReiWebhookEndpoint);

            var data = JsonSerializer.Serialize(new
            {
                text = slackText,
            });

            var content = new StringContent(data, Encoding.UTF8, @"application/json");

            try
            {
                var response = await HttpClient.PostAsync(uri, content);
            }
            catch (HttpRequestException e)
            {
                throw new HttpRequestException(e.Message);
            }
        }

        public async Task ReiSpeakDinnerAsync()
        {
            var slackText = "またイワトびっくりパンですか...栄養が偏りますよ。";
            var uri = new Uri(SlackConfiguration.ReiWebhookEndpoint);

            var data = JsonSerializer.Serialize(new
            {
                text = slackText,
            });

            var content = new StringContent(data, Encoding.UTF8, @"application/json");

            try
            {
                var response = await HttpClient.PostAsync(uri, content);
            }
            catch (HttpRequestException e)
            {
                throw new HttpRequestException(e.Message);
            }
        }

        #endregion

    }
}

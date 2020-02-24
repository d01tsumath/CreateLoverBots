using Microsoft.Azure.Functions.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(SlackCallbackEvent.Startup))]

namespace SlackCallbackEvent
{
    /// <summary>
    /// アプリケーションの起動に関する初期化処理を提供します。
    /// </summary>
    internal class Startup : FunctionsStartup
    {
        /// <summary>
        /// サービスを追加/構成します。
        /// </summary>
        /// <param name="builder"></param>
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var services = builder.Services;
            var appSettings = services.AddConfiguration();
            services.AddDomainServices(appSettings);
        }
    }
}

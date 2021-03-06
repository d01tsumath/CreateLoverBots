﻿using SlackCallbackEvent.Configurations;
using SlackCallbackEvent.Domain.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Net.Http;

namespace SlackCallbackEvent
{
    /// <summary>
    /// <see cref="IServiceCollection"/> の拡張機能を提供します。
    /// </summary>
    internal static class ServiceCollectionExtensions
    {
        /// <summary>
        /// アプリケーションの構成情報を DI サービスに登録します。
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static AppSettings AddConfiguration(this IServiceCollection services)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            // --- 既定の構成情報を読み込む
            var builder = new ConfigurationBuilder().AddEnvironmentVariables();
            var configuration = builder.Build();

            //--- Azure App Configuration を読み込む
            var connectionString = configuration.GetConnectionString("AppConfig");
            var appConfigManagedEndpoint = configuration.GetConnectionString("AppConfigManagedEndpoint");
            if (!string.IsNullOrWhiteSpace(connectionString) || !string.IsNullOrEmpty(appConfigManagedEndpoint))
            {
                /*
                configuration = new ConfigurationBuilder()
                    .AddAzureAppConfiguration(c =>
                    {
                        if (!string.IsNullOrEmpty(appConfigManagedEndpoint))
                        {
                            c.ConnectWithManagedIdentity(appConfigManagedEndpoint);
                        }
                        else if (!string.IsNullOrEmpty(connectionString))
                        {
                            c.ConnectionString = connectionString;
                        }
                        c.Use("*");
                    })
                    .AddEnvironmentVariables()
                    .Build();
                */
            }

            //--- DI に登録
            var appSettings = configuration.Get<AppSettings>(o => o.BindNonPublicProperties = true);
            services.AddSingleton(configuration);
            services.AddSingleton(appSettings);
            return appSettings;
        }

        /// <summary>
        /// ドメインサービスを DI に登録します。
        /// </summary>
        /// <param name="services"></param>
        /// <param name="appSettings"></param>
        /// <returns></returns>
        public static IServiceCollection AddDomainServices(this IServiceCollection services, AppSettings appSettings)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (appSettings == null) throw new ArgumentNullException(nameof(appSettings));

            services.AddSlack(appSettings);
            services.TryAddScoped<BotService>();
            services.TryAddSingleton<StorageQueueService>();
            return services;
        }

        /// <summary>
        /// Slack の構成を DI に登録します。
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddSlack(this IServiceCollection services, AppSettings appSettings)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (appSettings == null) throw new ArgumentNullException(nameof(appSettings));

            //--- HttpClient を構成
            const string HttpClientName = "SlackCallbackEvent.Domain.Services.BotService.HttpClient";
            services.AddHttpClient(HttpClientName);

            services.TryAddScoped(p =>
            {
                var factory = p.GetRequiredService<IHttpClientFactory>();
                var client = factory.CreateClient(HttpClientName);
                return new BotService(client, appSettings.SlackConfiguration);
            });

            return services;
        }

    }
}

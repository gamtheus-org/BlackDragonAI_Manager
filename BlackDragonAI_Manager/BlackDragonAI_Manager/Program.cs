using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using BlackDragonAI_Manager.BlbApi;
using BlackDragonAI_Manager.Models;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Refit;

namespace BlackDragonAI_Manager
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            var refitSettings = new RefitSettings(new NewtonsoftJsonContentSerializer());

            builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddRefitClient<IBlbApi>(refitSettings).ConfigureHttpClient(httpClient =>
                httpClient.BaseAddress = new Uri("https://blackdragonai.nl/api"));

            builder.Services.AddSingleton<NotificationManager>();
            builder.Services.AddSingleton<BlbApiHandler>();
            builder.Services.AddSingleton<ILocalStorageService, LocalStorageService>();
            builder.Services.AddSingleton<LoggedInStatusManager>();

            await builder.Build().RunAsync();
        }
    }
}

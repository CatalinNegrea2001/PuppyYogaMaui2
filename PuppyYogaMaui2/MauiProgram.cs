using Microsoft.Extensions.DependencyInjection;
using PuppyYogaMaui2.Data;
using Microsoft.Maui.Hosting;
using System;
using System.IO;

namespace PuppyYogaMaui2
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton<PuppyYogaDatabase>(s =>
            {
                string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "PuppyYoga.db3");
                return new PuppyYogaDatabase(dbPath);
            });

           
            var app = builder.Build();

            return app;
        }
    }
}

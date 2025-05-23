﻿using ExpenseSplitterApp.DataAccess;
using ExpenseSplitterApp.DataAccess.Implementations;
using ExpenseSplitterApp.DataAccess.Interfaces;
using ExpenseSplitterApp.Services;
using ExpenseSplitterApp.ViewModels;
using ExpenseSplitterApp.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ExpenseSplitterApp
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

            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "expenses.db");

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite($"Filename={dbPath}"));

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddScoped<ServiceUnitOfWork>();

            builder.Services.AddSingleton<MainPageViewModel>();
            builder.Services.AddSingleton<PersonViewModel>();

            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<PersonPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif
#if WINDOWS
  Microsoft.Maui.Handlers.SwitchHandler.Mapper.AppendToMapping("NoLabel", (handler, View) =>
  {
      handler.PlatformView.OnContent = null;
      handler.PlatformView.OffContent = null;

      // Add this to remove the padding around the switch as well
      // handler.PlatformView.MinWidth = 0;
  });
#endif

            var app = builder.Build();

            using var scope = app.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            db.Database.EnsureCreated();

            return app;
        }
    }
}

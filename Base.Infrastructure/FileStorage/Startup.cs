using System;
using System.Collections.Generic;
using System.Text;
using Base.Application.FilesStorage.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Base.Infrastructure.FileStorage;

public static class Startup
{
    public static IServiceCollection AddStorage(this IServiceCollection services)
    {
        services.AddSingleton<IStorageProvider, LocalStorageProvider>();
        return services;
    }
}

﻿using DotnetApiTemplate.Core;
using DotnetApiTemplate.Core.Abstractions;
using DotnetApiTemplate.Infrastructure.Ticket.AmazonStorage;
using DotnetApiTemplate.Infrastructure.Ticket.GoogleStorage;
using DotnetApiTemplate.Persistence.Postgres;
using DotnetApiTemplate.Persistence.Postgres.Ticket;
using DotnetApiTemplate.Shared.Abstractions.Encryption;
using DotnetApiTemplate.Shared.Infrastructure;
using DotnetApiTemplate.Shared.Infrastructure.Api;
using DotnetApiTemplate.Shared.Infrastructure.Clock;
using DotnetApiTemplate.Shared.Infrastructure.Contexts;
using DotnetApiTemplate.Shared.Infrastructure.Encryption;
using DotnetApiTemplate.Shared.Infrastructure.Files.FileSystems;
using DotnetApiTemplate.Shared.Infrastructure.Initializer;
using DotnetApiTemplate.Shared.Infrastructure.Localization;
using DotnetApiTemplate.Shared.Infrastructure.Serialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;
using DotnetApiTemplate.Infrastructure.Ticket.Services;

[assembly: InternalsVisibleTo("DotnetApiTemplate.UnitTests")]

namespace DotnetApiTemplate.Infrastructure.Ticket;

public static class ServiceCollection
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCore();
        services.AddSharedInfrastructure();
        services.AddQueueConfiguration();
        services.AddSendGridConfiguration();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IFileRepositoryService, FileRepositoryService>();
        services.AddSingleton<ISalter, Salter>();
        services.AddScoped<IFileUploadService, FileUploadService>();
        services.AddSingleton<IAzureBlobService, AzureBlobService>();

        //use one of these
        //services.AddSqlServerDbContext(configuration, "sqlserver");
        //services.AddPostgresDbContext(configuration, "postgres");
        services.AddPostgresTicketDbContext(configuration, "postgres");

        services.AddFileSystemService();
        services.AddJsonSerialization();
        services.AddClock();
        services.AddContext();
        services.AddEncryption();
        services.AddCors();
        services.AddCorsPolicy();
        services.AddLocalizerJson();
        services.AddGoogleStorage(configuration);
        services.AddAws(configuration);

        //if use azure blob service
        //make sure app setting "azureBlobService":"" is filled
        //services.AddSingleton<IAzureBlobService, AzureBlobService>();

        services.AddInitializer<CoreInitializer>();
        services.AddApplicationInitializer();
    }
}
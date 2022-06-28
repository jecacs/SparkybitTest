using SparkybitTest.Consumer.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.AddSettings();

builder.AddAppServices();

builder.AddHostedServices();

var app = builder.Build();

app.Run();
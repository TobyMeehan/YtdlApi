using System.Text.Json;
using System.Text.Json.Serialization;
using FastEndpoints;
using YoutubeExplode;
using YtdlApi.Server.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFastEndpoints();

builder.Services.AddTransient<YoutubeClient>();
builder.Services.AddTransient<IYouTubeStreamService, YoutubeExplodeStreamService>();

// Add services to the container.

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseFastEndpoints();

app.Run();
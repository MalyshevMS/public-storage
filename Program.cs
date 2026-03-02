using public_storage.Extensions;
using public_storage.Services;
using public_storage.Configuration;

var builder = WebApplication.CreateBuilder();
builder.WebHost.UseUrls("http://0.0.0.0:5000");

builder.Services.Configure<UploadOptions>(builder.Configuration.GetSection("Storage"));

builder.Services.AddTransient<UploadService>();

var app = builder.Build();

app.MapHome();
app.MapDownload();
app.MapUpload();
app.MapRoot();

app.Run();
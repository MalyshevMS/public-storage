using public_storage.Middleware;

namespace public_storage.Extensions;

public static class EndpointExtension
{
    public static IApplicationBuilder MapHome(this IApplicationBuilder app)
    {
        return app.Map("/home", 
            builder => builder.UseMiddleware<HomeMiddleware>()
        );
    }

    public static IApplicationBuilder MapDownload(this IApplicationBuilder app)
    {
        return app.Map("/download", 
            builder => builder.UseMiddleware<DownloadMiddleware>()
        );
    }

    public static IApplicationBuilder MapUpload(this IApplicationBuilder app)
    {
        return app.Map("/upload", 
            builder => {
                builder.UseMiddleware<PasswordMiddleware>("123");
                builder.UseMiddleware<UploadMiddleware>();
            }
        );
    }

    public static IApplicationBuilder MapRoot(this IApplicationBuilder app)
    {
        return app.UseMiddleware<RootMiddleware>();
    }
}
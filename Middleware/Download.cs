using public_storage.Services;

namespace public_storage.Middleware;

public class DownloadMiddleware
{
    private readonly RequestDelegate next;

    public DownloadMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task InvokeAsync(HttpContext ctx)
    {
        var req = ctx.Request;
        var res = ctx.Response;
        var uploadPath = ctx.RequestServices.GetService<IUploadService>()!.GetUploadPath();

        var fileName = req.Path.Value!.Split("/").Last();
        var filePath = Path.Combine(uploadPath, fileName);

        if (!File.Exists(filePath))
        {
            res.ContentType = "text/html; charset=utf-8";
            res.StatusCode = 404;
            await res.WriteAsync("<center>404, File not found</center>");
            return;
        }

        res.ContentType = "application/octet-stream";
        res.Headers.ContentDisposition = $"attachment; filename=\"{fileName}\"";

        await res.SendFileAsync(filePath);
    }
}
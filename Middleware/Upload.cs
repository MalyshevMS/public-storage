using public_storage.Services;

namespace public_storage.Middleware;

public class UploadMiddleware
{
    private readonly RequestDelegate next;

    public UploadMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task InvokeAsync(HttpContext ctx)
    {
        var req = ctx.Request;
        var res = ctx.Response;
        var uploadPath = ctx.RequestServices.GetService<IUploadService>()!.GetUploadPath();

        if (!req.HasFormContentType)
        {
            res.StatusCode = 400;
            res.ContentType = "text/html; charset=utf-8";
            await res.WriteAsync("<center>Ivalid Form!</center>");
            return;
        }

        var form = await req.ReadFormAsync();
        var file = form.Files["file"];

        if (file == null || file.Length == 0)
        {
            res.ContentType = "text/html; charset=utf-8";
            res.StatusCode = 400;
            await res.WriteAsync("<center>No file selected or empty file!</center>");
            return;
        }

        var filePath = Path.Combine(uploadPath, file.FileName);

        using var stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream);

        res.Redirect("/");
    }
}
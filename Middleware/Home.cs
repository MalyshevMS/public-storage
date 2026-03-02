using System.Text;
using public_storage.Services;

namespace public_storage.Middleware;

public class HomeMiddleware
{
    private readonly RequestDelegate next;

    public HomeMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task InvokeAsync(HttpContext ctx)
    {
        var uploadPath = ctx.RequestServices.GetService<IUploadService>()!.GetUploadPath();
        var files = Directory.GetFiles(uploadPath).Select(Path.GetFileName);

        var html = new StringBuilder();

        html.Append("""
        <h1>Public File Storage</h1>

        <form action="/upload" method="post" enctype="multipart/form-data">
            <input type="file" name="file"/><br>
            <button type="submit">Upload</button>
        </form>

        <h2>Files:</h2>
        <ul>
        """);

        foreach (var file in files)
        {
            html.Append($"<li><a href=\"/download/{file}\">{file}</a></li>");
        }

        html.Append("</ul>");

        ctx.Response.ContentType = "text/html; charset=utf-8";
        await ctx.Response.WriteAsync(html.ToString());
    }
}
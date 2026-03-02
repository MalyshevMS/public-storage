using System.Text;

var builder = WebApplication.CreateBuilder();
builder.WebHost.UseUrls("http://0.0.0.0:5000");

var app = builder.Build();
 
var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "files");
Directory.CreateDirectory(uploadPath);

app.MapWhen(
    ctx => ctx.Request.Path == "/", 
    root =>
    {
        root.Run(async ctx =>
        {
            ctx.Response.Redirect("/home");
        });
    }
);

app.Map("/home", home =>
{
    home.Run(async ctx =>
    {
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
    });
});

app.Map("/upload", upload =>
{
    upload.Run(async ctx =>
    {
        var req = ctx.Request;
        var res = ctx.Response;

        if (!req.HasFormContentType)
        {
            res.StatusCode = 400;
            await res.WriteAsync("<center>Ivalid Form!</center>");
            return;
        }

        var form = await req.ReadFormAsync();
        var file = form.Files["file"];

        if (file == null || file.Length == 0)
        {
            res.StatusCode = 400;
            await res.WriteAsync("<center>No file selected or empty file!</center>");
            return;
        }

        var filePath = Path.Combine(uploadPath, file.FileName);

        using var stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream);

        res.Redirect("/");
    });
});

app.Map("/download", download =>
{
    download.Run(async ctx =>
    {
        var req = ctx.Request;
        var res = ctx.Response;

        var fileName = req.Path.Value!.Split("/").Last();
        var filePath = Path.Combine(uploadPath, fileName);

        if (!File.Exists(filePath))
        {
            res.StatusCode = 404;
            await res.WriteAsync("<center>404, File not found</center>");
            return;
        }

        res.ContentType = "application/octet-stream";
        res.Headers.ContentDisposition = $"attachment; filename=\"{fileName}\"";

        await res.SendFileAsync(filePath);
    });
});

app.Run();
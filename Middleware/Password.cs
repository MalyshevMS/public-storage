using public_storage.Services;

namespace public_storage.Middleware;

public class PasswordMiddleware
{
    private readonly RequestDelegate next;
    private readonly string password;

    public PasswordMiddleware(RequestDelegate next, PasswordService passwordService)
    {
        this.next = next;
        this.password = passwordService.GetPassword();
    }

    public async Task InvokeAsync(HttpContext ctx)
    {
        var form = await ctx.Request.ReadFormAsync();
        var pass = form["pass"];

        if (pass == password)
        {
            await next.Invoke(ctx);
        }
        else
        {
            ctx.Response.StatusCode = 403;
            ctx.Response.ContentType = "text/html; charset=utf-8";
            await ctx.Response.WriteAsync("<center>403, Forbidden</center>");
        }
    }
}
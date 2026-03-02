namespace public_storage.Middleware;

public class RootMiddleware
{
    private readonly RequestDelegate next;

    public RootMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task InvokeAsync(HttpContext ctx)
    {
        ctx.Response.Redirect("/home");
    }
}
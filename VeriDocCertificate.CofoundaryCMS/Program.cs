var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllersWithViews()
    .AddCofoundry(builder.Configuration);

var app = builder.Build();

app.UseHttpsRedirection();
app.UseCofoundry();
app.Use(async (context, next) =>
{
    string path = context.Request.Path;

    if (path.EndsWith(".css") || path.EndsWith(".js"))
    {

        //Set css and js files to be cached for 30 days
        TimeSpan maxAge = new TimeSpan(30, 0, 0, 0);     //30 days
        context.Response.Headers.Append("Cache-Control", "max-age=" + maxAge.TotalSeconds.ToString("0"));

    }
    else if (path.EndsWith(".svg") || path.EndsWith(".jpg") || path.EndsWith(".png"))
    {
        //Set css and js files to be cached for 30 days
        TimeSpan maxAge = new TimeSpan(30, 0, 0, 0);     //30 days
        context.Response.Headers.Append("Cache-Control", "max-age=" + maxAge.TotalSeconds.ToString("0"));

    }
    else
    {
        //Request for views fall here.
        context.Response.Headers.Append("Cache-Control", "no-cache");
        context.Response.Headers.Append("Cache-Control", "private, no-store");

    }
    await next();
});
app.Run();

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TodoAppWithLogin.Data;
using TodoAppWithLogin.Models;
using TodoAppWithLogin.Services;
namespace TodoAppWithLogin
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //Database Configuration
            var dbConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(dbConnectionString));


            builder.Services.AddIdentity<Users, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false; // relax defaults as you like
                options.User.RequireUniqueEmail = true;
            })
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

            // Add Authentication

            // Google
            builder.Services.AddAuthentication()
    .AddGoogle(options =>
    {
        options.ClientId = builder.Configuration["Authentication:Google:ClientId"]!;
        options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"]!;
    });

            // AWS SES Service
            builder.Services.AddScoped<IEmailSender, SesEmailSender>();

            // Add services to the container.
            builder.Services.AddRazorPages();

            // Add application services
            //builder.Services.AddScoped<IUserService, UserService>();

            var app = builder.Build();

            // google authentication redirect_uri_mismatch issue fix 

            /*
             * This app is behind CloudFront (deployed to aws EB and run behind CloudFront), which terminates HTTPS and forwards to EB over plain HTTP this is expected internally.
             * But ASP.NET Core needs to know the original request was HTTPS, or it'll generate http:// URLs (like this redirect) even though the user's actual connection was secure.
             * This is a classic "forwarded headers" issue with reverse proxies/CDNs.
             */

            /*
             * This tells ASP.NET Core to trust the X-Forwarded-Proto header that CloudFront sends,
             * so it correctly knows the original request was HTTPS — which fixes the redirect URI generation (and likely other subtle issues, like cookie security flags) app-wide,
             * not just for this one OAuth flow.
             */

            app.Use((context, next) =>
            {
                context.Request.Scheme = "https";
                return next();
            });

            //var forwardedHeadersOptions = new ForwardedHeadersOptions
            //{
            //    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            //};
            //forwardedHeadersOptions.KnownIPNetworks.Clear();
            //forwardedHeadersOptions.KnownProxies.Clear();

            //app.UseForwardedHeaders(forwardedHeadersOptions);

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapGet("/debug-headers", (HttpContext context) =>
            {
                var headers = context.Request.Headers
                    .Select(h => $"{h.Key}: {h.Value}")
                    .OrderBy(h => h);
                return string.Join("\n", headers);
            });

            app.MapStaticAssets();

            app.MapRazorPages()
               .WithStaticAssets();

            app.Run();
        }
    }
}

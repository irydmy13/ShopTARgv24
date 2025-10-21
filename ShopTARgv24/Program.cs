using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using ShopTARgv24.ApplicationServices.Services;
using ShopTARgv24.Core.ServiceInterface;
using ShopTARgv24.Data;

namespace ShopTARgv24
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();

            builder.Services.AddScoped<ISpaceshipsServices, SpaceshipsServices>();
            builder.Services.AddScoped<IFileServices, FileServices>();
            builder.Services.AddScoped<IRealEstateServices, RealEstateServices>();
            builder.Services.AddScoped<IWeatherForecastServices, WeatherForecastServices>();




            builder.Services.AddDbContext<ShopTARgv24Context>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<IChuckNorrisServices, ShopTARgv24.ApplicationServices.Services.ChuckNorrisServices>();
            builder.Services.AddHttpClient<IChuckNorrisServices, ShopTARgv24.ApplicationServices.Services.ChuckNorrisServices>();

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();
            app.UseStaticFiles();



            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
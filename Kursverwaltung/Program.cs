using Kursverwaltung.Models;
using Microsoft.EntityFrameworkCore;

namespace Kursverwaltung
{
    public class Program
    {
        public static void Main( string [] args )
        {
            var builder = WebApplication.CreateBuilder( args );

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<KursDbContext>( options =>
            options.UseLazyLoadingProxies().UseSqlServer( builder.Configuration.GetConnectionString( "MeineDatenbank" ) )
            );

            var app = builder.Build();

            using ( var scope = app.Services.CreateScope() )
            {
                var context = scope.ServiceProvider.GetRequiredService<KursDbContext>();
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }

            // Configure the HTTP request pipeline.
            if ( !app.Environment.IsDevelopment() )
            {
                app.UseExceptionHandler( "/Home/Error" );
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default" ,
                pattern: "{controller=Home}/{action=Index}/{id?}" );

            app.Run();
        }
    }
}
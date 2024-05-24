using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using talabat.Apis.Errors;
using talabat.Apis.Extentions;
using talabat.Apis.Helpers;
using talabat.Apis.Middlewares;
using talabat.core.Entites;
using talabat.core.Entites.identity;
using talabat.core.Repositories;
using talabat.Repository;
using talabat.Repository.Data;
using talabat.Repository.Identity;

namespace talabat.Apis
{
    public class Program
    {
        // Entry Point
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            #region configure service
            builder.Services.AddControllers();
            
            builder.Services.AddSwaggerService();

            builder.Services.AddDbContext<StoreContext>(Options =>
            {
                Options.UseSqlServer(builder.Configuration.GetConnectionString("Defaultconnection"));
            });
            builder.Services.AddAplictionService();

            builder.Services.AddSingleton<IConnectionMultiplexer>(Options =>
            {
                var Connection = builder.Configuration.GetConnectionString("Redis");
                return ConnectionMultiplexer.Connect(Connection);
            });

            builder.Services.AddScoped(typeof(IBasketRepositries), typeof(BasketRepository));

            builder.Services.AddDbContext<AppIdentityDBContext>(Options =>
            {
                Options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
            });

         
            builder.Services.AddIdentityServices(builder.Configuration);
            builder.Services.AddCors(Options =>
            {
                Options.AddPolicy("MyPolicy", Options =>
                {
                    Options.AllowAnyHeader()
                           .AllowAnyHeader()
                           .AllowAnyOrigin();
                });
            });
            
            
            
            
            // ?????? ? ????? extentions 
            ///builder.Services.AddScoped(typeof(iGenericRepository<>), typeof(GenericRepository<>));
            ///builder.Services.AddAutoMapper(typeof(MappingProfilles));
            ///builder.Services.Configure<ApiBehaviorOptions>(Options =>
            ///{
            ///    Options.InvalidModelStateResponseFactory = (actionContext) =>
            ///    {
            ///        var Errors = actionContext.ModelState.Where(p => p.Value.Errors.Count() > 0)
            ///                                                           .SelectMany(p => p.Value.Errors)
            ///                                                           .Select(E => E.ErrorMessage)
            ///                                                           .ToArray();
            ///        var ValidationErrorResponse = new ApiValidationErrorResponse()
            ///        {
            ///            Errors = Errors
            ///        };
            ///        return new BadRequestObjectResult(ValidationErrorResponse);
            ///    };
            ///});
            #endregion
            var app = builder.Build();
            
            using var scope = app.Services.CreateScope();
            var Services = scope.ServiceProvider;

            var loggerfactory = Services.GetRequiredService<ILoggerFactory>();
            try
            {

                var dbcontext = Services.GetRequiredService<StoreContext>(); // ask clr for creating object from Dbcontext Explicitly
                await dbcontext.Database.MigrateAsync();  // update database
                await StoreContextSeed.SeedAsync(dbcontext);


                var identityDbContext = Services.GetRequiredService<AppIdentityDBContext>();
                await identityDbContext.Database.MigrateAsync();

                var usermanagwr = Services.GetRequiredService<UserManager<AppUser>>();
                await AppIdentityDbContextSeed.SeedUSerAsync(usermanagwr);

            }
            catch (Exception ex)
            {
                var logger = loggerfactory.CreateLogger<Program>();
                logger.LogError(ex, "An Error Occured During Appling The Migrations");
            }


            //StoreContext dbcontext = new StoreContext();
            //dbcontext.Database.MigrateAsync(); // ubdate database

            // Configure the HTTP request pipeline.
            #region configure middlewares
            app.UseMiddleware<ExeptionMiddleWares>();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerMiddlewares();
            }

            app.UseStatusCodePagesWithRedirects("/Errors/{0}");
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCors("MyPolicy");
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            #endregion
            app.Run();
        }
    }
}